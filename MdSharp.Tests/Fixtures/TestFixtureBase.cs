using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MdSharp.Core.Components;

using static System.IO.Path;

namespace MdSharp.Tests.Fixtures
{
    public class TestFixtureBase
    {
        public readonly string TypeName = "MyFakeType";
        public readonly string Namespace = "MdSharp.Core.Components";
        public readonly string AssemblyName = "MdSharp.Core";

        #region "Fixture setup"

        public XElement GetFixture()
        {
            var xdoc = XDocument.Load(Combine("Fixtures", "Fixture.xml"));
            var document = xdoc.Element("doc");

            return document.Element("members");
        }
        public List<XElement> MembersOfType(XElement element, MemberType memberType)
        {
            return element.Elements().Where(e => e.FirstAttribute
                .Value
                .StartsWith($"{memberType.ToString().First()}:"))
                .ToList();
        }
        #endregion
    }
}
