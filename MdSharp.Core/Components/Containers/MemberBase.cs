using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
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
        public string FullName => _element.GetObjectName();
        /// <summary>
        /// Gets the Member's Short Name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        /// <remarks>
        /// For Method Members, this stops on parens.</remarks>
        public string ShortName => String.Concat(FullName.Replace("#ctor", "Constructor")
                                                         .Replace($"{TypeName}.", String.Empty)
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
        public string Summary => formatNodes(_element.TagsOfType(Tag.Summary).FirstOrDefault());
        /// <summary>
        /// Formats the the nodes nested under the given <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private string formatNodes(XElement element)
        {
            if (element == null)
                return String.Empty;

            var stringBuilder = new StringBuilder();
            foreach (var node in element.Nodes())
            {
                var text = node as XText;
                if (node is XText)
                    stringBuilder.Append($"{text.Value.FormatText()} ");
                else if (node is XElement)
                {
                    var tag = node as XElement;
                    if (tag.IsOfTag(Tag.See))
                        stringBuilder.Append($"{GetLink(tag)} ");
                    if (tag.IsOfTag(Tag.ParamRef) || tag.IsOfTag(Tag.TypeParamRef))
                        stringBuilder.Append($"{tag.Attribute("name").Value} ");
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public string GetLink(XElement element)
        {
            string value = element.Attribute("cref").Value.Substring(2, element.Attribute("cref").Value.Length - 2);

            if (value.StartsWith("!:http://"))
            {
                return $"[{value}]({value})";
            }
            else if (value.StartsWith("System."))
                return $"[{element.Attribute("cref").Value.FormatText()}]({value.FormatText()})";
            else
            {
                if (value.StartsWith(TypeName))
                {
                    string headerLink = value.Replace($"{TypeName}.", String.Empty);
                    return $"[{headerLink}](#{headerLink.ToLower().Replace(".",String.Empty)})";
                }
                string fileLink = value.Split('.').Last();
                return $"[{fileLink}](../{value}.md)";
            }
        }
        /// <summary>
        /// Returns XElements of the given Tag type.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public IEnumerable<XElement> TagsOfType(Tag tag)
        {
            return _element.TagsOfType(tag);
        }
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<Tuple<string, string>> Parameters
        {
            get
            {
                var parameters = new List<Tuple<string, string>>();
                parameters.AddRange(_element.TagsOfType(Tag.ParamRef)
                                            .Select(e => new Tuple<string, string>(e.Attribute("name").Value,
                                                                                   e.Value.FormatText())));
                parameters.AddRange(_element.TagsOfType(Tag.Param)
                                            .Select(e => new Tuple<string, string>(e.Attribute("name").Value,
                                                                                   e.Value.FormatText())));
                return parameters;
            }
        }
        /// <summary>
        /// Gets the Exceptions.
        /// </summary>
        /// <value>
        /// The Exceptions.
        /// </value>
        public IEnumerable<Tuple<string, string>> Exceptions =>
            _element.TagsOfType(Tag.Exception)
                    .Select(e => new Tuple<string, string>(e.Attribute("cref").Value,
                                                           e.Value.FormatText()));
    }
}
