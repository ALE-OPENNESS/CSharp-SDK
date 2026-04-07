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

namespace o2g.Types.CommunicationLogNS
{
    /// <summary>
    /// The <c>Page</c> class allows to define a paging mechanism to query the communication log.
    /// </summary>
    /// <see cref="ICommunicationLog.GetComRecordsAsync(QueryFilter, Page, bool, string)"/>
    public class Page
    {
        /// <summary>
        /// Construct a new Page with the specified offset and limit.
        /// </summary>
        /// <param name="offset">The offset value.</param>
        /// <param name="limit">The limit value.</param>
        public Page(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }

        /// <summary>
        /// Move the offset to the next page.
        /// </summary>
        public void Next()
        {
            Offset += Limit;
        }

        /// <summary>
        /// Move to the previous page.
        /// </summary>
        public void Previous()
        {
            Offset -= Limit;
            if (Offset < 0)
            {
                Offset = 0;
            }
        }


        /// <summary>
        /// The offset of the page that is the offset of the first com record.
        /// </summary>
        /// <value>
        /// A positive <see langword="int"/> that is the offset of the first com record.
        /// </value>
        public int Offset { get; set; }

        /// <summary>
        /// The number of record in this page.
        /// </summary>
        /// <value>
        /// A positive <see langword="int"/> that is the maximum com records in this page.
        /// </value>
        public int Limit { get; set; }
    }
}
