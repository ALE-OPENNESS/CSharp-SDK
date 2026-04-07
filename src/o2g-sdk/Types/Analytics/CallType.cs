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

using System.Text.Json.Serialization;

namespace o2g.Types.AnalyticsNS
{
    /// <summary>
    /// <c>CallType</c> represents the possible call type in a charging ticket.
    /// </summary>
    /// <seealso cref="Charging"/>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: CallType.Unspecified)]
    public enum CallType
    {
        /// <summary>
        /// A public network call.
        /// </summary>
        PublicNetworkCall,

        /// <summary>
        /// A public network call through a private network.
        /// </summary>
        PublicNetworkCallThroughPrivateNetwork,

        /// <summary>
        /// A private network call.
        /// </summary>
        PrivateNetworkCall,

        /// <summary>
        /// A local network call.
        /// </summary>
        LocalNetworkCall,

        /// <summary>
        /// An incoming call from the public network.
        /// </summary>
        PublicNetworkIncomingCall,

        /// <summary>
        /// An incoming call from the public network through the private network.
        /// </summary>
        PublicNetworkIncomingCallThroughPrivateNetwork,

        /// <summary>
        /// An outgoing call from private network to the public network.
        /// </summary>
        PrivateNetworkOutgoingCallToPublicNetwork,

        /// <summary>
        /// An outgoing call from private network to the private network.
        /// </summary>
        PrivateNetworkOutgoingCallToPrivateNetwork,

        /// <summary>
        /// An incoming call from the public network to the private network.
        /// </summary>
        PublicNetworkIncomingCallToPrivateNetwork,

        /// <summary>
        /// An incoming call from the private network to the private network.
        /// </summary>
        PrivateNetworkIncomingCallToPrivateNetwork,

        /// <summary>
        /// An outgoing call through to the private network.
        /// </summary>
        PublicOrPrivateNetworkOutgoingCallThroughPrivateNetwork,

        /// <summary>
        /// An incoming call through to the private network.
        /// </summary>
        PublicOrPrivateNetworkIncomingCallThroughPrivateNetwork,

        /// <summary>
        /// An incoming call from private network.
        /// </summary>
        PrivateNetworkIncomingCall,

        /// <summary>
        /// A local call.
        /// </summary>
        LocalNode,

        /// <summary>
        /// A call in transit on this node
        /// </summary>
        LocalTransit,

        /// <summary>
        /// Not specified call type.
        /// </summary>
        Unspecified
    }
}
