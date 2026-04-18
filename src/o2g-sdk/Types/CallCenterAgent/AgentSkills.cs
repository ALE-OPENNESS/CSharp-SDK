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

namespace o2g.Types.CallCenterAgentNS
{
    /// <summary>
    /// Represents a skill assigned to a CCD agent.
    /// <para>
    /// Skills are used by the <b>Advanced Call Routing</b> strategy to influence
    /// how calls are distributed among agents. Each skill has a unique identifier,
    /// a proficiency level, and may belong to a specific domain.
    /// </para>
    /// <para>
    /// An agent's skills are available via <see cref="AgentSkillSet"/>, which is returned
    /// as part of the agent configuration from <see cref="ICallCenterAgent.GetConfigurationAsync(string)"/>.
    /// </para>
    /// </summary>
    /// <seealso cref="AgentSkillSet"/>
    /// <seealso cref="ICallCenterAgent.GetConfigurationAsync(string)"/>
    public class AgentSkill
    {
        /// <summary>
        /// The unique identifier of this skill.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that uniquely identifies this skill.
        /// </value>
        public int Number { get; init; }

        /// <summary>
        /// The proficiency level of this skill.
        /// <para>
        /// A higher level typically indicates greater expertise or priority
        /// when routing calls using the Advanced Call Routing strategy.
        /// </para>
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that represents the skill level.
        /// </value>
        public int Level { get; init; }

        /// <summary>
        /// Whether this skill is currently active for the agent.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the skill is active; <see langword="false"/> otherwise.
        /// </value>
        public bool Active { get; init; }

        /// <summary>
        /// The domain this skill belongs to.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that identifies the domain of this skill.
        /// </value>
        /// <remarks>Added in O2G version 2.7.5.</remarks>
        public int Domain { get; init; }

        /// <summary>
        /// The full name of this skill.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the skill name,
        /// or <see langword="null"/> if not set.
        /// </value>
        /// <remarks>Added in O2G version 2.7.5.</remarks>
        public string Name { get; init; }

        /// <summary>
        /// The abbreviated name of this skill.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the abbreviated skill name,
        /// or <see langword="null"/> if not set.
        /// </value>
        /// <remarks>Added in O2G version 2.7.5.</remarks>
        public string AbvName { get; init; }
    }
}