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
        /// Gets or sets the Subtitle for the Method. (signature)
        /// </summary>
        /// <value>
        /// The sub title.
        /// </value>
        public string SubTitle => indexOfParams == -1 ? 
                                    String.Empty : 
                                    $"###### `{parameterSignature}` {Environment.NewLine}";

        private string parameterSignature => 
            FullName.Substring(indexOfParams, FullName.Length - indexOfParams).Replace("{", "<").Replace("}", ">");

        private int indexOfParams => FullName.IndexOf("(", StringComparison.Ordinal);


        public string Display()
        {
            return Title + Environment.NewLine +
                   (indexOfParams == -1 ? String.Empty : (SubTitle + Environment.NewLine))+
                   Environment.NewLine +
                   Summary + Environment.NewLine;
        }
    }
}
