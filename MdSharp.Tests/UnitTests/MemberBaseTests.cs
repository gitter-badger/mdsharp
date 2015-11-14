using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using MdSharp.Core.Components;
using MdSharp.Tests.Fixtures;
using Moq;
using Xunit;

namespace MdSharp.Tests.UnitTests
{
    public class MemberBaseTests : TestFixtureBase
    {
        public readonly string MyMethod = "#ctor(System.String)";
        public readonly string MyMethodWithoutArgs = "Constructor";
        public MethodMember testMember;

        public MemberBaseTests()
        {
            var memberFactory = new MemberFactory();
            var elements = MembersOfType(GetFixture(), MemberType.Method);
            testMember = memberFactory.GetMember(elements.First()) as MethodMember;
        }

        [Fact]
        public void MemberBase_Returns_Correct_AssemblyName()
        {
            Assert.Equal(testMember.AssemblyName, AssemblyName);
        }

        [Fact]
        public void MemberBase_MemberMethod_Returns_Correct_TypeName_For_Element_Defined_Type()
        {
            Assert.Equal(testMember.TypeName, $"{Namespace}.{TypeName}");
        }

        [Fact]
        public void MemberBase_Returns_Correct_FullName()
        {
            Assert.Equal(testMember.FullName, $"{Namespace}.{TypeName}.{MyMethod}");
        }

        [Fact]
        public void MemberBase_Returns_Correct_ShortName()
        {
            Assert.Equal(testMember.ShortName, MyMethodWithoutArgs);
        }
    }
}
