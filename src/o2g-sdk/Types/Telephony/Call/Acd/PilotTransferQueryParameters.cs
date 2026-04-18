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

namespace o2g.Types.TelephonyNS.CallNS.AcdNS
{
    /// <summary>
    /// Represents the set of criteria used to query a CCD pilot for call transfer possibilities.
    /// <para>
    /// Each criterion is optional. If a field is not set, it will be ignored when building the query.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// var params = new PilotTransferQueryParameters()
    ///     .SetAgentNumber("1234")
    ///     .SetPriorityTransfer(true)
    ///     .SetCallProfile(profile);
    /// </code>
    /// </example>
    /// <remarks>Added in version 2.7.4</remarks>
    public class PilotTransferQueryParameters
    {
        private string _agentNumber;
        private bool? _priorityTransfer;
        private bool? _supervisedTransfer;
        private CallProfile _callProfile;

        /// <summary>
        /// The agent number criterion.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the agent number to filter by,
        /// or <see langword="null"/> if not set.
        /// </value>
        public string AgentNumber => _agentNumber;

        /// <summary>
        /// The priority transfer criterion.
        /// </summary>
        /// <value>
        /// <see langword="true"/> for priority transfers, <see langword="false"/> for
        /// non-priority, or <see langword="false"/> if not set.
        /// </value>
        public bool PriorityTransfer => _priorityTransfer ?? false;

        /// <summary>
        /// The supervised transfer criterion.
        /// </summary>
        /// <value>
        /// <see langword="true"/> for supervised transfers, <see langword="false"/> for
        /// unsupervised, or <see langword="false"/> if not set.
        /// </value>
        public bool SupervisedTransfer => _supervisedTransfer ?? false;

        /// <summary>
        /// The call profile criterion.
        /// </summary>
        /// <value>
        /// A <see cref="CallProfile"/> to match, or <see langword="null"/> if not set.
        /// </value>
        public CallProfile CallProfile => _callProfile;

        /// <summary>
        /// Whether an agent number criterion has been set.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if <see cref="AgentNumber"/> is defined and non-empty;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool HasAgentNumber =>
            !string.IsNullOrWhiteSpace(_agentNumber);

        /// <summary>
        /// Whether the priority transfer criterion is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if <see cref="PriorityTransfer"/> has been explicitly set;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool HasPriorityTransferCriteria => _priorityTransfer.HasValue;

        /// <summary>
        /// Whether the supervised transfer criterion is active.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if <see cref="SupervisedTransfer"/> has been explicitly set;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool HasSupervisedTransferCriteria => _supervisedTransfer.HasValue;

        /// <summary>
        /// Whether a call profile criterion has been set.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if <see cref="CallProfile"/> is defined;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool HasCallProfile => _callProfile != null;

        /// <summary>
        /// Sets the agent number criterion.
        /// </summary>
        /// <param name="agentNumber">The agent number to filter by.</param>
        /// <returns>This instance for fluent chaining.</returns>
        public PilotTransferQueryParameters SetAgentNumber(string agentNumber)
        {
            _agentNumber = agentNumber;
            return this;
        }

        /// <summary>
        /// Sets the priority transfer criterion.
        /// </summary>
        /// <param name="priorityTransfer">
        /// <see langword="true"/> for priority only, <see langword="false"/> for non-priority,
        /// or <see langword="null"/> to clear the criterion.
        /// </param>
        /// <returns>This instance for fluent chaining.</returns>
        public PilotTransferQueryParameters SetPriorityTransfer(bool? priorityTransfer)
        {
            _priorityTransfer = priorityTransfer;
            return this;
        }

        /// <summary>
        /// Sets the supervised transfer criterion.
        /// </summary>
        /// <param name="supervisedTransfer">
        /// <see langword="true"/> for supervised only, <see langword="false"/> for unsupervised,
        /// or <see langword="null"/> to clear the criterion.
        /// </param>
        /// <returns>This instance for fluent chaining.</returns>
        public PilotTransferQueryParameters SetSupervisedTransfer(bool? supervisedTransfer)
        {
            _supervisedTransfer = supervisedTransfer;
            return this;
        }

        /// <summary>
        /// Sets the call profile criterion.
        /// </summary>
        /// <param name="callProfile">
        /// The <see cref="CallProfile"/> to match, or <see langword="null"/> to clear the criterion.
        /// </param>
        /// <returns>This instance for fluent chaining.</returns>
        public PilotTransferQueryParameters SetCallProfile(CallProfile callProfile)
        {
            _callProfile = callProfile;
            return this;
        }
    }
}
