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


using o2g.Events;
using o2g.Events.CallCenterAgent;
using o2g.Internal.Services;
using o2g.Types.CallCenterAgentNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>ICallCenterAgent</c> provides access to Contact Center features for CCD operators.
    /// A CCD operator can be either a CCD agent or a CCD supervisor.
    /// Using this service requires having a <b>CONTACTCENTER_AGENT</b> license.
    /// </summary>
    public interface ICallCenterAgent : IService
    {
        /// <summary>
        /// Occurs when an agent state has changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnAgentStateChangedEvent>> AgentStateChanged;

        /// <summary>
        /// Occurs when an agent requests help from their supervisor.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpRequestedEvent>> SupervisorHelpRequested;

        /// <summary>
        /// Occurs when an agent has requested the assistance of their supervisor and the request is
        /// cancelled by the agent or rejected by the supervisor.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpCancelledEvent>> SupervisorHelpCancelled;

        /// <summary>
        /// Occurs when agent skills have changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnAgentSkillChangedEvent>> AgentSkillChanged;

        /// <summary>
        /// Gets the operator configuration.
        /// </summary>
        /// <param name="loginName">The operator login name.</param>
        /// <returns>
        /// A <see cref="OperatorConfiguration"/> object that represents the operator configuration,
        /// or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<OperatorConfiguration> GetConfigurationAsync(string loginName = null);

        /// <summary>
        /// Gets the specified agent or supervisor state.
        /// </summary>
        /// <param name="loginName">The operator login name.</param>
        /// <returns>
        /// A <see cref="OperatorState"/> object that represents the operator state,
        /// or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="RequestSnaphotAsync(string)"/>
        Task<OperatorState> GetStateAsync(string loginName = null);

        /// <summary>
        /// Logs on an agent or a supervisor.
        /// </summary>
        /// <param name="proAcdNumber">The pro-ACD device number.</param>
        /// <param name="pgNumber">The agent processing group number.</param>
        /// <param name="headset">Activate the headset mode.</param>
        /// <param name="loginName">The CCD operator login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// For a supervisor, if the <c>pgNumber</c> is omitted, the supervisor is logged on out of group.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="LogoffAsync(string)"/>
        Task<bool> LogonAsync(string proAcdNumber, string pgNumber = null, bool headset = false, string loginName = null);

        /// <summary>
        /// Logs off an agent or a supervisor.
        /// </summary>
        /// <param name="loginName">The CCD operator login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and returns <see langword="true"/> if the agent or the supervisor is already logged off.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="LogonAsync(string, string, bool, string)"/>
        Task<bool> LogoffAsync(string loginName = null);

        /// <summary>
        /// Enters an agent group. Only for a supervisor.
        /// </summary>
        /// <param name="pgNumber">The agent processing group number.</param>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method is used by a supervisor to enter an agent group when in pre-assigned state
        /// (logged on but not in an agent group).
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="ExitAsync(string)"/>
        Task<bool> EnterAsync(string pgNumber, string loginName = null);

        /// <summary>
        /// Exits from an agent group. Only for a supervisor.
        /// </summary>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method is used by a supervisor to leave an agent group and go back to pre-assigned state
        /// (logged on but not in an agent group).
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="EnterAsync(string, string)"/>
        Task<bool> ExitAsync(string loginName = null);

        /// <summary>
        /// Puts the specified agent in wrapup state.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> SetWrapupAsync(string loginName = null);

        /// <summary>
        /// Puts the specified agent in ready state.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> SetReadyAsync(string loginName = null);

        /// <summary>
        /// Puts the specified agent in pause.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> SetPauseAsync(string loginName = null);

        /// <summary>
        /// Withdraws an agent with the specified reason.
        /// </summary>
        /// <param name="reason">The withdraw reason.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetWithdrawReasonsAsync(string, string)"/>
        Task<bool> SetWithdrawAsync(WithdrawReason reason, string loginName = null);

        /// <summary>
        /// Requests a supervisor to listen to the specified agent (permanent listening).
        /// </summary>
        /// <param name="agentNumber">The extension number of the agent to listen to.</param>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// On success, an <see cref="OnSupervisorHelpRequestedEvent"/> is raised for both the agent and the supervisor.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnSupervisorHelpRequestedEvent"/>
        /// <seealso cref="CancelPermanentListeningAsync(string)"/>
        Task<bool> RequestPermanentListeningAsync(string agentNumber, string loginName = null);

        /// <summary>
        /// Cancels a permanent listening by a supervisor.
        /// </summary>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// On success, an <see cref="OnSupervisorHelpCancelledEvent"/> is raised for both the agent and the supervisor.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="RequestPermanentListeningAsync(string, string)"/>
        Task<bool> CancelPermanentListeningAsync(string loginName = null);

        /// <summary>
        /// Requests intrusion in a CCD call.
        /// </summary>
        /// <param name="agentNumber">The extension number of the CCD agent who answers the CCD call.</param>
        /// <param name="intrusionMode">The intrusion mode.</param>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="ChangeIntrusionModeAsync(IntrusionMode, string)"/>
        Task<bool> RequestIntrusionAsync(string agentNumber, IntrusionMode intrusionMode = IntrusionMode.Normal, string loginName = null);

        /// <summary>
        /// Changes the intrusion mode.
        /// </summary>
        /// <param name="newIntrusionMode">The new intrusion mode.</param>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Calling this method allows changing the intrusion mode or cancelling an intrusion. To cancel an
        /// intrusion, pass the current mode in the <c>newIntrusionMode</c> parameter.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="RequestIntrusionAsync(string, IntrusionMode, string)"/>
        Task<bool> ChangeIntrusionModeAsync(IntrusionMode newIntrusionMode, string loginName = null);

        /// <summary>
        /// Requests help from the supervisor.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// On success, an <see cref="OnSupervisorHelpRequestedEvent"/> is raised for both the agent and the supervisor.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnSupervisorHelpRequestedEvent"/>
        /// <seealso cref="CancelSupervisorHelpRequestAsync(string, string)"/>
        Task<bool> RequestSupervisorHelpAsync(string loginName = null);

        /// <summary>
        /// Rejects a help request from an agent.
        /// </summary>
        /// <param name="agentNumber">The extension number of the agent who has requested help.</param>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method is invoked by a supervisor to reject a help request from an agent. On success,
        /// an <see cref="OnSupervisorHelpCancelledEvent"/> is raised.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="SupervisorHelpCancelled"/>
        Task<bool> RejectAgentHelpRequestAsync(string agentNumber, string loginName = null);

        /// <summary>
        /// Cancels a supervisor help request.
        /// </summary>
        /// <param name="supervisorNumber">The extension number of the requested supervisor.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method is invoked by an agent to cancel a help request. On success,
        /// an <see cref="OnSupervisorHelpCancelledEvent"/> is raised.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="RequestSupervisorHelpAsync(string)"/>
        Task<bool> CancelSupervisorHelpRequestAsync(string supervisorNumber, string loginName = null);

        /// <summary>
        /// Asks a snapshot event to receive an <see cref="OnAgentStateChangedEvent"/> event notification.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The <see cref="OnAgentStateChangedEvent"/> event contains the operator <see cref="OperatorState"/>.
        /// If a second request is asked while the previous one is still in progress, it has no effect.
        /// </para>
        /// <para>
        /// If an administrator invokes this method with <c>loginName</c> set to <see langword="null"/>,
        /// the snapshot event request is done for all the agents. The event processing can be long
        /// depending on the number of users.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetStateAsync(string)"/>
        /// <seealso cref="OperatorState"/>
        Task<bool> RequestSnaphotAsync(string loginName = null);

        /// <summary>
        /// Activates the specified skills.
        /// </summary>
        /// <param name="skillNumbers">The list of skill numbers to activate.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does not validate skill numbers. If a skill number is invalid (not assigned to
        /// the operator), it is ignored and the method still returns <see langword="true"/>.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="DeactivateSkillsAsync(List{int}, string)"/>
        Task<bool> ActivateSkillsAsync(List<int> skillNumbers, string loginName = null);

        /// <summary>
        /// Deactivates the specified skills.
        /// </summary>
        /// <param name="skillNumbers">The list of skill numbers to deactivate.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does not validate skill numbers. If a skill number is invalid (not assigned to
        /// the operator), it is ignored and the method still returns <see langword="true"/>.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="ActivateSkillsAsync(List{int}, string)"/>
        Task<bool> DeactivateSkillsAsync(List<int> skillNumbers, string loginName = null);

        /// <summary>
        /// Returns the list of withdraw reasons for the specified processing group.
        /// </summary>
        /// <param name="pgNumber">The agent processing group number.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns>
        /// A list of <see cref="WithdrawReason"/> objects representing the withdraw reasons defined
        /// in the agent processing group, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="SetWithdrawAsync(WithdrawReason, string)"/>
        Task<List<WithdrawReason>> GetWithdrawReasonsAsync(string pgNumber, string loginName = null);

    }
}
