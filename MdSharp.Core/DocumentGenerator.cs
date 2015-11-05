using System;
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
   
            WriteDocuments(members, assembly);
        }

        private void WriteDocuments(IEnumerable<IMember> members, string assembly)
        {
            var types = members.GroupBy(m => m.TypeName);

            foreach (var type in types)
            {
                string markdownContent = type.Aggregate(String.Empty, (current, member) => current + member.Display());
                CreateDocument(assembly, type.Key, markdownContent);
            }
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
