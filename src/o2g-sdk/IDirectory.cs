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
    /// The <c>DirectoryService</c> is used to search contacts in the OmniPCX Enterprise phone book.Using this service requires having a <b>TELEPHONY_ADVANCED</b> license.
    /// </summary>
    /// <para>
    /// A directory search is a set of 2 or more sequential operations:
    /// <list type="bullet">
    /// <item>The first operation initiate the search with a set of criteria</item>
    /// <item>The second and next operations retrieve results</item>
    /// </list>
    /// </para>
    /// <remarks>
    /// Note: For each session (user or administrator), only 5 concurrent searches are authorized. An unused search context is freed after 1 minute.
    /// </remarks>
    public interface IDirectory : IService
    {
        /// <summary>
        /// Initiates a search for the specified user with the specified filter. The search will be limited to the specified number of results.
        /// </summary>
        /// <param name="filter">The search filter</param>
        /// <param name="limit">The maximum number of results. The range of supported values is [1 .. 100]</param>
        /// <param name="loginName">The user login name</param>
        /// <returns><c>true</c> in case of success; <c>false</c> otherwise.</returns>
        /// <seealso cref="Criteria"/>
        /// <remark>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, but it is mandatory if the session has been opened by an administrator.
        /// </remark>
        Task<bool> SearchAsync(Criteria filter, int? limit, string loginName = null);

        /// <summary>
        /// Cancel a search query for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name</param>
        /// <returns><c>true</c> in case of success; <c>false</c> otherwise.</returns>
        /// <seealso cref="SearchAsync(Criteria, int?, string)"/>
        /// <remark>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, but it is mandatory if the session has been opened by an administrator.
        /// </remark>
        Task<bool> CancelAsync(string loginName = null);

        /// <summary>
        /// Gets the next available results for the current search.
        /// <c>GetResultAsync</c> is generally called in a loop and for each iteration:
        /// <list type="bullet">
        /// <item>
        /// <term>If the status result code is <c>Nok</c></term><description>the search is in progress but no result is available : it is recommended to wait before the next iteration (500ms for example)</description>
        /// </item>
        /// <item>
        /// <term>If the status result code is <c>Ok</c></term><description>Results are available and can be processed.</description>
        /// </item>
        /// <item>
        /// <term>If the status result code is <c>Finish</c> or <c>TimeOut</c></term><description>The search is ended, exit from the loop.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns>A <see cref="SearchResult"/> object in case of success; <c>null</c> otherwise.</returns>
        /// <remark>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, but it is mandatory if the session has been opened by an administrator.
        /// </remark>
        Task<SearchResult> GetResultsAsync(string loginName = null);
    }
}
