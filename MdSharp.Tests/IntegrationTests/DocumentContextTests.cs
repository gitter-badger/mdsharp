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
            // HACK: We shouldn't depend on Debug being built here.
            string fileName = @"../../../MdSharp.Core/bin/Debug/MdSharp.Core.xml";
            var documentContext = new DocumentContext(fileName, Path.GetDirectoryName(fileName));
            documentContext.CreateMarkdown();
        }
    }
}
