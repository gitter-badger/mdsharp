using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    public sealed class PropertyMember : MemberBase, IMember
    {
        public PropertyMember(XElement element) : base(element) { }

        public string Title => $"### {ShortName} - `Property`";

        public string Display()
        {
            throw new NotImplementedException();
        }
    }
}
