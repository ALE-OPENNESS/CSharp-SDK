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

using System.Text.Json.Serialization;

namespace o2g.Types.RoutingNS
{
    /// <summary>
    /// Represents the routing capabilities available to a user.
    /// <para>
    /// Capabilities indicate which routing features the user is allowed to configure,
    /// based on their OmniPCX Enterprise profile and license. Query capabilities
    /// before attempting to activate routing features to avoid unnecessary errors.
    /// </para>
    /// </summary>
    /// <seealso cref="IRouting.GetCapabilitiesAsync(string)"/>
    public class RoutingCapabilities
    {
        /// <summary>
        /// Whether the user can manage their remote extension activation.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can activate or deactivate their
        /// remote extension; <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="IRouting.ActivateRemoteExtensionAsync(string)"/>
        /// <seealso cref="IRouting.DeactivateRemoteExtensionAsync(string)"/>
        [JsonPropertyName("presentationRoute")]
        public bool CanManageRemoteExtension { get; set; }

        /// <summary>
        /// Whether the user can configure a forward.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can activate, modify, or cancel
        /// a forward; <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="IRouting.ForwardOnNumberAsync(string, Forward.ForwardCondition, string)"/>
        /// <seealso cref="IRouting.ForwardOnVoiceMailAsync(Forward.ForwardCondition, string)"/>
        /// <seealso cref="IRouting.CancelForwardAsync(string)"/>
        [JsonPropertyName("forwardRoute")]
        public bool CanManageForward { get; set; }

        /// <summary>
        /// Whether the user can configure an overflow.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can activate, modify, or cancel
        /// an overflow; <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="IRouting.OverflowOnVoiceMailAsync(Overflow.OverflowCondition, string)"/>
        /// <seealso cref="IRouting.CancelOverflowAsync(string)"/>
        [JsonPropertyName("overflowRoute")]
        public bool CanManageOverflow { get; set; }

        /// <summary>
        /// Whether the user can manage Do Not Disturb.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can activate or cancel
        /// Do Not Disturb; <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="IRouting.ActivateDndAsync(string)"/>
        /// <seealso cref="IRouting.CancelDndAsync(string)"/>
        [JsonPropertyName("dnd")]
        public bool CanManageDnd { get; set; }
    }
}

