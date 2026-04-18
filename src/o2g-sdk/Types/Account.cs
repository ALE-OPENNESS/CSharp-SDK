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

namespace o2g.Types
{
    /// <summary>
    /// This class represents an Account that open an O2G session.
    /// </summary>
    /// <para>With version 2.7.4 security has been reinforced. User authentication can be delegated to and external LDAP server. The Account class allow to retrieve the O2G user authenticated when LDAP credential is used.
    /// </para>
    /// <remarks>
    /// Added in version 2.7.4
    /// </remarks>
    public interface IAccount
    {
        /// <summary>
        /// This property is the Login name used for authentication. This is the login passed by the application in the <see cref="O2G.Application.LoginAsync(string, string)"/>.
        /// </summary>
        string LoginName { get; }

        /// <summary>
        /// This property is the OG2 user login corresponding to the LoginName. It has the form oxeXXXXX, where XXXXX is the OXE subscriber directory number.
        /// </summary>
        string O2GUserLoginName { get; }

        /// <summary>
        /// This property indicates whether the password used to authenticate this account is foing to expired.
        /// is about to expire and must be changed.
        /// </summary>
        bool IsGoingToExpired { get; }
    }
}
