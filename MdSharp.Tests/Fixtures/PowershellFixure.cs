using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace MdSharp.Tests.Fixtures
{
    //Credit to Brian Hartstock
    public abstract class NugetPowershellFixture
    {
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
