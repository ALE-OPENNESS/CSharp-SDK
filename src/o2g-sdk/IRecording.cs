/*
* Copyright 2025 ALE International
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
using o2g.Internal.Services;
using o2g.Types.RecordingNS;
using System.Collections.Generic;
using System.Threading.Tasks;
using Types.Recording;

namespace o2g
{
    /// <summary>
    /// The ROD OXR Recording service allows starting, pausing, or resuming voice recording
    /// of an OXE device on one or more OXR recorders.
    /// </summary>
    /// <remarks>
    /// All necessary permissions must be configured on the OXR side for the dedicated user
    /// to perform start/pause/resume operations.
    ///
    /// If a device is configured in multiple OXRs, the request is sent to all relevant OXRs.
    /// A successful action on any one OXR is considered a success for the overall request.
    ///
    /// Modifications on an OXR may require a restart of the OXR service. To apply such changes
    /// immediately, O2G services must also be restarted; otherwise, the O2G server refreshes
    /// automatically after a scheduled period (4 hours). After a new device is created on an
    /// OXR, the O2G server is refreshed automatically for that device.
    ///
    /// This service does not provide real-time notifications of changes in recording state caused
    /// by call evolutions (e.g., hold, retrieve, transfer, conference, intrusion). Applications
    /// may deduce the recording state using call monitoring.
    ///
    /// Only two telephony call states are relevant for recording actions:
    /// <list type="bullet">
    ///   <item>
    ///     <description><b>HELD:</b> If a call was recording before hold, the recording state is memorized and actions are disabled.</description>
    ///   </item>
    ///   <item>
    ///     <description><b>ACTIVE:</b>
    ///       <list type="bullet">
    ///         <item><description>New call without recording: "start" action possible.</description></item>
    ///         <item><description>Call from transfer/conference: recording stopped, new "start" action possible.</description></item>
    ///         <item><description>Hold retrieve: recording resumes previous state.</description></item>
    ///         <item><description>Other cases: no change.</description></item>
    ///       </list>
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// While the API allows inferring recording state from telephony events, it is recommended
    /// to query the device to get the updated recording state. Typical event-to-state mappings:
    /// <list type="bullet">
    ///   <item><description>ACTIVE -> HELD: recording stalled, no action possible</description></item>
    ///   <item><description>ACTIVE caused by TRANSFERRED/CONFERENCE: no recording in progress, "start" possible</description></item>
    ///   <item><description>HELD -> ACTIVE (not TRANSFERRED/CONFERENCE): previous recording resumes</description></item>
    ///   <item><description>ACTIVE -> ACTIVE (not TRANSFERRED/CONFERENCE): no change</description></item>
    ///   <item><description>Other -> ACTIVE (not TRANSFERRED/CONFERENCE): new "start" possible</description></item>
    ///   <item><description>New callref with state=ACTIVE: new "start" possible</description></item>
    /// </list>
    ///
    /// <para>Since version 2.7.3</para>
    /// </remarks>
    public interface IRecording : IService
    {
        /// <summary>
        /// Retrieves the identifiers of all recorded devices (administrator only).
        /// </summary>
        /// <returns>A list of recorded device phone numbers, or <see langword="null"/> on error.</returns>
        Task<List<string>> GetRecordedDevicesAsync();

        /// <summary>
        /// Retrieves detailed recording information about a specific device.
        /// </summary>
        /// <param name="deviceId">The phone number of the device.</param>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="DeviceRecordingInfo"/> representing the device recording info on success;
        /// <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<DeviceRecordingInfo> GetDeviceRecordingInfoAsync(string deviceId, string loginName = null);

        /// <summary>
        /// Starts recording on the specified device.
        /// </summary>
        /// <param name="deviceId">The phone number of the device.</param>
        /// <param name="callRef">The reference of the call for which the recording is requested.</param>
        /// <param name="startType">The recording start mode.</param>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="DeviceRecordingInfo"/> representing the device recording info on success;
        /// <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<DeviceRecordingInfo> StartRecordingAsync(string deviceId, string callRef, RecordingStartType startType, string loginName = null);

        /// <summary>
        /// Pauses recording on the specified device.
        /// </summary>
        /// <param name="deviceId">The phone number of the device.</param>
        /// <param name="callRef">The reference of the call for which the recording pause is requested.</param>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="DeviceRecordingInfo"/> representing the device recording info on success;
        /// <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<DeviceRecordingInfo> PauseRecordingAsync(string deviceId, string callRef, string loginName = null);

        /// <summary>
        /// Resumes recording on the specified device.
        /// </summary>
        /// <param name="deviceId">The phone number of the device.</param>
        /// <param name="callRef">The reference of the call for which the recording resume is requested.</param>
        /// <param name="loginName">The user login name, or <see langword="null"/> for the session user.</param>
        /// <returns>
        /// A <see cref="DeviceRecordingInfo"/> representing the device recording info on success;
        /// <see langword="null"/> on error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored,
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<DeviceRecordingInfo> ResumeRecordingAsync(string deviceId, string callRef, string loginName = null);

        /// <summary>
        /// Retrieves the current status of the recording service.
        /// </summary>
        /// <returns>
        /// A <see cref="RecordingStatus"/> representing the current status of the configured recording service,
        /// or <see langword="null"/> on error or if no recording is configured.
        /// </returns>
        Task<RecordingStatus> GetRecordingStatusAsync();
    }
}
