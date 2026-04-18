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
using o2g.Events.Telephony;
using o2g.Internal.Services;
using o2g.Types.TelephonyNS;
using o2g.Types.TelephonyNS.CallNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using o2g.Types.TelephonyNS.DeviceNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>ITelephony</c> allows a user to initiate calls and activate any kind of OmniPCX Enterprise telephony services.
    /// <para>
    /// Using this service requires a <b>TELEPHONY_ADVANCED</b> license, except for the three basic methods
    /// <see cref="BasicMakeCallAsync(string, string, bool)"/>, <see cref="BasicAnswerCallAsync(string)"/> and
    /// <see cref="BasicDropMeAsync(string)"/>, which are available without any license.
    /// </para>
    /// </summary>
    public interface ITelephony : IService
    {
        /// <summary>
        /// Occurs when a new call is created.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnCallCreatedEvent>> CallCreated;

        /// <summary>
        /// Occurs when an existing call is modified.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnCallModifiedEvent>> CallModified;

        /// <summary>
        /// Occurs when a call has been removed.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnCallRemovedEvent>> CallRemoved;

        /// <summary>
        /// Occurs when a user's availability state has been modified.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnUserStateModifiedEvent>> UserStateModified;

        /// <summary>
        /// Occurs in response to a snapshot request, carrying the full telephonic state.
        /// </summary>
        /// <seealso cref="RequestSnapshotAsync(string)"/>
        public event EventHandler<O2GEventArgs<OnTelephonyStateEvent>> TelephonyState;

        /// <summary>
        /// Occurs when a device's operational state has been modified.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnDeviceStateModifiedEvent>> DeviceStateModified;

        /// <summary>
        /// Occurs when a user's dynamic state changes (hunting group membership, desk sharing, etc.).
        /// </summary>
        public event EventHandler<O2GEventArgs<OnDynamicStateChangedEvent>> DynamicStateChanged;

        /// <summary>
        /// Initiates a basic call from the specified device to the specified called number.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number used to place the call. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="callee">The called phone number.</param>
        /// <param name="autoAnswer">
        /// If <see langword="true"/>, the callee is called immediately; if <see langword="false"/>,
        /// the user's device is called first before placing the call to the callee.
        /// </param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>This method does not require a license.</remarks>
        Task<bool> BasicMakeCallAsync(string deviceId, string callee, bool autoAnswer = true);

        /// <summary>
        /// Answers an incoming ringing call on the specified device.
        /// </summary>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>This method does not require a license.</remarks>
        Task<bool> BasicAnswerCallAsync(string deviceId);

        /// <summary>
        /// Exits from the current call for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does not require a license.
        /// </para>
        /// <para>
        /// If the call is a single call it is released; if it is a conference, the call continues without the user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> BasicDropMeAsync(string loginName = null);

        /// <summary>
        /// Retrieves the calls currently in progress for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>A list of <see cref="Call"/> representing the active calls, or <see langword="null"/> on error.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<IReadOnlyList<Call>> GetCallsAsync(string loginName = null);

        /// <summary>
        /// Returns the call identified by the specified reference.
        /// </summary>
        /// <param name="callRef">The unique call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="Call"/> object representing the call, or <see langword="null"/> if not found.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<Call> GetCallAsync(string callRef, string loginName = null);

        /// <summary>
        /// Initiates a call from the specified device to the specified called number, with extended options.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number from which the call is placed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="callee">The called phone number.</param>
        /// <param name="autoAnswer">
        /// If <see langword="true"/>, the callee is called immediately; if <see langword="false"/>,
        /// the user's device is called first before placing the call to the callee.
        /// </param>
        /// <param name="inhibitProgressTone">
        /// If <see langword="true"/>, the progress tone is inhibited on the outbound call.
        /// </param>
        /// <param name="correlatorData">
        /// Optional correlator data to attach to the call. See <see cref="CorrelatorData"/>.
        /// </param>
        /// <param name="callingNumber">
        /// Optional calling number to present on the public network, used to mask the real extension number.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="OnCallCreatedEvent"/>
        /// <seealso cref="OnCallModifiedEvent"/>
        Task<bool> MakeCallAsync(string deviceId, string callee, bool autoAnswer = true, bool inhibitProgressTone = false, CorrelatorData correlatorData = null, string callingNumber = null, string loginName = null);

        /// <summary>
        /// Initiates a private call to the specified callee, identified by a PIN code.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number from which the call is placed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="callee">The called phone number.</param>
        /// <param name="pin">The PIN code identifying the caller.</param>
        /// <param name="autoAnswer">
        /// If <see langword="true"/>, the callee is called immediately; if <see langword="false"/>,
        /// the user's device is called first before placing the call to the callee.
        /// </param>
        /// <param name="secretCode">An optional secret code used to confirm the PIN.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// A private call allows the user to flag a call as personal rather than professional,
        /// enabling specific charging processing.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnCallCreatedEvent"/>
        /// <seealso cref="IPhoneSetProgramming.GetPinCodeAsync(string, string)"/>
        Task<bool> MakePrivateCallAsync(string deviceId, string callee, string pin, bool autoAnswer = true, string secretCode = null, string loginName = null);

        /// <summary>
        /// Initiates a business call to the specified callee, charged to the specified cost center.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number from which the call is placed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="callee">The called phone number.</param>
        /// <param name="businessCode">The cost center code to charge the call to.</param>
        /// <param name="autoAnswer">
        /// If <see langword="true"/>, the callee is called immediately; if <see langword="false"/>,
        /// the user's device is called first before placing the call to the callee.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="OnCallCreatedEvent"/>
        Task<bool> MakeBusinessCallAsync(string deviceId, string callee, string businessCode, bool autoAnswer = true, string loginName = null);

        /// <summary>
        /// Initiates a call from a CCD agent to their supervisor.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number from which the call is placed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="autoAnswer">
        /// If <see langword="true"/>, the supervisor is called immediately; if <see langword="false"/>,
        /// the agent's device is called first.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> MakeSupervisorCallAsync(string deviceId, bool autoAnswer = true, string loginName = null);

        /// <summary>
        /// Initiates a supervised transfer enquiry call from a CCD agent to a pilot or RSI point.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number from which the call is placed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="pilot">The CCD pilot or RSI point number to call.</param>
        /// <param name="correlatorData">Optional correlator data to attach to the call.</param>
        /// <param name="callProfile">
        /// The call profile required when the <em>Advanced Call Routing</em> distribution strategy is configured.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The pilot or RSI point performs call distribution to select an agent to alert.
        /// The <paramref name="callProfile"/> is mandatory when the <em>Advanced Call Routing</em>
        /// distribution strategy is in use.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> MakePilotOrRSISupervisedTransferCallAsync(string deviceId, string pilot, CorrelatorData correlatorData = null, CallProfile callProfile = null, string loginName = null);

        /// <summary>
        /// Initiates a local call to a CCD pilot or RSI point.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number from which the call is placed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="pilot">The CCD pilot or RSI point number to call.</param>
        /// <param name="autoAnswer">
        /// If <see langword="true"/>, the pilot is called immediately; if <see langword="false"/>,
        /// the user's device is called first.
        /// </param>
        /// <param name="correlatorData">Optional correlator data to attach to the call.</param>
        /// <param name="callProfile">
        /// The call profile required when the <em>Advanced Call Routing</em> distribution strategy is configured.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The pilot or RSI point performs call distribution to select an agent to alert.
        /// The <paramref name="callProfile"/> is mandatory when the <em>Advanced Call Routing</em>
        /// distribution strategy is in use.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> MakePilotOrRSICallAsync(string deviceId, string pilot, bool autoAnswer = true, CorrelatorData correlatorData = null, CallProfile callProfile = null, string loginName = null);

        /// <summary>
        /// Releases the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> ReleaseCallAsync(string callRef, string loginName = null);

        /// <summary>
        /// Puts the specified active call on hold and retrieves a previously held call.
        /// </summary>
        /// <param name="callRef">The active call reference.</param>
        /// <param name="deviceId">
        /// The device phone number for which the operation is performed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> AlternateAsync(string callRef, string deviceId);

        /// <summary>
        /// Answers a ringing incoming call on the specified device.
        /// </summary>
        /// <param name="callRef">The incoming call reference.</param>
        /// <param name="deviceId">
        /// The device phone number for which the operation is performed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Answering a call will fail if the call state is not correct. The state can be checked by
        /// listening to the telephony events, and more specifically by checking the capabilities of the
        /// involved leg (answer capability on the leg).
        /// </remarks>
        Task<bool> AnswerAsync(string callRef, string deviceId);

        /// <summary>
        /// Attaches the specified correlator data to the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is performed.</param>
        /// <param name="correlatorData">The correlator data to attach. Limited to 32 bytes.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This is used by the application to provide application-related information (limited to 32 bytes).
        /// In general, it is used to convey context from a previously established call to the party of a second call.
        /// </remarks>
        /// <seealso cref="CorrelatorData"/>
        Task<bool> AttachDataAsync(string callRef, string deviceId, CorrelatorData correlatorData);

        /// <summary>
        /// Transfers the active call to another party without keeping control of the call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="transferTo">The phone number to transfer the call to.</param>
        /// <param name="anonymous">
        /// If <see langword="true"/>, the call is transferred anonymously.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> BlindTransferAsync(string callRef, string transferTo, bool anonymous = false, string loginName = null);

        /// <summary>
        /// Requests a callback on the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> CallbackAsync(string callRef, string loginName = null);

        /// <summary>
        /// Returns the legs associated to the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="Leg"/> representing the legs of this call,
        /// or <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetLegAsync(string, string, string)"/>
        Task<IReadOnlyList<Leg>> GetLegsAsync(string callRef, string loginName = null);

        /// <summary>
        /// Returns the specified leg of the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="legId">The leg identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// The <see cref="Leg"/> with the given identifier, or <see langword="null"/> if not found.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetLegsAsync(string, string)"/>
        Task<Leg> GetLegAsync(string callRef, string legId, string loginName = null);

        /// <summary>
        /// Exits from the specified call for the specified user.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If the call is a single call it is released; if it is a conference, the call continues without the user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> DropmeAsync(string callRef, string loginName = null);

        /// <summary>
        /// Puts the specified call on hold on the specified device.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">
        /// The device phone number from which the hold is requested. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> HoldAsync(string callRef, string deviceId, string loginName = null);

        /// <summary>
        /// Creates a 3-party conference from the specified active call and a held call.
        /// </summary>
        /// <param name="callRef">The active call reference.</param>
        /// <param name="heldCallRef">The held call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> MergeAsync(string callRef, string heldCallRef, string loginName = null);

        /// <summary>
        /// Redirects an outgoing ringing call to the voice mail of the called user.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> OverflowToVoiceMailAsync(string callRef, string loginName = null);

        /// <summary>
        /// Returns a snapshot of the current telephonic state for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="TelephonicState"/> object representing the user's current telephonic state,
        /// or <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method performs a synchronous REST query. For an event-driven approach,
        /// use <see cref="RequestSnapshotAsync(string)"/> instead, which raises a
        /// <see cref="TelephonyState"/> event asynchronously.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="RequestSnapshotAsync(string)"/>
        Task<TelephonicState> GetStateAsync(string loginName = null);

        /// <summary>
        /// Parks the specified active call on a target device.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="parkTo">
        /// The target device extension number. If not provided, the call is parked on the current device.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="UnParkAsync(string, string)"/>
        Task<bool> ParkAsync(string callRef, string parkTo = null, string loginName = null);

        /// <summary>
        /// Returns the participants of the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="Participant"/> representing the call participants,
        /// or <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetParticipantAsync(string, string, string)"/>
        Task<IReadOnlyList<Participant>> GetParticipantsAsync(string callRef, string loginName = null);

        /// <summary>
        /// Returns the specified participant of the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="participantId">The participant identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// The <see cref="Participant"/> with the given identifier, or <see langword="null"/> if not found.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<Participant> GetParticipantAsync(string callRef, string participantId, string loginName = null);

        /// <summary>
        /// Drops the specified participant from the call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="participantId">The participant identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If the call is a single call it is released; if it is a conference, the call continues
        /// without the dropped participant.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> DropParticipantAsync(string callRef, string participantId, string loginName = null);

        /// <summary>
        /// Releases the current call to retrieve a previously held call (cancels a consultation call).
        /// </summary>
        /// <param name="callRef">The current call reference.</param>
        /// <param name="deviceId">
        /// The device phone number for which the operation is performed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="enquiryCallRef">The reference of the enquiry call to cancel.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> ReconnectAsync(string callRef, string deviceId, string enquiryCallRef, string loginName = null);

        /// <summary>
        /// Starts, stops, pauses, or resumes the recording of the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="action">The recording action to perform.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> DoRecordActionAsync(string callRef, RecordingAction action, string loginName = null);

        /// <summary>
        /// Redirects an incoming ringing call to another number or to voice mail.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="redirectTo">
        /// The phone number to redirect to, or <c>"VOICEMAIL"</c> to redirect to the user's voice mail.
        /// </param>
        /// <param name="anonymous">
        /// If <see langword="true"/>, the redirect is anonymous and the caller identity is hidden.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> RedirectAsync(string callRef, string redirectTo, bool anonymous = false, string loginName = null);

        /// <summary>
        /// Retrieves a call that has been previously put on hold.
        /// </summary>
        /// <param name="callRef">The held call reference.</param>
        /// <param name="deviceId">
        /// The device phone number for which the operation is performed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="HoldAsync(string, string, string)"/>
        Task<bool> RetrieveAsync(string callRef, string deviceId, string loginName = null);

        /// <summary>
        /// Sends DTMF codes on the specified active call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">
        /// The device phone number for which the operation is performed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="number">The DTMF digit sequence to send.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> SendDtmfAsync(string callRef, string deviceId, string number);

        /// <summary>
        /// Sends the transaction code for the specified call on the specified device.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">
        /// The device phone number for which the operation is performed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="accountInfo">The transaction code (numeric values only).</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Used by a CCD agent to send the transaction code at the end of a call.
        /// The value must comply with the OmniPCX Enterprise transaction code format (numeric only).
        /// </remarks>
        Task<bool> SendAccountInfoAsync(string callRef, string deviceId, string accountInfo);

        /// <summary>
        /// Transfers the specified active call to the specified held call.
        /// </summary>
        /// <param name="callRef">The active call reference.</param>
        /// <param name="heldCallRef">The held call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> TransferAsync(string callRef, string heldCallRef, string loginName = null);

        /// <summary>
        /// Logs the specified user onto a desk sharing set.
        /// </summary>
        /// <param name="dssDeviceNumber">The desk sharing set phone number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The user must be configured as a desk sharing user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="DeskSharingLogOffAsync(string)"/>
        Task<bool> DeskSharingLogOnAsync(string dssDeviceNumber, string loginName = null);

        /// <summary>
        /// Logs the specified user off from their desk sharing set.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The user must be configured as a desk sharing user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="DeskSharingLogOnAsync(string, string)"/>
        Task<bool> DeskSharingLogOffAsync(string loginName = null);

        /// <summary>
        /// Returns the operational state of all devices belonging to the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="DeviceState"/> representing each device's state,
        /// or <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetDeviceStateAsync(string, string)"/>
        Task<IReadOnlyList<DeviceState>> GetDevicesStateAsync(string loginName = null);

        /// <summary>
        /// Returns the operational state of the specified device.
        /// </summary>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="DeviceState"/> for the requested device, or <see langword="null"/>
        /// on error or if the device does not belong to the user.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetDevicesStateAsync(string)"/>
        Task<DeviceState> GetDeviceStateAsync(string deviceId, string loginName = null);

        /// <summary>
        /// Picks up an incoming call ringing on another user's device.
        /// </summary>
        /// <param name="deviceId">
        /// The device phone number from which the pickup is performed. If the session is opened by a user,
        /// this must be one of the user's devices.
        /// </param>
        /// <param name="otherCallRef">The reference of the call to pick up on the remote user.</param>
        /// <param name="otherPhoneNumber">The phone number on which the call is ringing.</param>
        /// <param name="autoAnswer">
        /// If <see langword="true"/>, the call is automatically answered after pickup.
        /// </param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> PickUpAsync(string deviceId, string otherCallRef, string otherPhoneNumber, bool autoAnswer = false);

        /// <summary>
        /// Intrudes into the active call of a busy user.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the intrusion is initiated.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Intrusion requires that the current device is in releasing state while calling a user
        /// who is engaged in a call, and that both the current device and the engaged users have
        /// the intrusion capability configured.
        /// </para>
        /// <para>Available from O2G 2.4.</para>
        /// </remarks>
        Task<bool> IntrusionAsync(string deviceId);

        /// <summary>
        /// Unparks a previously parked call onto the specified device.
        /// </summary>
        /// <param name="deviceId">The device from which the unpark is requested.</param>
        /// <param name="heldCallRef">The reference of the parked call to retrieve.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ParkAsync(string, string, string)"/>
        Task<bool> UnParkAsync(string deviceId, string heldCallRef);

        /// <summary>
        /// Toggles interphony or hands-free mode on the specified device.
        /// </summary>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If the device has an active call, this toggles the microphone (hands-free mode).
        /// If the device is idle, this toggles interphony.
        /// This has no effect if the device is ringing on an incoming call.
        /// </para>
        /// <para>
        /// The operation is blind — no state event is raised, and the microphone returns to its
        /// active state when the device goes idle.
        /// </para>
        /// </remarks>
        Task<bool> ToggleInterphonyAsync(string deviceId);

        /// <summary>
        /// Returns the hunting group login status of the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="HuntingGroupStatus"/> indicating whether the user is logged into their
        /// hunting group, or <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="DynamicStateChanged"/>
        Task<HuntingGroupStatus> GetHuntingGroupStatusAsync(string loginName = null);

        /// <summary>
        /// Logs the specified user into their current hunting group.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The user must be configured as a member of a hunting group.
        /// Has no effect and returns <see langword="true"/> if the user is already logged in.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="HuntingGroupLogOffAsync(string)"/>
        /// <seealso cref="DynamicStateChanged"/>
        Task<bool> HuntingGroupLogOnAsync(string loginName = null);

        /// <summary>
        /// Logs the specified user off from their current hunting group.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The user must be configured as a member of a hunting group.
        /// Has no effect and returns <see langword="true"/> if the user is already logged off.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="HuntingGroupLogOnAsync(string)"/>
        /// <seealso cref="DynamicStateChanged"/>
        Task<bool> HuntingGroupLogOffAsync(string loginName = null);

        /// <summary>
        /// Adds the specified user as a member of an existing hunting group.
        /// </summary>
        /// <param name="hgNumber">The hunting group phone number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The request will fail if the hunting group does not exist.
        /// Has no effect and returns <see langword="true"/> if the user is already a member.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="RemoveMeFromHuntingGroupAsync(string, string)"/>
        Task<bool> AddMeToHuntingGroupAsync(string hgNumber, string loginName = null);

        /// <summary>
        /// Removes the specified user from an existing hunting group.
        /// </summary>
        /// <param name="hgNumber">The hunting group phone number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The request will fail if the hunting group does not exist.
        /// Has no effect and returns <see langword="true"/> if the user is not a member.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="AddMeToHuntingGroupAsync(string, string)"/>
        Task<bool> RemoveMeFromHuntingGroupAsync(string hgNumber, string loginName = null);

        /// <summary>
        /// Returns the hunting groups available on the OmniPCX Enterprise node the specified user belongs to.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="HuntingGroups"/> object listing the available hunting groups and the
        /// user's current membership, or <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="AddMeToHuntingGroupAsync(string, string)"/>
        /// <seealso cref="RemoveMeFromHuntingGroupAsync(string, string)"/>
        Task<HuntingGroups> QueryHuntingGroupsAsync(string loginName = null);

        /// <summary>
        /// Returns the pending callback requests for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="Callback"/> representing the pending requests,
        /// or <see langword="null"/> on error or if there are no pending requests.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<IReadOnlyList<Callback>> GetCallbacksAsync(string loginName = null);

        /// <summary>
        /// Deletes all pending callback requests for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetCallbacksAsync(string)"/>
        Task<bool> DeleteCallbacksAsync(string loginName = null);

        /// <summary>
        /// Deletes the specified callback request.
        /// </summary>
        /// <param name="callbackId">The callback identifier as returned by <see cref="GetCallbacksAsync(string)"/>.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetCallbacksAsync(string)"/>
        /// <seealso cref="DeleteCallbacksAsync(string)"/>
        Task<bool> DeleteCallbackAsync(string callbackId, string loginName = null);

        /// <summary>
        /// Returns the next unread mini message for the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="MiniMessage"/> representing the message, or <see langword="null"/>
        /// if there are no unread messages or on error.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Messages are consumed on read — once retrieved, a message is deleted from the OXE
        /// and cannot be read again. Messages are returned in Last In First Out order.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="SendMiniMessageAsync(string, string, string)"/>
        Task<MiniMessage> GetMiniMessageAsync(string loginName = null);

        /// <summary>
        /// Sends a mini message to the specified recipient.
        /// </summary>
        /// <param name="recipient">The phone number of the message recipient.</param>
        /// <param name="message">The message text.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetMiniMessageAsync(string)"/>
        Task<bool> SendMiniMessageAsync(string recipient, string message, string loginName = null);

        /// <summary>
        /// Requests a callback from an idle device of the specified user.
        /// </summary>
        /// <param name="callee">The phone number of the party to request a callback from.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetCallbacksAsync(string)"/>
        /// <seealso cref="DeleteCallbacksAsync(string)"/>
        Task<bool> RequestCallbackAsync(string callee, string loginName = null);

        /// <summary>
        /// Requests a snapshot event to receive the current telephonic state via a <see cref="TelephonyState"/> event.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The resulting <see cref="OnTelephonyStateEvent"/> will contain the full
        /// <see cref="TelephonicState"/> including active calls and device capabilities.
        /// If a second request is issued while the first is still in progress, it has no effect.
        /// </para>
        /// <para>
        /// If an administrator calls this with <c>loginName = null</c>, the snapshot is requested
        /// for all users, which may take time depending on the number of users.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetStateAsync(string)"/>
        /// <seealso cref="TelephonyState"/>
        Task<bool> RequestSnapshotAsync(string loginName = null);

        /// <summary>
        /// Returns transfer possibilities for the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The CCD pilot directory number.</param>
        /// <param name="pilotTransferQueryParameters">
        /// Optional query criteria to filter results by agent number, priority transfer,
        /// supervised transfer, or call profile. See <see cref="PilotTransferQueryParameters"/>.
        /// </param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="PilotInfo"/> object describing the pilot's queue state and transfer
        /// possibilities, or <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="PilotTransferQueryParameters"/>
        /// <seealso cref="MakePilotOrRSISupervisedTransferCallAsync(string, string, CorrelatorData, CallProfile, string)"/>
        Task<PilotInfo> GetPilotInfoAsync(int nodeId, string pilotNumber, PilotTransferQueryParameters pilotTransferQueryParameters = null, string loginName = null);
    }
}
