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
    /// Provides operations to manage a user's call routing configuration, including
    /// forward, overflow, Do Not Disturb, and remote extension activation.
    /// <para>
    /// Using this service requires a <b>TELEPHONY_ADVANCED</b> license.
    /// </para>
    /// <para>
    /// <b>Forward</b> — redirects incoming calls to the user's voice mail or to another
    /// phone number, subject to an optional <see cref="Forward.ForwardCondition"/>.
    /// A number must be authorized by the OmniPCX Enterprise numbering policy.
    /// Use <see cref="ForwardOnNumberAsync"/> or <see cref="ForwardOnVoiceMailAsync"/>
    /// to activate, and <see cref="CancelForwardAsync"/> to cancel.
    /// </para>
    /// <para>
    /// <b>Overflow</b> — redirects incoming calls to the user's voice mail when a
    /// specified <see cref="Overflow.OverflowCondition"/> is met. Unlike a forward,
    /// an overflow only applies when no forward is active.
    /// Use <see cref="OverflowOnVoiceMailAsync"/> to activate, and
    /// <see cref="CancelOverflowAsync"/> to cancel.
    /// </para>
    /// <para>
    /// <b>Do Not Disturb</b> — when active, no calls are presented to the user.
    /// Use <see cref="ActivateDndAsync"/> to activate and <see cref="CancelDndAsync"/>
    /// to cancel.
    /// </para>
    /// <para>
    /// <b>Remote extension</b> — when a remote extension (e.g. a mobile device) is
    /// deactivated, it does not ring on incoming calls but can still place outgoing calls.
    /// Use <see cref="ActivateRemoteExtensionAsync"/> to activate and
    /// <see cref="DeactivateRemoteExtensionAsync"/> to deactivate.
    /// </para>
    /// </summary>
    /// <remarks>
    /// For all methods, if the session has been opened for a user the <c>loginName</c>
    /// parameter is ignored. It is mandatory only when the session has been opened by
    /// an administrator acting on behalf of a specific user.
    /// </remarks>
    public interface IRouting : IService
    {
        /// <summary>
        /// Raised whenever the routing state of the user changes.
        /// </summary>
        /// <seealso cref="RequestSnapshotAsync(string)"/>
        public event EventHandler<O2GEventArgs<OnRoutingStateChangedEvent>> RoutingStateChanged;

        /// <summary>
        /// Returns the routing capabilities available to the specified user.
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="RoutingCapabilities"/> object, or <see langword="null"/> on error.
        /// </returns>
        Task<RoutingCapabilities> GetCapabilitiesAsync(string loginName = null);

        /// <summary>
        /// Returns the complete routing state of the specified user.
        /// <para>
        /// The routing state includes the forward, overflow, Do Not Disturb and remote
        /// extension activation status in a single call.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="RoutingState"/> object, or <see langword="null"/> on error.
        /// </returns>
        Task<RoutingState> GetRoutingStateAsync(string loginName = null);

        /// <summary>
        /// Requests a <see cref="RoutingStateChanged"/> event to be fired with the current
        /// routing state of the specified user.
        /// <para>
        /// If a snapshot request is already in progress for the same user, this call has no effect.
        /// </para>
        /// <para>
        /// If called by an administrator with <c>loginName = null</c>, the snapshot is requested
        /// for all users. Processing time may be significant depending on the number of users.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for all users (administrator only).</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> RequestSnapshotAsync(string loginName = null);

        #region Do Not Disturb

        /// <summary>
        /// Returns the Do Not Disturb state of the specified user.
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="DndState"/> object, or <see langword="null"/> on error.
        /// </returns>
        Task<DndState> GetDndStateAsync(string loginName = null);

        /// <summary>
        /// Activates Do Not Disturb for the specified user.
        /// <para>
        /// When active, no calls are presented to the user. If DND is already active,
        /// this method does nothing and returns <see langword="true"/>.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="CancelDndAsync(string)"/>
        Task<bool> ActivateDndAsync(string loginName = null);

        /// <summary>
        /// Cancels Do Not Disturb for the specified user.
        /// <para>
        /// If DND is not active, this method does nothing and returns <see langword="true"/>.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ActivateDndAsync(string)"/>
        Task<bool> CancelDndAsync(string loginName = null);

        #endregion

        #region Forward

        /// <summary>
        /// Returns the forward currently configured for the specified user.
        /// <para>
        /// If no forward is active, returns a <see cref="Forward"/> with
        /// <see cref="Destination.None"/> as the destination.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="Forward"/> object, or <see langword="null"/> on error.
        /// </returns>
        Task<Forward> GetForwardAsync(string loginName = null);

        /// <summary>
        /// Activates a forward to the specified phone number for the specified user.
        /// <para>
        /// The number must be authorized by the OmniPCX Enterprise numbering policy.
        /// If a forward is already active, it is replaced by the new one.
        /// </para>
        /// </summary>
        /// <param name="number">The target phone number.</param>
        /// <param name="condition">The condition under which the forward is triggered.</param>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ForwardOnVoiceMailAsync(Forward.ForwardCondition, string)"/>
        /// <seealso cref="CancelForwardAsync(string)"/>
        Task<bool> ForwardOnNumberAsync(string number, Forward.ForwardCondition condition, string loginName = null);

        /// <summary>
        /// Activates a forward to the user's voice mail for the specified user.
        /// <para>
        /// This method returns <see langword="false"/> if the user does not have a voice mail.
        /// If a forward is already active, it is replaced by the new one.
        /// </para>
        /// </summary>
        /// <param name="condition">The condition under which the forward is triggered.</param>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ForwardOnNumberAsync(string, Forward.ForwardCondition, string)"/>
        /// <seealso cref="CancelForwardAsync(string)"/>
        Task<bool> ForwardOnVoiceMailAsync(Forward.ForwardCondition condition, string loginName = null);

        /// <summary>
        /// Cancels the active forward for the specified user.
        /// <para>
        /// If no forward is active, this method does nothing and returns <see langword="true"/>.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ForwardOnNumberAsync(string, Forward.ForwardCondition, string)"/>
        /// <seealso cref="ForwardOnVoiceMailAsync(Forward.ForwardCondition, string)"/>
        Task<bool> CancelForwardAsync(string loginName = null);

        #endregion

        #region Overflow

        /// <summary>
        /// Returns the overflow currently configured for the specified user.
        /// <para>
        /// If no overflow is active, returns an <see cref="Overflow"/> with
        /// <see cref="Destination.None"/> as the destination.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// An <see cref="Overflow"/> object, or <see langword="null"/> on error.
        /// </returns>
        Task<Overflow> GetOverflowAsync(string loginName = null);

        /// <summary>
        /// Activates an overflow to the user's voice mail for the specified user.
        /// <para>
        /// The overflow only applies when no forward is active. If an overflow is already
        /// active, it is replaced by the new one.
        /// This method returns <see langword="false"/> if the user does not have a voice mail.
        /// </para>
        /// </summary>
        /// <param name="condition">The condition under which the overflow is triggered.</param>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="CancelOverflowAsync(string)"/>
        Task<bool> OverflowOnVoiceMailAsync(Overflow.OverflowCondition condition, string loginName = null);

        /// <summary>
        /// Cancels the active overflow for the specified user.
        /// <para>
        /// If no overflow is active, this method does nothing and returns <see langword="true"/>.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="OverflowOnVoiceMailAsync(Overflow.OverflowCondition, string)"/>
        Task<bool> CancelOverflowAsync(string loginName = null);

        #endregion

        #region Remote extension

        /// <summary>
        /// Activates the remote extension device for the specified user.
        /// <para>
        /// When activated, the remote extension rings on incoming calls on the user's
        /// company phone. If it is already activated, this method does nothing and
        /// returns <see langword="true"/>.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="DeactivateRemoteExtensionAsync(string)"/>
        Task<bool> ActivateRemoteExtensionAsync(string loginName = null);

        /// <summary>
        /// Deactivates the remote extension device for the specified user.
        /// <para>
        /// When deactivated, the remote extension does not ring on incoming calls,
        /// but can still be used to place outgoing calls.
        /// </para>
        /// </summary>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ActivateRemoteExtensionAsync(string)"/>
        Task<bool> DeactivateRemoteExtensionAsync(string loginName = null);

        #endregion
    }
}
