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
using System.Linq;

namespace o2g.Types.CallCenterAgentNS
{
    /// <summary>
    /// <c>AgentSkillSet</c> class represents a set of skills for a CCD operator.
    /// </summary>
    public class AgentSkillSet
    {
        private readonly Dictionary<int, AgentSkill> _skills;

        internal Dictionary<int, AgentSkill> Map
        {
            init
            {
                _skills = value;
            }
        }


        /// <summary>
        /// Return the skill with the specified number.
        /// </summary>
        /// <param name="number">The skill number.</param>
        /// <value>
        /// The <see cref="AgentSkill"/> object or <see langword="null"/> if there is no skill with the specified number.
        /// </value>
        public AgentSkill this[int number]
        {
            get
            {
                if (_skills.ContainsKey(number))
                {
                    return _skills[number];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Determines whether the the specified skill exist in this skill set.
        /// </summary>
        /// <param name="number">The skill number to search.</param>
        /// <returns>
        /// <see langword="true"/> if the specified skill exists; <see langword="false"/> otherwise
        /// </returns>
        public bool Contains(int number) => _skills.ContainsKey(number);


        /// <summary>
        /// Return the list of the skill numbers.
        /// </summary>
        /// <value>
        /// A list of <see langword="int"/> that represents the skill numbers.
        /// </value>
        public List<int> Numbers { get => _skills.Keys.ToList(); }

        /// <summary>
        /// Return the list of skills.
        /// </summary>
        /// <returns>
        /// A list of <see cref="AgentSkill"/> that represents the skills of the operator.
        /// </returns>
        public List<AgentSkill> AsList() => _skills.Values.ToList();
    }
}
