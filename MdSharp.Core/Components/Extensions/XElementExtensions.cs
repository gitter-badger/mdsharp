using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Extensions for XElement
    /// </summary>
    public static class XElementExtensions
    {
        public static bool IsOfMemberType(this XElement element, MemberType memberType)
        {
            return element.FirstAttribute?.Value
                .StartsWith($"{memberType.ToString().First()}:")
                   ?? false;
        }

        /// <summary>
        /// Gets sub-elements of the given Tag type
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static List<XElement> TagsOfType(this XElement element, Tag tag)
        {
            return element.Elements(tag.ToString().ToLower()).ToList();
        }
        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static string GetLink(this XElement element, string typeName)
        {
            if (element == null)
                return null;

            string value = element.Attribute("cref").Value.Substring(2, element.Attribute("cref").Value.Length - 2);

            if (value.StartsWith("http"))
            {
                return $"[{value}]({value})";
            }
            else if (value.StartsWith("System."))
                return $"[{value}]({value})";
            else
            {
                if (value.StartsWith(typeName))
                {
                    string headerLink = value.Replace($"{typeName}.", String.Empty);
                    return $"[{headerLink}](#{headerLink.ToLower().Replace(".", String.Empty)})";
                }
                string fileLink = value.Split('.').Last();
                return $"[{fileLink}](../{value}.md)";
            }
        }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static string GetObjectName(this XElement element)
        {
            return element.Attribute("name").Value.Remove(0, 2);
        }
        public static bool IsOfTag(this XElement element, Tag tag)
        {
            return element.Name.LocalName.Equals(tag.ToString(),
                StringComparison.OrdinalIgnoreCase);
        }
    }

}
