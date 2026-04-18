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
    /// Represents the forward currently configured for a user.
    /// <para>
    /// A forward redirects incoming calls to a target destination — either the user's
    /// voice mail or another phone number — subject to an optional condition.
    /// Use <see cref="IRouting.ForwardOnNumberAsync(string, Forward.ForwardCondition, string)"/>
    /// or <see cref="IRouting.ForwardOnVoiceMailAsync(Forward.ForwardCondition, string)"/>
    /// to activate a forward, and <see cref="IRouting.CancelForwardAsync(string)"/> to cancel it.
    /// </para>
    /// </summary>
    /// <seealso cref="IRouting.GetForwardAsync(string)"/>
    /// <seealso cref="IRouting.ForwardOnNumberAsync(string, Forward.ForwardCondition, string)"/>
    /// <seealso cref="IRouting.ForwardOnVoiceMailAsync(Forward.ForwardCondition, string)"/>
    /// <seealso cref="IRouting.CancelForwardAsync(string)"/>
    public class Forward
    {
        /// <summary>
        /// Represents the condition under which a forward is triggered.
        /// </summary>
        public enum ForwardCondition
        {
            /// <summary>
            /// All incoming calls are immediately forwarded to the target,
            /// regardless of the user's availability.
            /// </summary>
            Immediate,

            /// <summary>
            /// Incoming calls are forwarded to the target only if the user is busy.
            /// </summary>
            Busy,

            /// <summary>
            /// Incoming calls are forwarded to the target only if the user does not answer.
            /// </summary>
            NoAnswer,

            /// <summary>
            /// Incoming calls are forwarded to the target if the user is busy
            /// or does not answer.
            /// </summary>
            BusyOrNoAnswer
        }

        /// <summary>
        /// The destination to which calls are forwarded.
        /// </summary>
        /// <value>
        /// A <see cref="Destination"/> value indicating where calls are redirected.
        /// <see cref="Destination.None"/> indicates no forward is configured.
        /// </value>
        public Destination Destination { get; set; }

        /// <summary>
        /// The condition under which this forward is triggered.
        /// </summary>
        /// <value>
        /// A <see cref="ForwardCondition"/> value, or <see langword="null"/> if
        /// no forward is configured (i.e. <see cref="Destination"/> is <see cref="Destination.None"/>).
        /// </value>
        public ForwardCondition? Condition { get; set; }

        /// <summary>
        /// The phone number to which calls are forwarded.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the target extension number,
        /// or <see langword="null"/> if no forward is active or if the forward
        /// destination is the voice mail rather than a number.
        /// </value>
        public string Number { get; set; }
    }
}
