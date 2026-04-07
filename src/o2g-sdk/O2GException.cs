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

namespace o2g
{
    /// <summary>
    /// <c>O2GException</c> specifies information about a generic O2G SDK exception.
    /// </summary>
    public class O2GException : Exception
    {
        /// <summary>
        /// Create a new instance of the <see cref="O2GException"/> class.
        /// </summary>
        public O2GException()
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="O2GException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public O2GException(string message): base(message)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="O2GException"/> class with a specified error message and a 
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a <see langword="null"/> reference if no inner exception is specified.</param>
        public O2GException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
