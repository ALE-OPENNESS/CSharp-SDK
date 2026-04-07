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

namespace o2g.Types.TelephonyNS.CallNS.AcdNS
{
    /// <summary>
    /// <c>AcdData</c> represents the acd extension for an acd call.
    /// </summary>
    public class AcdData
    {
        /// <summary>
        /// Return the information associated to this acd call.
        /// </summary>
        /// <value>
        /// A <see cref="Info"/> object that gives the information on the acd call.
        /// </value>
        public Info CallInfo { get; init; }

        /// <summary>
        /// Return the information about the queue that has distributed this call.
        /// </summary>
        /// <value>
        /// A <see cref="QueueData"/> that gives information on the queue.
        /// </value>
        public QueueData QueueData { get; init; }

        /// <summary>
        /// Return the pilot who has distributed this call.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the pilot directory number.
        /// </value>
        public string PilotNumber { get; init; }

        /// <summary>
        /// Return the RSI point that has distribuet this call.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the RSI point directory number.
        /// </value>
        public string RsiNumber { get; init; }

        /// <summary>
        /// Return whether the transfer on the pilot was supervised.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the transfer has been supervised; <see langword="false"/> otherwise.
        /// </value>
        public bool SupervisedTransfer { get; init; }

        /// <summary>
        /// Return The information about the possible transfer on a pilot.
        /// </summary>
        /// <value>
        /// A <see cref="PilotTransferInfo"/> The information about the possible transfer or <see langword="null"/> if there is no transfer in progress.
        /// </value>
        public PilotTransferInfo PilotTransferInfo { get; init; }
    }
}
