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
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using System.Collections.Generic;

namespace o2g.Types.TelephonyNS.CallNS
{
    /// <summary>
    /// <c>CallData</c> represents the data associated to a call.
    /// </summary>
    public class CallData
    {
        /// <summary>
        /// The initial intended destination of the call, before any redirection.
        /// </summary>
        /// <value>
        /// A <see cref="PartyInfo"/> identifying the original callee, or
        /// <see langword="null"/> if the information is not available.
        /// </value>
        public PartyInfo InitialCalled { get; init; }

        /// <summary>
        /// The last party that redirected this call.
        /// </summary>
        /// <value>
        /// A <see cref="PartyInfo"/> identifying the party that most recently
        /// redirected this call (e.g. via forward or overflow), or
        /// <see langword="null"/> if the call has not been redirected.
        /// </value>
        public PartyInfo LastRedirecting { get; init; }

        /// <summary>
        /// Whether this is a device call rather than a user call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call is addressed to a specific device;
        /// <see langword="false"/> if the call is addressed to the user.
        /// </value>
        public bool DeviceCall { get; init; }

        /// <summary>
        /// Whether the calling party identity is hidden.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the caller has withheld their identity;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool Anonymous { get; init; }

        /// <summary>
        /// The globally unique identifier of this call.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> UUID that uniquely identifies this call
        /// across the entire O2G network, regardless of transfers or redirections.
        /// </value>
        public string CallUUID { get; init; }

        /// <summary>
        /// The current media state of this call.
        /// </summary>
        /// <value>
        /// A <see cref="MediaState"/> value such as <see cref="MediaState.Active"/>,
        /// <see cref="MediaState.Held"/>, or <see cref="MediaState.RingingIncoming"/>.
        /// </value>
        public MediaState State { get; init; }

        /// <summary>
        /// The current recording state of this call.
        /// </summary>
        /// <value>
        /// A <see cref="RecordState"/> value indicating whether recording is
        /// in progress, paused, or unknown.
        /// <see langword="null"/> if recording is not active on this call.
        /// </value>
        public RecordState RecordState { get; init; }

        /// <summary>
        /// The tags attached to this call.
        /// </summary>
        /// <value>
        /// A list of <see cref="Tag"/> providing application-defined name/value
        /// pairs associated to this call, or <see langword="null"/> if no tags
        /// are attached.
        /// </value>
        public List<Tag> Tags { get; init; }

        /// <summary>
        /// The telephony operations available on this call in its current state.
        /// </summary>
        /// <value>
        /// A <see cref="CallCapabilities"/> object indicating which operations
        /// (transfer, hold, redirect, etc.) are currently permitted on this call.
        /// </value>
        public CallCapabilities Capabilities { get; init; }

        /// <summary>
        /// The correlator data attached to this call, if any.
        /// </summary>
        /// <value>
        /// A <see cref="CorrelatorData"/> instance, or <see langword="null"/> 
        /// if no correlator data is present.
        /// </value>
        /// <remarks>
        /// Correlator data is application-provided context (up to 32 bytes) that
        /// travels with a call across telephony operations such as transfer.
        /// See <see cref="CorrelatorData"/> for details.
        /// </remarks>
        public CorrelatorData CorrelatorData { get; init; }

        /// <summary>
        /// The account code associated to this call, if any.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> carrying the account code entered by the
        /// caller, or <see langword="null"/> if no account info is present.
        /// </value>
        public string AccountInfo { get; init; }

        /// <summary>
        /// The ACD data associated to this call, if it is an ACD call.
        /// </summary>
        /// <value>
        /// An <see cref="AcdData"/> object providing queue, pilot, and routing
        /// information, or <see langword="null"/> if this is not an ACD call.
        /// </value>
        public AcdData AcdCallData { get; init; }

        /// <summary>
        /// The trunk identification for external calls.
        /// </summary>
        /// <value>
        /// A <see cref="TrunkIdentification"/> object providing the network
        /// timeslot and trunk NEQTs, or <see langword="null"/> if this is not
        /// an external call.
        /// </value>
        public TrunkIdentification TrunkIdentification { get; init; }
    }
}
