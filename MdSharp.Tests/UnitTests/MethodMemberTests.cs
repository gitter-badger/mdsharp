using System;
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
    public class MethodMemberTests : TestFixtureBase
    {
        public readonly string Method = "#ctor(System.String)";
        public readonly string MethodWithoutArgs = "Constructor";
        public readonly string ParameterSignature = "(System.String)";
        public readonly string ParamName = "myParam";
        public readonly string ParamDescription = "My string parameter";
        public readonly string ExceptionName = "ArgumentNullException";
        public readonly string ExceptionDescription = "Thrown if myParam is null";

        private MethodMember testMember;

        public MethodMemberTests()
        {
            var memberFactory = new MemberFactory();
            var elements = MembersOfType(GetFixture(), MemberType.Method);
            testMember = memberFactory.GetMember(elements.First()) as MethodMember;
        }

        [Fact]
        public void MethodMember_Returns_Short_Method_Name_In_Title()
        {
            Assert.Contains(MethodWithoutArgs, testMember.Title);
            Assert.DoesNotContain(ParameterSignature, testMember.Title);
        }

        [Fact]
        public void MethodMember_Returns_Param_Signature_In_Subtitle()
        {
            Assert.Contains(ParameterSignature, testMember.SubTitle);
        }

        // These are actually in MemberBase 
        // but we want to test them specifically on MethodMember as well
        #region MemberBaseTests
        [Fact]
        public void MethodMember_Returns_Parameters_Name_And_Description()
        {
            var param = testMember.Parameters.First();
            Assert.Equal(param.Item1, ParamName);
            Assert.Equal(param.Item2, ParamDescription);
        }

        [Fact]
        public void MethodMember_Returns_Exception_Name_And_Description()
        {
            var param = testMember.Exceptions.First();
            Assert.Equal(param.Item1, ExceptionName);
            Assert.Equal(param.Item2, ExceptionDescription);
        } 
        #endregion
    }
}
