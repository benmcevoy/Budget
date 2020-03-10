using System.Collections.Generic;
using System.Linq;
using Budget.Facets;
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

            // [
            //  [ columnanmes, etc ],
            //  [ data row]

            // i think I want to break this down a bit
            // i still don't feel the linq groupBy

            // PIVOT was what I thought
            // date, sum(value), Tag
            //      sum(value), Tag
            //      sum(value), Tag
            //      sum(value), Tag

            var query = QueryContext.Query;

            var lastMonthsDebitTransactions = _service
                .Transactions()
                .Where(t => t.Amount.IsDebit())
                .Where(t => Dates.InRange(t, query.DateRange))
                .ToList();

            var columns = new object[] { "Tags" }.Concat(
                        lastMonthsDebitTransactions
                        .Select(transaction => transaction.Tags.First())
                        .Distinct()
                        .OrderBy(s => s)
                    )
                .ToList();


            var groups = lastMonthsDebitTransactions
                .GroupBy(
                    t => Dates.Scale(t, query.DateResolution),
                    t => new{ t.Amount, Tag = t.Tags.First() },
                    (key, g) => new { Date = key, Transaction = g }
                )
                .OrderByDescending(arg => arg.Date)
                ;


            // and pivot?
            // https://www.nrecosite.com/pivot_data_library_net.aspx
            // https://www.reflectionit.nl/Blog/2009/c-linq-pivot-function


            var result = new
            {
                data = new List<object> { columns }
            };

            return result;
        }
    }
}
