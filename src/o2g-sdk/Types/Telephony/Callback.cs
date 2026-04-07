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
using o2g.Types.CommonNS;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// <c>Callback</c> class represenst a callback request. A callback request is invoked by a caller to ask the user to call him back as sson as possible.
    /// </summary>
    public class Callback
    {
        /// <summary>
        /// This property gives the identifier of this callback.
        /// </summary>
        /// <value>
        /// A unique <see langword="string"/> that is this callback identifier.
        /// </value>
        public string CallbackId { get; init; }

        /// <summary>
        /// This property represents the party who has requested the callback.
        /// </summary>
        /// <value>
        /// A <see cref="PartyInfo"/> class that  represents the originator of the callback request.
        /// </value>
        public PartyInfo PartyInfo { get; init; }
    }
}
