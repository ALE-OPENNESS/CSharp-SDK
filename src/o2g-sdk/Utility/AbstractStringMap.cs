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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace o2g.Utility
{
    /// <summary>
    /// An abstract string map. It provides method to manipulate object references by name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractStringMap<T> : IEnumerable<T>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected Dictionary<string, T> _map;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Return the object model with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// The object or <see langword="null"/> if there is no such object model.
        /// </returns>
        public T this[string name]
        {
            get
            {
                if (_map.ContainsKey(name))
                {
                    return _map[name];
                }
                else
                {
                    return default(T);
                }
            }
        }


        /// <summary>
        /// Determines whether the <c>AbstractStringMap</c> contains the specified key.
        /// </summary>
        /// <param name="key">The key parameter to search.</param>
        /// <returns>
        /// <see langword="true"/> if the specified key is in the map; <see langword="false"/> otherwise
        /// </returns>
        public bool Contains(string key) => _map.ContainsKey(key);


        /// <summary>
        /// Return the list of the names.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the name.
        /// </value>
        public List<string> Names { get => _map.Keys.ToList(); }

        /// <summary>
        /// Return the list of child object models.
        /// </summary>
        /// <returns>
        /// A list of objects that represents the child object models.
        /// </returns>
        public List<T> AsList() => _map.Values.ToList();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return AsList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return AsList().GetEnumerator();
        }
    }
}