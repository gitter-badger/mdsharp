using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using MdSharp.Core.Components;
using Moq;
using Xunit;

namespace MdSharp.Tests.UnitTests
{
    public class MemberBaseTests
    {
        public readonly string MyType = "MyParentType";
        public readonly string MyAssembly = "MyAssembly";
        public readonly string MyMethod = "MyMethod(System.String)";
        public readonly string MyMethodWithoutArgs = "MyMethod";

        public MethodMember MethodMemberWithDefinedType;
        public MethodMember MethodMemberWithoutDefinedType;

        public MemberBaseTests()
        {
            var memberFactory = new MemberFactory();
            MethodMemberWithDefinedType = memberFactory.GetMember(CreateXDocWithTypeDefined()) as MethodMember;
            MethodMemberWithoutDefinedType = memberFactory.GetMember(CreateXDocWithoutTypeDefined()) as MethodMember;
        }

        [Fact]
        public void MemberBase_Returns_Correct_AssemblyName()
        {
            Assert.Equal(MethodMemberWithDefinedType.AssemblyName, MyAssembly);
        }

        [Fact]
        public void MemberBase_MemberMethod_Returns_Correct_TypeName_For_Element_Defined_Type()
        {
            Assert.Equal(MethodMemberWithDefinedType.TypeName, $"{MyAssembly}.{MyType}");
        }

        [Fact]
        //Essentially it should infer the type given it's assembly and member name
        public void MemberBase_Returns_Correct_TypeName_For_Undefined_Type()
        {
            Assert.Equal(MethodMemberWithoutDefinedType.TypeName, $"{MyAssembly}.{MyType}");
        }

        [Fact]
        public void MemberBase_Returns_Correct_FullName()
        {
            Assert.Equal(MethodMemberWithDefinedType.FullName, $"{MyAssembly}.{MyType}.{MyMethod}");
        }

        [Fact]
        public void MemberBase_Returns_Correct_ShortName()
        {
            Assert.Equal(MethodMemberWithDefinedType.ShortName, MyMethodWithoutArgs);
        }

        #region "Fixture setup"
        private XElement CreateXDocWithTypeDefined()
        {
            var method = new XElement("member") { Value = $"{MyAssembly}.{MyType}.{MyMethod}" };
            method.Add(new XAttribute("name", $"M:{MyAssembly}.{MyType}.{MyMethod}"));
            var parent = new XElement("member");
            parent.Add(new XAttribute("name", $"T:{MyAssembly}.{MyType}"));
            var members = new XElement("members", parent, method);

            return CreateXDocument(members);
        }
        private XElement CreateXDocWithoutTypeDefined()
        {
            var method = new XElement("member") { Value = $"{MyAssembly}.{MyType}.{MyMethod}" };
            method.Add(new XAttribute("name", $"M:{MyAssembly}.{MyType}.{MyMethod}"));
            var members = new XElement("members", method);

            return CreateXDocument(members);
        }

        private XElement CreateXDocument(XElement members)
        {
            var doc = new XDocument();
            var assembly = new XElement("assembly") { Value = MyAssembly };
            doc.Add(new XElement("doc", members, assembly));
            return doc.Element("doc").Element("members").MembersOfType(MemberType.Method).First();
        }
        #endregion
    }
}
