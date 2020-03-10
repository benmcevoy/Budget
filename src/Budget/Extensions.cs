using System.Diagnostics;
using System.Text;

namespace Budget
{
    public static class Extensions
    {
        [DebuggerStepThrough]
        public static string NormalizeString(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            // lowercase
            // replace all non aplha and non digit
            // compact empty space

            var result = new StringBuilder();
            var isDoubleSpace = false;

            foreach (var character in value)
            {
                if (char.IsLetterOrDigit(character))
                {
                    result.Append(char.ToLower(character));
                    isDoubleSpace = false;
                    continue;
                }

                if (isDoubleSpace) continue;

                result.Append(" ");
                isDoubleSpace = true;
            }

            return result
                .ToString()
                .Trim();
        }

        public static bool IsCredit(this decimal value) => value >= 0;
        public static bool IsDebit(this decimal value) => value < 0;
    }
}