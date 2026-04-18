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
using o2g.Internal.Types.CallCenterAgent;
using o2g.Tests.Helpers;
using o2g.Types.CallCenterAgentNS;
using Xunit;

namespace o2g.Tests.Types.CallCenterAgent
{
    public class OperatorConfigurationTests : JsonTestBase
    {
        private const string FullConfig = """
            {
                "type": "Agent",
                "proacd": "1000",
                "processingGroups": {
                    "preferred": "PG001",
                    "processingGroups": ["PG001", "PG002"]
                },
                "skills": {
                    "skills": [
                        { "number": 101, "level": 3, "active": true,  "domain": 1, "name": "routing" },
                        { "number": 102, "level": 2, "active": false, "domain": 1, "name": "support" }
                    ]
                },
                "selfAssign": true,
                "headset": true,
                "help": false,
                "multiline": true
            }
            """;

        private const string MinimalConfig = """
            {
                "type": "Supervisor",
                "selfAssign": false,
                "headset": false,
                "help": false,
                "multiline": false
            }
            """;

        #region Scalar properties

        [Fact]
        public void ToOperatorConfiguration_FullConfig_MapsScalarProperties()
        {
            var config = Deserialize<O2GAgentConfig>(FullConfig)
                .ToOperatorConfiguration();

            config.Type.Should().Be(OperatorType.Agent);
            config.Proacd.Should().Be("1000");
            config.SelfAssign.Should().BeTrue();
            config.Headset.Should().BeTrue();
            config.Help.Should().BeFalse();
            config.Multiline.Should().BeTrue();
        }

        [Fact]
        public void ToOperatorConfiguration_MinimalConfig_NullablePropertiesAreNull()
        {
            var config = Deserialize<O2GAgentConfig>(MinimalConfig)
                .ToOperatorConfiguration();

            config.Type.Should().Be(OperatorType.Supervisor);
            config.Proacd.Should().BeNull();
            config.Groups.Should().BeNull();
        }

        #endregion

        #region Groups

        [Fact]
        public void ToOperatorConfiguration_FullConfig_MapsGroups()
        {
            var config = Deserialize<O2GAgentConfig>(FullConfig)
                .ToOperatorConfiguration();

            config.Groups.Should().NotBeNull();
            config.Groups.Preferred.Should().Be("PG001");
            config.Groups.Groups.Should().HaveCount(2);
            config.Groups.Groups.Should().Contain("PG001");
            config.Groups.Groups.Should().Contain("PG002");
        }

        [Fact]
        public void ToOperatorConfiguration_MinimalConfig_GroupsIsNull()
        {
            var config = Deserialize<O2GAgentConfig>(MinimalConfig)
                .ToOperatorConfiguration();

            config.Groups.Should().BeNull();
        }

        #endregion

        #region Skills

        [Fact]
        public void ToOperatorConfiguration_FullConfig_MapsSkills()
        {
            var config = Deserialize<O2GAgentConfig>(FullConfig)
                .ToOperatorConfiguration();

            config.Skills.Should().NotBeNull();
            config.Skills.Count.Should().Be(2);
            config.Skills.Contains(101).Should().BeTrue();
            config.Skills.Contains(102).Should().BeTrue();
        }

        [Fact]
        public void ToOperatorConfiguration_FullConfig_SkillPropertiesAreCorrect()
        {
            var config = Deserialize<O2GAgentConfig>(FullConfig)
                .ToOperatorConfiguration();

            var skill = config.Skills.Get(101);
            skill.Should().NotBeNull();
            skill!.Level.Should().Be(3);
            skill.Active.Should().BeTrue();
            skill.Domain.Should().Be(1);
            skill.Name.Should().Be("routing");
        }

        [Fact]
        public void ToOperatorConfiguration_FullConfig_SkillsIndexedByDomainAndName()
        {
            var config = Deserialize<O2GAgentConfig>(FullConfig)
                .ToOperatorConfiguration();

            config.Skills.Get(1, "routing").Should().NotBeNull();
            config.Skills.Get(1, "support").Should().NotBeNull();
            config.Skills.Get(1, "unknown").Should().BeNull();
        }

        [Fact]
        public void ToOperatorConfiguration_MinimalConfig_SkillsIsEmpty()
        {
            var config = Deserialize<O2GAgentConfig>(MinimalConfig)
                .ToOperatorConfiguration();

            config.Skills.Should().NotBeNull();
            config.Skills.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void ToOperatorConfiguration_NoSkillsField_SkillsIsEmpty()
        {
            var json = """
                {
                    "type": "Agent",
                    "selfAssign": false,
                    "headset": false,
                    "help": false,
                    "multiline": false
                }
                """;

            var config = Deserialize<O2GAgentConfig>(json)
                .ToOperatorConfiguration();

            config.Skills.IsEmpty.Should().BeTrue();
        }

        #endregion
    }
}
