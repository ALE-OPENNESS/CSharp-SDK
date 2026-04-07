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
using o2g.Types.TelephonyNS.CallNS;
using System.Collections.Generic;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// <c>CallCapabilities</c> represents the capabilities available on a call : the action that can be carried out on the call according to its state.
    /// </summary>
    public class CallCapabilities
    {
        /// <summary>
        /// Return whether a device can be added.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a device can be added; <see langword="false"/> otherwise.
        /// </value>
        public bool AddDevice { get; init; }

        /// <summary>
        /// Return whether a participant can be added.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a participant can be added; <see langword="false"/> otherwise.
        /// </value>
        public bool AddParticipant { get; init; }

        /// <summary>
        /// Return whether this call can be intruted by another user.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be intruded; <see langword="false"/> otherwise.
        /// </value>
        public bool Intruded { get; init; }

        /// <summary>
        /// Return whether it is possible to make intrusion on the user called through this call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the intrusion is possible; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>Available from O2G 2.4</remarks>
        public bool Intrusion { get; init; }

        /// <summary>
        /// Return whether the call can be transferred.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be transferred; <see langword="false"/> otherwise.
        /// </value>
        public bool Transfer { get; init; }

        /// <summary>
        /// Return whether the call can be blind transferred.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be blind transferred; <see langword="false"/> otherwise.
        /// </value>
        public bool BlindTransfer { get; init; }

        /// <summary>
        /// Return whether the call can be merged with another one.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be merged; <see langword="false"/> otherwise.
        /// </value>
        public bool Merge { get; init; }

        /// <summary>
        /// Return whether the call can be redirected to another device.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be redirected; <see langword="false"/> otherwise.
        /// </value>
        public bool Redirect { get; init; }

        /// <summary>
        /// Return whether a call can be picked-up by another user.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be picked-up; <see langword="false"/> otherwise.
        /// </value>
        public bool PickedUp { get; init; }

        /// <summary>
        /// Return whether the call can be redirected to the voice mail.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be redirected to the voice mail; <see langword="false"/> otherwise.
        /// </value>
        public bool RedirectToVoiceMail { get; init; }

        /// <summary>
        /// Return whether a call can be overflowed on the voice mail of the called user.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be overflowed; <see langword="false"/> otherwise.
        /// </value>
        public bool OverflowToVoiceMail { get; init; }

        /// <summary>
        /// Return whether this user can be dropped from this call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can be dropped; <see langword="false"/> otherwise.
        /// </value>
        public bool DropMe { get; init; }

        /// <summary>
        /// Return whether the call can be terminated for all users.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be terminated; <see langword="false"/> otherwise.
        /// </value>
        public bool Terminate { get; init; }

        /// <summary>
        /// Return whether the call can be rejected.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be rejected; <see langword="false"/> otherwise.
        /// </value>
        public bool Reject { get; init; }

        /// <summary>
        /// Return whether a callback request is possible.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a callback request is possible; <see langword="false"/> otherwise.
        /// </value>
        public bool CallBack { get; init; }

        /// <summary>
        /// Return whether the call can be parked.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call can be parked; <see langword="false"/> otherwise.
        /// </value>
        public bool Park { get; init; }

        /// <summary>
        /// Return whether it's possible to start the recording of this call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if it's possible to start the recording; <see langword="false"/> otherwise.
        /// </value>
        public bool StartRecord { get; init; }

        /// <summary>
        /// Return whether it's possible to stop the recording of this call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if it's possible to stop the recording; <see langword="false"/> otherwise.
        /// </value>
        public bool StopRecord { get; init; }

        /// <summary>
        /// Return whether it's possible to pause the recording of this call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if it's possible to pause the recording; <see langword="false"/> otherwise.
        /// </value>
        public bool PauseRecord { get; init; }

        /// <summary>
        /// Return whether it's possible to resume the paused recording of this call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if it's possible to resume the recording; <see langword="false"/> otherwise.
        /// </value>
        public bool ResumeRecord { get; init; }

        /// <summary>
        /// Return whether a participant can be dropped.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a participant can be dropped; <see langword="false"/> otherwise.
        /// </value>
        public bool DropParticipant { get; init; }

        /// <summary>
        /// Return whether a participant can be muted.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a participant can be muted; <see langword="false"/> otherwise.
        /// </value>
        public bool MuteParticipant { get; init; }
        /// <summary>
        /// Return whether a participant can be put on hold.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if a participant can be put on hold; <see langword="false"/> otherwise.
        /// </value>
        public bool HoldParticipant { get; init; }
    }


    /// <summary>
    /// Represent a call. A call is identified by its <see cref="CallRef"/>, a unique <see langword="string"/> generated by the O2G server.
    /// </summary>
    public class PbxCall
    {
        /// <summary>
        /// This property is the reference of this call.
        /// </summary>
        /// <value>
        /// The unique <see langword="string"/> value that identifies this call.
        /// </value>
        public string CallRef { get; init; }

        /// <summary>
        /// This property get  the data associated to this call.
        /// </summary>
        /// <value>
        /// A <see cref="CallData"/> object that represents the data associated to this call.
        /// </value>
        public CallData CallData { get; init; }

        /// <summary>
        /// The list of legs associated to this call.
        /// </summary>
        /// <value>
        /// A list of <see cref="Leg"/>. A <c>Leg</c> represents the user's device involved in a call for a dedicated media (audio or video).
        /// </value>
        public List<Leg> Legs { get; init; }

        /// <summary>
        /// The list of participants associated to this call.
        /// </summary>
        /// <value>
        /// A list of <see cref="Participant"/>. A <c>Participant</c> represents the other party in this call.
        /// </value>
        public List<Participant> Participants { get; init; }
    }
}
