using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core
{
    public partial class Markdown : MarkdownBase
    {
        public XElement Type { get; set; }
        public IEnumerable<XElement> Members { get; set; }
        public string AssemblyNamespace { get; set; }
        public string Namespace { get; set; }
    }
}
