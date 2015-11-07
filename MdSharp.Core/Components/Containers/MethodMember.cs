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
                return FullName.Substring(indexOfParams, FullName.Length - indexOfParams);
            }
        }

        private int indexOfParams => FullName.IndexOf("(", StringComparison.Ordinal);
        private string Parameters
        {
            get
            {
                if (_element.TagsOfType(Tag.Param).Any() || _element.TagsOfType(Tag.ParamRef).Any())
                {
                    return "##### Parameters #####" + Environment.NewLine +
                           "| Name | Description |" + Environment.NewLine +
                           "| ---- | ----------- |" + Environment.NewLine+
                           //_element.TagsOfType(Tag.ParamRef).Select(CreateTableRow).Aggregate((a, b) => a + b) +
                           _element.TagsOfType(Tag.Param).Select(CreateTableRow).Aggregate((a, b) => a + b);
                }
                return String.Empty;
            }
        }

        public string Display()
        {
            return Title + Environment.NewLine +
                   (indexOfParams == -1 ? String.Empty : (SubTitle + Environment.NewLine))+
                   Environment.NewLine +
                   Summary + Environment.NewLine +
                   Parameters + Environment.NewLine;
        }
    }
}
