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
using o2g.Events.CommunicationLog;
using o2g.Internal.Services;
using o2g.Types.CommunicationLogNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// The <c>ICommunicationLog</c> service allows a user to retrieve his last communication history records and to manage them.
    /// Using this service requires having a <b>TELEPHONY_ADVANCED</b> license.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The call history records are built at O2G server level, thanks to its telephony call monitoring and therefore, 
    /// the comlog database is built independently of the OmniPcx Enterprise internal comlog.<br/>
    /// Acknowledgement/Unacknowledgement of an incoming missed call using O2G is <b>NOT synchronized</b> with the OmniPCX Enterprise. 
    /// </para>
    /// <para>
    /// The O2G communication log has a depth of 100 records maximum per user. When the limit is reached, oldest records are removed.
    /// </para>
    /// </remarks>
    public interface ICommunicationLog : IService
    {
        /// <summary>
        /// Occurs when a new comlog entry has been created.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnAgentRtiChangedEvent>> ComRecordCreated;

        /// <summary>
        /// Occurs when one or more records have been modified.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnComRecordModifiedEvent>> ComRecordModified;

        /// <summary>
        /// Occurs when one or more call log records have been deleted.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnComRecordsDeletedEvent>> ComRecordsDeleted;

        /// <summary>
        /// Occurs when one or more unanswered comlog records have been acknowledged.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnComRecordsAckEvent>> ComRecordsAck;

        /// <summary>
        /// Occurs when one or more unanswered comlog records have been unacknowledged.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnComRecordsUnAckEvent>> ComRecordsUnAck;

        /// <summary>
        /// Get the com records corresponding to the given criterias.
        /// </summary>
        /// <param name="filter">The filter describing the query criterias.</param>
        /// <param name="page">Optional page description.</param>
        /// <param name="optimized"><see langword="true"/> to activate optimization.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="QueryResult"/> object that represents the results of this query in case of success; <see langword="null"/> otherwise.
        /// </returns>
        /// <remarks>
        /// <para>
        /// When used, the <c>page</c> parameter of type <see cref="Page"/> allows to query the communication log by page. The <see cref="QueryResult"/> result contains the same parameters 
        /// and the total number of records retrieved by the query.
        /// </para>
        /// <para>
        /// When used, the <c>filter</c> parameter of type <see cref="QueryFilter"/> defines the search criteria for the query.
        /// </para>
        /// <para>
        /// When <c>optimized</c> is used and set to <see langword="true"/> the query returns the full identity of a participant only the first time it occurs, 
        /// when the same participant appears in several records. When omitted this query returns the records with no optimization.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <example>
        /// This exemple shows how to use the <c>Page</c> object to retrieve com records.
        /// <code>
        ///     ICommunicationLog comLog = myApp.CommunicationLogService;
        ///	
        ///     Page page = new()
        ///     {
        ///         Offset = 0,
        ///         Limit = 10
        ///     }
        ///	
        ///     for (;;)
        ///     {	
        ///         QueryResult result = await comLog.GetCallRecordsAsync(null, page);
        ///
        ///         // Display page of result
        ///         if (result.Count > (page.Offset + page.Limit))
        ///         {
        ///             // Load next page
        ///             page.Next();
        ///			
        ///             // Wait a user action (click on a button for exemple)
        ///             // and load the next page.
        ///         }
        ///         else
        ///         {
        ///             // end of pages display
        ///         }
        ///     }
        /// </code>
        /// </example>
        Task<QueryResult> GetComRecordsAsync(QueryFilter filter = null, Page page = null, bool optimized = false, string loginName = null);

        /// <summary>
        /// Return the com record specified by its identifier.
        /// </summary>
        /// <param name="recordId">The com record identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A <see cref="ComRecord"/> object that represents the com record with the requested identifier in case of success; <see langword="null"/> in case
        /// of error or if there is no com record with the specified identifier.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<ComRecord> GetComRecordAsync(long recordId, string loginName = null);

        /// <summary>
        /// Delete the com record with the given identifier.
        /// </summary>
        /// <param name="recordId">The com record identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<bool> DeleteComRecordAsync(long recordId, string loginName = null);

        /// <summary>
        /// Delete the com records corresponding to the given criterias.
        /// </summary>
        /// <param name="filter">The filter describing the query criterias.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// When used, the <c>filter</c> parameter of type <see cref="QueryFilter"/> defines the search criteria for the delete operation.
        /// A <see cref="OnComRecordsDeletedEvent"/> will be raised with the list of deleted com records.
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="ComRecordsDeleted"/>
        Task<bool> DeleteComRecordsAsync(QueryFilter filter = null, string loginName = null);

        /// <summary>
        /// Delete the specified list of com records.
        /// </summary>
        /// <param name="recordIds">The list of com records.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnComRecordsDeletedEvent"/> contains the list of the com record identifier that have been really deleted. 
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="ComRecordsDeleted"/>
        Task<bool> DeleteComRecordsAsync(List<long> recordIds, string loginName = null);

        /// <summary>
        /// Acknowledge the specified list of com records.
        /// </summary>
        /// <param name="recordIds">The list of com records.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnComRecordsAckEvent"/> contains the list of the com record identifier that have been really acknowledged. 
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="ComRecordsAck"/>
        Task<bool> AcknowledgeComRecordsAsync(List<long> recordIds, string loginName = null);


        /// <summary>
        /// Acknowledge the specified com records.
        /// </summary>
        /// <param name="recordId">The com record identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnComRecordsAckEvent"/> contains the list of the com record identifier that have been really acknowledged. 
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="AcknowledgeComRecordsAsync(List{long}, string)"/>
        /// <seealso cref="ComRecordsAck"/>
        Task<bool> AcknowledgeComRecordAsync(long recordId, string loginName = null);

        /// <summary>
        /// Unacknowledge the specified list of com records.
        /// </summary>
        /// <param name="recordIds">The list of com records.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnComRecordsUnAckEvent"/> contains the list of the com record identifier that have been really unacknowledged. 
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="ComRecordsAck"/>
        Task<bool> UnacknowledgeComRecordsAsync(List<long> recordIds, string loginName = null);

        /// <summary>
        /// Unacknowledge the specified com records.
        /// </summary>
        /// <param name="recordId">The com record identifier.</param>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// The event <see cref="OnComRecordsUnAckEvent"/> contains the list of the com record identifier that have been really unacknowledged. 
        /// </para>
        /// <para>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, 
        /// but it is mandatory if the session has been opened by an administrator.
        /// </para>
        /// </remarks>
        /// <seealso cref="UnacknowledgeComRecordsAsync(List{long}, string)"/>
        /// <seealso cref="ComRecordsAck"/>
        Task<bool> UnacknowledgeComRecordAsync(long recordId, string loginName = null);
    }
}
