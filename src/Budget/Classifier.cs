using System;
using System.Collections.Generic;
using System.Linq;
using Budget.Statements;

namespace Budget
{
    public static class Classifier
    {
        public static Func<string, Func<string, string>> Contains(params string[] patterns)
            => classification
                => raw
                    => Contains(patterns, classification, raw);

        private static string Contains(IEnumerable<string> patterns, string classification, string raw)
        {
            var matchedPattern = patterns
                .FirstOrDefault(pattern => raw.NormalizeString().Contains(pattern.NormalizeString()));

            return matchedPattern != null
                ? classification
                : null;
        }

        public static Transaction Tag(Transaction transaction, params Func<string, string>[] taggers)
        {
            foreach (var tagger in taggers)
            {
                var c = tagger(transaction.Description);

                if (c == null) continue;

                transaction.AddTag(c);
            }

            if (!transaction.Tags.Any()) transaction.AddTag("_Other");

            return transaction;
        }
    }
}
