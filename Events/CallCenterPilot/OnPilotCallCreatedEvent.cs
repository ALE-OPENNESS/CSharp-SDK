/*
* Copyright 2024 ALE International
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

namespace o2g.Events.CallCenterPilot
{
    /// <summary>
    /// This event is raised when a new call arrive on a CCD pilot.
    /// </summary>
    public class OnPilotCallCreatedEvent : O2GEvent
    {
        /// <summary>
        /// Return the pilot number on which the call arrived.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the pilot directory number.
        /// </value>
        public string Pilot { get; init; }

        /// <summary>
        /// Return the call reference.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the unique call identifier.
        /// </value>
        public string CallRef { get; init; }

        /// <summary>
        /// Return the cause of this call creation.
        /// </summary>
        /// <value>
        /// A <see cref="Cause"/> that give the reason of this call creation.
        /// </value>
        public Cause Cause { get; init; }

        /// <summary>
        /// Return the caller number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the caller number.
        /// </value>
        public string Caller { get; init; }

    }
}
