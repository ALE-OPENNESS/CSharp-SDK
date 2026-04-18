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
using o2g.Types.CallCenterAgentNS;
using Xunit;

namespace o2g.Tests.Types.CallCenterAgent
{
    public class AgentGroupsTests : JsonTestBase
    {
        [Fact]
        public void Deserialize_MapsPreferredAndGroups()
        {
            var json = """
                {
                    "preferred": "PG001",
                    "processingGroups": ["PG001", "PG002", "PG003"]
                }
                """;

            var groups = Deserialize<AgentGroups>(json);

            groups.Preferred.Should().Be("PG001");
            groups.Groups.Should().HaveCount(3);
            groups.Groups.Should().ContainInOrder("PG001", "PG002", "PG003");
        }

        [Fact]
        public void Deserialize_NoPreferredGroup_PreferredIsNull()
        {
            var json = """
                {
                    "processingGroups": ["PG001", "PG002"]
                }
                """;

            var groups = Deserialize<AgentGroups>(json);

            groups.Preferred.Should().BeNull();
            groups.Groups.Should().HaveCount(2);
        }

        [Fact]
        public void Deserialize_EmptyGroups_ReturnsEmptyList()
        {
            var json = """
                {
                    "preferred": null,
                    "processingGroups": []
                }
                """;

            var groups = Deserialize<AgentGroups>(json);

            groups.Preferred.Should().BeNull();
            groups.Groups.Should().BeEmpty();
        }

        [Fact]
        public void Deserialize_NoGroups_GroupsIsNull()
        {
            var json = """
                {
                    "preferred": "PG001"
                }
                """;

            var groups = Deserialize<AgentGroups>(json);

            groups.Preferred.Should().Be("PG001");
            groups.Groups.Should().BeNull();
        }
    }
}