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
    /// Represents the Do Not Disturb (DND) state of a user.
    /// <para>
    /// When Do Not Disturb is active, incoming calls are not presented to the user.
    /// Use <see cref="IRouting.ActivateDndAsync(string)"/> to activate and
    /// <see cref="IRouting.CancelDndAsync(string)"/> to deactivate.
    /// </para>
    /// </summary>
    /// <seealso cref="IRouting.GetDndStateAsync(string)"/>
    /// <seealso cref="IRouting.ActivateDndAsync(string)"/>
    /// <seealso cref="IRouting.CancelDndAsync(string)"/>
    /// <seealso cref="RoutingState.DndState"/>
    public class DndState
    {
        /// <summary>
        /// Whether Do Not Disturb is currently activated.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if DND is active and calls are not presented to the user;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool Activate { get; init; }
    }
}