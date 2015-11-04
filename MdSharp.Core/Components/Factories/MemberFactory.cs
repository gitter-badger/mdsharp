using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MdSharp.Core.Components;

namespace MdSharp.Core.Components
{
    public class MemberFactory
    {
        public IMember GetMember(XElement element)
        {
            if(element.IsOfMemberType(MemberType.Method))
                return new MethodMember(element);
            else if(element.IsOfMemberType(MemberType.Property))
                return new PropertyMember(element);
            else if(element.IsOfMemberType(MemberType.Field))
                return new FieldMember(element);
            else if(element.IsOfMemberType(MemberType.Event))
                return new EventMember(element);

            throw new ArgumentException($"{nameof(element)} is not a valid Member", nameof(element));
        }
    }
}
