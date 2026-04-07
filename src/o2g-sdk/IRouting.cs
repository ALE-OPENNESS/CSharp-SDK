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
using o2g.Events.Routing;
using o2g.Internal.Services;
using o2g.Types.RoutingNS;
using System;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// The <c>IRouting</c> service allows a user to manage forward, overflow, Do Not Disturb and activation of his remote extension device (if any).
    /// Using this service requires having a <b>TELEPHONY_ADVANCED</b> license.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <h3>Forward:</h3>
    /// A forward can be activated on the voice mail or on any number as far as this number is authorized by the 
    /// OmniPCX Enterprise numbering policy. Use one of the methods:
    /// <see cref="ForwardOnNumberAsync(string, Forward.ForwardCondition, string)"/> or <see cref="ForwardOnVoiceMailAsync(Forward.ForwardCondition, string)"/> to activate a forward.
    /// </para>
    /// <para>
    /// A <see cref="Forward.Condition"/> can be associated to the forward:
    /// <list type="table">
    /// <listheader>
    /// <term>Condition</term><description>Description</description>
    /// </listheader>
    /// <item><term>Immediate</term><description>Incoming calls are immediately forwarded on the target.</description></item>
    /// <item><term>Busy</term><description>Incoming calls are forwarded on the target if the user is busy.</description></item>
    /// <item><term>NoAnswer</term><description>Incoming calls are forwarded on the target if the user does not answer the call.</description></item>
    /// <item><term>BusyOrNoAnswer</term><description>One of the two last conditions.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// <h3>Overflow:</h3>
    /// An overflow can be activated on the voice mail (if any). Use the methods:
    /// <see cref="OverflowOnVoiceMailAsync(Overflow.OverflowCondition, string)"/> to activate an overflow.
    /// </para>
    /// <para>
    /// A <see cref="Overflow.Condition"/> can be associated to the overflow:
    /// <list type="table">
    /// <listheader>
    /// <term>Condition</term><description>Description</description>
    /// </listheader>
    /// <item><term>Busy</term><description>Incoming calls are redirected on the target if the user is busy.</description></item>
    /// <item><term>NoAnswer</term><description>Incoming calls are redirected on the target if the user does not answer the call.</description></item>
    /// <item><term>BusyOrNoAnswer</term><description>One of the two last conditions.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// <h3>Do Not Disturb:</h3>
    /// When the Do Not Disturb (DND) is activated, the user does not receive any call. The DND is activated using method <see cref="ActivateDndAsync(string)"/>.
    /// </para>
    /// <para>
    /// <h3>Remote extension activation:</h3>
    /// When a remote extension is not activated, it does not ring on incoming call. Use the method <see cref="ActivateRemoteExtensionAsync(string)"/> to activate the remote extension.
    /// </para>
    /// </remarks>
    public interface IRouting : IService
    {
        /// <summary>
        /// <c>RoutingStateChanged</c> event is raised for each routing modification.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnRoutingStateChangedEvent>> RoutingStateChanged;

        /// <summary>
        /// Allows to know what the specified user is allowed to do with <c>IRouting</c> services.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>A <see cref="RoutingCapabilities"/> in case of success; <see langword="null"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<RoutingCapabilities> GetCapabilitiesAsync(string loginName = null);

        /// <summary>
        /// Activate the routing on the specified user's remote extension device.
        /// <para>
        /// When the remote extension is activated, it rings on incoming call on the user 
        /// company phone. When it is deactivated, it never rings, but it can be used to 
        /// place an outgoing call.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if the remote extension is already activated.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> ActivateRemoteExtensionAsync(string loginName = null);

        /// <summary>
        /// Deactivate the routing on the specified user's remote extension device.
        /// <para>
        /// When the remote extension is activated, it rings on incoming call on the user 
        /// company phone. When it is deactivated, it never rings, but it can be used to 
        /// place an outgoing call.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> DeactivateRemoteExtensionAsync(string loginName = null);

        /// <summary>
        /// Get the Do Not Disturb state of the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="DndState"/> object that represents the Do Not Disturb state.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<DndState> GetDndStateAsync(string loginName = null);

        /// <summary>
        /// Activate the Do Not Disturb for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if the Do Not Disturb is already activated.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> ActivateDndAsync(string loginName = null);

        /// <summary>
        /// Cancel the Do Not Disturb for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if the Do Not Disturb is not activated.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> CancelDndAsync(string loginName = null);

        /// <summary>
        /// Get the forward state of the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="Forward"/> object that represents the forward state.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<Forward> GetForwardAsync(string loginName = null);

        /// <summary>
        /// Cancel the forward for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if there is no forward activated.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> CancelForwardAsync(string loginName = null);

        /// <summary>
        /// Activate a forward on the user voicemail with the specified condition.
        /// </summary>
        /// <param name="condition">The forward condition.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// If a forward was already activated, it is replaced by the new forward.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> ForwardOnVoiceMailAsync(Forward.ForwardCondition condition, string loginName = null);

        /// <summary>
        /// Activate a forward on the specified number with the specified condition, for the specifed user.
        /// </summary>
        /// <param name="number">The phone number on which the forward is activated.</param>
        /// <param name="condition">The forward condition.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// If a forward was already activated, it is replaced by the new forward.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> ForwardOnNumberAsync(string number, Forward.ForwardCondition condition, string loginName = null);

        /// <summary>
        /// Cancel the overflow for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if there is no overflow activated.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> CancelOverflowAsync(string loginName = null);

        /// <summary>
        /// Get the overflow state of the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="Overflow"/> object that represents the overflow state.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<Overflow> GetOverflowAsync(string loginName = null);

        /// <summary>
        /// Activate an overflow on the user voicemail with the specified condition.
        /// </summary>
        /// <param name="condition">The overflow condition.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// If an overflow was already activated, it is replaced by the new forward.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> OverflowOnVoiceMailAsync(Overflow.OverflowCondition condition, string loginName = null);

        // <summary>
        // Activate an overflow on the user associate with the specified condition.
        // </summary>
        // <param name="condition">The overflow condition.</param>
        // <param name="loginName">The user login name.</param>
        // <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        // <remarks>
        // <para>
        // If an overflow was already activated, it is replaced by the new forward.
        // </para>
        // <para>
        // If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        // but it is mandatory if the session has been opened by an administrator.
        // </para>
        // </remarks>
//        Task<bool> OverflowOnAssociateAsync(Overflow.OverflowCondition condition, string loginName = null);

        /// <summary>
        /// Get the global routing state of the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="RoutingState"/> object that represents the user routing state.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<RoutingState> GetRoutingStateAsync(string loginName = null);

        /// <summary>
        /// Ask a snapshot event to receive an <see cref="OnRoutingStateChangedEvent"/> event notification.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnRoutingStateChangedEvent"/> will contain the dynamic forward, overflow, do not disturb an remote extension activation status.
        /// If a second request is asked since the previous one is still in progress, it has no effect.
        /// </para>
        /// <para>
        /// If an administrator invokes <c>RequestSnapshotAsync</c> with <c>loginName = null</c>, the snapshot event request is done for all the users.
        /// The event processing can be long depending on the number of users.
        /// </para>
        /// </remarks>
        Task<bool> RequestSnapshotAsync(string loginName = null);
    }
}
