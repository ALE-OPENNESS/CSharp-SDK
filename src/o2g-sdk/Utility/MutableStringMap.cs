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

using System.Collections.Generic;

namespace o2g.Utility
{
    /// <summary>
    /// A mutable string map object. It provides method to add object referenced by name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MutableStringMap<T> : AbstractStringMap<T>
    {
        internal Dictionary<string, T> Map
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
            }
        }

        /// <summary>
        /// Add a new Value with the specified name.
        /// </summary>
        /// <param name="name">The name of the new value.</param>
        /// <param name="value">The new value.</param>
        public void Add(string name, T value)
        {
            if (_map == null)
            {
                _map = new();
            }
            _map.Add(name, value);
        }
    }
}
