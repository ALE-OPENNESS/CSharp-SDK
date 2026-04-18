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
    public class ReportObservationPeriodTests
    {
        #region Simple periods

        [Fact]
        public void OnCurrentDay_HasCurrentDayType()
        {
            var p = ReportObservationPeriod.OnCurrentDay();
            p.PeriodType.Should().Be(ReportObservationPeriodType.CurrentDay);
        }

        [Fact]
        public void OnCurrentDay_LastUnitsIsMinusOne()
        {
            var p = ReportObservationPeriod.OnCurrentDay();
            p.LastUnits.Should().Be(-1);
        }

        [Fact]
        public void OnCurrentDay_DatesAreNull()
        {
            var p = ReportObservationPeriod.OnCurrentDay();
            p.BeginDate.Should().BeNull();
            p.EndDate.Should().BeNull();
        }

        [Fact]
        public void OnCurrentWeek_HasCurrentWeekType()
        {
            var p = ReportObservationPeriod.OnCurrentWeek();
            p.PeriodType.Should().Be(ReportObservationPeriodType.CurrentWeek);
        }

        [Fact]
        public void OnCurrentMonth_HasCurrentMonthType()
        {
            var p = ReportObservationPeriod.OnCurrentMonth();
            p.PeriodType.Should().Be(ReportObservationPeriodType.CurrentMonth);
        }

        [Fact]
        public void OnLastMonth_HasLastMonthType()
        {
            var p = ReportObservationPeriod.OnLastMonth();
            p.PeriodType.Should().Be(ReportObservationPeriodType.LastMonth);
        }

        #endregion

        #region LastDays

        [Fact]
        public void OnLastDays_HasLastDaysType()
        {
            var p = ReportObservationPeriod.OnLastDays(7);
            p.PeriodType.Should().Be(ReportObservationPeriodType.LastDays);
        }

        [Fact]
        public void OnLastDays_LastUnitsIsSet()
        {
            var p = ReportObservationPeriod.OnLastDays(7);
            p.LastUnits.Should().Be(7);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(31)]
        public void OnLastDays_BoundaryValues_Succeed(int nbDays)
        {
            var p = ReportObservationPeriod.OnLastDays(nbDays);
            p.LastUnits.Should().Be(nbDays);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(32)]
        public void OnLastDays_InvalidValues_ThrowsArgumentOutOfRangeException(int nbDays)
        {
            Action act = () => ReportObservationPeriod.OnLastDays(nbDays);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        #endregion

        #region LastWeeks

        [Fact]
        public void OnLastWeeks_HasLastWeeksType()
        {
            var p = ReportObservationPeriod.OnLastWeeks(2);
            p.PeriodType.Should().Be(ReportObservationPeriodType.LastWeeks);
        }

        [Fact]
        public void OnLastWeeks_LastUnitsIsSet()
        {
            var p = ReportObservationPeriod.OnLastWeeks(2);
            p.LastUnits.Should().Be(2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void OnLastWeeks_InvalidValues_ThrowsArgumentOutOfRangeException(int nbWeeks)
        {
            Action act = () => ReportObservationPeriod.OnLastWeeks(nbWeeks);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        #endregion

        #region FromDate

        [Fact]
        public void FromDate_HasFromDateToDateType()
        {
            var from = DateTime.Now.AddDays(-30);
            var p = ReportObservationPeriod.FromDate(from, 10);
            p.PeriodType.Should().Be(ReportObservationPeriodType.FromDateToDate);
        }

        [Fact]
        public void FromDate_BeginDateIsFromDate()
        {
            var from = DateTime.Now.AddDays(-30);
            var p = ReportObservationPeriod.FromDate(from, 10);
            p.BeginDate.Should().Be(from);
        }

        [Fact]
        public void FromDate_EndDateIsFromPlusNbDays()
        {
            var from = DateTime.Now.AddDays(-30);
            var p = ReportObservationPeriod.FromDate(from, 10);
            p.EndDate.Should().Be(from.AddDays(10));
        }

        [Fact]
        public void FromDate_FutureDate_ThrowsArgumentException()
        {
            Action act = () => ReportObservationPeriod.FromDate(DateTime.Now.AddDays(1), 10);
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(32)]
        public void FromDate_InvalidNbDays_ThrowsArgumentOutOfRangeException(int nbDays)
        {
            var from = DateTime.Now.AddDays(-30);
            Action act = () => ReportObservationPeriod.FromDate(from, nbDays);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        #endregion
    }
}
