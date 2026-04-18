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

using o2g.Internal.Services;
using o2g.Types.AnalyticsNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// The <c>IAnalytics</c> service provides access to OmniPCX Enterprise charging information and incident reports.
    /// Using this service requires an <b>ANALYTICS</b> license and an administrative login.
    /// </summary>
    /// <remarks>
    /// O2G uses SSH to collect information from an OmniPCX Enterprise node, so <b>SSH must be enabled</b> on the node.
    /// </remarks>
    public interface IAnalytics : IService
    {
        /// <summary>
        /// Retrieves a list of incidents from the specified OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="last">An optional parameter that limits the query to the N most recent incidents. Pass <c>0</c> to retrieve all incidents currently in progress.</param>
        /// <returns>
        /// A list of <see cref="Incident"/> objects representing the incidents on the specified node, or <see langword="null"/> in case of error.
        /// </returns>
        Task<List<Incident>> GetIncidentsAsync(int nodeId, int last = 0);

        /// <summary>
        /// Retrieves the list of charging files available on the specified node.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="filter">An optional date range filter. When omitted, all available charging files are returned.</param>
        /// <returns>
        /// A list of <see cref="ChargingFile"/> objects representing the charging files available on the node, or <see langword="null"/> in case of error.
        /// </returns>
        /// <seealso cref="GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        Task<List<ChargingFile>> GetChargingFilesAsync(int nodeId, DateRange filter = null);

        /// <summary>
        /// Queries the charging information for the specified node using a date range filter and the given options.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="filter">An optional date range filter.</param>
        /// <param name="topResults">An optional limit to return only the top N tickets.</param>
        /// <param name="all"><see langword="true"/> to include tickets with a 0 cost; <see langword="false"/> to return aggregated totals per user.</param>
        /// <returns>
        /// A <see cref="ChargingResult"/> object representing the result of the query, or <see langword="null"/> in case of error or if the filter yields no results.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <c>all</c> is <see langword="true"/>, all tickets are returned, including zero-cost tickets and the called party.
        /// If <c>all</c> is <see langword="false"/>, the total charging information is returned per user, with the call count
        /// reflecting only calls that have a non-null charging cost.
        /// </para>
        /// <para>
        /// Processing is limited to a maximum of 100 charging files for performance reasons. If the date range filter is too
        /// wide and the number of files to process exceeds 100, the method fails and returns <see langword="null"/>.
        /// In that case, a narrower date range must be specified.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        Task<ChargingResult> GetChargingsAsync(int nodeId, DateRange filter = null, int? topResults = null, bool all = false);

        /// <summary>
        /// Queries the charging information for the specified node, processing the given charging files with the specified options.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="files">The list of charging files to process. Use <see cref="GetChargingFilesAsync(int, DateRange)"/> to obtain the available files.</param>
        /// <param name="topResults">An optional limit to return only the top N tickets.</param>
        /// <param name="all"><see langword="true"/> to include tickets with a 0 cost; <see langword="false"/> to return aggregated totals per user.</param>
        /// <returns>
        /// A <see cref="ChargingResult"/> object representing the result of the query, or <see langword="null"/> in case of error or if the specified files yield no results.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <c>all</c> is <see langword="true"/>, all tickets are returned, including zero-cost tickets and the called party.
        /// If <c>all</c> is <see langword="false"/>, the total charging information is returned per user, with the call count
        /// reflecting only calls that have a non-null charging cost.
        /// </para>
        /// <para>
        /// This method gives finer control over the request by letting the caller specify the exact list of charging files to
        /// process. The list size must not exceed 100 files; if it does, the method fails and returns <see langword="null"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetChargingFilesAsync(int, DateRange)"/>
        /// <seealso cref="GetChargingsAsync(int, DateRange, int?, bool)"/>
        Task<ChargingResult> GetChargingsAsync(int nodeId, List<ChargingFile> files, int? topResults = null, bool all = false);
    }
}
