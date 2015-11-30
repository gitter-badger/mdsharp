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
        private static readonly string _binPath = Directory.GetCurrentDirectory();
        private static readonly string _projectDirectory = Path.Combine(_binPath, "MdSharp.Tests/bin/Debug");
        private readonly string _fakeCmdletsPath = Path.Combine(_binPath, @"Fixtures/FakeCommandlets.ps1");
        private readonly string _modulePath = Path.Combine(_projectDirectory, @"MdSharp.Powershell/bin/Debug/MdSharp.ps1");
        private readonly string _coreDocumentationPath = Path.Combine(_projectDirectory, @"MdSharp.Core/bin/Debug/MdSharp.Core.xml");
        /// <summary>
        /// Initializes a new instance of the <see cref="NugetPowershellTests"/> class.
        /// </summary>
        public NugetPowershellTests()
        {
            CreateRunspace();
            ImportModule(_modulePath);
            ImportModule(_fakeCmdletsPath);
            var coreDllPath = Path.Combine(_binPath, "MdSharp.Core.dll");
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
            Assert.Equal(result[0].ToString(), "MdSharp - Fetching XML for MdSharp.Core");
            Assert.Equal(result[1].ToString(), $"MdSharp - Loading Document Context for {_coreDocumentationPath}");
            Assert.Equal(result[2].ToString(), "MdSharp - Generating markdown");
        }

        public void Dispose() => CloseRunspace();
    }
}
