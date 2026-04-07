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

namespace o2g.Events
{
    /// <summary>
    /// An <see cref="EventArgs"/> class that contain an <see cref="O2GEvent"/>.
    /// </summary>
    /// <typeparam name="T">
    /// An <see cref="O2GEvent"/> class.
    /// </typeparam>
    public class O2GEventArgs<T> : EventArgs where T : O2GEvent
    {
        /// <summary>
        /// Construct a new <see cref="O2GEventArgs{T}"/> with the specified event.
        /// </summary>
        /// <param name="ev">
        /// An <see cref="O2GEvent"/> object.
        /// </param>
        public O2GEventArgs(T ev)
        {
            Event = ev;
        }

        /// <summary>
        /// Return the event.
        /// </summary>
        /// <value>
        /// An <see cref="O2GEvent"/> object that is the transported event.
        /// </value>
        public T Event { get; init; }
    }


    /// <summary>
    /// <c>O2GEvent</c> Represents the base class for all <see cref="O2GEvent"/> events.
    /// </summary>
    public class O2GEvent
    {
        /// <summary>
        /// Return the event name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the event name.
        /// </value>
        public string EventName { get; init; }
    }
}
