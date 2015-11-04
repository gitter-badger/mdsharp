using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Method type of Member
    /// </summary>
    public sealed class MethodMember : MemberBase, IMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodMember"/> class.
        /// </summary>
        /// <param name="element">The XElement we want to create a Member container from.</param>
        public MethodMember(XElement element) : base(element) { }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => $"### {ShortName} - `Method`";
        /// <summary>
        /// Gets or sets the Subtitle for the Method.
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
            return $"{Title}" + Environment.NewLine +
                   $"{SubTitle}" + Environment.NewLine + Environment.NewLine +
                   $"{Summary}" + Environment.NewLine;

        }

    }
}
