using System;
using System.IO;
using MdSharp.Core;
using Xunit;

namespace MdSharp.Tests.IntegrationTests
{ 
    public class DocumentContextTests
    {
        [Fact]
        public void Test_Run()
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "MdSharp.Core.xml");
            var documentContext = new DocumentContext(fileName, Path.GetDirectoryName(fileName));
            documentContext.CreateMarkdown();
        }
    }
}
