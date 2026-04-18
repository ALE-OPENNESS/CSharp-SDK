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

namespace o2g.Types.TelephonyNS.CallNS.AcdNS
{
    /// <summary>
    /// Represents a pilot handling a call, including its queue information,
    /// transfer status, and possible pilot transfer details.
    /// </summary>
    public class PilotInfo
    {
        /// <summary>
        /// The pilot number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the pilot directory number,
        /// or <see langword="null"/> if not available.
        /// </value>
        public string Number { get; init; }

        /// <summary>
        /// The estimated waiting time in the queue.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> representing the waiting time in seconds,
        /// or <see langword="null"/> if not available.
        /// </value>
        public int? WaitingTime { get; init; }

        /// <summary>
        /// Whether this queue is currently saturated.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the queue is saturated; <see langword="false"/> otherwise.
        /// </value>
        public bool Saturation { get; init; }

        /// <summary>
        /// Whether the transfer on this pilot can be supervised.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if supervised transfer is possible; <see langword="false"/> otherwise.
        /// </value>
        public bool SupervisedTransfer { get; init; }

        /// <summary>
        /// Information about a possible transfer on this pilot.
        /// </summary>
        /// <value>
        /// A <see cref="PilotTransferInfo"/> object, or <see langword="null"/> if
        /// no transfer information is available.
        /// </value>
        public PilotTransferInfo PilotTransferInfo { get; init; }
    }
}