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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Types.CallCenterRsiNS
{
    /// <summary>
    /// <c>RouteRequest</c> represents a routing request done from a RSI point to the listening application.
    /// </summary>
    public class RouteRequest
    {
        /// <summary>
        /// Return the routing session identifier.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats represents the unique identifier of this route request.
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
        /// Return the calling name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats represents the name of the caller who made the call.
        /// </value>
        public string CallingName { get; init; }

        /// <summary>
        /// Return whether the call is internal or external.
        /// </summary>
        /// <value>
        /// A <see cref="RoutingCallerType"/> that represents the call origin.
        /// </value>
        public RoutingCallerType CallerType { get; init; }

        /// <summary>
        /// Return the called number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats represents the called number.
        /// </value>
        public string Called { get; init; }

        /// <summary>
        /// Return the call reference of the call for which the route request is made.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the call reference.
        /// </value>
        public string RoutedCallRef { get; init; }

        /// <summary>
        /// Return the number of overflows.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that represents the maximum number of route requests that can be done before considering an error.
        /// </value>
        public int NbOverflows { get; init; }

        /// <summary>
        /// Return the reason associated to this request.
        /// </summary>
        /// <value>
        /// A <see cref="RoutingReason"/> value that represents the reason associated the route request.
        /// </value>
        public RoutingReason Reason { get; init; }
    }
}
