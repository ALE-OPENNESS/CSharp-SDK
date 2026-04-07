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
using o2g.Types.TelephonyNS.CallNS;
using o2g.Types.TelephonyNS.DeviceNS;
using System.Collections.Generic;

namespace o2g.Events.Telephony
{
    /// <summary>
    /// <c>OnCallModifiedEvent</c> is raised when an existing call has been modified.
    /// <para>Modification of a call can be triggered for various reason: changes on legs, on participants, changes on states, ...</para>
    /// </summary>
    public class OnCallModifiedEvent : O2GEvent
    {
        /// <summary>
        /// Return the login name of the user for which the event is raised.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the user login name.
        /// </value>
        public string LoginName { get; init; }

        /// <summary>
        /// Return the call reference.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the unique call identifier.
        /// </value>
        public string CallRef { get; init; }

        /// <summary>
        /// Return the cause of this call modification.
        /// </summary>
        /// <value>
        /// A <see cref="Cause"/> that give the reason of this call creation.
        /// </value>
        public Cause Cause { get; init; }

        /// <summary>
        /// Return the previous call reference.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the previous call reference, or <see langword="null"/> if the call reference has not been changed.
        /// </value>
        /// <remarks>
        /// If not <see langword="null"/>, this call reference indicates that the "<c>callRef</c>" replace "<c>previousCallRef</c>". This also indicates that "previousCallRef" has been removed <b>(call removed event is not generated).</b>
        /// </remarks>
        public string PreviousCallRef { get; init; }

        /// <summary>
        /// Return the call reference that has replaced the call reference
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the call reference that replace the "<c>CallRef</c>".
        /// </value>
        /// <remarks>
        /// This call reference is not <see langword="null"/> when a call is released. The replacedByCallRef indicates that the "callRef" is replaced by this one.
        /// </remarks>
        public string ReplacedByCallRef { get; init; }

        /// <summary>
        /// Return the data associated to this call.
        /// </summary>
        /// <value>
        /// A <see cref="CallData"/> that represents the data associated to this call.
        /// </value>
        public CallData CallData { get; init; }

        /// <summary>
        /// Return the modified legs.
        /// </summary>
        /// <value>
        /// A list of <see cref="Leg"/> that corresponds to all the modified legs.
        /// </value>
        public List<Leg> ModifiedLegs { get; init; }

        /// <summary>
        /// Return the list of added legs.
        /// </summary>
        /// <value>
        /// A list of <see cref="Leg"/> that corresponds to all the added legs.
        /// </value>
        public List<Leg> AddedLegs { get; init; }

        /// <summary>
        /// Return the list of removed legs.
        /// </summary>
        /// <value>
        /// A list of <see cref="Leg"/> that corresponds to all the removed legs.
        /// </value>
        public List<Leg> RemovedLegs { get; init; }

        /// <summary>
        /// Return the list of modified participants.
        /// </summary>
        /// <value>
        /// A list of <see cref="Participant"/> that corresponds to all the modified participants.
        /// </value>
        public List<Participant> ModifiedParticipants { get; init; }

        /// <summary>
        /// Return the list of added participants
        /// </summary>
        /// <value>
        /// A list of <see cref="Participant"/> that corresponds to all the added participants.
        /// </value>
        public List<Participant> AddedParticipants { get; init; }

        /// <summary>
        /// Return the list of removed participant.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/>. Each string is the participantId of a removed participant.
        /// </value>
        public List<string> RemovedParticipantIds { get; init; }

        /// <summary>
        /// Return the capabilities of the devices associated to this call.
        /// </summary>
        /// <value>
        /// A list of <see cref="DeviceCapabilities"/>. 
        /// </value>
        public List<DeviceCapabilities> DeviceCapabilities { get; init; }
    }
}
