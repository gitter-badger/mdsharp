using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Property  
    /// </summary>
    public sealed class PropertyMember : MemberBase, IMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMember"/> class.
        /// </summary>
        /// <param name="element">The XElement we want to create a Member container from.</param>
        public PropertyMember(XElement element) : base(element) { }

        /// <summary>
        /// Title of the member
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => $"### {ShortName} - `Property`";

        /// <summary>
        /// Gets the Value description
        /// </summary>
        /// <value>
        /// The value description
        /// </value>
        public string Value => String.IsNullOrWhiteSpace(value) ?
                                    String.Empty :
                                    $"{Environment.NewLine}**Returns**{Environment.NewLine}{Environment.NewLine}{value.FormatText()}";
        private string value => _element.TagsOfType(Tag.Value).FirstOrDefault()?.Value;

        /// <summary>
        /// Gets the template for this member type
        /// </summary>
        /// <returns>Razor template for this member type</returns>
        public string Template => String.Empty;
    }
}
