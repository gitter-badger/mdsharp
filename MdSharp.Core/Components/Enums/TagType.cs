using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdSharp.Core.Components
{
    /// <summary>
    /// Tags
    /// </summary>
    public enum Tag
    {
        //<c>
        C,
        //<para>
        Para,
        //<see>
        See,
        //<code>
        Code,
        //<param>
        Param,
        /// <summary>
        /// The seealso tag
        /// </summary>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/xhd7ehkk.aspx" />
        SeeAlso,
        //<example>
        Example,
        //<paramref>
        ParamRef,
        //<summary>
        Summary,
        //<exception>
        Exception,
        //<permission>
        Permission,
        //<typeparam>
        TypeParam,
        //<include>
        Include,
        //<remarks>
        Remarks,
        //<typeparamref>
        TypeParamRef,
        //<list>
        List,
        //<returns>
        Returns,
        //<value>
        Value
    }
}
