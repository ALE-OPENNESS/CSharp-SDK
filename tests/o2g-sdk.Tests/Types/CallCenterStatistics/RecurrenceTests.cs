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
using o2g.Types.CallCenterStatisticsNS.Scheduled;
using System;
using Xunit;

namespace o2g.Tests.Types.CallCenterStatistics
{
    public class RecurrenceTests
    {
        #region Daily

        [Fact]
        public void Daily_HasDailyType()
        {
            var r = Recurrence.Daily();
            r.Type.Should().Be(RecurrenceType.DAILY);
        }

        [Fact]
        public void Daily_DaysInWeekIsNull()
        {
            var r = Recurrence.Daily();
            r.DaysInWeek.Should().BeNull();
        }

        [Fact]
        public void Daily_DayInMonthIsMinusOne()
        {
            var r = Recurrence.Daily();
            r.DayInMonth.Should().Be(-1);
        }

        #endregion

        #region Weekly

        [Fact]
        public void Weekly_HasWeeklyType()
        {
            var r = Recurrence.Weekly(DayOfWeek.Monday, DayOfWeek.Wednesday);
            r.Type.Should().Be(RecurrenceType.WEEKLY);
        }

        [Fact]
        public void Weekly_ContainsSpecifiedDays()
        {
            var r = Recurrence.Weekly(DayOfWeek.Monday, DayOfWeek.Wednesday);
            r.DaysInWeek.Should().BeEquivalentTo(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });
        }

        [Fact]
        public void Weekly_SingleDay_ContainsThatDay()
        {
            var r = Recurrence.Weekly(DayOfWeek.Friday);
            r.DaysInWeek.Should().ContainSingle().Which.Should().Be(DayOfWeek.Friday);
        }

        [Fact]
        public void Weekly_DayInMonthIsMinusOne()
        {
            var r = Recurrence.Weekly(DayOfWeek.Friday);
            r.DayInMonth.Should().Be(-1);
        }

        [Fact]
        public void Weekly_WithNoDays_ThrowsArgumentException()
        {
            Action act = () => Recurrence.Weekly();
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        #region Monthly

        [Fact]
        public void Monthly_HasMonthlyType()
        {
            var r = Recurrence.Monthly(15);
            r.Type.Should().Be(RecurrenceType.MONTHLY);
        }

        [Fact]
        public void Monthly_DayInMonthIsSet()
        {
            var r = Recurrence.Monthly(15);
            r.DayInMonth.Should().Be(15);
        }

        [Fact]
        public void Monthly_DaysInWeekIsNull()
        {
            var r = Recurrence.Monthly(15);
            r.DaysInWeek.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(31)]
        public void Monthly_BoundaryDays_Succeed(int day)
        {
            var r = Recurrence.Monthly(day);
            r.DayInMonth.Should().Be(day);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(32)]
        public void Monthly_InvalidDay_ThrowsArgumentOutOfRangeException(int day)
        {
            Action act = () => Recurrence.Monthly(day);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        #endregion
    }
}
