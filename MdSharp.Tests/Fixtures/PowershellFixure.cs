using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace MdSharp.Tests.Fixtures
{
    //Credit to Brian Hartstock
    public abstract class NugetPowershellFixture
    {
        private static readonly string nugetModulesPath =
            @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\tj1plvkw.npa\Modules\NuGet";
        private readonly string profileScript = $"& \"{Path.Combine(nugetModulesPath, "Profile")}\"";
        private readonly string modulesScript =
            Path.Combine(nugetModulesPath, "NuGet.psd1");
        private readonly string nugetScript =
            Path.Combine(nugetModulesPath, "nuget");
        private readonly string wrapperMembersScript =
            Path.Combine(nugetModulesPath, "Add-WrapperMembers");

        protected Runspace Runspace { get; set; }

        private RunspaceConfiguration CreateConfiguration() => RunspaceConfiguration.Create();


        public void ImportModule(string module)
        {
            var runspace = new RunspaceInvoke(Runspace);
            runspace.Invoke($"Import-Module \"{module}\"");
        }

        public void CreateRunspace()
        {
            Runspace = RunspaceFactory.CreateRunspace(CreateConfiguration());
            Runspace.Open();
            var securityPolicy = new RunspaceInvoke(Runspace);
            securityPolicy.Invoke("Set-ExecutionPolicy RemoteSigned");
        }

        public void CloseRunspace()
        {
            Runspace.Close();
            Runspace.Dispose();
        }

        protected Collection<PSObject> ExecutePowershell(string script)
        {
            return Runspace.CreatePipeline(script).Invoke();
        }
    }
}
