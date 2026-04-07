/*
* Copyright 2025 ALE International
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
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterRealtimeNS
{
    /// <summary>
    /// Represents the subscription to CCD realtime events provided by the 
    /// <see cref="ICallCenterRealtime"/> service.
    /// </summary>
    public class RtiContext
    {
        /// <summary>
        /// Gets a value indicating whether this RTI context is currently active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the observation period in number of minutes.
        /// </summary>
        /// <remarks>
        /// Can be a value between 15 and 60 minutes. This defines the duration 
        /// during which the RTI context will actively receive realtime events.
        /// </remarks>
        [JsonPropertyName("obsPeriod")]
        public int ObservationPeriod { get; }

        /// <summary>
        /// Gets the notification frequency in number of seconds.
        /// </summary>
        /// <remarks>
        /// This indicates how often events are delivered. The minimum value is 5 seconds.
        /// </remarks>
        [JsonPropertyName("notifFrequency")]
        public int NotificationFrequency { get; }

        /// <summary>
        /// Gets the RTI filter associated with this context.
        /// </summary>
        /// <remarks>
        /// The filter defines which categories of CCD objects and their parameters 
        /// should be included in the realtime eventing.
        /// </remarks>
        public RtiFilter Filter { get; }

        /// <summary>
        /// Constructs a new <see cref="RtiContext"/> with the specified observation period, 
        /// notification frequency, and RTI filter.
        /// </summary>
        /// <param name="observationPeriod">
        /// The observation period in number of minutes (15–60 minutes).
        /// </param>
        /// <param name="notificationFrequency">
        /// The notification frequency in seconds (minimum 5 seconds).
        /// </param>
        /// <param name="filter">
        /// The RTI filter associated with this context.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="filter"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="observationPeriod"/> is not between 15 and 60,
        /// or if <paramref name="notificationFrequency"/> is less than 5.
        /// </exception>
        public RtiContext(int observationPeriod, int notificationFrequency, RtiFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter), "RTI filter cannot be null.");

            if (observationPeriod < 15 || observationPeriod > 60)
                throw new ArgumentOutOfRangeException(nameof(observationPeriod), "Observation period must be between 15 and 60 minutes.");

            if (notificationFrequency < 5)
                throw new ArgumentOutOfRangeException(nameof(notificationFrequency), "Notification frequency must be at least 5 seconds.");

            IsActive = false;
            ObservationPeriod = observationPeriod;
            NotificationFrequency = notificationFrequency;
            Filter = filter;
        }
    }
}
