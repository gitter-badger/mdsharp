using System;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Common interface for Member types.
    /// </summary>
    public interface IMember
    {
        string TypeName { get; }
        /// <summary>
        /// Gets the title of the Member
        /// </summary>
        /// <value>
        /// The title of the Member
        /// </value>
        string Title { get; }
        /// <summary>
        /// Displays this instance.
        /// </summary>
        /// <returns></returns>
        string Display();
    }
}
