using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MdSharp.Core;

namespace MdSharp.Powershell
{
    [Cmdlet(VerbsCommon.Get, "Markdown")]
    public class GetMarkdown : Cmdlet
    {
        [Parameter(Mandatory = true,
          Position = 0,
          HelpMessage = "Specify the name of the assembly to generate markdown from")]
        [Alias("n", "name")]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            var inputFile = $"{Name}.xml";

            var context = new DocumentContext(inputFile);
            context.CreateMarkdown();
        }
    }
}
