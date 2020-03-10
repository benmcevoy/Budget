using System;
using Budget.Statements;

namespace Budget.Facets
{
    public static class Dates
    {
        public enum Resolution { Day, Week, Month, Year, All }
        public enum Range { Today, LastSevenDays, LastMonth, LastSixMonths, YearToDate, LastYear, LastFiveYears, All }

        public static DateTime Scale(Transaction t, Resolution resolution) => resolution switch
        {
            Resolution.Week => Week(t.Date),
            Resolution.Month => Month(t.Date),
            Resolution.Year => Year(t.Date),
            Resolution.All => DateTime.MinValue,
            _ => t.Date.Date
        };

        public static bool InRange(Transaction t, Range range) => range switch
        {
            Range.Today => t.Date.Date >= DateTime.Now.Date,
            Range.LastSevenDays => t.Date.Date >= DateTime.Now.Date.AddDays(-7),
            Range.LastMonth => t.Date.Date >= DateTime.Now.Date.AddMonths(-1),
            Range.LastSixMonths => t.Date.Date >= DateTime.Now.Date.AddMonths(-6),
            Range.YearToDate => Year(t.Date) == Year(DateTime.Now),
            Range.LastYear => t.Date.Date >= DateTime.Now.Date.AddYears(-1),
            Range.LastFiveYears => t.Date.Date >= DateTime.Now.Date.AddYears(-5),
            Range.All => true,
            _ => false
        };

        private static DateTime Week(DateTime date) => date.AddDays(-(int)date.DayOfWeek);
        private static DateTime Month(DateTime date) => new DateTime(date.Year, date.Month, 1);
        private static DateTime Year(DateTime date) => new DateTime(date.Year, 1, 1);
    }
}
