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

using o2g.Events.CallCenterRsi;

namespace o2g.Types.CallCenterRsiNS
{
    /// <summary>
    /// <c>RouteSession</c> represents a route request session between a RSI point and an application.
    /// <para>
    /// A route session is initiated by a RSI point by sending a <see cref="OnRouteRequestEvent"/>. The application selects a route and answer using <see cref="ICallCenterRsi.RouteSelectAsync(string, string, string, string, string, bool?)"/>.
    /// </para>
    /// </summary>
    /// <see cref="OnRouteRequestEvent"/>
    /// <see cref="ICallCenterRsi.RouteSelectAsync(string, string, string, string, string, bool?)"/>
    public class RouteSession
    {
        /// <summary>
        /// Return the routing session unique identifier.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that identify the route request session.
        /// </value>
        public string RouteCrid { get; init; }

        /// <summary>
        /// Returne the caller number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats represents the number of the caller who made the call.
        /// </value>
        public string Caller { get; init; }

        /// <summary>
        /// Return the called number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats represents the called number.
        /// </value>
        public string Called { get; init; }

        /// <summary>
        /// Return the call reference of the call who which teh route request is made.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the call reference.
        /// </value>
        public string RoutedCallRef { get; init; }
    }
}
