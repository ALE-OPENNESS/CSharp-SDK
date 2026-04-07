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

using o2g.Types;

namespace o2g.Internal.Services
{
    /// <summary>
    /// The IService is the base interface for all O2G services. 
    /// </summary>
    /// <remarks>
    /// Services are available from an opened session.
    /// <list type="table">
    ///     <listheader><term>Service</term><description>Description</description></listheader>
    ///     <item><term><see cref="ITelephony"/></term><description>The telephony service provides access to the basic and advanced OmniPCX Enterprise telpehony services.</description></item>
    ///     <item><term><see cref="IUsers"/></term><description>The users service allows to get information about users and to change passwords.</description></item>
    ///     <item><term><see cref="IRouting"/></term><description>The routing service allows to configure the forward, overflow and do not disturb feature.</description></item>
    ///     <item><term><see cref="IMessaging"/></term><description>The messaging service allows to retrieve voice messages.</description></item>
    /// </list>
    /// </remarks>
    public interface IService
    {
        /// <summary>
        /// The <c>LastError</c> property represents the error raised by the last service invoked. It has the <see langword="null"/> value, if no error have been raised.
        /// </summary>
        /// <remarks>
        /// <value>
        /// The <c>LastError</c> is a <see cref="RestErrorInfo"/> object that provides information about the error.
        /// </value>
        /// The last error is set each time a service invocation return an error. It contains all information from errors sent while invoking REST operations on resources.
        /// But it's possible depending on the context than no error is set on service failure.
        /// </remarks>
        public RestErrorInfo LastError { get; }
    }

}
