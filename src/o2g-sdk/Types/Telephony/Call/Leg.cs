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

namespace o2g.Types.TelephonyNS.CallNS
{
    /// <summary>
    /// <c>Leg</c> class represents a Leg in a call. A leg represents the user's device involved in a call for a dedicated media (audio or video).
    /// </summary>
    public class Leg
    {
        /// <summary>
        /// <c>LegCapabilities</c> represents the capability of a <see cref="Leg"/>. The action that can be carried out on the leg according to its state.
        /// </summary>
        public class LegCapabilities
        {
            /// <summary>
            /// Return whether the leg can answer to an incoming call.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the call can be answered; <see langword="false"/> otherwise.
            /// </value>
            public bool Answer { get; init; }

            /// <summary>
            /// Return whether the leg can drop this media device.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the media device can be dropped; <see langword="false"/> otherwise.
            /// </value>
            public bool Drop { get; init; }

            /// <summary>
            /// Return whether the leg can put this call on hold in order to retrieve it later.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the call can be put on hold; <see langword="false"/> otherwise.
            /// </value>
            public bool Hold { get; init; }

            /// <summary>
            /// Return whether the leg can retrieve a call on hold.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the call on hold can retrieved; <see langword="false"/> otherwise.
            /// </value>
            public bool Retrieve { get; init; }

            /// <summary>
            /// Return whether the leg can retrieve an hold call and releasing the current active call.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the call on hold can be retrieved and the active call released; <see langword="false"/> otherwise.
            /// </value>
            public bool Reconnect { get; init; }

            /// <summary>
            /// Return whether the leg can be muted.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the leg can be muted; <see langword="false"/> otherwise.
            /// </value>
            public bool Mute { get; init; }

            /// <summary>
            /// Return whether the leg can be un-muted.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the leg can be un-muted; <see langword="false"/> otherwise.
            /// </value>
            public bool UnMute { get; init; }

            /// <summary>
            /// Return whether DTMF codes can be sent.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if DTMF codes can be sent; <see langword="false"/> otherwise.
            /// </value>
            public bool SendDtmf { get; init; }

            /// <summary>
            /// Return whether this call can be switched to another device.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the call can be switched; <see langword="false"/> otherwise.
            /// </value>
            public bool SwitchDevice { get; init; }
        }

        /// <summary>
        /// This property is the device extension number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the device extension number.
        /// </value>
        public string DeviceId { get; init; }

        /// <summary>
        /// This property return the device state.
        /// </summary>
        /// <value>
        /// A <see cref="MediaState"/> that is the state of the device associated to this leg.
        /// </value>
        public MediaState State { get; init; }

        /// <summary>
        /// This property return whether the leg is in <c>RingingOutgoing</c> state and if the remote party is ringing.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the leg is ringing; <see langword="false"/> otherwise.
        /// </value>
        public bool RingingRemote { get; init; }

        /// <summary>
        /// This property gives the leg capabilities.
        /// </summary>
        /// <value>
        /// A <see cref="LegCapabilities"/> object that gives the leg capabilities.
        /// </value>
        public LegCapabilities Capabilities { get; init; }
    }
}
