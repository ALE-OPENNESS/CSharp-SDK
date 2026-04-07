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

namespace o2g.Types.CallCenterAgentNS
{
    /// <summary>
    /// <c>AgentGroups</c> represents the group configuration of a CCD operator. 
    /// The list of groups the operator is attaching in and the preferred group.
    /// </summary>
    public class AgentGroups
    {
        /// <summary>
        /// Return the preferred agent group.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the preferred agent group in which the CCD operator logs in, 
        /// or a <see langword="null"/> if there is no preferred group.
        /// </value>
        public string Preferred { get; init; }

        /// <summary>
        /// Return the list of agent groups the operator is attaching in.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the agent groups the operator is attaching in.
        /// </value>
        [JsonPropertyName("processingGroups")]
        public List<string> Groups { get; init; }
    }
}
