using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdSharp.Core.Components
{
    public static class StringHelpers
    {
        /// <summary>
        /// Formats the text.
        /// </summary>
        /// <param name="input">The input value.</param>
        /// <returns>Formatted text</returns>
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
