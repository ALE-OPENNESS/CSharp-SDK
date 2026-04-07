/*
* Copyright 2025 ALE International
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
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterRealtimeNS
{

    /// <summary>
    /// Represents an abstract RTI filter that can filter objects by numbers and attributes.
    /// </summary>
    /// <typeparam name="T">The type of the attributes in the filter.</typeparam>
    public abstract class AbstractRtiFilter<T>
    {
        private readonly HashSet<string> numbers = new HashSet<string>();
        private readonly HashSet<T> attributes = new HashSet<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractRtiFilter{T}"/> class.
        /// </summary>
        protected AbstractRtiFilter()
        {
        }

        /// <summary>
        /// Returns the set of numbers in this filter.
        /// </summary>
        /// <returns>A <see cref="HashSet{String}"/> containing the numbers.</returns>
        public HashSet<string> GetNumbers()
        {
            return numbers;
        }

        /// <summary>
        /// Returns the set of attributes in this filter.
        /// </summary>
        /// <returns>A <see cref="HashSet{T}"/> containing the attributes.</returns>
        public HashSet<T> GetAttributes()
        {
            return attributes;
        }

        /// <summary>
        /// Adds an array of numbers to this filter.
        /// </summary>
        /// <param name="numbers">The directory numbers of the objects to add in the RTI filter.</param>
        public void AddNumbers(params string[] numbers)
        {
            foreach (var number in numbers)
            {
                this.numbers.Add(number);
            }
        }

        /// <summary>
        /// Adds an array of attributes to this filter.
        /// </summary>
        /// <param name="attributes">The attributes to add in the RTI filter.</param>
        public void AddAttributes(params T[] attributes)
        {
            foreach (var attr in attributes)
            {
                this.attributes.Add(attr);
            }
        }
    }
}
