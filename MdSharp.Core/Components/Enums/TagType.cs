using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdSharp.Core.Components
{
    public enum Tag
    {
        //<c>
        C,
        //<para>
        Para,
        //<see>*
        See,
        //<code>
        Code,
        //<param>*
        Param,
        //<seealso>*
        SeeAlso,
        //<example>
        Example,
        //<paramref>
        ParamRef,
        //<summary>
        Summary,
        //<exception>*
        Exception,
        //<permission>*
        Permission,
        //<typeparam>*
        TypeParam,
        //<include>*
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
