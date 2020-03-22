using System.Collections.Generic;
using System.Linq;
using Budget.Facets;
using Budget.Statements;

namespace Budget.Web
{
    public static class Queries
    {
        public static IQueryable<Transaction> DebitInRange(this IEnumerable<Transaction> source, Query query)
            => source
                .Where(transaction => transaction.Amount.IsDebit())
                .Where(t => Dates.InRange(t, query.DateRange))
                .AsQueryable();


        public static IQueryable<Transaction> InRange(this IEnumerable<Transaction> source, Query query)
            => source
                .Where(t => Dates.InRange(t, query.DateRange))
                .AsQueryable();

        public static IQueryable<Transaction> WithTags(this IEnumerable<Transaction> source, Query query)
            => source
                .Where(t => query.Tags.Contains(t.Tags.First()))
                .AsQueryable();

    }
}
