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
using o2g.Types.CallCenterManagementNS.CalendarNS;

namespace o2g.Tests.Types.CallCenterManagement.Calendar
{
    public class TransitionTimeTests
    {
        #region Parse

        [Theory]
        [InlineData("09:30", 9, 30)]
        [InlineData("00:00", 0, 0)]
        [InlineData("23:59", 23, 59)]
        [InlineData("08:00", 8, 0)]
        public void Parse_ValidTime_ReturnsCorrectHourAndMinute(
            string value, int expectedHour, int expectedMinute)
        {
            var time = Transition.Time.Parse(value);

            time.Hour.Should().Be(expectedHour);
            time.Minute.Should().Be(expectedMinute);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Parse_NullOrEmpty_ThrowsArgumentException(string value)
        {
            Action act = () => Transition.Time.Parse(value);
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("25:00")]
        [InlineData("12:60")]
        [InlineData("ab:cd")]
        public void Parse_InvalidValues_ThrowsArgumentException(string value)
        {
            Action act = () => Transition.Time.Parse(value);
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_PadsHourAndMinute()
        {
            new Transition.Time(9, 5).ToString().Should().Be("09:05");
        }

        [Fact]
        public void ToString_Midnight_ReturnsCorrectString()
        {
            new Transition.Time(0, 0).ToString().Should().Be("00:00");
        }

        [Theory]
        [InlineData("08:00")]
        [InlineData("12:30")]
        [InlineData("23:59")]
        [InlineData("00:00")]
        public void ToString_RoundTrip_ParseAndStringify(string original)
        {
            Transition.Time.Parse(original).ToString().Should().Be(original);
        }

        #endregion
    }

    public class TransitionTests : JsonTestBase
    {
        [Fact]
        public void Transition_MapsAllProperties()
        {
            var transition = new Transition
            {
                TransitionTime = Transition.Time.Parse("09:30"),
                RuleNumber = 3,
                Mode = PilotOperatingMode.Normal
            };

            transition.TransitionTime.ToString().Should().Be("09:30");
            transition.RuleNumber.Should().Be(3);
            transition.Mode.Should().Be(PilotOperatingMode.Normal);
        }

        [Theory]
        [InlineData("normal", PilotOperatingMode.Normal)]
        [InlineData("closed", PilotOperatingMode.Closed)]
        [InlineData("forward", PilotOperatingMode.Forward)]
        public void Deserialize_AllModes_MappedCorrectly(
            string jsonValue, PilotOperatingMode expected)
        {
            var json = $$"""
                {
                    "time": "08:00",
                    "ruleNumber": 1,
                    "mode": "{{jsonValue}}"
                }
                """;

            var o2g = Deserialize<O2GTransitionJson>(json);
            o2g.Mode.Should().Be(expected);
        }

        [Fact]
        public void O2GTransitionEntry_ToTransition_MapsAllProperties()
        {
            var json = """
                {
                    "number": 1,
                    "transition": {
                        "time": "08:00",
                        "ruleNumber": 3,
                        "mode": "normal"
                    }
                }
                """;

            var entry = Deserialize<O2GTransitionEntry>(json);
            var transition = entry.ToTransition();

            entry.Number.Should().Be(1);
            transition.TransitionTime.ToString().Should().Be("08:00");
            transition.RuleNumber.Should().Be(3);
            transition.Mode.Should().Be(PilotOperatingMode.Normal);
        }
    }
}
