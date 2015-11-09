using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdSharp.Core.Components
{
    public static class StringExtensions
    {
        public static string FormatText(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.Replace("\r\n", String.Empty)
                .Replace("\n", String.Empty)
                .Replace("\t", String.Empty)
                .Trim();
        }
    }
}
