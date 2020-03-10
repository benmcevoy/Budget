using System;
using System.Collections.Generic;

namespace Budget.Statements
{
    public class Transaction
    {
        public string Id() => ToString();

        public override string ToString() => $@"{Account}-{Date}-{Description}-{Amount}";

        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag)) Tags.Add(tag);
        }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Account { get; set; }
        public IList<string> Tags { get; } = new List<string>();
    }
}