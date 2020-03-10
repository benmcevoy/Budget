using System.Collections.Generic;

namespace Budget.Statements
{
    public class Statement
    {
        public Statement() => Transactions = new List<Transaction>();

        public IReadOnlyCollection<Transaction> Transactions { get; set; }
    }
}
