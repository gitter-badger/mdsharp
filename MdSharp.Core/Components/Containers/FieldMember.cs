using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Field type of Member
    /// </summary>
    public sealed class FieldMember : MemberBase, IMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMember"/> class.
        /// </summary>
        /// <param name="element">The XElement we want to create a Member container from.</param>
        public FieldMember(XElement element) : base(element) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => $"### {ShortName} - `Field`";

        /// <summary>
        /// Displays this instance.
        /// </summary>
        /// <returns></returns>
        public string Display()
        {
            return Title + Environment.NewLine +
                   Summary + Environment.NewLine;
        }
    }
}
