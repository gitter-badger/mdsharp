using System;
﻿using System.Linq;
using System.Text.RegularExpressions;
using MdSharp.Core.Components;
using MdSharp.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace MdSharp.Tests.UnitTests
{
    public class MemberBaseTests : TestFixtureBase
    {
        public readonly string MyMethod = "#ctor(System.String)";
        public readonly string MyMethodWithoutArgs = "Constructor";
        public readonly string SummaryText = "Initializes a new instance of the";
        public readonly string SummaryLink = "Initializes a new instance of the";
        public readonly string RemarksTest = "Remarks about how this relates to my parameter";
        public readonly string ParaText = $"Fake method summary, first paragraph.  {Environment.NewLine}{Environment.NewLine}Fake method summary, second paragraph.";
        public readonly string Paramref = "myParam";
        public readonly Regex Example = new Regex(
            @"```.*MyFakeMethodWithExample\(\);.*```",
            RegexOptions.Compiled | RegexOptions.Singleline
        );


        public MethodMember testMember;
        public MethodMember testMemberWithParagraphs;
        public MethodMember testMemberWithSummaryExample;

        public MemberBaseTests()
        {
            var memberFactory = new MemberFactory();
            var elements = MembersOfType(GetFixture(), MemberType.Method);
            testMember = memberFactory.GetMember(elements.First()) as MethodMember;
            testMemberWithParagraphs = memberFactory
                .GetMember(elements[2]) as MethodMember;
            testMemberWithSummaryExample = memberFactory
                .GetMember(elements[3]) as MethodMember;
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

        [Fact]
        public void MemberBase_Returns_Summary_With_Text_And_Links()
        {
            Assert.Contains(SummaryText, testMember.Summary);
            Assert.Contains($"[{Namespace}.{TypeName}]", testMember.Summary);
        }

        [Fact]
        public void MemberBase_Returns_Remarks_With_Text_And_ParamRef()
        {
            Assert.Contains(RemarksTest, testMember.Remarks);
            Assert.Contains(Paramref, testMember.Remarks);
        }

        [Fact]
        public void MemberBase_Returns_Para_Text()
        {
            Assert.Contains(ParaText, testMemberWithParagraphs.Summary);
        }

        [Fact]
        public void MemberBase_Returns_Example_Blocks()
        {
            Assert.Matches(Example, testMemberWithSummaryExample.Summary);
        }
    }
}
