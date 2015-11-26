using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MdSharp.Core.Components;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace MdSharp.Core
{
    /// <summary>
    /// Context for methods reading XML documents and creating markdown
    /// </summary>
    public class DocumentContext
    {
        /// <summary>
        /// The XML Document path
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        /// <exception cref="System.IO.FileNotFoundException">Could not find template file.</exception>
        private static string Template
        {
            get
            {
                var fileName = Path.Combine("Templates", "Type.cshtml");
                if (File.Exists(fileName))
                    return File.ReadAllText(fileName);

                throw new FileNotFoundException("Could not find template file.");
            }
        }

        private TemplateServiceConfiguration TemplateConfig => new TemplateServiceConfiguration
            {
                BaseTemplateType = typeof(MarkdownTemplateBase<>)
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentContext"/> class.
        /// </summary>
        /// <param name="fileName">Name of the XML Document.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public DocumentContext(string fileName)
        {         
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(nameof(fileName));

            _fileName = fileName;
        }

        /// <summary>
        /// Creates the Markdown documentation based on the XML documentation.
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        public void CreateMarkdown()
        {
            var elements = GetXDocElements(_fileName);
            var members = GetMembers(elements);

            CreateDocuments(members);
        }

        /// <summary>
        /// Gets the x document elements.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static IEnumerable<XElement> GetXDocElements(string fileName)
        {
            var xdoc = XDocument.Load(fileName);
            var document = xdoc.Element("doc");
            return document.Element("members").Elements("member");
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        private static IEnumerable<IMember> GetMembers(IEnumerable<XElement> elements)
        {
            var factory = new MemberFactory();
            var members = elements.Where(e => !e.IsOfMemberType(MemberType.Type))
                .Select(e => factory.GetMember(e));
            return members;
        }

        /// <summary>
        /// Creates the documents.
        /// </summary>
        /// <param name="members">The members.</param>
        private void CreateDocuments(IEnumerable<IMember> members)
        {
            string razorTemplate = Template;
            var types = members.GroupBy(m => m.TypeName);
            using (var razorService = RazorEngineService.Create(TemplateConfig))
            {
                foreach (var typeMembers in types)
                {
                    var result = razorService.RunCompile(razorTemplate, "type", typeof(DocumentModel), new DocumentModel
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

        private static void CreateDocument(string assembly, string typeName, string markdownContent)
        {
            string path = Path.Combine("..", "..", "doc", assembly);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(Path.Combine(path, $"{typeName}.md"), markdownContent);
        }
    }
}
