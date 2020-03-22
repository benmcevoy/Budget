using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budget.Facets;
using Budget.Statements;

namespace Budget.Web
{
    public static class Queries
    {
        public static IQueryable<Transaction> DebitInRange(this IEnumerable<Transaction> source, Query query)
        {
            return source
                .Where(transaction => transaction.Amount.IsDebit())
                .Where(t => Dates.InRange(t, query.DateRange))
                .AsQueryable()
                ;
        }
    }
}
