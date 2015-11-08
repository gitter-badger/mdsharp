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
        /// <summary>
        /// Initializes a new instance of the <see cref="EventMember"/> class.
        /// </summary>
        /// <param name="element">The XElement we want to create a Member container from.</param>
        public EventMember(XElement element) : base(element) { }

        /// <summary>
        /// Gets the Title for the Member.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => $"### {ShortName} - `Event`";

        /// <summary>
        /// Gets or sets the Subtitle for the Method. (signature)
        /// </summary>
        /// <value>
        /// The sub title.
        /// </value>
        public string SubTitle => $"###### `{parameterPrototype}`";

        /// <summary>
        /// Gets the parameter prototype.
        /// </summary>
        /// <value>
        /// The parameter prototype.
        /// </value>
        private string parameterPrototype
        {
            get
            {
                int startParens = FullName.IndexOf("(", StringComparison.Ordinal);
                return FullName.Substring(startParens, FullName.Length - startParens);
            }
        }

        public string Display()
        {
            return Title + Environment.NewLine +
                   SubTitle + Environment.NewLine +
                   Environment.NewLine +
                   Summary + Environment.NewLine;
        }
    }
}
