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

using o2g.Types.CallCenterAgentNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace o2g.Types.TelephonyNS.CallNS.AcdNS
{
    /// <summary>
    /// <c>AcrSkill</c> represents a skill associated to a call. When the "Advanced Call Routing" call distribution strategy is configured,
    /// the CCD agent are selected according to their <see cref="AgentSkill"/>. The selected agent skills must match the skill required by the call.
    /// </summary>
    public class AcrSkill
    {
        /// <summary>
        /// The skill number.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that is the unique identifier of the skill.
        /// </value>
        public int Number { get; init; }

        /// <summary>
        /// Return whether this skill is a mandatory skill.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the skill is mandatory; <see langword="false"/> otherwise.
        /// </value>
        [JsonPropertyName("acrStatus")]
        public bool Mandatory { get; init; }

        /// <summary>
        /// Return the skill level.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that represents the skill level required.
        /// </value>
        public int ExpertEvalLevel { get; init; }
    }
}
