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
using o2g.Types.DirectoryNS;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// The <c>IDirectory</c> service allows searching for contacts in the OmniPCX Enterprise phone book.
    /// Using this service requires having a <b>TELEPHONY_ADVANCED</b> license.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A directory search involves a sequence of operations:
    /// <list type="number">
    /// <item>Initiate the search with a set of criteria.</item>
    /// <item>Retrieve results using one or more subsequent calls to <see cref="GetResultsAsync"/>.</item>
    /// </list>
    /// </para>
    /// <para>
    /// For each session (user or administrator), only 5 concurrent searches are allowed.
    /// An unused search context is automatically freed after 1 minute.
    /// </para>
    /// </remarks>
    public interface IDirectory : IService
    {
        /// <summary>
        /// Initiates a directory search with the specified filter, limited to the specified
        /// number of results.
        /// </summary>
        /// <param name="filter">The search filter.</param>
        /// <param name="limit">The maximum number of results. The supported range is [1..100].</param>
        /// <param name="loginName">The target user's login name.</param>
        /// <returns>
        /// <see langword="true"/> if the search was successfully initiated; <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="Criteria"/>
        /// <seealso cref="GetResultsAsync"/>
        /// <seealso cref="CancelAsync"/>
        Task<bool> SearchAsync(Criteria filter, int? limit, string loginName = null);

        /// <summary>
        /// Cancels the current search query for the specified user.
        /// </summary>
        /// <param name="loginName">The target user's login name.</param>
        /// <returns>
        /// <see langword="true"/> if the search was successfully cancelled; <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="SearchAsync(Criteria, int?, string)"/>
        Task<bool> CancelAsync(string loginName = null);

        /// <summary>
        /// Retrieves the next available results for the current search.
        /// </summary>
        /// <param name="loginName">The target user's login name.</param>
        /// <returns>
        /// A <see cref="SearchResult"/> object in case of success; <see langword="null"/> otherwise.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <see cref="GetResultsAsync"/> is generally called in a loop. For each iteration:
        /// <list type="bullet">
        /// <item>
        /// <term>If the result code is <c>Nok</c></term>
        /// <description>the search is in progress but no results are available yet —
        /// it is recommended to wait before the next iteration (e.g., 500&#160;ms).</description>
        /// </item>
        /// <item>
        /// <term>If the result code is <c>Ok</c></term>
        /// <description>results are available and can be processed.</description>
        /// </item>
        /// <item>
        /// <term>If the result code is <c>Finish</c> or <c>TimeOut</c></term>
        /// <description>the search has ended — exit the loop.</description>
        /// </item>
        /// </list>
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="SearchAsync(Criteria, int?, string)"/>
        Task<SearchResult> GetResultsAsync(string loginName = null);
    }
}
