using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdSharp.Core.Components
{
    public class DocumentModel
    {
        public string TypeName { get; set; }
        public IGrouping<string,IMember> Members { get; set; }
    }
}
