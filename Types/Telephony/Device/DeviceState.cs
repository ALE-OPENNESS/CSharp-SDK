﻿/*
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

namespace o2g.Types.TelephonyNS.DeviceNS
{
    /// <summary>
    /// <c>DeviceState</c> represents the state of a device.
    /// </summary>
    public class DeviceState
    {
        /// <summary>
        /// This property gives the device phone number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the device extension number.
        /// </value>
        public string DeviceId { get; init; }

        /// <summary>
        /// This property return the device operational state.
        /// </summary>
        /// <value>
        /// A <see cref="OperationalState"/> value that give sthe device operational state.
        /// </value>
        public OperationalState State { get; init; }
    }
}
