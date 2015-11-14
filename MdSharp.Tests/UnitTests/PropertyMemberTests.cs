using System;
using System.Linq;
using MdSharp.Core.Components;
using MdSharp.Tests.Fixtures;
using Xunit;

namespace MdSharp.Tests.UnitTests
{
    public class PropertyMemberTests : TestFixtureBase
    {
        public static class Property
        {
            public static readonly string PropertyName = "MyFakeProperty";
            public static readonly string ValueDescription = "The title of the Member";                   
        }
        private PropertyMember propertyMember;
        private PropertyMember emptyValuePropertyMember;

        public PropertyMemberTests()
        {
            var memberFactory = new MemberFactory();
            var elements = MembersOfType(GetFixture(), MemberType.Property);
            propertyMember = memberFactory.GetMember(elements.First()) as PropertyMember;
            emptyValuePropertyMember = memberFactory.GetMember(elements.Last()) as PropertyMember;
        }

        [Fact]
        public void PropertyMember_Returns_Short_Property_Name_In_Title()
        {
            Assert.Contains(Property.PropertyName, propertyMember.Title);
        }

        [Fact]
        public void PropertyMember_Returns_Value_Description_In_Value()
        {
            Assert.Contains(Property.ValueDescription, propertyMember.Value);
        }

        [Fact]
        public void PropertyMember_Returns_Empty_String_When_No_Value_Tag_Is_Given()
        {
            Assert.Equal(String.Empty, emptyValuePropertyMember.Value);
        }
    }
}
