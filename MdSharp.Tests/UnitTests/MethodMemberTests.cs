using System;
using System.Linq;
using MdSharp.Core.Components;
using MdSharp.Tests.Fixtures;
using Xunit;

namespace MdSharp.Tests.UnitTests
{
    public class MethodMemberTests : TestFixtureBase
    {
        public static class ConstructorMethod
        {
            public static readonly string Method = "#ctor(System.String)";
            public static readonly string MethodWithoutArgs = "Constructor";
            public static readonly string ParameterSignature = "(System.String)";
                   
            public static readonly string ParamName = "myParam";
            public static readonly string ParamDescription = "My string parameter";
                   
            public static readonly string ExceptionName = "ArgumentNullException";
            public static readonly string ExceptionDescription = "Thrown if myParam is null";
        }

        public static class ValueMethod
        {
            public static readonly string Method = "MyFakeMethod";
            public static readonly string MethodWithoutArgs = "MyFakeMethod";
            public static readonly string ParameterSignature = "(System.String)";

            public static readonly string ReturnsValue = "Returns a value";
        }

        private MethodMember constructorMethod;
        private MethodMember valueMethod;

        public MethodMemberTests()
        {
            var memberFactory = new MemberFactory();
            var elements = MembersOfType(GetFixture(), MemberType.Method);
            constructorMethod = memberFactory.GetMember(elements.First()) as MethodMember;
            valueMethod = memberFactory.GetMember(elements.Last()) as MethodMember;
        }

        [Fact]
        public void MethodMember_Returns_Short_Method_Name_In_Title()
        {
            Assert.Contains(ConstructorMethod.MethodWithoutArgs, constructorMethod.Title);
            Assert.DoesNotContain(ConstructorMethod.ParameterSignature, constructorMethod.Title);
        }

        [Fact]
        public void MethodMember_Returns_Param_Signature_In_Subtitle()
        {
            Assert.Contains(ConstructorMethod.ParameterSignature, constructorMethod.SubTitle);
        }

        [Fact]
        public void MethodMember_Returns_Empty_String_When_No_Return_Tag_Is_Given()
        {
            Assert.Equal(String.Empty, constructorMethod.Returns);
        }
        
        [Fact]
        public void MethodMember_Returns_Value_When_Return_Tag_Is_Given()
        {
            Assert.Contains(ValueMethod.ReturnsValue, valueMethod.Returns);
        }

        // These are actually in MemberBase 
        // but we want to test them specifically on MethodMember as well
        #region MemberBaseTests
        [Fact]
        public void MethodMember_Returns_Parameters_Name_And_Description()
        {
            var param = constructorMethod.Parameters.First();
            Assert.Equal(param.Item1, ConstructorMethod.ParamName);
            Assert.Equal(param.Item2, ConstructorMethod.ParamDescription);
        }

        [Fact]
        public void MethodMember_Returns_Exception_Name_And_Description()
        {
            var param = constructorMethod.Exceptions.First();
            Assert.Equal(param.Item1, ConstructorMethod.ExceptionName);
            Assert.Equal(param.Item2, ConstructorMethod.ExceptionDescription);
        } 
        #endregion
    }
}
