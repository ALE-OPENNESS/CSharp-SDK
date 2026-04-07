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
    /// The <c>ITelephony</c> service allows a user or an administrator to manage OmniPCX Enterprise calls.
    /// Using this service requires having a <b>TELEPHONY_ADVANCED</b> license.
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
        /// Occurs when a user's state has been modified.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnUserStateModifiedEvent>> UserStateModified;

        /// <summary>
        /// Occurs in response to a snapshot request.
        /// </summary>
        /// <seealso cref="RequestSnapshotAsync(string)"/>
        public event EventHandler<O2GEventArgs<OnTelephonyStateEvent>> TelephonyState;

        /// <summary>
        /// Occurs when a device's state has been modified.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnDeviceStateModifiedEvent>> DeviceStateModified;

        /// <summary>
        /// Occurs when a user's dynamic state change.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnDynamicStateChangedEvent>> DynamicStateChanged;

        /// <summary>
        /// Initiates a call to another user, from the specified device. This method is a basic make call, it doesn't require any license.
        /// </summary>
        /// <param name="deviceId">Device phone number used by the usr to initiate the call. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="callee">Called phone number.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <c>false</c> the user's device (deviceId) is called before placing the call to callee, else callee is called immediately.</param>
        /// <returns><see langword="true"/> on success ; else <see langword="false"/>.</returns>
        Task<bool> BasicMakeCallAsync(string deviceId, string callee, bool autoAnswer = true);

        /// <summary>
        /// Answer to an ringing incoming call. This method is a basic answer call, it doesn't require any license.
        /// </summary>
        /// <param name="deviceId">The ringing device phone number.</param>
        /// <returns><see langword="true"/> on success ; else <see langword="false"/>.</returns>
        Task<bool> BasicAnswerCallAsync(string deviceId);

        /// <summary>
        /// Release the call. 
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If the call is a single call, it is released; if it is a conference, the call carries on without the user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> BasicDropMeAsync(string loginName = null);

        /// <summary>
        /// Get calls in progress for this session.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>A list of <see cref="PbxCall"/> that represents calls in progress.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<PbxCall>> GetCallsAsync(string loginName = null);

        /// <summary>
        /// Get a call from the specified reference.
        /// </summary>
        /// <param name="callRef">The call reference that identify the requested call.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>A <see cref="PbxCall"/> object that represents the call.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<PbxCall> GetCallAsync(string callRef, string loginName = null);

        /// <summary>
        /// Initiates a new call to another user (the callee), using the specified deviceId and options.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="callee">Called phone number.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <see langword="false"/> the device <paramref name="deviceId"/> is called before calling the callee, else callee is called immediately.</param>
        /// <param name="inhibitProgressTone">Allows to inhibit the progress tone on the current external call.</param>
        /// <param name="associatedData">Correlator data to add to the call.</param>
        /// <param name="callingNumber">Calling number to present to the public network.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakeCallAsync</c> service to initiated a call from one of the devices of the logged user. First, the call server initiates a call on the user <paramref name="deviceId"/>. Then when the call is answered the call server starts the call to the <paramref name="callee"/>.
        /// an <see cref="OnCallCreatedEvent"/> is raised.
        /// </para>
        /// <para>
        /// If <c>inhibitProgressTone</c> is <see langword="true"/>, progress tone is inhibited on an outbound call.
        /// </para>
        /// <para>
        /// The <c>callingNumber</c> is used on an outbound call to  to present another calling number on the public network call in order to hide the real calling extension number.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnCallCreatedEvent"/><seealso cref="OnCallModifiedEvent"/>
        [Obsolete("use version with byte array associated data")]
        Task<bool> MakeCallAsync(string deviceId, string callee, bool autoAnswer = true, bool inhibitProgressTone = false, string associatedData = null, string callingNumber = null, string loginName = null);

        /// <summary>
        /// Initiates a new call to another user (the callee), using the specified deviceId and options.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="callee">Called phone number.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <see langword="false"/> the device <paramref name="deviceId"/> is called before calling the callee, else callee is called immediately.</param>
        /// <param name="inhibitProgressTone">Allows to inhibit the progress tone on the current external call.</param>
        /// <param name="associatedData">The byte array data associated to the call.</param>
        /// <param name="callingNumber">Calling number to present to the public network.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakeCallAsync</c> service to initiated a call from one of the devices of the logged user. First, the call server initiates a call on the user <paramref name="deviceId"/>. Then when the call is answered the call server starts the call to the <paramref name="callee"/>.
        /// an <see cref="OnCallCreatedEvent"/> is raised.
        /// </para>
        /// <para>
        /// If <c>inhibitProgressTone</c> is <see langword="true"/>, progress tone is inhibited on an outbound call.
        /// </para>
        /// <para>
        /// The <c>callingNumber</c> is used on an outbound call to  to present another calling number on the public network call in order to hide the real calling extension number.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnCallCreatedEvent"/><seealso cref="OnCallModifiedEvent"/>
        Task<bool> MakeCallAsync(string deviceId, string callee, bool autoAnswer = true, bool inhibitProgressTone = false, byte[] associatedData = null, string callingNumber = null, string loginName = null);

        /// <summary>
        /// Initiates a new private call to another user (the callee), using a pin code and an optional secret code.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="callee">Called phone number.</param>
        /// <param name="pin">The PIN code to identify the caller.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <see langword="false"/> the device <paramref name="deviceId"/> is called before calling the callee, else callee is called immediately.</param>
        /// <param name="secretCode">The optional secret code used to confirm the PIN code.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The private call is a service which allows a user to specify that the external call made is personal and not professional. The charging for this
        /// type of call can then be given specific processing.
        /// </para>
        /// <para>
        /// Use the <c>MakePrivateCallAsync</c> service to initiated a private call from one of the devices of the logged user. First, the call server initiates a 
        /// call on the user <paramref name="deviceId"/>. Then when the call is answered the call server starts the call to the <paramref name="callee"/>.
        /// an <see cref="OnCallCreatedEvent"/> is raised.
        /// </para>
        /// <para>
        /// The private call requires the user enters a PIN code (Personal Identification Number). 
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnCallCreatedEvent"/><seealso cref="OnCallModifiedEvent"/>
        /// <seealso cref="IPhoneSetProgramming.GetPinCodeAsync(string, string)"/>
        Task<bool> MakePrivateCallAsync(string deviceId, string callee, string pin, bool autoAnswer = true, string secretCode = null, string loginName = null);

        /// <summary>
        /// Initiates a new business call to another user (the callee), using the specified business code.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="callee">Called phone number.</param>
        /// <param name="businessCode">The cost center on which the call will be charged.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <see langword="false"/> the device <paramref name="deviceId"/> is called before calling the callee, else callee is called immediately.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakeBusinessCallAsync</c> service to initiated a business call from one of the devices of the logged user. First, the call server initiates a 
        /// call on the user <paramref name="deviceId"/>. Then when the call is answered the call server starts the call to the <paramref name="callee"/>.
        /// an <see cref="OnCallCreatedEvent"/> is raised.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="OnCallCreatedEvent"/><seealso cref="OnCallModifiedEvent"/>
        Task<bool> MakeBusinessCallAsync(string deviceId, string callee, string businessCode, bool autoAnswer = true, string loginName = null);

        /// <summary>
        /// Initiates a call from a CCD agent to a supervisor.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <see langword="false"/> the device <paramref name="deviceId"/> is called before calling the callee, else callee is called immediately.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakeSupervisorCallAsync</c> service to initiated a call from an agent to a supervisor. First, the call server initiates a 
        /// call on the agent <paramref name="deviceId"/>. Then, when the call is answered the call server calls the supervisor.
        /// an <see cref="OnCallCreatedEvent"/> is raised.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> MakeSupervisorCallAsync(string deviceId, bool autoAnswer = true, string loginName = null);

        /// <summary>
        /// Initiates an enquiry call from a CCD agent to a pilot or a RSI point.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="pilot">Called CCD pilot or RSI point number.</param>
        /// <param name="associatedData">Correlator data to add to the call.</param>
        /// <param name="callProfile">The call profile associated to this call.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakePilotOrRSISupervisedTransferCallAsync</c> service to initiated an enquiry call to a CCD pilot or an RSI point from a CCD operator. 
        /// </para>
        /// <para>
        /// The CCD pilot or the RSI point performs a call distribution to select an agent that will be alerted by this call. The <paramref name="callProfile"/> is mandatory
        /// in case of "Advanced Call Routing" call distribution strategy.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> MakePilotOrRSISupervisedTransferCallAsync(string deviceId, string pilot, string associatedData = null, List<AcrSkill> callProfile = null, string loginName = null);


        /// <summary>
        /// Initiates an enquiry call from a CCD agent to a pilot or a RSI point.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="pilot">Called CCD pilot or RSI point number.</param>
        /// <param name="associatedData">The byte array data associated to the call.</param>
        /// <param name="callProfile">The call profile associated to this call.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakePilotOrRSISupervisedTransferCallAsync</c> service to initiated an enquiry call to a CCD pilot or an RSI point from a CCD operator. 
        /// </para>
        /// <para>
        /// The CCD pilot or the RSI point performs a call distribution to select an agent that will be alerted by this call. The <paramref name="callProfile"/> is mandatory
        /// in case of "Advanced Call Routing" call distribution strategy.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> MakePilotOrRSISupervisedTransferCallAsync(string deviceId, string pilot, byte[] associatedData = null, List<AcrSkill> callProfile = null, string loginName = null);

        /// <summary>
        /// Initiates an local call to a CCD pilot or a RSI point.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="pilot">Called CCD pilot or RSI point number.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <see langword="false"/> the device <paramref name="deviceId"/> is called before calling the callee, else callee is called immediately.</param>
        /// <param name="associatedData">Correlator data to add to the call.</param>
        /// <param name="callProfile">The call profile associated to this call.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakePilotOrRSICallAsync</c> service to initiated a local call to a CCD pilot or an RSI point. 
        /// </para>
        /// <para>
        /// The CCD pilot or the RSI point performs a call distribution to select an agent that will be alerted by this call. The <paramref name="callProfile"/> is mandatory
        /// in case of "Advanced Call Routing" call distribution strategy.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> MakePilotOrRSICallAsync(string deviceId, string pilot, bool autoAnswer = true, string associatedData = null, List<AcrSkill> callProfile = null, string loginName = null);


        /// <summary>
        /// Initiates an local call to a CCD pilot or a RSI point.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the call is placed. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="pilot">Called CCD pilot or RSI point number.</param>
        /// <param name="autoAnswer">Automatic answer on make call. If this parameter is set to <see langword="false"/> the device <paramref name="deviceId"/> is called before calling the callee, else callee is called immediately.</param>
        /// <param name="associatedData">The byte array data associated to the call.</param>
        /// <param name="callProfile">The call profile associated to this call.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Use the <c>MakePilotOrRSICallAsync</c> service to initiated a local call to a CCD pilot or an RSI point. 
        /// </para>
        /// <para>
        /// The CCD pilot or the RSI point performs a call distribution to select an agent that will be alerted by this call. The <paramref name="callProfile"/> is mandatory
        /// in case of "Advanced Call Routing" call distribution strategy.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> MakePilotOrRSICallAsync(string deviceId, string pilot, bool autoAnswer = true, byte[] associatedData = null, List<AcrSkill> callProfile = null, string loginName = null);


        /// <summary>
        /// Release the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> ReleaseCallAsync(string callRef, string loginName = null);

        /// <summary>
        /// Puts on hold the specified active call and retrieve a call that has been previously put in hold.
        /// </summary>
        /// <param name="callRef">The active call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is invoked.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> AlternateAsync(string callRef, string deviceId);

        /// <summary>
        /// Responds to an incoming ringing call.
        /// </summary>
        /// <param name="callRef">The incomming call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is invoked.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> AnswerAsync(string callRef, string deviceId);

        /// <summary>
        /// Associates data to the specified established call. 
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is invoked.</param>
        /// <param name="associatedData">The data associated to the call.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Associates data can be encoded as clear string or binary encoded string. The associated data has a 32 bytes length.
        /// </remarks>
        [Obsolete("use version with byte array associated data")]
        Task<bool> AttachDataAsync(string callRef, string deviceId, string associatedData);

        /// <summary>
        /// Associates data to the specified established call. 
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is invoked.</param>
        /// <param name="associatedData">The byte array data associated to the call.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Associates data can be encoded as clear string or binary encoded string. The associated data has a 32 bytes length.
        /// </remarks>
        Task<bool> AttachDataAsync(string callRef, string deviceId, byte[] associatedData);

        /// <summary>
        /// Transfers the active call to another user, without keeping control on this call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="transferTo">The phone number to which the call is transfered.</param>
        /// <param name="anonymous">Anonymous transfer.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If <c>anonymous</c> is <see langword="true"/>, the call is transfered as anonymous; otherwise, the identity is provided.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// 
        Task<bool> BlindTransferAsync(string callRef, string transferTo, bool anonymous = false, string loginName = null);

        /// <summary>
        /// Request a callback on the call specified by the call reference for the specified user.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> CallbackAsync(string callRef, string loginName = null);

        /// <summary>
        /// Get the legs associated to this call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="Leg"/> that represents the legs associated with this call in case of success; <see langword="null"/> otherwise.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<Leg>> GetDeviceLegsAsync(string callRef, string loginName = null);

        /// <summary>
        /// Get the specified leg form the specified this call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="legId">The leg identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="Leg"/> that represents the leg with the given identifier in case of success; 
        /// <see langword="null"/> in case of error or if there is no such leg associated to this call.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetDeviceLegsAsync(string, string)"/>
        Task<Leg> GetDeviceLegAsync(string callRef, string legId, string loginName = null);

        /// <summary>
        /// Exits from the call specified by its reference.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If the call is a single call, it is released; if it is a conference, the call continue without the user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> DropmeAsync(string callRef, string loginName = null);

        /// <summary>
        /// Put on hold the call specified by its reference, on the specified device.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number from which the call put on hold. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// 
        Task<bool> HoldAsync(string callRef, string deviceId, string loginName = null);

        /// <summary>
        /// Make a 3-party conference with a specified active call and the specified held call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="heldCallRef">The held call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// 
        Task<bool> MergeAsync(string callRef, string heldCallRef, string loginName = null);

        /// <summary>
        /// Redirects an outgoing ringing call specified by its reference to the voice mail of the called user.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// 
        Task<bool> OverflowToVoiceMailAsync(string callRef, string loginName = null);

        /// <summary>
        /// Park the specified active call to a target device.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="parkTo">The target device extension number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If <c>parkTo</c> is not provided, the call is parked on the current device.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="UnParkAsync(string, string)"/>
        Task<bool> ParkAsync(string callRef, string parkTo = null, string loginName = null);

        /// <summary>
        /// Get the list of participants in this call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="Participant"/> object that represents the participants of this call, or <see langword="null"/> in case
        /// of error or if there is no such paricipant associatde to this call.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<Participant>> GetParticipantsAsync(string callRef, string loginName = null);

        /// <summary>
        /// Get the specified participant from this call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="participantId">The participant identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="Participant"/> object that represents the requested participant, or <see langword="null"/> in case
        /// of error or if there is no such paricipant associated to this call.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<Participant> GetParticipantAsync(string callRef, string participantId, string loginName = null);

        /// <summary>
        /// Drop the specified participant from the call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="participantId">The participant identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If the call is a single call, it is released; if it is a conference, the call continues without the participant.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// 
        Task<bool> DropParticipantAsync(string callRef, string participantId, string loginName = null);

        /// <summary>
        /// Release the current call (active or ringing) to retrieve a previously put in hold call (cancel a consultation call).
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is done. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="enquiryCallRef">The reference of the enquiry call to cancel.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> ReconnectAsync(string callRef, string deviceId, string enquiryCallRef, string loginName = null);


        /// <summary>
        /// Starts, stops, pauses or resumes the recording of a the specified call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="action">The recording action.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> RecordingAsync(string callRef, RecordingAction action, string loginName = null);

        /// <summary>
        /// Redirects an incoming ringing call to another user or number, instead of responding to it.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="redirectTo">Phone number of the redirection, or <c>"VOICEMAIL"</c></param>
        /// <param name="anonymous">Anonymous transfer.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If <c>anonymous</c> is <see langword="true"/>, the call is redirected as anonymous; otherwise, the identity is provided.
        /// </para>
        /// <para>
        /// If <c>redirectTo</c> is equal to <c>"VOICEMAIL"</c>, the incoming ringing call is redirected the call to the user voice mail.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<bool> RedirectAsync(string callRef, string redirectTo, bool anonymous = false, string loginName = null);

        /// <summary>
        /// Retrieves a call that has been previously put in hold.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is done. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> RetrieveAsync(string callRef, string deviceId, string loginName = null);

        /// <summary>
        /// Send DTMF codes on the specified active call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is done. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="number">the DTMF codes to send.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> SendDtmfAsync(string callRef, string deviceId, string number);

        /// <summary>
        /// Send the transaction code for the specified call, on the specified device.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="deviceId">The device phone number for which the operation is done. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="accountInfo">the transaction code.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This operation is used by a CCD agent to send the transaction code at the end of the call. 
        /// The string value MUST complain with the transaction code accepted by OmniPcx Ennterprise(that is numerical value only)
        /// </remarks>
        Task<bool> SendAccountInfoAsync(string callRef, string deviceId, string accountInfo);

        /// <summary>
        /// Transfers a specified active call to a specified held call.
        /// </summary>
        /// <param name="callRef">The call reference.</param>
        /// <param name="heldCallRef">The reference of the held call.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> TransferAsync(string callRef, string heldCallRef, string loginName = null);

        /// <summary>
        /// Log the specified user on a specified desk sharing set.
        /// </summary>
        /// <param name="dssDeviceNumber">The desk sharing set phone number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The user must be configured as a desk sharing user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="DeskSharingLogOffAsync(string)"/>
        Task<bool> DeskSharingLogOnAsync(string dssDeviceNumber, string loginName = null);

        /// <summary>
        /// Log off the specified user from the desk sharing set.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The user must be configured as a desk sharing user.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="DeskSharingLogOnAsync(string, string)"/>
        Task<bool> DeskSharingLogOffAsync(string loginName = null);

        /// <summary>
        /// Return the state of the user's devices.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="DeviceState"/> that represents the state of the user's devices in 
        /// case of success, or <see langword="null"/> otherwise.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<DeviceState>> GetDevicesStateAsync(string loginName = null);

        /// <summary>
        /// Return the state of the specified user's device.
        /// </summary>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="DeviceState"/> that represents the state of requested device in case of success, or <see langword="null"/>
        /// in case of error or if the device doesn't belong to the user.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetDevicesStateAsync(string)"/>
        Task<DeviceState> GetDeviceStateAsync(string deviceId, string loginName = null);

        /// <summary>
        /// Intrusion in the active call of a called user.
        /// </summary>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// No parameter is required to invoke the intrusion: it only depends on the current capability intrusion of the current device. 
        /// It is based on the fact that the current device must be in releasing state while calling a user which is in busy call with another user, 
        /// the current device has the intrusion capability and the 2 users engaged in the call have the capability to allow intrusion.
        /// </para>
        /// <para>Available from O2G 2.4</para>
        /// </remarks>
        Task<bool> IntrusionAsync(string deviceId);

        /// <summary>
        /// Toggle activation of interphony/hands free. Invoking this service is equivalent to press the corresponding key on a phon set.
        /// </summary>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the device has an established call, it activates or deactivates the microphone (hands free mode). 
        /// It the device is idle, it activates or deactivates the interphony. 
        /// his operation is done in blind mode: no state event is provided on the push but when the device returns to idle after a call, 
        /// the microphone comes back in the active state.
        /// </remarks>
        Task<bool> ToggleInterphonyAsync(string deviceId);

        /// <summary>
        /// Picks up the specified incoming call to another user.
        /// </summary>
        /// <param name="deviceId">The device phone number from which the pickup is done. If the session is opened by a user, the device phone number must be one of the user.</param>
        /// <param name="otherCallRef">Reference of the call to pickup (on the remote user).</param>
        /// <param name="otherPhoneNumber">The phone number on which the call is ringing.</param>
        /// <param name="autoAnswer">Automatic answer the call after the pickup. </param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> PickUpAsync(string deviceId, string otherCallRef, string otherPhoneNumber, bool autoAnswer = false);

        /// <summary>
        /// UnPark a call from a target device.
        /// </summary>
        /// <param name="deviceId">the device from where the unpark request is requested.</param>
        /// <param name="heldCallRef">Reference of the parked call.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ParkAsync(string, string, string)"/>
        Task<bool> UnParkAsync(string deviceId, string heldCallRef);

        /// <summary>
        /// Retrieve the user hunting group status.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="HuntingGroupStatus"/> that represents the status of the user in case of 
        /// success; <see langword="null"/> otherwise.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="DynamicStateChanged"/>
        Task<HuntingGroupStatus> GetHuntingGroupStatusAsync(string loginName = null);

        /// <summary>
        /// Log on the specified user in his current hunting group.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if the user is already logged in his hunting group.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="HuntingGroupLogOffAsync(string)"/>
        /// <seealso cref="DynamicStateChanged"/>
        Task<bool> HuntingGroupLogOnAsync(string loginName = null);

        /// <summary>
        /// Log off the specified user from his current hunting group.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if the user is already logged off from his hunting group.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="HuntingGroupLogOnAsync(string)"/>
        /// <seealso cref="DynamicStateChanged"/>
        Task<bool> HuntingGroupLogOffAsync(string loginName = null);

        /// <summary>
        /// Set the specified user as member of an existing hunting group.
        /// </summary>
        /// <param name="hgNumber">The hunting group number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if the user is already member of the hunting group.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="DeleteHuntingGroupMemberAsync(string, string)"/>
        Task<bool> AddHuntingGroupMemberAsync(string hgNumber, string loginName = null);

        /// <summary>
        /// Remove the specified user from an existing hunting group.
        /// </summary>
        /// <param name="hgNumber">The hunting group number.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This method does nothing and return <see langword="true"/> if the user is not member of the hunting group.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="AddHuntingGroupMemberAsync(string, string)"/>
        Task<bool> DeleteHuntingGroupMemberAsync(string hgNumber, string loginName = null);

        /// <summary>
        /// Query the list of hunting groups existing on the OmniPcx Enterprise node the specified user belong to.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="HuntingGroups"/> object that gives the list of existing hunting group on the specified node.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetHuntingGroupStatusAsync(string)"/>
        /// <seealso cref="AddHuntingGroupMemberAsync(string, string)"/>
        /// <seealso cref="DeleteHuntingGroupMemberAsync(string, string)"/>
        Task<HuntingGroups> QueryHuntingGroupsAsync(string loginName = null);

        /// <summary>
        /// Get the list of callback requests.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="Callback"/> that represents the callback requests in case of success;
        /// <see langword="null"/> in case of error or if there is no callback request.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<Callback>> GetCallbacksAsync(string loginName = null);

        /// <summary>
        /// Delete all callback requests.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetCallbacksAsync(string)"/>
        Task<bool> DeleteCallbacksAsync(string loginName = null);

        /// <summary>
        /// Delete a callback requests.
        /// </summary>
        /// <param name="callbackId">The callback ID as return by <c>GetCallbacksAsync</c>.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetCallbacksAsync(string)"/>
        /// <seealso cref="DeleteCallbacksAsync(string)"/>
        Task<bool> DeleteCallbackAsync(string callbackId, string loginName = null);

        /// <summary>
        /// Returns the current new message.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="MiniMessage"/> object that represents the message receive by the user.
        /// </returns>
        /// <remarks>
        /// <para>
        /// As soon as a message is read, it is erased from OXE and cannot be read again. The messages 
        /// are retrieved in Last In First Out mode.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="SendMiniMessageAsync(string, string, string)"/>
        Task<MiniMessage> GetMiniMessageAsync(string loginName = null);

        /// <summary>
        /// Send the specified mini message to the specified recipient.
        /// </summary>
        /// <param name="recipient">The message destinator.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetMiniMessageAsync(string)"/>
        Task<bool> SendMiniMessageAsync(string recipient, string message, string loginName = null);

        /// <summary>
        /// Requests for call back from an idle device.
        /// </summary>
        /// <param name="callee">Callee phone number of the called party for which a call back is requested.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <seealso cref="GetCallAsync(string, string)"/>
        /// <seealso cref="DeleteCallbacksAsync(string)"/>
        Task<bool> RequestCallbackAsync(string callee, string loginName = null);

        /// <summary>
        /// Redirects an outgoing ringing call specified by its reference to the voice mail of the called user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="TelephonicState"/> object that represents the telephonic state of the user in case of success; 
        /// <see langword="null"/> otherwise.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <paramref name="loginName"/> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        /// <see cref="RequestSnapshotAsync(string)"/>
        /// 
        Task<TelephonicState> GetStateAsync(string loginName = null);

        /// <summary>
        /// Ask a snapshot event to receive an <see cref="OnTelephonyStateEvent"/> event notification.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnTelephonyStateEvent"/> will contain the <see cref="TelephonicState"/> (calls[] and  deviceCapabilities[]).
        /// If a second request is asked since the previous one is still in progress, it has no effect.
        /// </para>
        /// <para>
        /// If an administrator invokes <c>RequestSnapshotAsync</c> with <c>loginName = null</c>, the snapshot event request is done for all the users.
        /// The event processing can be long depending on the number of users.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetStateAsync(string)"/>
        /// <seealso cref="TelephonyState"/>
        Task<bool> RequestSnapshotAsync(string loginName = null);
    }
}
