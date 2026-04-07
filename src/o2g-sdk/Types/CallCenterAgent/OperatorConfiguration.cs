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
    /// <c>Configuration</c> class represents a CCD operator configuration.
    /// </summary>
    public class OperatorConfiguration
    {
        /// <summary>
        /// Return the type of CCD operator, agent or supervisor.
        /// </summary>
        /// <value>
        /// A <see cref="OperatorType"/> that represents the type of CCD operator.
        /// </value>
        public OperatorType Type { get; init; }

        /// <summary>
        /// Return the associated pro-acd station.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the associated pro-acd station extension number, or 
        /// <see langword="null"/> if this operator has no associated pro-acd station.
        /// </value>
        public string Proacd { get; init; }

        /// <summary>
        /// Return the agents groups the operator is attached in and the preferred agent group if any.
        /// </summary>
        /// <value>
        /// A <see cref="AgentGroups"/> object that represents the agents groups the operator is 
        /// attached in and the preferred agent group if any.
        /// </value>
        public AgentGroups Groups { get; init; }

        /// <summary>
        /// Return the operator skills.
        /// </summary>
        /// <value>
        /// A <see cref="AgentSkillSet"/> object that represents the operator skills.
        /// </value>
        public AgentSkillSet Skills { get; init; }

        /// <summary>
        /// Return whether the operator is allowed to choose his agent group.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator can choose his agent group; <see langword="false"/> otherwise.
        /// </value>
        public bool SelfAssign { get; init; }

        /// <summary>
        /// Return whether the operator has the headset feature configured.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator has the headset feature configured; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// The headset feature allows a CCD operator to answer calls using a headset.
        /// </remarks>
        /// <seealso cref="ICallCenterAgent.LogonOperatorAsync(string, string, bool, string)"/>
        public bool Headset { get; init; }

        /// <summary>
        /// Return whether the operator can request help from a supervisor.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator can request help; <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="ICallCenterAgent.RequestSupervisorHelpAsync(string)"/>
        public bool Help { get; init; }

        /// <summary>
        /// Return whether the operator is multiline.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator is multiline; <see langword="false"/> otherwise.
        /// </value>
        public bool Multiline { get; init; }
    }
}
