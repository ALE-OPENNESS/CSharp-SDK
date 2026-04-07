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

using o2g.Types.CallCenterRsiNS;

namespace o2g.Events.CallCenterRsi
{
    /// <summary>
    /// This event is sent from a Routing point to close a route session (routing crid is no longer valid).
    /// </summary>
    public class OnRouteEndEvent : O2GEvent
    {
        /// <summary>
        /// Return the RSI point number which is the origin of the request.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the RSI point extension number which is the origin of the request.
        /// </value>
        public string RsiPoint { get; init; }

        /// <summary>
        /// Return the routing session identifier.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats represents the unique identifier of this route request.
        /// </value>
        public string RouteCrid { get; init; }

        /// <summary>
        /// Return the call reference of the call who which teh route request is made.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the call reference.
        /// </value>
        public string RoutedCallRef { get; init; }

        /// <summary>
        /// Return the reason associated to the request.
        /// </summary>
        /// <value>
        /// A <see cref="RoutingReason"/> value that represents the reason associated the route request.
        /// </value>
        public RoutingReason Reason { get; init; }
    }
}
