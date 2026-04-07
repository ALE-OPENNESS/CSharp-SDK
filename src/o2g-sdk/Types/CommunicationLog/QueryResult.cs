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
using System.Text.Json.Serialization;

namespace o2g.Types.CommunicationLogNS
{
    /// <summary>
    /// <c>QueryResult</c> class represents the result of a communication log query.
    /// </summary>
    /// <remarks>
    /// The <c>ICommunicationLog</c> service provides a paging mechanism that allows the application to query the result by pages.
    /// </remarks>
    /// <seealso cref="ICommunicationLog.GetComRecordsAsync(QueryFilter, Page, bool, string)"/>
    public class QueryResult
    {
        /// <summary>
        /// Get the list of com record returned by the query.
        /// </summary>
        /// <value>
        /// A list of <see cref="ComRecord"/> that represents the call history.
        /// </value>
        [JsonPropertyName("comHistoryRecords")]
        public List<ComRecord> Records { get; init; }

        /// <summary>
        /// Return the offset of the first record which is returned.
        /// </summary>
        /// <value>
        /// The <see langword="int"/> value that is the first record offset.
        /// </value>
        public int Offset { get; init; }

        /// <summary>
        /// Return the number of record returned.
        /// </summary>
        /// <value>
        /// The <see langword="int"/> value that is the number of records returned.
        /// </value>
        public int Limit { get; init; }

        /// <summary>
        /// Return the total number of record the query has retrieved.
        /// </summary>
        /// <value>
        /// The <see langword="int"/> value that is the total number of records.
        /// </value>
        [JsonPropertyName("totalCount")]
        public int Count { get; init; }
    }
}
