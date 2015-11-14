﻿using System;
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
        /// Value of the property
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value => _element.TagsOfType(Tag.Value)
                                        .FirstOrDefault()?
                                        .Value.FormatText();

        /// <summary>
        /// Gets the template for this member type
        /// </summary>
        /// <returns>Razor template for this member type</returns>
        public string Template => String.Empty;
    }
}
