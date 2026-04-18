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

using o2g.Types.TelephonyNS.CallNS;
using System.Collections.Generic;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// Represents the trunk identification of an external call.
    /// <para>
    /// Provides the network timeslot and trunk NEQTs (Network Equipment Queuing
    /// Terminations) associated with the trunk used for the call.
    /// Available via <see cref="CallData.TrunkIdentification"/> on external calls.
    /// </para>
    /// </summary>
    public class TrunkIdentification
    {
        /// <summary>
        /// The network timeslot used by this trunk.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that identifies the network timeslot.
        /// </value>
        public int NetworkTimeslot { get; init; }

        /// <summary>
        /// The list of trunk NEQTs (Network Equipment Queuing Terminations).
        /// </summary>
        /// <value>
        /// A list of <see langword="int"/> representing the trunk NEQTs.
        /// </value>
        public List<int> TrunkNeqt { get; init; }
    }
}