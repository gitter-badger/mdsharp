using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using MdSharp.Core.Components;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

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
            var config = new TemplateServiceConfiguration
            {
                BaseTemplateType = typeof (MarkdownTemplateBase<>)
            };
            string template = getTemplate();
            // You can use the @inherits directive instead (this is the fallback if no @inherits is found).
            var types = members.GroupBy(m => m.TypeName);
            using (var razorService = RazorEngineService.Create(config))
            {
                foreach (var typeMembers in types)
                {
                    var result = razorService.RunCompile(template, "type", typeof(DocumentModel), new DocumentModel
                        {
                            TypeName = typeMembers.Key,
                            Members = typeMembers
                        });

                    CreateDocument((typeMembers.First() as MemberBase)?.AssemblyName ?? "UnknownAssembly",
                        typeMembers.Key,
                        result);
                }
            }
        }

        private string getTemplate()
        {
            return File.ReadAllText(@"Templates\Type.cshtml");
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
