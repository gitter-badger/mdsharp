using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Helper methods for rendering common Markdown elements
    /// </summary>
    public class MarkdownHelper
    {
        /// <summary>
        /// Renders the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public IEncodedString RenderTable(string tableName, IEnumerable<Tuple<string, string>> rows)
        {
            if(!rows.Any())
                return new RawString(String.Empty);
            
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"##### {tableName} #####");
            stringBuilder.AppendLine("| Name | Description |");
            stringBuilder.AppendLine("| ---- | ----------- |");
            foreach (var row in rows)
            {
                stringBuilder.AppendLine($"| {row.Item1} | {row.Item2} |");
            }
            return new RawString(stringBuilder.ToString());
        }

        /// <summary>
        /// Renders raw text for the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IEncodedString Raw(string value)
        {
            return new RawString(value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MarkdownTemplateBase<T> : TemplateBase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownTemplateBase{T}"/> class.
        /// </summary>
        protected MarkdownTemplateBase()
        {
            Markdown = new MarkdownHelper();
        }

        public MarkdownHelper Markdown { get; set; }
    }
}
