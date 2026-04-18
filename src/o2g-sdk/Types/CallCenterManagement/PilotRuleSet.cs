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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace o2g.Types.CallCenterManagementNS
{
    /// <summary>
    /// Represents a collection of routing rules associated with a CCD pilot.
    /// <para>
    /// Provides convenient access to individual rules by number, checks for
    /// rule existence, and allows retrieval of all rule numbers or rules.
    /// </para>
    /// </summary>
    /// <seealso cref="Pilot.Rules"/>
    public class PilotRuleSet
    {
        private readonly Dictionary<int, PilotRule> _rules;

        /// <summary>
        /// Initializes a <see cref="PilotRuleSet"/> from a list of <see cref="PilotRule"/> objects.
        /// </summary>
        /// <param name="rules">The rules to include in this set.</param>
        internal PilotRuleSet(IEnumerable<PilotRule> rules)
        {
            _rules = new Dictionary<int, PilotRule>();
            foreach (var rule in rules)
                _rules[rule.RuleNumber] = rule;
        }

        /// <summary>
        /// The number of rules in this rule set.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that is the number of rules.
        /// </value>
        public int Count => _rules.Count;

        /// <summary>
        /// Whether this rule set contains no rules.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this rule set is empty; <see langword="false"/> otherwise.
        /// </value>
        public bool IsEmpty => _rules.Count == 0;

        /// <summary>
        /// Returns the rule with the specified number.
        /// </summary>
        /// <param name="number">The unique rule number.</param>
        /// <returns>
        /// The <see cref="PilotRule"/> with the specified number,
        /// or <see langword="null"/> if no rule with that number exists.
        /// </returns>
        public PilotRule Get(int number)
            => _rules.TryGetValue(number, out var rule) ? rule : null;

        /// <summary>
        /// Determines whether a rule with the specified number exists in this rule set.
        /// </summary>
        /// <param name="number">The rule number to search for.</param>
        /// <returns>
        /// <see langword="true"/> if the rule exists; <see langword="false"/> otherwise.
        /// </returns>
        public bool Contains(int number)
            => _rules.ContainsKey(number);

        /// <summary>
        /// The rule numbers contained in this rule set.
        /// </summary>
        /// <value>
        /// A read-only collection of <see langword="int"/> representing the rule numbers.
        /// </value>
        public IReadOnlyCollection<int> RuleNumbers
            => new ReadOnlyCollection<int>(_rules.Keys.ToList());

        /// <summary>
        /// All rules in this rule set.
        /// </summary>
        /// <value>
        /// A read-only collection of <see cref="PilotRule"/> objects.
        /// </value>
        public IReadOnlyCollection<PilotRule> Rules
            => new ReadOnlyCollection<PilotRule>(_rules.Values.ToList());

        public static implicit operator List<object>(PilotRuleSet v)
        {
            throw new NotImplementedException();
        }
    }
}
