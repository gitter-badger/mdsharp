using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdSharp.Core.Components
{
    public static class StringExtensions
    {
        public static string FormatReturn(this string input)
        {
            string description = input.FormatText();
            //Not going to waste our time displaying emtpy returns
            return String.IsNullOrWhiteSpace(description) ? String.Empty :
                   $"*Returns*{Environment.NewLine}`{description}`";
        }

        public static string FormatText(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.Replace("\r\n", String.Empty)
                .Replace("\n", String.Empty)
                .Replace("\t", String.Empty)
                .Trim();
        }

        public static string ToTypeCase(this string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input));

            return $"{input.Substring(0, 1).ToUpper()}{input.Substring(1, input.Length - 1).ToLower()}";
        }
    }

}
