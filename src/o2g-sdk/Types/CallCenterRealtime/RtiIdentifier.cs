/*
* Copyright 2026 ALE International
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Types.CallCenterRealtimeNS
{
    /// <summary>
    /// Represents a CCD object identifier for which real-time information is available.
    ///
    /// Instances of this class provide details about a CCD object, such as agents, pilots,
    /// or queues, that can be monitored using the CallCenterRealtime.
    ///
    /// Each object includes a directory number, last name, first name, and a unique key.
    /// This class is typically used to retrieve and reference objects for real-time events.
    /// </summary>
    public class RtiObjectIdentifier
    {
        /// <summary>
        /// Gets the directory number associated with this identifier.
        /// </summary>
        public string Number { get; init; }

        /// <summary>
        /// Gets the last name associated with this identifier.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the first name associated with this identifier.
        /// Returns null if not provided.
        /// </summary>
        public string FirstName { get; init; }
    }
}