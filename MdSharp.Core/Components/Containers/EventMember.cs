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

        private string Parameters
        {
            get
            {
                if (_element.TagsOfType(Tag.Param).Any() || _element.TagsOfType(Tag.ParamRef).Any())
                {
                    return @"##### Parameters #####
                             | Name | Description | 
                             | ---- | ----------- |" + Environment.NewLine +
                           _element.TagsOfType(Tag.ParamRef).Select(CreateTableRow).Aggregate((a, b) => a + b) +
                           _element.TagsOfType(Tag.Param).Select(CreateTableRow).Aggregate((a, b) => a + b);
                }
                return String.Empty;
            }
        }

        public string Display()
        {
            return Title + Environment.NewLine +
                   SubTitle + Environment.NewLine +
                   Environment.NewLine +
                   Summary + Environment.NewLine +
                   Parameters + Environment.NewLine;
        }
    }
}
