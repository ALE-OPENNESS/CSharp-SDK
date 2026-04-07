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
    /// <c>RoutingState</c> represente a user routing state. 
    /// <para>A routing state is composed of four elements</para>
    /// <list type="table">
    /// <item><term>Remote extension activation</term><description>When the user is configured with a remote extension, he has the possibility to activate or deactivate this remote extension. when the remote extension is de-activated, call are not presented on the mobile device.</description></item>
    /// <item><term>Forward</term><description>The user can configure a forward, on his voice mail if any or on any other number (depending on the cOmniPCX Enterprise configuration).</description></item>
    /// <item><term>Overflow</term><description>The user can configure an overflow on his asociate or on his voce mail. If a forward is configured, it is considered prio the overflow.</description></item>
    /// <item><term>Do Not Disturb</term><description>When Do Not Disturb (DND) is activated, call are not presented to the user.</description></item>
    /// </list>
    /// </summary>
    public class RoutingState
    {
        /// <summary>
        /// Return whether the remote extension is activated.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the remote extension is activated; <see langword="false"/> otherwise.
        /// </value>
        public bool? RemoteExtensionActivated { get; set; }

        /// <summary>
        /// Return the forward.
        /// </summary>
        /// <value>
        /// A <see cref="Forward"/> object that represents the forward state.
        /// </value>
        public Forward Forward { get; set; }

        /// <summary>
        /// Return the Overflow.
        /// </summary>
        /// <value>
        /// A <see cref="Overflow"/> object that represents the overflow state.
        /// </value>
        public Overflow Overflow { get; set; }

        /// <summary>
        /// Return the Do not Disturb state.
        /// </summary>
        /// <value>
        /// A <see cref="DndState"/> object that represents the do not disturb state.
        /// </value>
        public DndState DndState { get; set; }
    }
}
