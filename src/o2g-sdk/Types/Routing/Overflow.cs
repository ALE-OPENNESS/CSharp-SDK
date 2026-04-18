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

namespace o2g.Types.RoutingNS
{
    /// <summary>
    /// Represents the overflow currently configured for a user.
    /// <para>
    /// An overflow redirects incoming calls to the user's voice mail when a
    /// specified condition is met. Unlike a forward, an overflow only applies
    /// when a forward is not already active. Use
    /// <see cref="IRouting.OverflowOnVoiceMailAsync(Overflow.OverflowCondition, string)"/>
    /// to activate an overflow, and <see cref="IRouting.CancelOverflowAsync(string)"/>
    /// to cancel it.
    /// </para>
    /// </summary>
    /// <seealso cref="IRouting.GetOverflowAsync(string)"/>
    /// <seealso cref="IRouting.OverflowOnVoiceMailAsync(Overflow.OverflowCondition, string)"/>
    /// <seealso cref="IRouting.CancelOverflowAsync(string)"/>
    public class Overflow
    {
        /// <summary>
        /// Represents the condition under which an overflow is triggered.
        /// </summary>
        public enum OverflowCondition
        {
            /// <summary>
            /// Incoming calls are redirected to the target if the user is busy.
            /// </summary>
            Busy,

            /// <summary>
            /// Incoming calls are redirected to the target if the user does not answer.
            /// </summary>
            NoAnswer,

            /// <summary>
            /// Incoming calls are redirected to the target if the user is busy
            /// or does not answer.
            /// </summary>
            BusyOrNoAnswer
        }

        /// <summary>
        /// The destination to which calls are redirected on overflow.
        /// </summary>
        /// <value>
        /// A <see cref="Destination"/> value indicating where calls are redirected.
        /// <see cref="Destination.None"/> indicates no overflow is configured.
        /// </value>
        public Destination Destination { get; set; }

        /// <summary>
        /// The condition under which this overflow is triggered.
        /// </summary>
        /// <value>
        /// An <see cref="OverflowCondition"/> value, or <see langword="null"/> if
        /// no overflow is configured (i.e. <see cref="Destination"/> is <see cref="Destination.None"/>).
        /// </value>
        public OverflowCondition? Condition { get; set; }
    }
}

