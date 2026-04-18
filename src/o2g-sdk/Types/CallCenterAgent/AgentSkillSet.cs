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
using System.Collections.ObjectModel;
using System.Linq;

namespace o2g.Types.CallCenterAgentNS
{
    /// <summary>
    /// Represents the set of skills assigned to a CCD agent.
    /// <para>
    /// An <c>AgentSkillSet</c> is a collection of <see cref="AgentSkill"/> objects indexed by
    /// skill number, and also by domain and name for skills that have both defined.
    /// It is returned as part of the agent's configuration and state, and can be
    /// used to check which skills an agent has and whether specific skills are active.
    /// </para>
    /// </summary>
    /// <seealso cref="ICallCenterAgent.GetConfigurationAsync(string)"/>
    /// <seealso cref="ICallCenterAgent.ActivateSkillsAsync(List{int}, string)"/>
    /// <seealso cref="ICallCenterAgent.DeactivateSkillsAsync(List{int}, string)"/>
    public class AgentSkillSet
    {
        private readonly Dictionary<int, AgentSkill> _skillsByNumber;
        private readonly Dictionary<string, AgentSkill> _skillsByDomainAndName;

        internal Dictionary<int, AgentSkill> Map
        {
            init
            {
                _skillsByNumber = value;
                _skillsByDomainAndName = new Dictionary<string, AgentSkill>();
                foreach (var skill in value.Values)
                {
                    if (skill.Domain != 0 && skill.Name != null)
                    {
                        _skillsByDomainAndName[$"{skill.Domain}:{skill.Name}"] = skill;
                    }
                }
            }
        }

        /// <summary>
        /// The number of skills in this skill set.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that is the number of skills.
        /// </value>
        public int Count => _skillsByNumber.Count;

        /// <summary>
        /// Whether this skill set contains no skills.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this skill set is empty; <see langword="false"/> otherwise.
        /// </value>
        public bool IsEmpty => _skillsByNumber.Count == 0;

        /// <summary>
        /// Returns the skill with the specified number.
        /// </summary>
        /// <param name="number">The skill number.</param>
        /// <returns>
        /// The <see cref="AgentSkill"/> with the specified number,
        /// or <see langword="null"/> if no skill with that number exists.
        /// </returns>
        public AgentSkill Get(int number)
            => _skillsByNumber.TryGetValue(number, out var skill) ? skill : null;

        /// <summary>
        /// Returns the skill with the specified name in the given domain.
        /// </summary>
        /// <param name="domain">The domain identifier.</param>
        /// <param name="name">The skill name.</param>
        /// <returns>
        /// The <see cref="AgentSkill"/> with the given name in the specified domain,
        /// or <see langword="null"/> if not found.
        /// </returns>
        /// <remarks>Added in O2G version 2.7.4.</remarks>
        public AgentSkill Get(int domain, string name)
            => _skillsByDomainAndName.TryGetValue($"{domain}:{name}", out var skill) ? skill : null;

        /// <summary>
        /// Determines whether a skill with the specified number exists in this skill set.
        /// </summary>
        /// <param name="number">The skill number to search for.</param>
        /// <returns>
        /// <see langword="true"/> if the skill exists; <see langword="false"/> otherwise.
        /// </returns>
        public bool Contains(int number)
            => _skillsByNumber.ContainsKey(number);

        /// <summary>
        /// Determines whether a skill with the specified name exists in the given domain.
        /// </summary>
        /// <param name="domain">The domain identifier.</param>
        /// <param name="name">The skill name.</param>
        /// <returns>
        /// <see langword="true"/> if the skill exists in the domain; <see langword="false"/> otherwise.
        /// </returns>
        /// <remarks>Added in O2G version 2.7.4.</remarks>
        public bool Contains(int domain, string name)
            => _skillsByDomainAndName.ContainsKey($"{domain}:{name}");

        /// <summary>
        /// The skill numbers contained in this skill set.
        /// </summary>
        /// <value>
        /// A read-only collection of <see langword="int"/> representing the skill numbers.
        /// </value>
        public IReadOnlyCollection<int> SkillNumbers
            => new ReadOnlyCollection<int>(_skillsByNumber.Keys.ToList());

        /// <summary>
        /// All skills in this skill set.
        /// </summary>
        /// <value>
        /// A read-only collection of <see cref="AgentSkill"/> objects.
        /// </value>
        public IReadOnlyCollection<AgentSkill> Skills
            => new ReadOnlyCollection<AgentSkill>(_skillsByNumber.Values.ToList());
    }
}