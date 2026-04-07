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
using o2g.Internal.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// Class <c>ProductVersion</c> gives information about the O2G server version.
    /// </summary>
    internal class ProductVersion
    {
        /// <summary>
        /// Return the major release number of this product version.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the major release number.
        /// </value>
        public string Major { get; init; }

        /// <summary>
        /// Return the minor release number of this product version.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the minor release number.
        /// </value>
        public string Minor { get; init; }
    }

    /// <summary>
    /// Class <c>ServerInfo</c> gives information about the O2G server.
    /// </summary>
    internal class ServerInfo
    {
        /// <summary>
        /// Return the O2G server product name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the friendly description of the product.
        /// </value>
        public string ProductName { get; init; }

        /// <summary>
        /// Return the O2G server product type.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the product short name.
        /// </value>
        public string ProductType { get; init; }

        /// <summary>
        /// Return the O2G product version.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the O2G product version.
        /// </value>
        public ProductVersion ProductVersion { get; init; }

        /// <summary>
        /// Return whether this O2G is deployed in high availability mode.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if O2G is deployed in high availability mode; <see langword="false"/> otherwise.
        /// </value>
        public bool HaMode { get; init; }
    }

    internal class Version
    {
        public String Id { get; set; }
        public string Status { get; set; }
        public string PublicUrl { get; set; }

        public string InternalUrl { get; set; }
    }

    internal class RoxeRestApiDescriptor
    {
        public ServerInfo ServerInfo { get; set; }
        public List<Version> Versions { get; set; }

        internal Version GetCurrent()
        {
            foreach (Version version in Versions)
            {
                if (version.Status == "CURRENT")
                {
                    return version;
                }
            }

            throw new O2GException("Unable to retrieve current API version");
        }

        internal Version Get(string versionId)
        {
            foreach (Version version in Versions)
            {
                if (version.Id == versionId)
                {
                    return version;
                }
            }

            return null;
        }
    }


    internal interface IO2G : IService
    {
        Task<RoxeRestApiDescriptor> Get();
    }
}
