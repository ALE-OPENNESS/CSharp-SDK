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
using System.Collections.Generic;
using Types.Recording;

namespace o2g.Types.RecordingNS
{
    /// <summary>
    /// Represent the global status of recording service.
    /// </summary>
    public class RecordingStatus
    {
        /// <summary>
        /// The list of configured recorders.
        /// </summary>
        /// <value>
        /// A list of <see cref="RecorderInfo"/> object that represents configured recorders, or <see langword="null"/> in case
        /// of error or if there is no recorder configured.
        /// </value>
        public List<RecorderInfo> Recorders { get; init; }

        /// <summary>
        /// The list of recorded devices.
        /// </summary>
        /// <value>
        /// A list of <see cref="RecordedDevice"/> object that represents recorded devices, or <see langword="null"/> in case
        /// of error or if there is no device recorded.
        /// </value>
        public List<RecordedDevice> Devices { get; init; }
    }
}
