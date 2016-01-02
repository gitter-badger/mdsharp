using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Reflection;
using MdSharp.Tests.Fixtures;
using Xunit;

namespace MdSharp.Tests.IntegrationTests
{
    public class NugetPowershellTests : NugetPowershellFixture, IDisposable
    {
        private static readonly string OutputPath = Directory.GetCurrentDirectory();
        private static readonly string FakeCmdletsPath = Path.Combine(OutputPath, "Fixtures", "FakeCommandlets.ps1");
        private static readonly string ModulePath = Path.Combine(OutputPath, "MdSharp.psm1");
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NugetPowershellTests"/> class.
        /// </summary>
        public NugetPowershellTests()
        {
            CreateRunspace();
            string coreDllPath = Path.Combine(OutputPath, "MdSharp.Core.dll");
            ExecutePowershell($"Add-Type -Path {coreDllPath}");
            ImportModule(FakeCmdletsPath);
            ImportModule(ModulePath);      
        }

        [Fact]
        public void Test_GetMarkdown_Writes_Outputs()
        {
#if DEBUG

            var pipeline = Runspace.CreatePipeline();

            var getMarkdown = new Command("Get-Markdown");
            getMarkdown.Parameters.Add("assemblyName", "MdSharp.Core");
            pipeline.Commands.Add(getMarkdown);

            var result = pipeline.Invoke();
            Assert.Equal("MdSharp - Fetching XML for MdSharp.Core", result[0].ToString());
            Assert.StartsWith("MdSharp - Loading Document Context", result[1].ToString());
            Assert.Equal("MdSharp - Generating markdown", result[2].ToString());
            Assert.Equal("MdSharp - Markdown generated!", result[3].ToString());

            //Check for Write-Error in the pipeline
            Assert.True(pipeline.Error.Count == 0);
#endif
        }

        public void Dispose() => CloseRunspace();
    }
}
