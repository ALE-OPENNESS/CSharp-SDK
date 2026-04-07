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
using o2g.Types.MessagingNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>IMessaging</c> service provides access to user's voice mail box. It's possible using this service to connect 
    /// to the voice mail box, retrieve the information and the list of voice mails and manage the mail box.
    /// Using this service requires having a <b>TELEPHONY_ADVANCED</b> license.
    /// <para>
    /// It's possible to download the voice mail as a wav file and to delete an existing messages.
    /// </para>
    /// </summary>
    public interface IMessaging : IService
    {
        /// <summary>
        /// Get the specified user's mailboxes. This is the logical first step to access further operation on voice mail feature. 
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="MailBox"/>.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<MailBox>> GetMailboxesAsync(string loginName = null);

        /// <summary>
        /// Get the information on the specified mailbox.
        /// </summary>
        /// <param name="mailboxId">The mailbox identifier.</param>
        /// <param name="password">The mailbox password.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="MailBoxInfo"/> that contains the mailbox information or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <c>password</c> is optional. if not set, the user password is used to connect on the voicemail. 
        /// This is only possible if the OmniPCX Enterprise administrator has managed the same pasword for the user and his mailbox.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<MailBoxInfo> GetMailboxInfoAsync(string mailboxId, string password = null, string loginName = null);

        /// <summary>
        /// Get the list of voice mail in this voice mail box.
        /// </summary>
        /// <param name="mailboxId">The mailbox identifier.</param>
        /// <param name="newOnly">Filter only unread voicemail if set to <see langword="true"/> (Default value is <see langword="false"/>)</param>
        /// <param name="offset">The offset from which to start retrieving the voicemail list (Default value is 0).</param>
        /// <param name="limit">The maximum number of items to return (Default value is -1: no limit).</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// The list of <see cref="VoiceMessage"/> objects that represents the voice messages.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<List<VoiceMessage>> GetVoiceMessagesAsync(string mailboxId, bool newOnly = false, int? offset = null, int? limit = null, string loginName = null);

        /// <summary>
        /// Delete the specified voice message.
        /// </summary>
        /// <param name="mailboxId">The mailbox identifier.</param>
        /// <param name="voicemailId">The voice message to delete.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        Task<bool> DeleteVoiceMessageAsync(string mailboxId, string voicemailId, string loginName = null);

        /// <summary>
        /// Delete the specified list of voice messages.
        /// </summary>
        /// <param name="mailboxId">The mailbox identifier.</param>
        /// <param name="msgIds">The voice messages to delete.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        Task<bool> DeleteVoiceMessagesAsync(string mailboxId, List<string> msgIds, string loginName = null);

        /// <summary>
        /// Download a voice mail as a wav file.
        /// </summary>
        /// <param name="mailboxId">The mailbox identifier.</param>
        /// <param name="voicemailId">The voice mail identifier.</param>
        /// <param name="wavPath">An optional destination file name.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// Return a <see langword="true"/> that is the path to the downloaded wav file.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the <c>wavPath</c> is not set, the wav file is downloaded in the default system <c>download</c> folder.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        Task<string> DownloadVoiceMessageAsync(string mailboxId, string voicemailId, string wavPath = null, string loginName = null);
    }
}
