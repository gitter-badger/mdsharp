using System;
using System.Linq;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Base class for Member container classes
    /// </summary>
    public class MemberBase
    {
        /// <summary>
        /// The XElement for the Member container
        /// </summary>
        protected readonly XElement _element;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberBase"/> class.
        /// </summary>
        /// <param name="element">The XElement we want to create a Member container from.</param>
        public MemberBase(XElement element)
        {
            _element = element;
        }

        /// <summary>
        /// Gets the name of the parent Type for the Member.
        /// </summary>
        /// <value>
        /// The name of the parent Type of the Member.
        /// </value>
        public string TypeName
        {
            get
            {
                //This was previously looking for a type element first, but it seemed advantageous to consolidate it
                //TODO Make this string manipulation less fugly... there's probably a better way of doing this
                string replace = $".{String.Concat(FullName.TakeWhile(s => s != '(')).Split('.').Last()}";
                return String.Concat(FullName.Replace(replace, String.Empty).TakeWhile(s => s != '('));
            }
        }
        /// <summary>
        /// Gets or sets the assembly name.
        /// </summary>
        /// <value>
        /// The assembly name.
        /// </value>
        public string AssemblyName => _element.Document?
                                              .Element("doc")?
                                              .Element("assembly")?
                                              .Value ?? String.Empty;

        /// <summary>
        /// Gets the Full Member name of the Member.
        /// </summary>
        /// <value>
        /// The full name of the Member.
        /// </value>
        public string FullName => _element.Attribute("name").Value.Remove(0, 2);

        /// <summary>
        /// Gets the Member's Short Name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        /// <remarks>
        /// For Method Members, this stops on parens.</remarks>
        public string ShortName => String.Concat(FullName.Replace($"{TypeName}.", String.Empty)
                                                         .TakeWhile(s => s != '('));


        /// <summary>
        /// Gets the Member's Summary text.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        /// <remarks>
        /// For Method Members, this stops on parens.
        /// </remarks>
        public string Summary => FormatText(_element.TagsOfType(Tag.Summary)
                                                    .FirstOrDefault()?
                                                    .Value);

        /// <summary>
        /// Formats the text.
        /// </summary>
        /// <param name="input">The input value.</param>
        /// <returns>Formatted text</returns>
        public static string FormatText(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.Replace("\r\n", String.Empty)
                        .Replace("\n", String.Empty)
                        .Replace("\t", String.Empty)
                        .Trim();
        }

        protected string CreateTableRow(XElement subElement)
        {
            return subElement.Attributes().Any(a => a.Name == "cref")
                ? $"| {subElement.GetReferenceLink(AssemblyName, TypeName)} | {subElement.Value.FormatText()} |{Environment.NewLine}"
                : $"| {subElement.Attribute("name").Value} | {subElement.Value.FormatText()} |{Environment.NewLine}";
        }
    }
}
