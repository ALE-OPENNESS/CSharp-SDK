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
    /// <c>RsiPoint</c> represents a RSI point. When a call is receive by a RSI routing point, a <see cref="OnRouteRequestEvent"/> is send to the 
    /// application.
    /// </summary>
    public class RsiPoint
    {
        /// <summary>
        /// The RSI point extension number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the RSI point extension number.
        /// </value>
        public string Number { get; init; }

        /// <summary>
        /// The RSI point extension name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the RSI point extension name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// The OmniPcxEnterprise node on which this RSI point is configured.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> that represents the OmniPcxEnterprise node.
        /// </value>
        public int Node { get; init; }

        /// <summary>
        /// Return whether this RSI point is registered.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the RSI point is registered; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// When the RSI point is registered, it can receive call and send route requests.
        /// </remarks>
        public bool Registered { get; init; }
    }
}
