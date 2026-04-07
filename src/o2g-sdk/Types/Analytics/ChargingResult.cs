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
using System.Collections.Generic;

namespace o2g.Types.AnalyticsNS
{
    /// <summary>
    /// <c>ChargingResult</c> class represents the result of a query to the OmniPCX Enterprise.
    /// </summary>
    /// <seealso cref="IAnalytics.GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
    /// <seealso cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
    public class ChargingResult
    {
        /// <summary>
        /// Return the list of charging elements.
        /// </summary>
        /// <value>
        /// A list of <see cref="Charging"/> that represents the charging information.
        /// </value>
        public List<Charging> Chargings { get; init; }

        /// <summary>
        /// Return the range that has been specified to query this result.
        /// </summary>
        /// <value>
        /// A <see cref="Range"/> object that represents the range used to query this result, or null if the range was not specified.
        /// </value>
        public TimeRange Range { get; init; }

        /// <summary>
        /// Return the number of analysed charging files in the OmniPCX Enterprise.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the number of analysed charging files.
        /// </value>
        public int ChargingFileCount { get; init; }

        /// <summary>
        /// Return the total number of analysed tickets.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the total number of analysed tickets.
        /// </value>
        public int TotalTicketCount { get; init; }

        /// <summary>
        /// Return the total number of valuable tickets.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the total number of valuable tickets; tickets with a non zero cost.
        /// </value>
        public int ValuableTicketCount { get; init; }
    }
}
