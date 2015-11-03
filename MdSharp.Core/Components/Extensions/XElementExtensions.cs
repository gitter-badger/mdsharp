using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    public static class XElementExtensions
    {
        public static bool IsOfMemberType(this XElement element, MemberType memberType)
        {
            return element.FirstAttribute?.Value
                .StartsWith($"{memberType.ToString().First()}:")
                   ?? false;
        }

        public static List<XElement> MembersOfType(this XElement element, MemberType memberType)
        {
            return element.Elements().Where(e => e.FirstAttribute
                .Value
                .StartsWith($"{memberType.ToString().First()}:"))
                .ToList();
        }

        public static string MemberTypeTitle(this XElement element)
        {
            var member = Enum.GetNames(typeof(MemberType)).ToList()
                .First(m => m.StartsWith(element
                    .FirstAttribute
                    .Value
                    .First()
                    .ToString()));
            return member;
        }

        public static List<XElement> TagsOfType(this XElement element, Tag tag)
        {
            return element.Elements(tag.ToString().ToLower()).ToList();
        }

        public static string CreateTableRow(this XElement element, string assembly, string type)
        {
            return element.Attributes().Any(a => a.Name == "cref")
                ? $"| {element.GetReferenceLink(assembly, type)} | {element.Value.FormatText()} |"
                : $"| {element.Attribute("name").Value} | {element.Value.FormatText()} |";
        }
        public static string GetName(this XElement element)
        {
            return element.Attribute("name").Value.Remove(0, 2);
        }
        public static string GetMemberName(this XElement element, string assembly, string type)
        {
            return element.GetName().GetShortName(assembly, type);
        }
        public static string GetShortName(this string value, string assembly, string type)
        {
            return value.Replace($"{type}.", String.Empty)
                .Replace($"{assembly}.", String.Empty)
                .Replace("``1", "&lt;T&gt;")
                .Replace("``0", "&lt;T&gt;")
                .Replace("{", "&lt;")
                .Replace("}", "&gt;")
                .Replace(":!", String.Empty)
                .Replace("#ctor", "Constructor")
                .Replace("System.", String.Empty);
        }

        public static string GetTrimmedName(this string name)
        {
            return name.TakeWhile(c => c != '(').ToString();
        }

        public static bool IsOfTag(this XElement element, Tag tag)
        {
            return element.Name.LocalName.Equals(tag.ToString(),
                StringComparison.OrdinalIgnoreCase);
        }

        private static string FormatMarkdownRef(this XElement element, string assembly)
        {
            string linkedType = element.Attribute("cref")
                                        .Value
                                        .Replace("!:", "../");
            return ($"{linkedType}.md");
        }

        public static string GetReferenceLink(this XElement element, string assembly, string type)
        {
            string value = element.Attribute("cref").Value.FormatText();

            if (value.StartsWith("!:http://"))
            {
                string formattedUrl = value.Substring(2, value.Length - 2);
                return $"[{formattedUrl}]({formattedUrl})";
            }
            else if (value.StartsWith("System."))
                return $"[{element.Attribute("cref").Value.FormatText()}]({value.FormatText()})";
            else
                return $"[{value.FormatText()}]({FormatMarkdownRef(element, assembly)})";
            // ? $"[{this.GetReferenceName(useShortName).Escape()}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:{this.MsdnName} '{this.StrippedName}')"
        }
    }

}
