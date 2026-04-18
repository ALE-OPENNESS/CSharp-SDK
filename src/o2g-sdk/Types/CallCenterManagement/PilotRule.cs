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

namespace o2g.Types.CallCenterManagementNS
{
    /// <summary>
    /// Represents a routing rule associated with a CCD pilot.
    /// <para>
    /// A pilot rule defines a specific routing configuration or behaviour for a
    /// CCD pilot. Each rule has a unique number, an optional display name, and
    /// an active status indicating whether it is currently applied.
    /// </para>
    /// </summary>
    /// <seealso cref="Pilot.Rules"/>
    public class PilotRule
    {
        /// <summary>
        /// The unique number identifying this rule.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that is the rule identifier.
        /// </value>
        public int RuleNumber { get; init; }

        /// <summary>
        /// The display name of this rule.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the rule name,
        /// or <see langword="null"/> if no name is defined.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Whether this routing rule is currently active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the rule is active and being applied;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool Active { get; init; }
    }
}