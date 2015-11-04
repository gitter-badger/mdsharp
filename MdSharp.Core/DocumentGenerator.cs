using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MdSharp.Core.Components;

namespace MdSharp.Core
{
    public class DocumentGenerator
    {

        public void Read(string fileName)
        {
            var xdoc = XDocument.Load(fileName);
            var document = xdoc.Element("doc");
            string assembly = document.Element("assembly").Value;

            var elements = document.Element("members").Elements("member");
            var factory = new MemberFactory();
            var members = elements.Where(e => !e.IsOfMemberType(MemberType.Type))
                                  .Select(e => factory.GetMember(e));
            var typename = members.Select(e => (e as MemberBase).TypeName).ToList();
            var fullname = members.OfType<MethodMember>().First().FullName;
            var assemblyname = members.OfType<MethodMember>().First().AssemblyName;
            var allMembers = elements.Where(e => e.IsOfMemberType(MemberType.Event) ||
                                                 e.IsOfMemberType(MemberType.Field) ||
                                                 e.IsOfMemberType(MemberType.Method) ||
                                                 e.IsOfMemberType(MemberType.Property)
                                            );

            var types = elements.Where(e => e.IsOfMemberType(MemberType.Type));
   
            WriteDocument(types, allMembers, assembly);
        }

        private static void WriteDocument(IEnumerable<XElement> types, IEnumerable<XElement> allMembers, string assembly)
        {
            var membersUnderTypes = new List<XElement>();
            foreach (var typeElement in types)
            {
                string typeName = typeElement.GetName();
                var childMembers = allMembers.Where(m => m.GetName()
                    .StartsWith(typeName));
                membersUnderTypes.AddRange(childMembers);
                var markdown = new Markdown
                {
                    AssemblyNamespace = assembly,
                    Type = typeElement,
                    Members = childMembers.GroupBy(e => e.MemberTypeTitle())
                        .SelectMany(m => m)
                };
                var markdownContent = markdown.TransformText();
                CreateDocument(assembly, typeName, markdownContent);
            }
            var unTypedMembers = allMembers.Where(m => !membersUnderTypes.Contains(m));

        }


        private static void CreateDocument(string assembly, string typeName, string markdownContent)
        {
            string path = $@"..\..\doc\{assembly}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText($@"{path}\{typeName}.md", markdownContent);
        }
    }
}
