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
using o2g.Types.CallCenterManagementNS;
using System.Collections.Generic;
using Xunit;

namespace o2g.Tests.Types.CallCenterManagement
{
    public class PilotRuleSetTests
    {
        #region Fixtures

        private static PilotRuleSet BuildRuleSet(params PilotRule[] rules)
            => new PilotRuleSet(rules);

        private static readonly PilotRule Rule1 = new()
        {
            RuleNumber = 1,
            Name = "Default",
            Active = true
        };

        private static readonly PilotRule Rule2 = new()
        {
            RuleNumber = 2,
            Name = "Overflow",
            Active = false
        };

        private static readonly PilotRule Rule3 = new()
        {
            RuleNumber = 3,
            Name = null,
            Active = true
        };

        #endregion

        #region Count and IsEmpty

        [Fact]
        public void Count_ReturnsNumberOfRules()
        {
            var set = BuildRuleSet(Rule1, Rule2, Rule3);

            set.Count.Should().Be(3);
        }

        [Fact]
        public void IsEmpty_WhenEmpty_ReturnsTrue()
        {
            var set = BuildRuleSet();

            set.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void IsEmpty_WhenNotEmpty_ReturnsFalse()
        {
            var set = BuildRuleSet(Rule1);

            set.IsEmpty.Should().BeFalse();
        }

        #endregion

        #region Get

        [Fact]
        public void Get_ExistingNumber_ReturnsRule()
        {
            var set = BuildRuleSet(Rule1, Rule2);

            var rule = set.Get(1);

            rule.Should().NotBeNull();
            rule!.RuleNumber.Should().Be(1);
            rule.Name.Should().Be("Default");
            rule.Active.Should().BeTrue();
        }

        [Fact]
        public void Get_NonExistingNumber_ReturnsNull()
        {
            var set = BuildRuleSet(Rule1);

            set.Get(999).Should().BeNull();
        }

        [Fact]
        public void Get_RuleWithNullName_ReturnsRule()
        {
            var set = BuildRuleSet(Rule3);

            var rule = set.Get(3);

            rule.Should().NotBeNull();
            rule!.Name.Should().BeNull();
        }

        #endregion

        #region Contains

        [Fact]
        public void Contains_ExistingNumber_ReturnsTrue()
        {
            var set = BuildRuleSet(Rule1, Rule2);

            set.Contains(1).Should().BeTrue();
            set.Contains(2).Should().BeTrue();
        }

        [Fact]
        public void Contains_NonExistingNumber_ReturnsFalse()
        {
            var set = BuildRuleSet(Rule1);

            set.Contains(999).Should().BeFalse();
        }

        #endregion

        #region RuleNumbers and Rules

        [Fact]
        public void RuleNumbers_ReturnsAllNumbers()
        {
            var set = BuildRuleSet(Rule1, Rule2, Rule3);

            set.RuleNumbers.Should().BeEquivalentTo(new[] { 1, 2, 3 });
        }

        [Fact]
        public void Rules_ReturnsAllRules()
        {
            var set = BuildRuleSet(Rule1, Rule2, Rule3);

            set.Rules.Should().HaveCount(3)
                .And.Contain(r => r.RuleNumber == 1)
                .And.Contain(r => r.RuleNumber == 2)
                .And.Contain(r => r.RuleNumber == 3);
        }

        [Fact]
        public void Rules_IsReadOnly()
        {
            var set = BuildRuleSet(Rule1);

            set.Rules.Should().BeAssignableTo<IReadOnlyCollection<PilotRule>>();
        }

        [Fact]
        public void RuleNumbers_IsReadOnly()
        {
            var set = BuildRuleSet(Rule1);

            set.RuleNumbers.Should().BeAssignableTo<IReadOnlyCollection<int>>();
        }

        #endregion

        #region Empty rule set

        [Fact]
        public void EmptyRuleSet_AllOperationsReturnEmpty()
        {
            var set = BuildRuleSet();

            set.Count.Should().Be(0);
            set.IsEmpty.Should().BeTrue();
            set.Get(1).Should().BeNull();
            set.Contains(1).Should().BeFalse();
            set.RuleNumbers.Should().BeEmpty();
            set.Rules.Should().BeEmpty();
        }

        #endregion

        #region Duplicate rule number

        [Fact]
        public void Constructor_DuplicateRuleNumber_LastOneWins()
        {
            var duplicate = new PilotRule { RuleNumber = 1, Name = "Updated", Active = false };
            var set = BuildRuleSet(Rule1, duplicate);

            set.Count.Should().Be(1);
            set.Get(1)!.Name.Should().Be("Updated");
        }

        #endregion
    }
}
