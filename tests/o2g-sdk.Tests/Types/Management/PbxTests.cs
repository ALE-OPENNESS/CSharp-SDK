/*
* Copyright 2026 ALE International
*
* Permission is hereby granted, free of charge, to any person obtaining a copy of this
* software and associated documentation files (the "Software"), to deal in the Software
* without restriction, including without limitation the rights to use, copy, modify, merge,
* publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
* to whom the Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all copies or
* substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
* BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
* DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using FluentAssertions;
using o2g.Tests.Helpers;
using o2g.Types.ManagementNS;
using Xunit;

namespace o2g.Tests.Types.Management
{
    public class PbxTests : JsonTestBase
    {
        [Fact]
        public void Deserialize_Pbx_MapsBothFields()
        {
            var json = """{ "nodeId": 1, "fqdn": "pbx01.example.com" }""";

            var pbx = Deserialize<Pbx>(json);

            pbx.Should().NotBeNull();
            pbx.NodeId.Should().Be(1);
            pbx.Fqdn.Should().Be("pbx01.example.com");
        }

        [Fact]
        public void Deserialize_Pbx_NullFqdn_MapsNodeId()
        {
            var json = """{ "nodeId": 5 }""";

            var pbx = Deserialize<Pbx>(json);

            pbx.NodeId.Should().Be(5);
            pbx.Fqdn.Should().BeNull();
        }

        [Fact]
        public void Deserialize_Pbx_DifferentNodeIds()
        {
            var json = """{ "nodeId": 99, "fqdn": "pbx99.corp.local" }""";

            var pbx = Deserialize<Pbx>(json);

            pbx.NodeId.Should().Be(99);
            pbx.Fqdn.Should().Be("pbx99.corp.local");
        }
    }
}
