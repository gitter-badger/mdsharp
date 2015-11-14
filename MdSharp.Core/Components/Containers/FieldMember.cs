using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    public sealed class FieldMember : MemberBase, IMember
    {
        public FieldMember(XElement element) : base(element) { }

        public string Title => $"### {ShortName} - `Field`";

        /// <summary>
        /// Gets the template for this member type
        /// </summary>
        /// <returns>Razor template for this member type</returns>
        public string Template => String.Empty;
    }
}
