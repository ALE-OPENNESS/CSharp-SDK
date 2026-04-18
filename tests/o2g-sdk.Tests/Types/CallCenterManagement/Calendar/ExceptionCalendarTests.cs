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
    public class ExceptionCalendarTests : JsonTestBase
    {
        private const string CalendarJson = """
            {
                "calendar": [
                    {
                        "date": "20260714",
                        "list": [
                            { "number": 1, "transition": { "time": "08:00", "ruleNumber": 1, "mode": "normal" } },
                            { "number": 2, "transition": { "time": "12:00", "ruleNumber": 2, "mode": "closed" } }
                        ]
                    },
                    {
                        "date": "20261225",
                        "list": [
                            { "number": 1, "transition": { "time": "09:30", "ruleNumber": 3, "mode": "forward" } }
                        ]
                    }
                ]
            }
            """;

        private ExceptionCalendar BuildCalendar()
            => Deserialize<O2GExceptionCalendar>(CalendarJson).ToExceptionCalendar();

        #region ExceptionDates

        [Fact]
        public void ToExceptionCalendar_ParsesDatesCorrectly()
        {
            var calendar = BuildCalendar();

            calendar.ExceptionDates.Should().HaveCount(2);
            calendar.ExceptionDates.Should().Contain(new DateTime(2026, 7, 14));
            calendar.ExceptionDates.Should().Contain(new DateTime(2026, 12, 25));
        }

        [Fact]
        public void ToExceptionCalendar_EmptyCalendar_NoDates()
        {
            var calendar = Deserialize<O2GExceptionCalendar>("""{ "calendar": [] }""")
                .ToExceptionCalendar();

            calendar.ExceptionDates.Should().BeEmpty();
        }

        [Fact]
        public void ToExceptionCalendar_NullCalendar_NoDates()
        {
            var calendar = Deserialize<O2GExceptionCalendar>("""{}""")
                .ToExceptionCalendar();

            calendar.ExceptionDates.Should().BeEmpty();
        }

        #endregion

        #region GetTransitions

        [Fact]
        public void GetTransitions_FirstDate_ReturnsAllTransitions()
        {
            var calendar = BuildCalendar();

            var transitions = calendar.GetTransitions(new DateTime(2026, 7, 14));

            transitions.Should().HaveCount(2);
            transitions![0].TransitionTime.ToString().Should().Be("08:00");
            transitions![0].Mode.Should().Be(PilotOperatingMode.Normal);
            transitions![1].TransitionTime.ToString().Should().Be("12:00");
            transitions![1].Mode.Should().Be(PilotOperatingMode.Closed);
        }

        [Fact]
        public void GetTransitions_SecondDate_ReturnsCorrectTransitions()
        {
            var calendar = BuildCalendar();

            var transitions = calendar.GetTransitions(new DateTime(2026, 12, 25));

            transitions.Should().HaveCount(1);
            transitions![0].TransitionTime.ToString().Should().Be("09:30");
            transitions![0].Mode.Should().Be(PilotOperatingMode.Forward);
        }

        [Fact]
        public void GetTransitions_DateWithTimeComponent_StripsTime()
        {
            var calendar = BuildCalendar();

            var transitions = calendar.GetTransitions(new DateTime(2026, 7, 14, 15, 30, 0));

            transitions.Should().NotBeNull();
            transitions.Should().HaveCount(2);
        }

        [Fact]
        public void GetTransitions_NonExistingDate_ReturnsNull()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitions(new DateTime(2026, 1, 1)).Should().BeNull();
        }

        [Fact]
        public void GetTransitions_IsReadOnly()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitions(new DateTime(2026, 7, 14))
                .Should().BeAssignableTo<IReadOnlyList<Transition>>();
        }

        #endregion

        #region GetTransitionAt

        [Fact]
        public void GetTransitionAt_FirstIndex_ReturnsCorrectTransition()
        {
            var calendar = BuildCalendar();

            var transition = calendar.GetTransitionAt(new DateTime(2026, 7, 14), 0);

            transition.Should().NotBeNull();
            transition!.TransitionTime.ToString().Should().Be("08:00");
            transition.RuleNumber.Should().Be(1);
            transition.Mode.Should().Be(PilotOperatingMode.Normal);
        }

        [Fact]
        public void GetTransitionAt_SecondIndex_ReturnsCorrectTransition()
        {
            var calendar = BuildCalendar();

            var transition = calendar.GetTransitionAt(new DateTime(2026, 7, 14), 1);

            transition.Should().NotBeNull();
            transition!.RuleNumber.Should().Be(2);
            transition.Mode.Should().Be(PilotOperatingMode.Closed);
        }

        [Fact]
        public void GetTransitionAt_OutOfRangeIndex_ReturnsNull()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitionAt(new DateTime(2026, 7, 14), 5).Should().BeNull();
        }

        [Fact]
        public void GetTransitionAt_NonExistingDate_ReturnsNull()
        {
            var calendar = BuildCalendar();

            calendar.GetTransitionAt(new DateTime(2026, 1, 1), 0).Should().BeNull();
        }

        [Fact]
        public void GetTransitionAt_DateWithTimeComponent_StripsTime()
        {
            var calendar = BuildCalendar();

            var transition = calendar.GetTransitionAt(
                new DateTime(2026, 7, 14, 15, 30, 0), 0);

            transition.Should().NotBeNull();
            transition!.Mode.Should().Be(PilotOperatingMode.Normal);
        }

        #endregion

        #region Sparse index (1-based number in JSON)

        [Fact]
        public void ToExceptionCalendar_SparseTransitions_PlacedAtCorrectIndex()
        {
            var json = """
                {
                    "calendar": [
                        {
                            "date": "20260714",
                            "list": [
                                { "number": 2, "transition": { "time": "12:00", "ruleNumber": 2, "mode": "closed" } }
                            ]
                        }
                    ]
                }
                """;

            var calendar = Deserialize<O2GExceptionCalendar>(json).ToExceptionCalendar();

            calendar.GetTransitionAt(new DateTime(2026, 7, 14), 0).Should().BeNull();
            var second = calendar.GetTransitionAt(new DateTime(2026, 7, 14), 1);
            second.Should().NotBeNull();
            second!.TransitionTime.ToString().Should().Be("12:00");
        }

        #endregion
    }
}