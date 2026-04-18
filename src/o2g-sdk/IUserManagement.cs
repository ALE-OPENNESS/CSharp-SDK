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

using o2g.Events;
using o2g.Events.Users;
using o2g.Internal.Services;
using o2g.Types.UsersNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// The <c>IUserManagement</c> service allows an administrator to create, delete, and retrieve O2G users.
    /// O2G allows users to be created according to different methods:
    /// <list type="bullet">
    /// <item>Automatically when O2G starts, according to the automatic user creation mode.</item>
    /// <item>Through provisioning files.</item>
    /// <item>On demand through this service, which allows creating one user, a list of users, or all users on a given OmniPCX Enterprise node.</item>
    /// </list>
    /// Using this service does not require any specific license on the O2G server.
    /// </summary>
    public interface IUserManagement : IService
    {
        /// <summary>
        /// Raised when an O2G user is created.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnUserCreatedEvent>> UserCreated;

        /// <summary>
        /// Raised when an O2G user is deleted.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnUserDeletedEvent>> UserDeleted;

        /// <summary>
        /// Raised when an O2G user's information has changed.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnUserInfoChangedEvent>> UserInfoChanged;

        /// <summary>
        /// Retrieves a list of user login names from the connected OmniPCX Enterprise nodes.
        /// </summary>
        /// <param name="nodeIds">A list of OXE node ids to restrict the query to, or <see langword="null"/> to query all connected nodes.</param>
        /// <returns>The list of user login names, or <see langword="null"/> in case of error.</returns>
        Task<List<string>> GetLoginsAsync(int[] nodeIds = null);

        /// <summary>
        /// Retrieves the login name of a user identified by one of their device directory numbers.
        /// </summary>
        /// <param name="deviceNumber">A directory number of a device belonging to the user being searched for.</param>
        /// <returns>
        /// A <see langword="string"/> that represents the login name of the user in case of success;
        /// <see langword="null"/> in case of error or if no user owns a device with the specified directory number.
        /// </returns>
        Task<string> GetLoginAsync(string deviceNumber);

        /// <summary>
        /// Retrieves the information of a user identified by their login name.
        /// </summary>
        /// <param name="loginName">The login name of the user to retrieve.</param>
        /// <returns>
        /// A <see cref="User"/> that represents the user in case of success;
        /// <see langword="null"/> in case of error or if there is no user with the specified login name.
        /// </returns>
        Task<User> GetUserAsync(string loginName);

        /// <summary>
        /// Creates and monitors the specified O2G users on the given OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OXE node number on which the users are created.</param>
        /// <param name="deviceNumbers">The list of device directory numbers identifying the users to create.</param>
        /// <returns>
        /// A list of <see cref="User"/> objects representing the created users in case of success; <see langword="null"/> otherwise.
        /// </returns>
        Task<List<User>> CreateUsersAsync(int nodeId, string[] deviceNumbers);

        /// <summary>
        /// Creates and monitors all O2G users configured on the given OmniPCX Enterprise node.
        /// </summary>
        /// <param name="nodeId">The OXE node number on which the users are created.</param>
        /// <returns>
        /// A list of <see cref="User"/> objects representing the created users in case of success; <see langword="null"/> otherwise.
        /// </returns>
        Task<List<User>> CreateAllUsersAsync(int nodeId);

        /// <summary>
        /// Deletes the O2G user identified by their login name.
        /// </summary>
        /// <param name="loginName">The login name of the user to delete.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> DeleteUserAsync(string loginName);
    }
}
