﻿/*
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

namespace o2g.Types.CommunicationLogNS
{
    /// <summary>
    /// The <c>Option</c> enum defines the possible filter option when querying a communication log.
    /// </summary>
    /// <remarks>
    /// It's possible to combine the options
    /// <code>
    ///     // define an option to filter unanswered outgoing calls
    ///     option = Option.Unanswered | Option.Unacknowledged;
    /// </code>
    /// </remarks>
    /// <seealso cref="ICommunicationLog.GetComRecordsAsync(QueryFilter, Page, bool, string)"/>
    [Flags]
    public enum Option
    {
        /// <summary>
        /// The default value that represents no filtering.
        /// </summary>
        None = 0,

        /// <summary>
        /// Use this value to filter unanswered calls.
        /// </summary>
        Unanswered = 1, 

        /// <summary>
        /// Use this value to filter unacknowledged calls.
        /// </summary>
        Unacknowledged = 2
    }
}
