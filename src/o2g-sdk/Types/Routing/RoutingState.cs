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
    /// Represents the complete routing state of a user.
    /// <para>
    /// The routing state aggregates four independent routing features:
    /// </para>
    /// <list type="table">
    /// <listheader><term>Feature</term><description>Description</description></listheader>
    /// <item>
    ///   <term>Remote extension</term>
    ///   <description>
    ///   When configured, a remote extension (e.g. a mobile device) can be activated or
    ///   deactivated. When deactivated, the device does not ring on incoming calls but can
    ///   still be used to place outgoing calls.
    ///   </description>
    /// </item>
    /// <item>
    ///   <term>Forward</term>
    ///   <description>
    ///   Redirects incoming calls to the user's voice mail or to another number,
    ///   subject to an optional condition. Takes priority over overflow.
    ///   </description>
    /// </item>
    /// <item>
    ///   <term>Overflow</term>
    ///   <description>
    ///   Redirects incoming calls to the user's voice mail when the user is busy
    ///   or does not answer. Only applies when no forward is active.
    ///   </description>
    /// </item>
    /// <item>
    ///   <term>Do Not Disturb</term>
    ///   <description>
    ///   When active, no calls are presented to the user.
    ///   </description>
    /// </item>
    /// </list>
    /// </summary>
    /// <seealso cref="IRouting.GetRoutingStateAsync(string)"/>
    public class RoutingState
    {
        /// <summary>
        /// Whether the user's remote extension is currently activated.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the remote extension is activated and will ring
        /// on incoming calls; <see langword="false"/> if it is deactivated;
        /// <see langword="null"/> if the user has no remote extension configured.
        /// </value>
        /// <seealso cref="IRouting.ActivateRemoteExtensionAsync(string)"/>
        /// <seealso cref="IRouting.DeactivateRemoteExtensionAsync(string)"/>
        public bool? RemoteExtensionActivated { get; set; }

        /// <summary>
        /// The forward currently configured for the user.
        /// </summary>
        /// <value>
        /// A <see cref="Forward"/> object describing the active forward,
        /// or a <see cref="Forward"/> with <see cref="Destination.None"/> if
        /// no forward is configured.
        /// </value>
        /// <seealso cref="IRouting.GetForwardAsync(string)"/>
        public Forward Forward { get; set; }

        /// <summary>
        /// The overflow currently configured for the user.
        /// </summary>
        /// <value>
        /// An <see cref="Overflow"/> object describing the active overflow,
        /// or an <see cref="Overflow"/> with <see cref="Destination.None"/> if
        /// no overflow is configured.
        /// </value>
        /// <seealso cref="IRouting.GetOverflowAsync(string)"/>
        public Overflow Overflow { get; set; }

        /// <summary>
        /// The current Do Not Disturb state of the user.
        /// </summary>
        /// <value>
        /// A <see cref="DndState"/> object indicating whether DND is active,
        /// or <see langword="null"/> if the DND state is not available.
        /// </value>
        /// <seealso cref="IRouting.GetDndStateAsync(string)"/>
        public DndState DndState { get; set; }
    }
}

