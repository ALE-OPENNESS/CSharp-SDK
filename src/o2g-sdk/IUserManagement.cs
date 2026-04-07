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

using o2g.Internal.Services;
using o2g.Types.MessagingNS;
using o2g.Types.UsersNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>IUserManagement</c> service allows an administrator to create/delete/get the O2G users. .
    /// <c>IUserManagement</c> service is available for an administrator, it doesn't require any specific license on the O2G server.
    /// <para>
    /// </para>
    /// <remark>
    /// Since O2G version 2.7.3
    /// </remark>
    /// </summary>
    public interface IUserManagement : IService
    {
        /// <summary>
        /// Retrieve a list of users login from the specified OmniPCX Enterprise nodes.
        /// </summary>
        /// <param name="nodeIds">Specify a list of OXE nodes Id in which the query is done.</param>
        /// <returns>The list of users identified by their O2G login.</returns>
        /// <remarks>
        /// if <c>NodeIds</c> is {@code null}, retrieves the O2G login of users from all the connected OmniPCX Enterprise nodes.
        /// </remarks>
        Task<List<string>> GetLoginsAsync(int[] nodeIds = null);

        /// <summary>
        /// Retrieves a user login from one of its phone directory number. 
        /// </summary>
        /// <param name="deviceNumber">The directory number of a device belonging to the user being searched for.</param>
        /// <returns>
        /// A <see langword="string"/> that represents the O2G login of the user in case of success; 
        /// <see langword="null"/> in case of error or if there is no user with this device directory number.
        /// </returns>
        Task<string> GetLoginAsync(string deviceNumber);

        /// <summary>
        /// Get the O2G user with the specified O2G login.
        /// </summary>
        /// <param name="loginName">Login name of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="User"/> that represents the User with the given O2G login in case of success; 
        /// <see langword="null"/> in case of error or if there is no such user with this O2G login.
        /// </returns>
        Task<User> GetUserAsync(string loginName);

        /// <summary>
        /// Create and monitor the specified list of O2G users on the specified OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OXE node on which the O2G users are created.</param>
        /// <param name="deviceNumbers">The list of device number identifying the O2G users to creates.</param>
        /// <returns>
        /// A list of <see cref="User"/> that represents created users in case of success; <see langword="null"/> otherwise.
        /// </returns>
        Task<List<User>> CreateUsersAsync(int nodeId, string[] deviceNumbers);

        /// <summary>
        /// Create and monitor tall the O2G users on the specified OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OXE node.</param>
        /// <returns>
        /// A list of <see cref="User"/> that represents created users in case of success; <see langword="null"/> otherwise.
        /// </returns>
        Task<List<User>> CreateAllUsersAsync(int nodeId);

        /// <summary>
        /// Delete the specified <c>User</c>.
        /// </summary>
        /// <param name="loginName">Login name of the user to delete.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> DeleteUserAsync(string loginName);
    }
}
