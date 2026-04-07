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
    /// The <c>IAnalytics</c> service allows to retrieve OmniPCX entreprise charging information and incidents.
    /// Using this service requires having a <b>ANALYTICS</b> license. This service requires an administrative login.
    /// </summary>
    /// <remarks>
    /// O2G uses SSH to get the information from an OmniPCX Enterprise node. So <b>SSH must be enabled</b> on the OmniPCX Enterprise node to use this service.
    /// </remarks>
    public interface IAnalytics : IService
    {
        /// <summary>
        /// Return a list of incident from the specified OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="last">An optional parameter to limit te query to the N latest incident.</param>
        /// <returns>
        /// A list of <see cref="Incident"/> that represents the incidents raised on the specified OmniPCX Enterprise node, or <see langword="null"/> in case of error.
        /// </returns>
        Task<List<Incident>> GetIncidentsAsync(int nodeId, int last = 0);

        /// <summary>
        /// Get the list of charging files on the specified node.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="filter">An optional time range filter.</param>
        /// <returns>
        /// The list of <see cref="ChargingFile"/> that represents the charging files.
        /// </returns>
        /// <seealso cref="GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        Task<List<ChargingFile>> GetChargingFilesAsync(int nodeId, TimeRange filter = null);

        /// <summary>
        /// Query the charging information for the specified node, using the specified options.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="filter">An optional time range filter.</param>
        /// <param name="topResults">An optional filter used to return only the 'top N' tickets.</param>
        /// <param name="all">Optional filter, <see langword="true"/> to include tickets with a 0 cost.</param>
        /// <returns>
        /// A <see cref="ChargingResult"/> object that represents the result of the query or <see langword="null"/> in case of error or if the specified filter does not return any result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <c>all</c> option is <see langword="true"/>, all the tickets are returned, including the zero cost ticket, and with the called party; 
        /// If <c>all</c> option is <see langword="false"/> or omitted, the total of charging info is returned for each user, the call number giving 
        /// the number of calls with non null charging cost.
        /// </para>
        /// <para>
        /// The request processes charging files on the OmniPCX Enterprise. The processing is limited to a maximum of 100 files for performance reason. If the range filter is too large and 
        /// the number of file to process is greater than 100, the method fails and returns <see langword="null"/>. In this case, a smaller range must be specified.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetChargingsAsync(int, List{ChargingFile}, int?, bool)"/>
        Task<ChargingResult> GetChargingsAsync(int nodeId, TimeRange filter = null, int? topResults = null, bool all = false);


        /// <summary>
        /// Query the charging information for the specified node, using the specified options.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node id.</param>
        /// <param name="files">The list of file to process.</param>
        /// <param name="topResults">An optional filter used to return only the 'top N' tickets.</param>
        /// <param name="all">Optional filter, <see langword="true"/> to include tickets with a 0 cost.</param>
        /// <returns>
        /// A <see cref="ChargingResult"/> object that represents the result of the query or <see langword="null"/> in case of error or if the specified filter does not return any result.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <c>all</c> option is <see langword="true"/>, all the tickets are returned, including the zero cost ticket, and with the called party; 
        /// If <c>all</c> option is <see langword="false"/> or omitted, the total of charging info is returned for each user, the call number giving 
        /// the number of calls with non null charging cost.
        /// </para>
        /// <para>
        /// This method allows to better control the processign of the request by spefifying the ist of charging files to consider. The list size must be lower than 100.
        /// If the number of file to process is greater than 100, the method fails and returns <see langword="null"/>.
        /// </para>
        /// </remarks>
        /// <see cref="GetChargingFilesAsync(int, TimeRange)"/>
        /// <seealso cref="GetChargingsAsync(int, TimeRange, int?, bool)"/>
        Task<ChargingResult> GetChargingsAsync(int nodeId, List<ChargingFile> files, int? topResults = null, bool all = false);
    }
}
