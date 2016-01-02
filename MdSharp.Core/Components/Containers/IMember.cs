using System;
using System.Reflection;
using System.Xml.Linq;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Common interface for Member types.
    /// </summary>
    public interface IMember
    {
        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        string AssemblyName { get; }
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
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
        string Template { get; }
    }
}
