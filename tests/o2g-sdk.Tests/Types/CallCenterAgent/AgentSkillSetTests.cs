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
using o2g.Types.CallCenterAgentNS;
using System.Collections.Generic;
using Xunit;

namespace o2g.Tests.Types.CallCenterAgent
{
    public class AgentSkillSetTests
    {
        #region Fixtures

        private static AgentSkillSet BuildSkillSet(params AgentSkill[] skills)
        {
            var map = new Dictionary<int, AgentSkill>();
            foreach (var skill in skills)
            {
                map[skill.Number] = skill;
            }
            return new AgentSkillSet { Map = map };
        }

        private static readonly AgentSkill Skill101 = new()
        {
            Number = 101,
            Level = 3,
            Active = true,
            Domain = 0,
            Name = null
        };

        private static readonly AgentSkill Skill102 = new()
        {
            Number = 102,
            Level = 2,
            Active = false,
            Domain = 1,
            Name = "routing"
        };

        private static readonly AgentSkill Skill103 = new()
        {
            Number = 103,
            Level = 1,
            Active = true,
            Domain = 1,
            Name = "support"
        };

        #endregion

        #region Count and IsEmpty

        [Fact]
        public void Count_ReturnsNumberOfSkills()
        {
            var set = BuildSkillSet(Skill101, Skill102, Skill103);

            set.Count.Should().Be(3);
        }

        [Fact]
        public void IsEmpty_WhenEmpty_ReturnsTrue()
        {
            var set = BuildSkillSet();

            set.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void IsEmpty_WhenNotEmpty_ReturnsFalse()
        {
            var set = BuildSkillSet(Skill101);

            set.IsEmpty.Should().BeFalse();
        }

        #endregion

        #region Get by number

        [Fact]
        public void Get_ExistingNumber_ReturnsSkill()
        {
            var set = BuildSkillSet(Skill101, Skill102);

            var skill = set.Get(101);

            skill.Should().NotBeNull();
            skill!.Number.Should().Be(101);
            skill.Level.Should().Be(3);
            skill.Active.Should().BeTrue();
        }

        [Fact]
        public void Get_NonExistingNumber_ReturnsNull()
        {
            var set = BuildSkillSet(Skill101);

            set.Get(999).Should().BeNull();
        }

        #endregion

        #region Get by domain and name

        [Fact]
        public void Get_ExistingDomainAndName_ReturnsSkill()
        {
            var set = BuildSkillSet(Skill101, Skill102, Skill103);

            var skill = set.Get(1, "routing");

            skill.Should().NotBeNull();
            skill!.Number.Should().Be(102);
        }

        [Fact]
        public void Get_NonExistingDomainAndName_ReturnsNull()
        {
            var set = BuildSkillSet(Skill101, Skill102);

            set.Get(1, "unknown").Should().BeNull();
        }

        [Fact]
        public void Get_SkillWithNullName_NotIndexedByDomainAndName()
        {
            var set = BuildSkillSet(Skill101);

            // Skill101 has Domain=0 and Name=null — should not be findable by domain/name
            set.Get(0, null!).Should().BeNull();
        }

        #endregion

        #region Contains by number

        [Fact]
        public void Contains_ExistingNumber_ReturnsTrue()
        {
            var set = BuildSkillSet(Skill101, Skill102);

            set.Contains(101).Should().BeTrue();
            set.Contains(102).Should().BeTrue();
        }

        [Fact]
        public void Contains_NonExistingNumber_ReturnsFalse()
        {
            var set = BuildSkillSet(Skill101);

            set.Contains(999).Should().BeFalse();
        }

        #endregion

        #region Contains by domain and name

        [Fact]
        public void Contains_ExistingDomainAndName_ReturnsTrue()
        {
            var set = BuildSkillSet(Skill102, Skill103);

            set.Contains(1, "routing").Should().BeTrue();
            set.Contains(1, "support").Should().BeTrue();
        }

        [Fact]
        public void Contains_NonExistingDomainAndName_ReturnsFalse()
        {
            var set = BuildSkillSet(Skill102);

            set.Contains(1, "unknown").Should().BeFalse();
            set.Contains(99, "routing").Should().BeFalse();
        }

        #endregion

        #region SkillNumbers and Skills

        [Fact]
        public void SkillNumbers_ReturnsAllNumbers()
        {
            var set = BuildSkillSet(Skill101, Skill102, Skill103);

            set.SkillNumbers.Should().BeEquivalentTo(new[] { 101, 102, 103 });
        }

        [Fact]
        public void Skills_ReturnsAllSkills()
        {
            var set = BuildSkillSet(Skill101, Skill102, Skill103);

            set.Skills.Should().HaveCount(3)
                .And.Contain(s => s.Number == 101)
                .And.Contain(s => s.Number == 102)
                .And.Contain(s => s.Number == 103);
        }

        [Fact]
        public void Skills_IsReadOnly()
        {
            var set = BuildSkillSet(Skill101);

            // IReadOnlyCollection cannot be cast to IList — verifies immutability
            set.Skills.Should().BeAssignableTo<IReadOnlyCollection<AgentSkill>>();
        }

        [Fact]
        public void SkillNumbers_IsReadOnly()
        {
            var set = BuildSkillSet(Skill101);

            set.SkillNumbers.Should().BeAssignableTo<IReadOnlyCollection<int>>();
        }

        #endregion

        #region Empty skill set

        [Fact]
        public void EmptySkillSet_AllOperationsReturnEmpty()
        {
            var set = BuildSkillSet();

            set.Count.Should().Be(0);
            set.IsEmpty.Should().BeTrue();
            set.Get(1).Should().BeNull();
            set.Get(1, "routing").Should().BeNull();
            set.Contains(1).Should().BeFalse();
            set.Contains(1, "routing").Should().BeFalse();
            set.SkillNumbers.Should().BeEmpty();
            set.Skills.Should().BeEmpty();
        }

        #endregion
    }
}