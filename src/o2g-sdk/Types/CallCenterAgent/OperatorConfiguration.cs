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
    /// Represents the configuration of a CCD operator.
    /// <para>
    /// A CCD operator can be an <see cref="OperatorType.Agent"/> or an
    /// <see cref="OperatorType.Supervisor"/>. This class provides access to the
    /// operator's type, associated pro-ACD station, group memberships, skills,
    /// and feature settings such as headset usage, self-assignment capability
    /// and multiline configuration.
    /// </para>
    /// </summary>
    /// <seealso cref="ICallCenterAgent.GetConfigurationAsync(string)"/>
    public class OperatorConfiguration
    {
        /// <summary>
        /// The type of this CCD operator.
        /// </summary>
        /// <value>
        /// An <see cref="OperatorType"/> value indicating whether this operator
        /// is an <see cref="OperatorType.Agent"/> or a <see cref="OperatorType.Supervisor"/>.
        /// </value>
        public OperatorType Type { get; init; }

        /// <summary>
        /// The pro-ACD station extension number associated with this operator.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the pro-ACD station extension number,
        /// or <see langword="null"/> if no pro-ACD station is configured.
        /// </value>
        public string Proacd { get; init; }

        /// <summary>
        /// The agent groups this operator belongs to, including the preferred group if defined.
        /// </summary>
        /// <value>
        /// An <see cref="AgentGroups"/> object describing the operator's group memberships
        /// and preferred group, or <see langword="null"/> if no groups are configured.
        /// </value>
        public AgentGroups Groups { get; init; }

        /// <summary>
        /// The skills assigned to this operator.
        /// </summary>
        /// <value>
        /// An <see cref="AgentSkillSet"/> containing the operator's skills,
        /// or <see langword="null"/> if no skills are defined.
        /// </value>
        /// <seealso cref="ICallCenterAgent.ActivateSkillsAsync(System.Collections.Generic.List{int}, string)"/>
        /// <seealso cref="ICallCenterAgent.DeactivateSkillsAsync(System.Collections.Generic.List{int}, string)"/>
        public AgentSkillSet Skills { get; init; }

        /// <summary>
        /// Whether the operator can choose their own processing group.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator can self-assign a group;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool SelfAssign { get; init; }

        /// <summary>
        /// Whether the headset feature is enabled for this operator.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if headset functionality is enabled;
        /// <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// When enabled, the operator can answer calls using a headset device.
        /// </remarks>
        /// <seealso cref="ICallCenterAgent.LogonAsync(string, string, bool, string)"/>
        public bool Headset { get; init; }

        /// <summary>
        /// Whether the operator can request help from a supervisor.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator can request supervisor help;
        /// <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="ICallCenterAgent.RequestSupervisorHelpAsync(string)"/>
        public bool Help { get; init; }

        /// <summary>
        /// Whether the operator is configured for multiline handling.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator supports multiline call handling;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool Multiline { get; init; }
    }
}