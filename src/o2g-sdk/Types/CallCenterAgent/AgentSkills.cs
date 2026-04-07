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

namespace o2g.Types.CallCenterAgentNS
{
    /// <summary>
    /// <c>AgentSkill</c> class represents a CCD operator skill. Skills are use by the "Advanced Call Routing" 
    /// call distribution strategy.
    /// </summary>
    public class AgentSkill
    {
        /// <summary>
        /// Return the skill number. A unique identifier.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that is the unique identifier of the skill.
        /// </value>
        public int Number { get; init; }

        /// <summary>
        /// Return the skill level.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that represents the skill level.
        /// </value>
        public int Level { get; init; }

        /// <summary>
        /// Return whether the skill is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the skill is active; <see langword="false"/> otherwise.
        /// </value>
        public bool Active { get; init; }
    }
}
