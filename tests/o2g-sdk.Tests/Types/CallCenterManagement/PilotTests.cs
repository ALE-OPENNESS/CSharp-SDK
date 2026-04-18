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
using o2g.Internal.Types.CallCenterManagementNS;
using o2g.Tests.Helpers;
using o2g.Types.CommonNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;

namespace o2g.Tests.Types.CallCenterManagement
{
    public class PilotTests : JsonTestBase
    {
        private const string FullPilot = """
            {
                "number": "3000",
                "name": "Support pilot",
                "state": "Opened",
                "detailedState": "OPEN",
                "waitingTime": 45,
                "saturation": false,
                "rules": {
                    "ruleList": [
                        { "ruleNumber": "1", "name": "Default", "active": true },
                        { "ruleNumber": "2", "name": "Overflow", "active": false }
                    ]
                },
                "possibleTransfer": true,
                "supervisedTransfer": true
            }
            """;

        private const string MinimalPilot = """
            {
                "number": "3001",
                "waitingTime": 0,
                "saturation": false,
                "possibleTransfer": false,
                "supervisedTransfer": false
            }
            """;

        #region Scalar properties

        [Fact]
        public void ToPilot_FullPilot_MapsScalarProperties()
        {
            var pilot = Deserialize<O2GPilot>(FullPilot).ToPilot();

            pilot.Number.Should().Be("3000");
            pilot.Name.Should().Be("Support pilot");
            pilot.WaitingTime.Should().Be(45);
            pilot.Saturation.Should().BeFalse();
            pilot.PossibleTransfer.Should().BeTrue();
            pilot.SupervisedTransfer.Should().BeTrue();
        }

        [Fact]
        public void ToPilot_FullPilot_MapsState()
        {
            var pilot = Deserialize<O2GPilot>(FullPilot).ToPilot();

            pilot.State.Should().Be(ServiceState.Opened);
        }

        [Fact]
        public void ToPilot_FullPilot_MapsDetailedState()
        {
            var pilot = Deserialize<O2GPilot>(FullPilot).ToPilot();

            pilot.DetailedState.Should().Be(PilotStatus.Opened);
        }

        [Fact]
        public void ToPilot_MinimalPilot_MapsScalarProperties()
        {
            var pilot = Deserialize<O2GPilot>(MinimalPilot).ToPilot();

            pilot.Number.Should().Be("3001");
            pilot.WaitingTime.Should().Be(0);
            pilot.Saturation.Should().BeFalse();
            pilot.PossibleTransfer.Should().BeFalse();
            pilot.SupervisedTransfer.Should().BeFalse();
        }

        #endregion

        #region Rules

        [Fact]
        public void ToPilot_FullPilot_MapsRules()
        {
            var pilot = Deserialize<O2GPilot>(FullPilot).ToPilot();

            pilot.Rules.Should().NotBeNull();
            pilot.Rules.Count.Should().Be(2);
            pilot.Rules.Contains(1).Should().BeTrue();
            pilot.Rules.Contains(2).Should().BeTrue();

            var rule1 = pilot.Rules.Get(1);
            rule1.Should().NotBeNull();
            rule1!.RuleNumber.Should().Be(1);
            rule1.Name.Should().Be("Default");
            rule1.Active.Should().BeTrue();

            var rule2 = pilot.Rules.Get(2);
            rule2.Should().NotBeNull();
            rule2!.RuleNumber.Should().Be(2);
            rule2.Name.Should().Be("Overflow");
            rule2.Active.Should().BeFalse();
        }

        [Fact]
        public void ToPilot_MinimalPilot_RulesIsEmpty()
        {
            var pilot = Deserialize<O2GPilot>(MinimalPilot).ToPilot();

            pilot.Rules.Should().NotBeNull();
            pilot.Rules.IsEmpty.Should().BeTrue();
        }

        #endregion

        #region Nullable properties

        [Fact]
        public void ToPilot_MinimalPilot_NullablePropertiesAreNull()
        {
            var pilot = Deserialize<O2GPilot>(MinimalPilot).ToPilot();

            pilot.Name.Should().BeNull();
            pilot.State.Should().BeNull();
            pilot.DetailedState.Should().BeNull();
        }

        #endregion

        #region Enum fallbacks

        [Fact]
        public void ToPilot_UnknownDetailedState_FallsBackToOther()
        {
            var json = """
                {
                    "number": "3000",
                    "detailedState": "SOME_FUTURE_VALUE",
                    "waitingTime": 0,
                    "saturation": false,
                    "possibleTransfer": false,
                    "supervisedTransfer": false
                }
                """;

            var pilot = Deserialize<O2GPilot>(json).ToPilot();

            pilot.DetailedState.Should().Be(PilotStatus.Other);
        }

        [Fact]
        public void ToPilot_SaturatedPilot_SaturationIsTrue()
        {
            var json = """
                {
                    "number": "3000",
                    "waitingTime": 120,
                    "saturation": true,
                    "possibleTransfer": false,
                    "supervisedTransfer": false
                }
                """;

            var pilot = Deserialize<O2GPilot>(json).ToPilot();

            pilot.Saturation.Should().BeTrue();
            pilot.WaitingTime.Should().Be(120);
        }

        #endregion
    }
}
