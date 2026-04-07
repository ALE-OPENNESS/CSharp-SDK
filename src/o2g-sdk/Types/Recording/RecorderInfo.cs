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

namespace Types.Recording
{
    /// <summary>
    /// Represent a recorder server.
    /// </summary>
    public class RecorderInfo
    {
        /// <summary>
        /// This property is the name of the recorder.
        /// </summary>
        /// <value>
        /// The unique <see langword="string"/> value that identifies the recorder.
        /// </value>
        public string Name { get; init; }

        public string Host { get; init; }

        public string IpAddress { get; init; }

        public bool Secured {  get; init; }

        public string SiteId { get; init; }

        public List<RecordedDevice> Devices { get; init; }

        public bool Connected { get; init; }

        public string status { get; init; }

    }
}
