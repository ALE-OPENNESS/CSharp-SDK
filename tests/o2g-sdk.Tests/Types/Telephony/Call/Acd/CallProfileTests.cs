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
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace o2g.Tests.Types.Telephony.Call.Acd
{
    public class CallProfileTests : JsonTestBase
    {
        #region Fixtures

        private static readonly CallProfile.Skill Skill101 = new(101, level: 3, mandatory: true);
        private static readonly CallProfile.Skill Skill102 = new(102, level: 2, mandatory: false);
        private static readonly CallProfile.Skill Skill103 = new(103, level: 1, mandatory: false);

        private class SkillsWrapper
        {
            public List<CallProfile.Skill> Skills { get; init; } = null!;
        }

        #endregion

        #region Constructors

        [Fact]
        public void Constructor_Empty_CreatesEmptyProfile()
        {
            var profile = new CallProfile();

            profile.Skills.Should().BeEmpty();
            profile.SkillNumbers.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_Varargs_MapsAllSkills()
        {
            var profile = new CallProfile(Skill101, Skill102, Skill103);

            profile.Skills.Should().HaveCount(3);
            profile.SkillNumbers.Should().BeEquivalentTo(new[] { 101, 102, 103 });
        }

        [Fact]
        public void Constructor_Collection_MapsAllSkills()
        {
            var skills = new[] { Skill101, Skill102, Skill103 };
            var profile = new CallProfile(skills);

            profile.Skills.Should().HaveCount(3);
            profile.SkillNumbers.Should().BeEquivalentTo(new[] { 101, 102, 103 });
        }

        [Fact]
        public void Constructor_DuplicateSkillNumber_LastOneWins()
        {
            var duplicate = new CallProfile.Skill(101, level: 5, mandatory: false);
            var profile = new CallProfile(Skill101, duplicate);

            profile.Skills.Should().HaveCount(1);
            profile.Get(101)!.Level.Should().Be(5);
        }

        #endregion

        #region Get

        [Fact]
        public void Get_ExistingSkill_ReturnsSkill()
        {
            var profile = new CallProfile(Skill101, Skill102);

            var skill = profile.Get(101);

            skill.Should().NotBeNull();
            skill!.Number.Should().Be(101);
            skill.Level.Should().Be(3);
            skill.Mandatory.Should().BeTrue();
        }

        [Fact]
        public void Get_NonExistingSkill_ReturnsNull()
        {
            var profile = new CallProfile(Skill101);

            profile.Get(999).Should().BeNull();
        }

        #endregion

        #region Contains

        [Fact]
        public void Contains_ExistingSkill_ReturnsTrue()
        {
            var profile = new CallProfile(Skill101, Skill102);

            profile.Contains(101).Should().BeTrue();
            profile.Contains(102).Should().BeTrue();
        }

        [Fact]
        public void Contains_NonExistingSkill_ReturnsFalse()
        {
            var profile = new CallProfile(Skill101);

            profile.Contains(999).Should().BeFalse();
        }

        #endregion

        #region Skills and SkillNumbers

        [Fact]
        public void Skills_ReturnsAllSkills()
        {
            var profile = new CallProfile(Skill101, Skill102, Skill103);

            profile.Skills.Should().HaveCount(3)
                .And.Contain(s => s.Number == 101)
                .And.Contain(s => s.Number == 102)
                .And.Contain(s => s.Number == 103);
        }

        [Fact]
        public void SkillNumbers_ReturnsAllNumbers()
        {
            var profile = new CallProfile(Skill101, Skill102, Skill103);

            profile.SkillNumbers.Should().BeEquivalentTo(new[] { 101, 102, 103 });
        }

        #endregion

        #region ToList

        [Fact]
        public void ToList_ReturnsAllSkills()
        {
            var profile = new CallProfile(Skill101, Skill102);

            var list = profile.ToList();

            list.Should().HaveCount(2);
            list.Should().Contain(s => s.Number == 101 && s.Level == 3 && s.Mandatory == true);
            list.Should().Contain(s => s.Number == 102 && s.Level == 2 && s.Mandatory == false);
        }

        [Fact]
        public void ToList_EmptyProfile_ReturnsEmptyList()
        {
            var profile = new CallProfile();

            profile.ToList().Should().BeEmpty();
        }

        #endregion

        #region Skill

        [Fact]
        public void Skill_Properties_AreCorrectlySet()
        {
            var skill = new CallProfile.Skill(101, level: 3, mandatory: true);

            skill.Number.Should().Be(101);
            skill.Level.Should().Be(3);
            skill.Mandatory.Should().BeTrue();
        }

        [Fact]
        public void Skill_NonMandatory_MandatoryIsFalse()
        {
            var skill = new CallProfile.Skill(102, level: 1, mandatory: false);

            skill.Mandatory.Should().BeFalse();
        }

        #endregion

        #region JSON serialization

        [Fact]
        public void Skill_SerializesToJson_WithCorrectPropertyNames()
        {
            var skill = new CallProfile.Skill(101, level: 3, mandatory: true);

            var json = Serialize(skill);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            root.GetProperty("skillNumber").GetInt32().Should().Be(101);
            root.GetProperty("expertEvalLevel").GetInt32().Should().Be(3);
            root.GetProperty("acrStatus").GetBoolean().Should().BeTrue();
        }

        [Fact]
        public void Skill_DeserializesFromJson_WithCorrectPropertyNames()
        {
            var json = """
                {
                    "skillNumber": 101,
                    "expertEvalLevel": 3,
                    "acrStatus": true
                }
                """;

            var skill = Deserialize<CallProfile.Skill>(json);

            skill.Number.Should().Be(101);
            skill.Level.Should().Be(3);
            skill.Mandatory.Should().BeTrue();
        }

        [Fact]
        public void ToList_RoundTrip_SerializeAndDeserialize()
        {
            var profile = new CallProfile(
                new CallProfile.Skill(101, level: 3, mandatory: true),
                new CallProfile.Skill(102, level: 2, mandatory: false)
            );

            var json = Serialize(new { skills = profile.ToList() });
            var deserialized = Deserialize<SkillsWrapper>(json);

            deserialized.Skills.Should().HaveCount(2);
            deserialized.Skills.Should().Contain(s => s.Number == 101 && s.Level == 3 && s.Mandatory == true);
            deserialized.Skills.Should().Contain(s => s.Number == 102 && s.Level == 2 && s.Mandatory == false);
        }

        #endregion
    }
}
