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
    /// <c>Forwar</c> represents a forward the user has activated.
    /// </summary>
    /// <seealso cref="IRouting.GetForwardAsync(string)"/>
    public class Forward
    {
        /// <summary>
        /// <c>ForwardCondition</c> represents the possible condition a user can associate to a forward.
        /// </summary>
        public enum ForwardCondition
        {
            /// <summary>
            /// Incoming calls are immediately forwarded on the target.
            /// </summary>
            Immediate,
            /// <summary>
            /// Incoming calls are forwarded on the target if the user is busy.
            /// </summary>
            Busy,
            /// <summary>
            /// Incoming calls are forwarded on the target if the user does not answer the call.
            /// </summary>
            NoAnswer,
            /// <summary>
            /// Incoming calls are forwarded on the target if the user is busy or if the user does not answer the call.
            /// </summary>
            BusyOrNoAnswer
        }


        /// <summary>
        /// Return the destination of this forward.
        /// </summary>
        /// <value>
        /// A <see cref="Destination"/> that represent the forward destination.
        /// </value>
        public Destination Destination { get; set; }

        /// <summary>
        /// Return the condition associated to this forward.
        /// </summary>
        /// <value>
        /// An optional <see cref="ForwardCondition"/> that represents the associated condition or an unset value when there 
        /// is no forward configured. (<c>Destination.Node</c>
        /// </value>
        public ForwardCondition? Condition { get; set; }

        /// <summary>
        /// Return the number on which teh user is forwarded.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the extension number on which the user is forwarded, or <see langword="null"/> 
        /// if there is no forward activated or if the forward is activated on the user voice mail.
        /// </value>
        public string Number { get; set; }
    }
}
