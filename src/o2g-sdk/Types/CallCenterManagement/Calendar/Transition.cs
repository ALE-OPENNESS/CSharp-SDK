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

using System;

namespace o2g.Types.CallCenterManagementNS.CalendarNS
{
    /// <summary>
    /// Represents a state transition of a CCD pilot.
    /// <para>
    /// Each transition indicates a change in the pilot's operating mode,
    /// triggered by a specific rule at a specific time of day.
    /// </para>
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Represents the time of a transition in a pilot calendar, expressed in 24-hour format (HH:mm).
        /// </summary>
        public class Time
        {
            /// <summary>
            /// The hour of the transition (0–23).
            /// </summary>
            /// <value>An <see langword="int"/> that is the hour component.</value>
            public int Hour { get; }

            /// <summary>
            /// The minute of the transition (0–59).
            /// </summary>
            /// <value>An <see langword="int"/> that is the minute component.</value>
            public int Minute { get; }

            /// <summary>
            /// Creates a new <see cref="Time"/> instance.
            /// </summary>
            /// <param name="hour">The hour (0–23).</param>
            /// <param name="minute">The minute (0–59).</param>
            public Time(int hour, int minute)
            {
                Hour = hour;
                Minute = minute;
            }

            /// <summary>
            /// Parses a time string in <c>HH:mm</c> format.
            /// </summary>
            /// <param name="value">The time string to parse.</param>
            /// <returns>A new <see cref="Time"/> instance.</returns>
            /// <exception cref="ArgumentException">
            /// If the string is null, empty, or contains out-of-range values.
            /// </exception>
            public static Time Parse(string value)
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"Invalid time format: null or empty");

                var parts = value.Split(':');
                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int hour) ||
                    !int.TryParse(parts[1], out int minute) ||
                    hour < 0 || hour > 23 ||
                    minute < 0 || minute > 59)
                {
                    throw new ArgumentException($"Invalid time format: {value}");
                }

                return new Time(hour, minute);
            }

            /// <summary>
            /// Returns the time formatted as <c>HH:mm</c>.
            /// </summary>
            public override string ToString()
                => $"{Hour:D2}:{Minute:D2}";
        }

        /// <summary>
        /// The time at which this transition occurs.
        /// </summary>
        /// <value>A <see cref="Time"/> representing the transition time.</value>
        public Time TransitionTime { get; init; }

        /// <summary>
        /// The number of the rule that triggered this transition.
        /// </summary>
        /// <value>An <see langword="int"/> that is the rule number.</value>
        public int RuleNumber { get; init; }

        /// <summary>
        /// The pilot's operating mode after this transition.
        /// </summary>
        /// <value>A <see cref="PilotOperatingMode"/> value.</value>
        public PilotOperatingMode Mode { get; init; }
    }
}