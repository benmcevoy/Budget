using System.Collections.Generic;
using System.Linq;
using Budget.Facets;
using Budget.Statements;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web
{
    [ApiController]
    public class TransactionsController
    {
        private readonly Service _service;
        public TransactionsController(Service service) => _service = service;

        [Route("/transactions")]
        public object Get()
        {
            // google charts wants the data as
            // a dataTable
            var query = QueryContext.Query;

            var transactions = _service
                .Transactions()
                .InRange(query)
                .WithTags(query)
                .ToList();

            var cols = DataColumns(transactions);

            var rows = DataRows(transactions, cols, query);

            return new DataTable { Cols = cols, Rows = rows };
        }

        [Route("/transactions/Other")]
        public object Unclassified()
        {
            var transactions = _service
                .Transactions()
                .ToList();

            return transactions.Where((transaction, i) => transaction.Tags.Any(s => s.Equals("Other")));
        }

        private static IEnumerable<IEnumerable<object>> DataRows(IEnumerable<Transaction> transactions, ICollection<DataColumn> cols, Query query)
        {
            // group into histogram
            var source = transactions
                .GroupBy(
                    t => Dates.Scale(t, query.DateResolution),
                    t => new { Key = t.Date, Value = t.Amount, Tag = t.Tags.First() },
                    (key, g) => new { Date = key, Transactions = g })
                .OrderByDescending(x => x.Date);
            
            // foreach col sum up
            foreach (var g in source)
            {
                var row = new List<object> {g.Date};
                
                foreach (var dataColumn in cols.Skip(1))
                {
                    row.Add(g.Transactions
                        .Where(arg => arg.Tag == dataColumn.Label)
                        .Sum(arg => arg.Value));
                }

                yield return row;
            }
        }

        private static ICollection<DataColumn> DataColumns(IEnumerable<Transaction> transactions)
        {
            var cols = transactions
                .Select(transaction => transaction.Tags.First())
                .Distinct()
                .OrderBy(s => s)
                .Select(s => new DataColumn { Label = s, Type = DataColumnType.Number });

            return new[]
            {
                new DataColumn
                {
                    Type = DataColumnType.String,
                    Label = "Date"
                }
            }.Concat(cols).ToList();
        }

        public class DataTable
        {
            public IEnumerable<DataColumn> Cols { get; set; }
            public IEnumerable<object> Rows { get; set; }
        }

        public class DataColumn
        {
            public string Type { get; set; }
            public string Label { get; set; }
            public string Id { get; set; }
            public string Role { get; set; }
            public string Pattern { get; set; }
        }

        public class DataColumnType
        {
            public const string String = "string";
            public const string Number = "number";
            public const string Boolean = "boolean";
            public const string Date = "date";
            public const string DateTime = "datetime";
            public const string TimeOfDay = "timeofday";
        }
        public class DataColumnRole
        {
            public const string Annotation = "annotation";
            public const string AnnotationText = "annotationText";
            public const string Certainty = "certainty";
            public const string Emphasis = "emphasis";
            public const string Interval = "interval";
            public const string Scope = "scope"; 
            public const string Style = "style";
            public const string Tooltip = "tooltip";
        }
    }
}
