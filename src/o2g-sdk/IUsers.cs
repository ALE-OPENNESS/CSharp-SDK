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
    /// The <c>IUsers</c> service allows:
    /// <list type="bullet">
    /// <item>An administrator to retrieve the list of O2G users.</item>
    /// <item>A user to get information on another user account.</item>
    /// <item>A user to change their password or get parameters such as supported languages.</item>
    /// </list>
    /// Using this service does not require any specific license on the O2G server.
    /// </summary>
    /// <seealso cref="O2G.Application"/>
    public interface IUsers : IService
    {
        /// <summary>
        /// Raised on any change on a user's data.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnUserInfoChangedEvent>> UserInfoChanged;

        /// <summary>
        /// Raised when a user is created.
        /// </summary>
        /// <remarks>
        /// This event can only be received by an administrator.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnUserCreatedEvent>> UserCreated;

        /// <summary>
        /// Raised when a user is deleted.
        /// </summary>
        /// <remarks>
        /// This event can only be received by an administrator.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnUserDeletedEvent>> UserDeleted;

        /// <summary>
        /// Retrieves a list of user login names from the connected OmniPCX Enterprise nodes.
        /// </summary>
        /// <param name="nodeIds">A list of OXE node ids to restrict the query to. This parameter is only valid for an administrator session.</param>
        /// <param name="onlyACD">If <see langword="true"/>, selects only ACD operators (agents or supervisors). This parameter is only valid for an administrator session.</param>
        /// <returns>The list of user login names. If used from a user session, returns only the current user's login name.</returns>
        /// <remarks>
        /// If <c>nodeIds</c> is <see langword="null"/>, retrieves the login names from all connected OmniPCX Enterprise nodes.
        /// </remarks>
        Task<List<string>> GetLoginsAsync(int[] nodeIds = null, bool onlyACD = false);

        /// <summary>
        /// Retrieves the information of a user identified by their login name.
        /// </summary>
        /// <param name="loginName">The login name of the user to retrieve.</param>
        /// <returns>A <see cref="User"/> that represents the user account information, or <see langword="null"/> in case of error or if there is no user with the specified login name.</returns>
        Task<User> GetByLoginNameAsync(string loginName);

        /// <summary>
        /// Retrieves the information of a user identified by their company extension number.
        /// </summary>
        /// <param name="companyPhone">The company extension number of the user to retrieve.</param>
        /// <returns>A <see cref="User"/> that represents the user account information, or <see langword="null"/> in case of error or if there is no user with the specified company extension number.</returns>
        Task<User> GetByCompanyPhoneAsync(string companyPhone);

        /// <summary>
        /// Returns the supported languages for the specified user.
        /// </summary>
        /// <param name="loginName">The login name of the user.</param>
        /// <returns>A <see cref="SupportedLanguages"/> that represents the user's supported languages, or <see langword="null"/> in case of error or if there is no user with the specified login name.</returns>
        Task<SupportedLanguages> GetSupportedLanguagesAsync(string loginName);

        /// <summary>
        /// Returns the preferences of the specified user.
        /// </summary>
        /// <param name="loginName">The login name of the user.</param>
        /// <returns>A <see cref="Preferences"/> that represents the user's preferences, or <see langword="null"/> in case of error or if there is no user with the specified login name.</returns>
        Task<Preferences> GetPreferencesAsync(string loginName);

        /// <summary>
        /// Changes the specified user's password.
        /// </summary>
        /// <param name="loginName">The login name of the user whose password is being changed.</param>
        /// <param name="oldPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This operation will fail if authentication is delegated to an external LDAP server.
        /// </remarks>
        Task<bool> ChangePasswordAsync(string loginName, string oldPassword, string newPassword);
    }
}
