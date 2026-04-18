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
using o2g.Types.CallCenterManagementNS;

namespace o2g.Tests.Types.CallCenterManagement
{
    public class PilotRuleTests : JsonTestBase
    {
        [Fact]
        public void Deserialize_MapsAllProperties()
        {
            var json = """
                {
                    "ruleNumber": 3,
                    "name": "Priority routing",
                    "active": true
                }
                """;

            var rule = Deserialize<PilotRule>(json);

            rule.RuleNumber.Should().Be(3);
            rule.Name.Should().Be("Priority routing");
            rule.Active.Should().BeTrue();
        }

        [Fact]
        public void Deserialize_NoName_NameIsNull()
        {
            var json = """
                {
                    "ruleNumber": 1,
                    "active": false
                }
                """;

            var rule = Deserialize<PilotRule>(json);

            rule.RuleNumber.Should().Be(1);
            rule.Name.Should().BeNull();
            rule.Active.Should().BeFalse();
        }

        [Fact]
        public void Deserialize_InactiveRule_ActiveIsFalse()
        {
            var json = """
                {
                    "ruleNumber": 2,
                    "name": "Overflow rule",
                    "active": false
                }
                """;

            var rule = Deserialize<PilotRule>(json);

            rule.Active.Should().BeFalse();
        }
    }
}