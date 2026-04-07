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
    /// <c>ICallCenterAgent</c> provides services for CCD operators.
    /// Using this service requires having a <b>CONTACTCENTER_AGENT</b> license.
    /// </summary>
    public interface ICallCenterAgent : IService
    {
        /// <summary>
        /// Occurs when an agent state has changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnAgentStateChangedEvent>> AgentStateChanged;

        /// <summary>
        /// Occurs when an agent request help from his supervisor
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpRequestedEvent>> SupervisorHelpRequested;

        /// <summary>
        /// Occurs when an agent has requested the assistance of his supervisor and when the request is 
        /// canceled by the agent or when the request is rejected by the supervisor.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpCancelledEvent>> SupervisorHelpCancelled;

        /// <summary>
        /// Occurs when agent skills have changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnAgentSkillChangedEvent>> AgentSkillChanged;

        /// <summary>
        /// Get the operator configuration.
        /// </summary>
        /// <param name="loginName">The operator login name.</param>
        /// <returns>
        /// A <see cref="OperatorConfiguration"/> object that represents the operator configuration.
        /// </returns>
        Task<OperatorConfiguration> GetOperatorConfigurationAsync(string loginName = null);

        /// <summary>
        /// Get the specified agent or supervisor state.
        /// </summary>
        /// <param name="loginName">The operator login name.</param>
        /// <returns>
        /// A <see cref="OperatorState"/> object thats represents the operator state.
        /// </returns>
        Task<OperatorState> GetOperatorStateAsync(string loginName = null);

        /// <summary>
        /// Logon an agent or a supervisor.
        /// </summary>
        /// <param name="proAcdNumber">The pro-acd device number.</param>
        /// <param name="pgNumber">The agent processing group number.</param>
        /// <param name="headset">Headset mode.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// For a Supervisor, if the <c>pgNumber</c> is omitted, the supervisor is logged on out off group.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnAgentStateChangedEvent"/>
        Task<bool> LogonOperatorAsync(string proAcdNumber, string pgNumber = null, bool headset = false, string loginName = null);

        /// <summary>
        /// Logoff an agent or a supervisor.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing an return <see langword="true"/> if the agent or the supervisor is already logged off.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnAgentStateChangedEvent"/>
        Task<bool> LogoffOperatorAsync(string loginName = null);

        /// <summary>
        /// Enter in a agent group. Only for a supervisor.
        /// </summary>
        /// <param name="pgNumber">The agent processing group number.</param>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method is used by a supervisor to enter an agent group when it is in pre-assigned state (logged but not in an agent group).
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> EnterAgentGroupAsync(string pgNumber, string loginName = null);

        /// <summary>
        /// Exit from an agent group. Only for a supervisor.
        /// </summary>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method is used by a supervisor to leave an agent group an go back in pre-assigned state (logged but not in an agent group).
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> ExitAgentGroupAsync(string loginName = null);

        /// <summary>
        /// Put the specified agent in wrapup.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> SetWrapupAsync(string loginName = null);

        /// <summary>
        /// Put the specified agent in ready state.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> SetReadyAsync(string loginName = null);

        /// <summary>
        /// Put the specified agent in pause.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> SetPauseAsync(string loginName = null);

        /// <summary>
        /// Withdraw an agent with the specified reason.
        /// </summary>
        /// <param name="reason">The withdraw reason.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> SetWithdrawAsync(WithdrawReason reason, string loginName = null);

        /// <summary>
        /// Request to listen to the agent by a supervisor.
        /// </summary>
        /// <param name="agentNumber">The listened agent number.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// On success, an <see cref="OnSupervisorHelpRequestedEvent"/> is raised. for both the agent and the supervisor.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnSupervisorHelpRequestedEvent"/>
        Task<bool> RequestPermanentListeningAsync(string agentNumber, string loginName = null);

        /// <summary>
        /// Request intrusion in a ccd call.
        /// </summary>
        /// <param name="agentNumber">The extension number of the ccd agent who answers the ccd call.</param>
        /// <param name="intrusionMode">The intrusion mode.</param>
        /// <param name="loginName">The supervisor login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> RequestIntrusionAsync(string agentNumber, IntrusionMode intrusionMode = IntrusionMode.Normal, string loginName = null);

        /// <summary>
        /// Change the intrusion mode.
        /// </summary>
        /// <param name="newIntrusionMode">The new intrusion mode.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// Calling this method allows to change the intrusion mode, or to cancel an intrusion. To cancel an intrusion,
        /// the application must pass the current mode in the <c>newIntrusionMode</c> parameter.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> ChangeIntrusionModeAsync(IntrusionMode newIntrusionMode, string loginName = null);

        /// <summary>
        /// Request help of the supervisor.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// On success, an <see cref="OnSupervisorHelpRequestedEvent"/> is raised. for both the agent and the supervisor.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnSupervisorHelpRequestedEvent"/>
        Task<bool> RequestSupervisorHelpAsync(string loginName = null);

        /// <summary>
        /// Reject an help request from an agent.
        /// </summary>
        /// <param name="agentNumber">The extension number of the agent who has requested help.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method is invoked by a supervisor when he reject an help request from an agent. On success, 
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
        /// Cancel a supervisor help request.
        /// </summary>
        /// <param name="supervisorNumber">The supervisor number.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method is invoked by an agent when he want to cancel an help request. On success, 
        /// an <see cref="OnSupervisorHelpCancelledEvent"/> is raised.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> CancelSupervisorHelpRequestAsync(string supervisorNumber, string loginName = null);

        /// <summary>
        /// Ask a snapshot event to receive an <see cref="OnAgentStateChangedEvent"/> event notification.
        /// </summary>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnAgentStateChangedEvent"/> will contain the <see cref="OperatorState"/>.
        /// If a second request is asked since the previous one is still in progress, it has no effect.
        /// </para>
        /// <para>
        /// If an administrator invokes <c>RequestSnapshotAsync</c> with <c>loginName = null</c>, the snapshot event request is done for all the agents.
        /// The event processing can be long depending on the number of users.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetOperatorStateAsync(string)"/>
        /// <seealso cref="OperatorState"/>
        Task<bool> RequestSnaphotAsync(string loginName = null);

        /// <summary>
        /// Activate a list of skills for this agent.
        /// </summary>
        /// <param name="skillNumbers">The list of skill to activate.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Skills are referenced by their numbers. <see cref="AgentSkill"/>. This method return <see langword="true"/> whatever the skill numbers passed. 
        /// Only valid skills are considered.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> ActivateSkillAsync(List<int> skillNumbers, string loginName = null);

        /// <summary>
        /// Deactivate a list of skills for this agent.
        /// </summary>
        /// <param name="skillNumbers">The list of skill to deactivate.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Skills are referenced by their numbers. <see cref="AgentSkill"/>. This method return <see langword="true"/> whatever the skill numbers passed. 
        /// Only valid skills are considered.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> DeactivateSkillAsync(List<int> skillNumbers, string loginName = null);

        /// <summary>
        /// Return the list of withdraw reason for the specified processing group.
        /// </summary>
        /// <param name="pgNumber">The agent processing group number.</param>
        /// <param name="loginName">The agent login name.</param>
        /// <returns>
        /// A list of <see cref="WithdrawReason"/> that represents the withdraw reason defined in the agent processing group in case of success;
        /// or a <see langword="null"/> value in case or error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<WithdrawReason>> GetWithdrawReasonsAsync(string pgNumber, string loginName = null);

    }
}
