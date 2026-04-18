/*
 * Copyright 2026 ALE International
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
using System.Text.Json.Serialization;

namespace o2g.Types.TelephonyNS.CallNS.AcdNS
{
    /// <summary>
    /// Represents an immutable collection of skills required to handle a call in a contact center.
    /// <para>
    /// Skills are used by the <em>Advanced Call Routing</em> strategy to influence how calls
    /// are distributed among agents. Each skill has a unique number, a proficiency level,
    /// and a flag indicating whether it is mandatory.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// // Create a call profile
    /// var profile = new CallProfile(
    ///     new CallProfile.Skill(101, level: 3, mandatory: true),
    ///     new CallProfile.Skill(102, level: 2, mandatory: false)
    /// );
    ///
    /// // Access a skill by number
    /// CallProfile.Skill skill = profile.Get(101);
    ///
    /// // Check if a skill exists
    /// bool hasSkill = profile.Contains(102);
    ///
    /// // Iterate over all skills
    /// foreach (CallProfile.Skill s in profile.Skills)
    ///     Console.WriteLine($"{s.Number}: level {s.Level}");
    ///
    /// // Pass to a telephony call
    /// await telephony.MakePilotOrRSICallAsync(deviceId, pilot, callProfile: profile);
    /// </code>
    /// </example>
    public class CallProfile
    {
        private readonly Dictionary<int, Skill> _skills;

        /// <summary>
        /// Creates an empty <see cref="CallProfile"/>.
        /// </summary>
        public CallProfile()
        {
            _skills = new Dictionary<int, Skill>();
        }

        /// <summary>
        /// Creates a <see cref="CallProfile"/> from a variable number of skills.
        /// </summary>
        /// <param name="skills">The skills to include in this profile.</param>
        public CallProfile(params Skill[] skills)
        {
            _skills = new Dictionary<int, Skill>();
            foreach (var skill in skills)
            {
                _skills[skill.Number] = skill;
            }
        }

        /// <summary>
        /// Creates a <see cref="CallProfile"/> from a collection of skills.
        /// </summary>
        /// <param name="skills">The skills to include in this profile.</param>
        public CallProfile(IEnumerable<Skill> skills)
        {
            _skills = new Dictionary<int, Skill>();
            foreach (var skill in skills)
            {
                _skills[skill.Number] = skill;
            }
        }

        /// <summary>
        /// Returns the skill with the specified number.
        /// </summary>
        /// <param name="number">The skill number (identifier).</param>
        /// <returns>
        /// The <see cref="Skill"/> with the given number, or <see langword="null"/> if not present.
        /// </returns>
        public Skill Get(int number)
            => _skills.TryGetValue(number, out var skill) ? skill : null;

        /// <summary>
        /// Determines whether a skill with the specified number exists in this profile.
        /// </summary>
        /// <param name="number">The skill number to search for.</param>
        /// <returns>
        /// <see langword="true"/> if the skill exists; <see langword="false"/> otherwise.
        /// </returns>
        public bool Contains(int number)
            => _skills.ContainsKey(number);

        /// <summary>
        /// Returns a read-only collection of all skill numbers in this profile.
        /// </summary>
        /// <value>
        /// A <see cref="IReadOnlyCollection{T}"/> of skill identifiers.
        /// </value>
        public IReadOnlyCollection<int> SkillNumbers
            => new ReadOnlyCollection<int>(_skills.Keys.ToList());

        /// <summary>
        /// Returns a read-only collection of all skills in this profile.
        /// </summary>
        /// <value>
        /// A <see cref="IReadOnlyCollection{T}"/> of <see cref="Skill"/> instances.
        /// </value>
        public IReadOnlyCollection<Skill> Skills
            => new ReadOnlyCollection<Skill>(_skills.Values.ToList());

        /// <summary>
        /// Converts this <see cref="CallProfile"/> to a list of <see cref="Skill"/>
        /// suitable for JSON serialization in telephony service requests.
        /// </summary>
        internal List<Skill> ToList() => _skills.Values.ToList();

        /// <summary>
        /// Represents a skill assigned to a <see cref="CallProfile"/>.
        /// <para>
        /// Each skill has a unique number (identifier), a proficiency level,
        /// and a flag indicating whether it is mandatory for advanced call routing.
        /// </para>
        /// </summary>
        public class Skill
        {
            /// <summary>
            /// The unique identifier of this skill.
            /// </summary>
            /// <value>
            /// An <see langword="int"/> that is the skill number.
            /// </value>
            [JsonPropertyName("skillNumber")]
            public int Number { get; init; }

            /// <summary>
            /// The proficiency level of this skill.
            /// <para>
            /// Higher values indicate greater expertise or priority for call routing.
            /// </para>
            /// </summary>
            /// <value>
            /// An <see langword="int"/> that represents the skill level.
            /// </value>
            [JsonPropertyName("expertEvalLevel")]
            public int Level { get; init; }

            /// <summary>
            /// Whether this skill is mandatory when evaluating a call profile.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the skill is mandatory; <see langword="false"/> otherwise.
            /// </value>
            [JsonPropertyName("acrStatus")]
            public bool Mandatory { get; init; }

            /// <summary>
            /// Constructs a new <see cref="Skill"/> instance.
            /// </summary>
            /// <param name="number">The unique skill identifier.</param>
            /// <param name="level">The skill proficiency level.</param>
            /// <param name="mandatory">Whether this skill is mandatory.</param>
            public Skill(int number, int level, bool mandatory)
            {
                Number = number;
                Level = level;
                Mandatory = mandatory;
            }
        }
    }
}
