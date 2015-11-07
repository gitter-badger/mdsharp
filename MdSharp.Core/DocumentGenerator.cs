using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using MdSharp.Core.Components;

namespace MdSharp.Core
{
    public class DocumentGenerator
    {
        public void CreateDocuments(string fileName)
        {
            var members = getMembers(fileName);

            WriteDocuments(members);
        }

        private IEnumerable<IMember> getMembers(string fileName)
        {
            var xdoc = XDocument.Load(fileName);
            var document = xdoc.Element("doc");

            var elements = document.Element("members").Elements("member");
            var factory = new MemberFactory();
            var members = elements.Where(e => !e.IsOfMemberType(MemberType.Type))
                .Select(e => factory.GetMember(e));
            return members;
        }

        private void WriteDocuments(IEnumerable<IMember> members)
        {
            var types = members.GroupBy(m => m.TypeName);

            foreach (var typeMembers in types)
            {
                string markdownContent = $"## {typeMembers.Key}{Environment.NewLine}";
                markdownContent += "---" + Environment.NewLine;
                markdownContent += typeMembers.Aggregate(String.Empty, (current, member) => current + member.Display());
                CreateDocument((typeMembers.First() as MemberBase)?.AssemblyName ?? "UnknownAssembly", 
                                typeMembers.Key, 
                                markdownContent);
            }
        }

        private static void CreateDocument(string assembly, string typeName, string markdownContent)
        {
            string path = $"..\\..\\doc\\{assembly}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText($"{path}\\{typeName}.md", markdownContent);
        }
    }
}
