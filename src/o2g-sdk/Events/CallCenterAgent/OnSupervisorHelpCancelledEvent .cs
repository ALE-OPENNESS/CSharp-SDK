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
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace o2g.Events.CallCenterAgent
{
    /// <summary>
    /// This event is raised when agent has requested the assistance of his supervisor and when 
    /// the request is canceled by the agent or when the request is rejected by the supervisor.
    /// This event is received by both the agent and the supervisor.
    /// </summary>
    public class OnSupervisorHelpCancelledEvent : O2GEvent
    {
        /// <summary>
        /// Return the supervisor login name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the agent or supervisor login name.
        /// </value>
        public string LoginName { get; init; }

        /// <summary>
        /// Return the agent or supervisor number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the agent or supervisor number.
        /// </value>
        /// <remarks>
        /// If the event is received by the supervisor, <c>OtherNumber</c> is the agent number. If this event is received by 
        /// the agent, <c>OtherNumber</c> is the supervisor number.
        /// </remarks>

        // Renaming of this property to have a better meaning
        [JsonPropertyName("agentNumber")]
        public string OtherNumber { get; init; }
    }
}
