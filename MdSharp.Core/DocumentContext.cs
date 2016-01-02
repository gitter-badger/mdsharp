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

        private readonly string _xmlDocumentPath;
        private readonly string _toolsPath;

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        /// <exception cref="System.IO.FileNotFoundException">Could not find template file.</exception>
        private string Template
        {
            get
            {
                var fileName = Path.Combine(_toolsPath, "Templates", "Type.cshtml");
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
        /// Initializes a new instance of the <see cref="DocumentContext" /> class.
        /// </summary>
        /// <param name="fileName">Name of the XML Document.</param>
        /// <param name="toolsPath">The Nuget tools path. Used to retrieve template</param>
        /// <exception cref="System.ArgumentException"></exception>
        public DocumentContext(string fileName, string toolsPath)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(nameof(fileName));

            _fileName = fileName;
            _toolsPath = toolsPath;
            _xmlDocumentPath = Path.GetDirectoryName(_fileName);
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
                    var result = RenderDocument(razorService, razorTemplate, typeMembers);

                    CreateDocument(typeMembers.First()?.AssemblyName ?? "UnknownAssembly",
                        typeMembers.Key,
                        result);
                }
            }
        }

        private static string RenderDocument(IRazorEngineService razorService, string razorTemplate, IGrouping<string, IMember> typeMembers)
        {
            var result = razorService.RunCompile(razorTemplate, "type", typeof (DocumentModel), new DocumentModel
            {
                TypeName = typeMembers.Key,
                Members = typeMembers
            });
            return result;
        }

        /// <summary>
        /// Creates the Markdown document.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="markdownContent">Content of the markdown.</param>
        private void CreateDocument(string assembly, string typeName, string markdownContent)
        {
            string targetPath = Path.Combine(_xmlDocumentPath, "..", "..", "doc", assembly);
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            File.WriteAllText(Path.Combine(targetPath, $"{typeName}.md"), markdownContent);
        }
    }
}
