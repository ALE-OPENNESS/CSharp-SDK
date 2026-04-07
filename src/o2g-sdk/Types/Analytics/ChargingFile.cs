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

using System;

namespace o2g.Types.AnalyticsNS
{
    /// <summary>
    /// <c>ChargingFile</c> class represent a chaging file on OmniPCX Enterprise.
    /// </summary>
    public class ChargingFile
    {
        /// <summary>
        /// Return the file name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the file name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Return this file timestamp.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the timestamp of this file.
        /// </value>
        public DateTime Timestamp { get; init; }
    }
}
