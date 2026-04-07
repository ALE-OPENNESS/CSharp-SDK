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

namespace o2g.Types.CallCenterRsiNS
{
    /// <summary>
    /// <c>AdditionalDigitCollectionCriteria</c> class is used to provide advanced digit collection criteria.
    /// </summary>
    public class AdditionalDigitCollectionCriteria
    {
        /// <summary>
        /// Return the set of digits used to consider that digits collection is aborted. 
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represent the abort digits.
        /// </value>
        public string AbortDigits { get; set; }

        /// <summary>
        /// Return the digits ignored during the collection.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represent the ignored digits.
        /// </value>
        public string IgnoreDigits { get; set; }

        /// <summary>
        /// Return the digits used to deleted the collected digit from the collection.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represent the delete digits.
        /// </value>
        public string BackspaceDigits { get; set; }

        /// <summary>
        /// Return the digits used to validate the collection. If the flushChar ECMA3 standard parameter is present, 
        /// the termDigits parameter is ignored by the OmniPCX Enterprise.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represent the termination digits.
        /// </value>
        public string TermDigits { get; set; }

        /// <summary>
        /// Return the digits used by caller to restart the collection.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represent the reset digits.
        /// </value>
        public string ResetDigits { get; set; }

        /// <summary>
        /// Return the number of seconds waiting for DTMF inputs from caller.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represent the timeout in seconds.
        /// </value>
        public int StartTimeout { get; set; }

        /// <summary>
        /// Return the number of seconds to wait between DTMF inputs.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represent the time between 2 dtmf in seconds.
        /// </value>
        public int DigitTimeout { get; set; }
    }
}
