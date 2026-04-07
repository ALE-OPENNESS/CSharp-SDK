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
    /// <item>A user to change its password or get some parameters like supported language.</item>
    /// </list>
    /// <c>IUsers</c> service is available from a connected application, it doesn't require any specific license on the O2G server.
    /// </summary>
    /// <example>
    /// <code>
    /// // Get the user service from an opened session
    /// IUsers usersService = myApplication.Users.
    /// </code>
    /// </example>
    /// <seealso cref="O2G.Application"/>
    public interface IUsers : IService
    {
        /// <summary>
        /// <c>UserInfoChanged</c> event is raised on any change on the user's data.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnUserInfoChangedEvent>> UserInfoChanged;

        /// <summary>
        /// <c>UserCreated</c> event is raised on creation of an user.
        /// </summary>
        /// <remarks>
        /// This event can only be receive by an administrator.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnUserCreatedEvent>> UserCreated;

        /// <summary>
        /// <c>UserCreated</c> event is raised when user is deleted.
        /// </summary>
        /// <remarks>
        /// This event can only be receive by an administrator.
        /// </remarks>
        public event EventHandler<O2GEventArgs<OnUserDeletedEvent>> UserDeleted;

        /// <summary>
        /// Retrieve a list of users login from the connected OXEs. 
        /// </summary>
        /// <param name="nodeIds">Specify a list of OXE nodes Id in which the query is done. This parameter is only valid for an administrator session.</param>
        /// <param name="onlyACD">Allows to select only the ACD operators (agents or supervisors) during the query. This parameter is only valid for an administrator session.</param>
        /// <returns>The list of users identified by their login. If <c>GetLogins</c> is used by a user session, it return only the user's login</returns>
        [Obsolete("Please use GetLoginsAsync(int[] nodeIds = null, bool onlyACD = false) instead.")]
        Task<List<string>> GetLoginsAsync(string[] nodeIds=null, bool onlyACD=false);

        /// <summary>
        /// Retrieve a list of users login from the connected OXEs. 
        /// </summary>
        /// <param name="nodeIds">Specify a list of OXE nodes Id in which the query is done. This parameter is only valid for an administrator session.</param>
        /// <param name="onlyACD">Allows to select only the ACD operators (agents or supervisors) during the query. This parameter is only valid for an administrator session.</param>
        /// <returns>The list of users identified by their login. If <c>GetLogins</c> is used by a user session, it return only the user's login</returns>
        Task<List<string>> GetLoginsAsync(int[] nodeIds = null, bool onlyACD = false);

        /// <summary>
        /// Returns an <c>User</c> object that represents the account information for the specified user.
        /// </summary>
        /// <param name="loginName">Login name of the user to retrieve the account information.</param>
        /// <returns>An <c>User</c> that represent the user account information.</returns>
        Task<User> GetByLoginNameAsync(string loginName);

        /// <summary>
        /// Returns an <c>User</c> object that represents the account information of the user with the specified company phone.
        /// </summary>
        /// <param name="companyPhone">Company phone of the user to retrieve the account informations.</param>
        /// <returns>An <c>User</c> that represent the user account information.</returns>
        Task<User> GetByCompanyPhoneAsync(string companyPhone);

        /// <summary>
        /// Return an <c>UserSupportedLanguages</c> that represents the user supported languages.
        /// </summary>
        /// <param name="loginName">Login name of the user to retrieve the account information.</param>
        /// <returns>An <c>UserSupportedLanguages</c> that represents the user supported languages.</returns>
        Task<SupportedLanguages> GetSupportedLanguagesAsync(string loginName);

        /// <summary>
        /// Return an <c>UserPreferences</c> that represents the user's preferences.
        /// </summary>
        /// <param name="loginName">Login name of the user to retrieve the account information.</param>
        /// <returns>An <c>UserPreferences</c> that represents the user's preferences.</returns>
        Task<Preferences> GetPreferencesAsync(string loginName);

        /// <summary>
        /// Change the password of the user or the administrator that have opened the session
        /// </summary>
        /// <remarks>
        /// This action will be refused if the authentication is delegated to an external LDAP server.
        /// </remarks>
        /// <param name="loginName">Login name of the user to retrieve the account information.</param>
        /// <param name="oldPassword">The old password to change</param>
        /// <param name="newPassword">The new password</param>
        /// <returns><see langword="true"/> on success ; else <see langword="false"/>.</returns>
        Task<bool> ChangePasswordAsync(string loginName, string oldPassword, string newPassword);
    }
}
