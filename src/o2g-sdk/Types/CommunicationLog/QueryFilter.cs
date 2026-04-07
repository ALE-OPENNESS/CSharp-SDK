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

namespace o2g.Types.CommunicationLogNS
{
    /// <summary>
    /// <c>Query</c> class represents a communication log query filter. 
    /// It is used to filter records returned by a <see cref="ICommunicationLog.GetComRecordsAsync(QueryFilter, Page, bool, string)"/> query.
    /// </summary>
    public class QueryFilter
    {
        /// <summary>
        /// The optional record start date.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> object that is the start date from which the query searches for matching the records.
        /// </value>
        /// <remarks>
        /// When used, the query returns the records not older than this date. When omitted the query searches for matching records starting from the oldest.
        /// </remarks>
        public DateTime? After { get; set; }

        /// <summary>
        /// The optional record end date.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> object that is the date from which the query stop searching the records.
        /// </value>
        /// <remarks>
        /// When used the query returns the records older than this date. When omitted the query searches for matching records until the newest.
        /// </remarks>
        public DateTime? Before { get; set; }

        /// <summary>
        /// The search options.
        /// </summary>
        /// <value>
        /// A combination of <see cref="Option"/> to filter the query.
        /// </value>
        /// <remarks>
        /// <c>Options</c> is a combination of values from <see cref="Option"/> that can be used to filter the query. 
        /// <list type="table">
        /// <listheader><term>Value</term><description>Description</description></listheader>
        /// <item><term><see cref="Option.Unacknowledged"/></term><description>To return only the unacknowledged records</description></item>
        /// <item><term><see cref="Option.Unanswered"/></term><description>To return only the records corresponding to an unanswered call.</description></item>
        /// </list>
        /// </remarks>
        public Option? Options { get; set; }

        /// <summary>
        /// The call reference of the call to retrieve.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the call reference of the call to retrieve.
        /// </value>
        public string CallRef { get; set; }

        /// <summary>
        /// Filter on the records in which the user is engaged with this remote party.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the name of the remote party the user is engaged with.
        /// </value>
        public string RemotePartyId { get; set; }

        /// <summary>
        /// Filter on the user's role in the communication. 
        /// </summary>
        /// <value>
        /// A <see cref="Role"/> value that defines the user's role.
        /// </value>
        public Role? Role { get; set; }
    }
}
