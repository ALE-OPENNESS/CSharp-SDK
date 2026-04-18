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

using o2g.Types.CallCenterManagementNS.CalendarNS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace o2g.Internal.Types.CallCenterManagementNS
{
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: O2GDayOfWeek.monday)]
    internal enum O2GDayOfWeek
    {
        [EnumMember(Value = "monday")] monday,
        [EnumMember(Value = "tuesday")] tuesday,
        [EnumMember(Value = "wednesday")] wednesday,
        [EnumMember(Value = "thursday")] thursday,
        [EnumMember(Value = "friday")] friday,
        [EnumMember(Value = "saturday")] saturday,
        [EnumMember(Value = "sunday")] sunday
    }

    internal class O2GTransitionJson
    {
        public string Time { get; set; }
        public int RuleNumber { get; set; }
        public PilotOperatingMode Mode { get; set; }
    }

    internal class O2GTransitionEntry
    {
        public int Number { get; set; }

        [JsonPropertyName("transition")]
        public O2GTransitionJson TransitionData { get; set; }

        internal o2g.Types.CallCenterManagementNS.CalendarNS.Transition ToTransition()
        {
            return new o2g.Types.CallCenterManagementNS.CalendarNS.Transition
            {
                TransitionTime = o2g.Types.CallCenterManagementNS.CalendarNS.Transition.Time.Parse(TransitionData.Time),
                RuleNumber = TransitionData.RuleNumber,
                Mode = TransitionData.Mode
            };
        }
    }

    internal class O2GNormalDay
    {
        public O2GDayOfWeek Day { get; set; }
        public List<O2GTransitionEntry> List { get; set; }

        private DayOfWeek ToDayOfWeek() =>
            Day switch
            {
                O2GDayOfWeek.monday => DayOfWeek.Monday,
                O2GDayOfWeek.tuesday => DayOfWeek.Tuesday,
                O2GDayOfWeek.wednesday => DayOfWeek.Wednesday,
                O2GDayOfWeek.thursday => DayOfWeek.Thursday,
                O2GDayOfWeek.friday => DayOfWeek.Friday,
                O2GDayOfWeek.saturday => DayOfWeek.Saturday,
                O2GDayOfWeek.sunday => DayOfWeek.Sunday,
                _ => DayOfWeek.Monday
            };

        internal (DayOfWeek, List<o2g.Types.CallCenterManagementNS.CalendarNS.Transition>) ToDayTransitions()
        {
            var transitions = TransitionListBuilder.Build(List);
            return (ToDayOfWeek(), transitions);
        }
    }

    internal class O2GExceptionDay
    {
        public string Date { get; set; }
        public List<O2GTransitionEntry> List { get; set; }

        internal DateTime GetDate() =>
            DateTime.ParseExact(Date, "yyyyMMdd", CultureInfo.InvariantCulture).Date;

        internal (DateTime, List<o2g.Types.CallCenterManagementNS.CalendarNS.Transition>) ToDayTransitions()
        {
            var transitions = TransitionListBuilder.Build(List);
            return (GetDate(), transitions);
        }
    }

    internal class O2GNormalCalendar
    {
        public List<O2GNormalDay> Calendar { get; set; }

        internal NormalCalendar ToNormalCalendar()
        {
            var dayTransitions = new Dictionary<DayOfWeek, List<Transition>>();

            if (Calendar != null)
            {
                foreach (var day in Calendar)
                {
                    var (dayOfWeek, transitions) = day.ToDayTransitions();
                    dayTransitions[dayOfWeek] = transitions;
                }
            }

            return new NormalCalendar(dayTransitions);
        }
    }

    internal class O2GExceptionCalendar
    {
        public List<O2GExceptionDay> Calendar { get; set; }

        internal ExceptionCalendar ToExceptionCalendar()
        {
            var dayTransitions = new Dictionary<DateTime, List<Transition>>();

            if (Calendar != null)
            {
                foreach (var day in Calendar)
                {
                    var (date, transitions) = day.ToDayTransitions();
                    dayTransitions[date] = transitions;
                }
            }

            return new ExceptionCalendar(dayTransitions);
        }
    }

    internal class O2GCalendar
    {
        public O2GNormalCalendar NormalDays { get; set; }
        public O2GExceptionCalendar ExceptionDays { get; set; }

        internal o2g.Types.CallCenterManagementNS.CalendarNS.Calendar ToCalendar() =>
            new o2g.Types.CallCenterManagementNS.CalendarNS.Calendar
            {
                NormalDays = NormalDays?.ToNormalCalendar(),
                ExceptionDays = ExceptionDays?.ToExceptionCalendar()
            };
    }

    // Shared helper — builds a 1-based indexed transition list
    internal static class TransitionListBuilder
    {
        internal static List<Transition> Build(
            List<O2GTransitionEntry> entries)
        {
            if (entries == null || entries.Count == 0)
                return new List<Transition>();

            var maxIndex = 0;
            foreach (var e in entries)
                maxIndex = Math.Max(maxIndex, e.Number);

            var list = new List<Transition>(
                new Transition[maxIndex]);

            foreach (var e in entries)
                list[e.Number - 1] = e.ToTransition();

            return list;
        }
    }
}