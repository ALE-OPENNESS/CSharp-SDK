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
using o2g.Types.CommonNS;
using o2g.Types.TelephonyNS.DeviceNS;
using o2g.Types.TelephonyNS.UserNS;
using System.Collections.Generic;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// <c>TelephonicState</c> represents a snapshot of a user's current telephony state,
    /// including all active calls, device capabilities, and availability.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An application should retrieve the telephonic state at startup to synchronize
    /// with the current state of the user, before processing any telephony events.
    /// </para>
    /// <para>
    /// This object is returned by <see cref="ITelephony.GetStateAsync(string)"/>.
    /// </para>
    /// </remarks>
    public class TelephonicState
    {
        /// <summary>
        /// The calls currently in progress for this user.
        /// </summary>
        /// <value>
        /// A list of <see cref="Call"/> representing each active call,
        /// or an empty list if no call is in progress.
        /// </value>
        public List<Call> Calls { get; init; }

        /// <summary>
        /// The capabilities of each of the user's devices.
        /// </summary>
        /// <value>
        /// A list of <see cref="DeviceCapabilities"/>, one entry per device.
        /// Each entry indicates which telephony actions (make call, unpark, etc.)
        /// are available on that device in the current state.
        /// </value>
        public List<Device.Capabilities> DeviceCapabilities { get; init; }

        /// <summary>
        /// The current availability state of the user.
        /// </summary>
        /// <value>
        /// A <see cref="UserState"/> value indicating whether the user is
        /// <see cref="UserState.Free"/>, <see cref="UserState.Busy"/>, or <see cref="UserState.Unknown"/>.
        /// </value>
        public UserState UserState { get; init; }

        /// <summary>
        /// The operational state of each of the user's devices.
        /// </summary>
        /// <value>
        /// A list of <see cref="DeviceState"/>, one entry per device,
        /// indicating whether each device is in service or out of service.
        /// </value>
        public List<DeviceState> DeviceStates { get; init; }
    }
}

