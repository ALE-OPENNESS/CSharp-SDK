/*
* Copyright 2021 ALE International
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

namespace o2g.Types.AnalyticsNS
{
    /// <summary>
    /// <c>Charging</c> represents a charging digest information.
    /// </summary>
    public class Charging
    {
        /// <summary>
        /// Return the caller phone number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the caller phone number.
        /// </value>
        public string Caller { get; init; }

        /// <summary>
        /// Return the caller name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the caller name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Return the called phone number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the called phone number.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public string Called { get; init; }

        /// <summary>
        /// Return the initial dialed number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the initial dialed number.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public string InitialDialedNumber { get; init; }

        /// <summary>
        /// Return the number of charged calls.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the number of charged calls.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public int CallNumber { get; init; }

        /// <summary>
        /// Return the number of charged units.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the number of charged units.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public int ChargingUnits { get; init; }

        /// <summary>
        /// Return the call charging cost.
        /// </summary>
        /// <value>
        /// A <see langword="float"/> value that represents the call charging cost.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public float Cost { get; init; }

        /// <summary>
        /// Return the call start date.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the call start date.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Return the call duration.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents total duration of the charged calls.
        /// </value>
        public int Duration { get; init; }

        /// <summary>
        /// Return the call type.
        /// </summary>
        /// <value>
        /// A <see cref="o2g.Types.AnalyticsNS.CallType"/> value that represents the call type.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public CallType CallType { get; init; }

        /// <summary>
        /// Return the effective call duration.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the effective call duration.
        /// </value>
        public int EffectiveCallDuration { get; init; }

        /// <summary>
        /// Return the acting extension node number.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the acting extension node number.
        /// </value>
        public int ActingExtensionNumberNode { get; init; }

        /// <summary>
        /// Return the internal facilities.
        /// </summary>
        /// <value>
        /// A List of <see cref="TelFacility"/> value that represents the internal factilities.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public List<TelFacility> InternalFacilities { get; init; }

        /// <summary>
        /// Return the external facilities.
        /// </summary>
        /// <value>
        /// A List of <see cref="TelFacility"/> value that represents the external factilities.
        /// </value>
        /// <remarks>
        /// This information is provided only if the query has been done with the <c>all</c> option.
        /// </remarks>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
        public List<TelFacility> ExternalFacilities { get; init; }
    }
}
