using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    public sealed class EventMember : MemberBase, IMember
    {
        public EventMember(XElement element) : base(element) { }

        /// <summary>
        /// Gets the Title for the Member.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => $"### {ShortName} - `Event`";

        public string Display()
        {
            return Title;
        }
    }
}
