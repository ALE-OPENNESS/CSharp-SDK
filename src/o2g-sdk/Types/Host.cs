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

namespace o2g.Types
{
    /// <summary>
    /// This class represents an O2G host.
    /// <para>An O2G host can be reacheable from the local network, or from a wide area network. In this case, enterprise access can be secured by a reverse proxy border element.
    /// </para>
    /// <para>
    /// An O2G host as a private address which is the IP address or the URI used to reach the O2G server on a local network, and a public address which is the IP address or the URI used to reach the O2G server through a reverse proxy.
    /// </para>
    /// </summary>
    /// <remarks>It's possible to have only one address configured, but an exception will be throws if both are <see langword="null"/>.
    /// </remarks>
    public class Host
    {
        /// <summary>
        /// This property is the private address of this host.
        /// </summary>
        /// <value>
        /// The private IP address of URI, or <see langword="null"/> if the private address is not defined.
        /// </value>
        public string PrivateAddress { get; set; }
        /// <summary>
        /// This property is the public address of this host.
        /// </summary>
        /// <value>
        /// The public IP address of URI, or <see langword="null"/> if the public address is not defined.
        /// </value>
        public string PublicAddress { get; set; }
    }
}
