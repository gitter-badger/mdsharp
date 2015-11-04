using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// MemberBase types defined in Visual Studio documentation.
    /// </summary>
    /// <example>Represented like "M:MyType.MyMethod(args)"</example>
    public enum MemberType
    {
        /// <summary>
        /// Types
        /// </summary>
        Type,
        /// <summary>
        /// Properties
        /// </summary>
        Property,
        /// <summary>
        /// Methods
        /// </summary>
        Method,
        /// <summary>
        /// Fields
        /// </summary>
        Field,
        /// <summary>
        /// Events
        /// </summary>
        Event,
        /// <summary>
        /// Namespace
        /// </summary>
        /// <remarks>
        /// Only used in cref references. Should not used in element declarations.
        /// </remarks>
        Namespace
    }
}
