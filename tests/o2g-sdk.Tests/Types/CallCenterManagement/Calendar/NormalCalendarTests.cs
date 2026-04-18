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
    public class NormalCalendarTests : JsonTestBase
    {
        private const string CalendarJson = """
            {
                "calendar": [
                    {
                        "day": "monday",
                        "list": [
                            { "number": 1, "transition": { "time": "08:00", "ruleNumber": 1, "mode": "normal" } },
                            { "number": 2, "transition": { "time": "18:00", "ruleNumber": 2, "mode": "closed" } }
                        ]
                    },
                    {
                        "day": "saturday",
                        "list": [
                            { "number": 1, "transition": { "time": "09:00", "ruleNumber": 1, "mode": "normal" } }
                        ]
                    }
                ]
            }
            """;

        private NormalCalendar BuildCalendar()
            => Deserialize<O2GNormalCalendar>(CalendarJson).ToNormalCalendar();

        #region Days

        [Fact]
        public void ToNormalCalendar_ParsesDaysCorrectly()
        {
            var calendar = BuildCalendar();

            calendar.Days.Should().HaveCount(2);
            calendar.Days.Should().Contain(DayOfWeek.Monday);
            calendar.Days.Should().Contain(DayOfWeek.Saturday);
        }

        [Fact]
        public void ToNormalCalendar_EmptyCalendar_NoDays()
        {
            var calendar = Deserialize<O2GNormalCalendar>("""{ "calendar": [] }""")
                .ToNormalCalendar();

            calendar.Days.Should().BeEmpty();
        }

        [Fact]
        public void ToNormalCalendar_NullCalendar_NoDays()
        {
            var calendar = Deserialize<O2GNormalCalendar>("""{}""")
                .ToNormalCalendar();

            calendar.Days.Should().BeEmpty();
        }

        [Theory]
        [InlineData("monday", DayOfWeek.Monday)]
        [InlineData("tuesday", DayOfWeek.Tuesday)]
        [InlineData("wednesday", DayOfWeek.Wednesday)]
        [InlineData("thursday", DayOfWeek.Thursday)]
        [InlineData("friday", DayOfWeek.Friday)]
        [InlineData("saturday", DayOfWeek.Saturday)]
        [InlineData("sunday", DayOfWeek.Sunday)]
        public void ToNormalCalendar_AllDays_ParsedCorrectly(
            string jsonDay, DayOfWeek expected)
        {
            var json = $$"""
                {
                    "calendar": [
                        {
                            "day": "{{jsonDay}}",
                            "list": [
                                { "number": 1, "transition": { "time": "08:00", "ruleNumber": 1, "mode": "normal" } }
                            ]
                        }
                    ]
                }
                """;

            var calendar = Deserialize<O2GNormalCalendar>(json).ToNormalCalendar();

            calendar.Days.Should().Contain(expected);
        }

        #endregion

        #region GetTransitions

        [Fact]
        public void GetTransitions_ExistingDay_ReturnsAllTransitions()
        {
            var calendar = BuildCalendar();

            var transitions = calendar.GetTransitions(DayOfWeek.Monday);

            transitions.Should().HaveCount(2);
            transitions![0].TransitionTime.ToString().Should().Be("08:00");
            transitions![0].Mode.Should().Be(PilotOperatingMode.Normal);
            transitions![1].TransitionTime.ToString().Should().Be("18:00");
            transitions![1].Mode.Should().Be(PilotOperatingMode.Closed);
        }

        [Fact]
        public void GetTransitions_NonExistingDay_ReturnsNull()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitions(DayOfWeek.Sunday).Should().BeNull();
        }

        [Fact]
        public void GetTransitions_IsReadOnly()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitions(DayOfWeek.Monday)
                .Should().BeAssignableTo<IReadOnlyList<Transition>>();
        }

        #endregion

        #region GetTransitionAt

        [Fact]
        public void GetTransitionAt_FirstIndex_ReturnsCorrectTransition()
        {
            var calendar = BuildCalendar();

            var transition = calendar.GetTransitionAt(DayOfWeek.Monday, 0);

            transition.Should().NotBeNull();
            transition!.TransitionTime.ToString().Should().Be("08:00");
            transition.RuleNumber.Should().Be(1);
            transition.Mode.Should().Be(PilotOperatingMode.Normal);
        }

        [Fact]
        public void GetTransitionAt_SecondIndex_ReturnsCorrectTransition()
        {
            var calendar = BuildCalendar();

            var transition = calendar.GetTransitionAt(DayOfWeek.Monday, 1);

            transition.Should().NotBeNull();
            transition!.TransitionTime.ToString().Should().Be("18:00");
            transition.Mode.Should().Be(PilotOperatingMode.Closed);
        }

        [Fact]
        public void GetTransitionAt_OutOfRangeIndex_ReturnsNull()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitionAt(DayOfWeek.Monday, 99).Should().BeNull();
        }

        [Fact]
        public void GetTransitionAt_NonExistingDay_ReturnsNull()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitionAt(DayOfWeek.Sunday, 0).Should().BeNull();
        }

        #endregion
    }
}