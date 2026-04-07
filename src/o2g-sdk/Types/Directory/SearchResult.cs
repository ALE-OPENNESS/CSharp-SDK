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
using o2g.Types.CommonNS;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace o2g.Types.DirectoryNS
{
    /// <summary>
    /// <c>SearchResult</c> represents the result of a directory search.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// <c>Code</c> represents the status of a directory search.
        /// </summary>
        /// <remarks>
        /// Each time a call to <see cref="IDirectory.GetResultsAsync(string)"/> is done, the returned result code must be tested.
        /// </remarks>
        /// <seealso cref="IDirectory"/>
        [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Code.Unknown)]
        public enum Code
        {
            /// <summary>
            /// Responses are provided this time. Continue to invoking <see cref="IDirectory.GetResultsAsync(string)"/> periodically to get the next results.
            /// </summary>
            [EnumMember(Value = "OK")]
            Ok,

            /// <summary>
            /// No responses receive. Continue to invoking <see cref="IDirectory.GetResultsAsync(string)"/> to get more results.
            /// </summary>
            [EnumMember(Value = "NOK")]
            Nok,

            /// <summary>
            /// Search is finished. 
            /// </summary>
            [EnumMember(Value = "FINISH")]
            Finish,

            /// <summary>
            /// Search is ended for timeout reason.
            /// </summary>
            [EnumMember(Value = "TIMEOUT")]
            Timeout,

            /// <summary>
            /// Unknown result code.
            /// </summary>
            Unknown
        }

        /// <summary>
        /// <c>Results</c> represents a set of result from a directory search.
        /// </summary>
        public class Results
        {
            /// <summary>
            /// Return the contact found in a directory search.
            /// </summary>
            /// <value>
            /// A list of <see cref="PartyInfo"/> that represents the contacts matching the directory search filters.
            /// </value>
            public List<PartyInfo> Contacts { get; set; }
        }

        /// <summary>
        /// Return the result code for this search result
        /// </summary>
        /// <value>
        /// A <see cref="Code"/> that gives the status and progress of the search request.
        /// </value>
        public Code ResultCode { get; set; }

        /// <summary>
        /// Return the search results
        /// </summary>
        /// <value>
        /// A list of <see cref="Results"/> containing the directory search results.
        /// </value>
        public List<Results> ResultElements { get; set; }
    }
}
