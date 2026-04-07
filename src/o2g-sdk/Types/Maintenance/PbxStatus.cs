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

namespace o2g.Types.MaintenanceNS
{
    /// <summary>
    /// <c>PbxStatus</c> represents the state of an OmniPCX Enterprise the O2G server is connected on.
    /// </summary>
    public class PbxStatus
    {
        /// <summary>
        /// Return the name of the OmniPCX Enterprise node.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats is the name of the OmniPCX Enterprise node.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Return the OmniPCX Enterprise node id.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value thats is the OmniPCX Enterprise node number.
        /// </value>
        public int NodeId { get; init; }

        /// <summary>
        /// Return the OmniPCX Enterprise node main address.
        /// </summary>
        /// <value>
        /// A <see cref="ServerAddress"/> object that represents the OmniPCX Enterprise node main address.
        /// </value>
        public ServerAddress MainAddress { get; init; }


        /// <summary>
        /// Return the OmniPCX Enterprise node secondary address.
        /// </summary>
        /// <value>
        /// A <see cref="ServerAddress"/> object that represents the OmniPCX Enterprise node secondary address, or a 
        /// <see langword="null"/> value if the OmniPCX Enterprise node is not duplicated.
        /// </value>
        public ServerAddress SecondaryAddress { get; init; }

        /// <summary>
        /// Return the version of the OmniPCX Enterprise node.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value thats is the version of the OmniPCX Enterprise node.
        /// </value>
        public string Version { get; init; }

        /// <summary>
        /// Return whether the O2G server is connected to this OmniPCX Enterprise node.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the O2G server is connected to this OmniPCX Enterprise node; <see langword="true"/> otherwise.
        /// </value>
        public bool Connected { get; init; }

        /// <summary>
        /// Return whether the O2G server has loaded all this OmniPCX Enterprise node's users.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the O2G server has loaded all this OmniPCX Enterprise node's users; <see langword="true"/> otherwise.
        /// </value>
        public bool Loaded { get; init; }

        /// <summary>
        /// Return the state of the CSTA link between the O2Gserver and this OmniPCX Enterprise node.
        /// </summary>
        /// <value>
        /// A <see cref="CtiLinkState"/> value that represents the CSTA link status.
        /// </value>
        public CtiLinkState CtiLinkState { get; init; }

        /// <summary>
        /// Return whether the OmniPCX Enterprise node is secured.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the OmniPCX Enterprise node is secured; <see langword="true"/> otherwise.
        /// </value>
        /// <remarks>
        /// If the OmniPCX Enterprise node is secured, the connection with the O2G server is done using SSH.
        /// </remarks>
        public bool Secured { get; init; }

        /// <summary>
        /// Return the number of monitored users on this OmniPCX Enterprise node.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value thats is the number of monitored users on this OmniPCX Enterprise node.
        /// </value>
        public int MonitoredUserNumber { get; init; }
    }
}
