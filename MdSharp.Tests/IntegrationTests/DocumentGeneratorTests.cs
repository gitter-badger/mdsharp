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
            // HACK: We shouldn't depend on Debug being built here.
            //string fileName = @"../../../MdSharp.Core/bin/Debug/MdSharp.Core.xml";
            //var documentGenerator = new DocumentGenerator();
            //documentGenerator.CreateDocuments(fileName);
        }
    }
}
