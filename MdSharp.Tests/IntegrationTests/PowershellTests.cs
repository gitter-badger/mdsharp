using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation.Runspaces;
using MdSharp.Tests.Fixtures;
using Xunit;

namespace MdSharp.Tests.IntegrationTests
{
    public class NugetPowershellTests : NugetPowershellFixture, IDisposable
    {
        private readonly string _binPath = @"C:\Users\BenMeadors\Source\Github\mdsharp\MdSharp.Core\bin\Debug\";
        private readonly string _fakeCmdletsPath = @"C:\Users\BenMeadors\Source\Github\mdsharp\MdSharp.Tests\bin\Debug\Fixtures\FakeCommandlets.ps1";
        private readonly string _modulePath = @"C:\Users\BenMeadors\Source\Github\mdsharp\MdSharp.Powershell\bin\Debug\MdSharp.ps1";
        /// <summary>
        /// Initializes a new instance of the <see cref="NugetPowershellTests"/> class.
        /// </summary>
        public NugetPowershellTests()
        {
            CreateRunspace();
            ImportModule(_modulePath);
            ImportModule(_fakeCmdletsPath);
            ExecutePowershell($"Add-Type -Path {_binPath}MdSharp.Core.dll");
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
            Assert.Equal(result[1].ToString(), @"MdSharp - Loading Document Context for C:\Users\BenMeadors\Source\Github\mdsharp\bin\Debug\MdSharp.Core.xml");
            Assert.Equal(result[2].ToString(), "MdSharp - Generating markdown");
        }
        public void Dispose() => CloseRunspace();
    }
}
