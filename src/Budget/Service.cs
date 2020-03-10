using System;
using System.Collections.Generic;
using System.Linq;
using Budget.Statements;

namespace Budget
{
    public class Service
    {
        private readonly IStatementProvider _statement;
        private readonly ITagsProvider _tags;

        public Service(IStatementProvider statement, ITagsProvider tags)
        {
            _statement = statement;
            _tags = tags;
        }

        public IReadOnlyCollection<Transaction> Transactions()
        {
            return Transactions(_tags.Taggers().ToArray());
        }

        public IReadOnlyCollection<Transaction> Transactions(params Func<string, string>[] taggers)
        {
            return _statement.Statement().Transactions
                .Select(transaction => Classifier.Tag(transaction, taggers))
                .ToList();
        }
    }
}
