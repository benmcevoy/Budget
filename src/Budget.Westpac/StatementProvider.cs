using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Budget.Statements;

namespace Budget.Westpac
{
    /// <summary>
    /// Provide Westpac statements from the source folder.
    /// This class can safely be instantiated as a singleton.
    /// </summary>
    public class StatementProvider : IStatementProvider
    {
        public StatementProvider(Configuration config) => _path = config.StatementsPath;
        
        public Statement Statement()
        {
            if (_statement != null) return _statement;

            _statement = new Statement();

            var files = Directory.EnumerateFiles(_path);
            var transactions = new Dictionary<string, Transaction>();

            foreach (var file in files)
            {
                if(!file.EndsWith(".csv")) continue;

                foreach (var t in Transactions(file))
                {
                    transactions[t.Id()] = t;
                }
            }

            _statement.Transactions = transactions.Values;

            return _statement;
        }

        private static IEnumerable<Transaction> Transactions(string file)
        {
            var lines = File.ReadAllLines(file);

            foreach (var line in lines)
            {
                if (!IsTransactionLine(line)) continue;

                var values = CsvLineToArray(line);

                yield return new Transaction
                {
                    Account = values[LineStructure.BankAccount],
                    Amount = Amount(values),
                    Date = DateTime.ParseExact(values[LineStructure.Date], "yyyyMMddHHmmss", DateTimeFormatInfo.CurrentInfo),
                    Description =
                        $"{values[LineStructure.Narrative]} {values[LineStructure.Categories]} {values[LineStructure.Serial]}"
                };
            }
        }

        private static decimal Amount(IList<string> values)
        {
            if (!string.IsNullOrEmpty(values[LineStructure.CreditAmount]))
            {
                return decimal.Parse(values[LineStructure.CreditAmount]);
            }

            return -decimal.Parse(values[LineStructure.DebitAmount]);
        }

        private static IList<string> CsvLineToArray(string line)
        {
            // leave empty entries
            return line.Split(new[] { ',' }, StringSplitOptions.None);
        }

        private static bool IsTransactionLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return false;
            if (line.Equals(@"Bank Account,Date,Narrative,Debit Amount,Credit Amount,Balance,Categories,Serial")) return false;
            if (CsvLineToArray(line).Count != 8) return false;

            return true;
        }

        private static class LineStructure
        {
            public const int BankAccount = 0;
            public const int Date = 1;
            public const int Narrative = 2;
            public const int DebitAmount = 3;
            public const int CreditAmount = 4;
            public const int Balance = 5;
            public const int Categories = 6;
            public const int Serial = 7;
        }
        
        private readonly string _path;
        private Statement _statement;
    }
}
