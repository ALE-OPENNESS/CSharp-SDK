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
    /// <c>Info</c> represents the acd information for this call
    /// </summary>
    public class Info
    {
        /// <summary>
        /// Return the waiting time in a queue from which the call has been distributed.
        /// </summary>
        /// <value>
        /// The estimated waiting time in seconds.
        /// </value>
        public int QueueWaitingTime { get; init; }

        /// <summary>
        /// Return the global waiting time in the CCD.
        /// </summary>
        /// <value>
        /// The estimated global waiting time.
        /// </value>
        public int GlobalWaitingTime { get; init; }

        /// <summary>
        /// Return the agent group the agent who answer the call is logged in.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the agent group number.
        /// </value>
        public string AgentGroup { get; init; }

        /// <summary>
        /// Return whether it is a local acd call
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the acd call is local; <see langword="false"/> otherwise.
        /// </value>
        public bool Local { get; init; }
    }
}
