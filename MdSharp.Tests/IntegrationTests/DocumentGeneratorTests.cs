using System;
using MdSharp.Core;
using Xunit;

namespace MdSharp.Tests.IntegrationTests
{ 
    public class DocumentGeneratorTests
    {
        [Fact]
        public void Test_Run()
        {
            string fileName = @"../../../MdSharp.Core/bin/Debug/MdSharp.Core.xml";
            var documentGenerator = new DocumentGenerator();
            documentGenerator.CreateDocuments(fileName);
        }
    }
}
