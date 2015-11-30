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
        private static readonly string ModulePath = Path.Combine(OutputPath, "MdSharp.ps1");
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NugetPowershellTests"/> class.
        /// </summary>
        public NugetPowershellTests()
        {
            CreateRunspace();
            ImportModule(ModulePath);
            ImportModule(FakeCmdletsPath);
            var coreDllPath = Path.Combine(OutputPath, "MdSharp.Core.dll");
            ExecutePowershell($"Add-Type -Path {coreDllPath}");
        }

        [Fact]
        public void Test_GetMarkdown_Writes_Outputs()
        {
            var pipeline = Runspace.CreatePipeline();

            var getMarkdown = new Command("Get-Markdown");
            getMarkdown.Parameters.Add("assemblyName", "MdSharp.Core");
            pipeline.Commands.Add(getMarkdown);

            var result = pipeline.Invoke();
            Assert.Equal("MdSharp - Fetching XML for MdSharp.Core", result[0].ToString());
            Assert.StartsWith("MdSharp - Loading Document Context", result[1].ToString());
            Assert.Equal("MdSharp - Generating markdown", result[2].ToString());

            Assert.True(pipeline.Error.Count == 0);
        }

        public void Dispose() => CloseRunspace();
    }
}
