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
using System.Collections.Generic;

namespace o2g.Types.RecordingNS
{
    /// <summary>
    /// Represent a RecordedDevice.
    /// </summary>
    public class DeviceRecordingInfo
    {
        /// <summary>
        /// This property is the phone number of the Device.
        /// </summary>
        /// <value>
        /// The <see langword="string"/> value that identifies this device.
        /// </value>
        public string Number { get; init; }

        /// <summary>
        /// Gets a value indicating whether this device can perform a record-on-demand request.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the device can be recorded on demand; <see langword="false"/> otherwise.
        /// </value>
        public bool Recordable { get; init; }

        /// <summary>
        /// Start mode capability of the device. <see langword="null"/> if device is not recordable on demand.
        /// </summary>
        /// <value>
        /// A <see cref="RecordingStartType"/> that represents the start recording capabilities.
        /// </value>
        public RecordingStartType StartCapabilities { get; init; }

        /// <summary>
        /// Gets the active call on this device, or <see langword="null"/> if there is no call in progress.
        /// </summary>
        /// <value>
        /// A <see cref="ActiveCall"/> that represents the call in progress, or <see langword="null"/> if there is no call in progress.
        /// </value>
        public ActiveCall ActiveCall { get; init; }
    }
}
