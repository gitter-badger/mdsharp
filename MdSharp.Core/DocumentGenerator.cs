using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MdSharp.Core.Components;

namespace MdSharp.Core
{
    public class DocumentGenerator
    {

        private static void Read(string[] args)
        {
            var xdoc = XDocument.Load(args[0]);
            var document = xdoc.Element("doc");
            var assembly = document.Element("assembly");

            var elements = document.Element("members").Elements("member");
            var allMembers = elements.Where(e => e.IsOfMemberType(MemberType.Event) ||
                                                 e.IsOfMemberType(MemberType.Field) ||
                                                 e.IsOfMemberType(MemberType.Method) ||
                                                 e.IsOfMemberType(MemberType.Property)
                                            );

            var types = elements.Where(e => e.IsOfMemberType(MemberType.Type));

            foreach (var typeElement in types)
            {
                string typeName = typeElement.GetName();
                var childMembers = allMembers.Where(m => m.GetName()
                    .StartsWith(typeName));

                var markdown = new Markdown
                {
                    AssemblyNamespace = assembly.Value,
                    Type = typeElement,
                    Members = childMembers.GroupBy(e => e.MemberTypeTitle())
                                          .SelectMany(m => m)
                };
                var markdownContent = markdown.TransformText();
                WriteMarkdown(assembly.Value, typeName, markdownContent);
            }
        }


        private static void WriteMarkdown(string assembly, string typeName, string markdownContent)
        {
            string path = $@"..\..\doc\{assembly}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText($@"{path}\{typeName}.md", markdownContent);
        }
    }
}
