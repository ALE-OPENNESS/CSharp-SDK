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

using o2g.Internal.Events;
using o2g.Internal.Services;
using o2g.Internal.Types;
using o2g.Internal.Utility;
using o2g.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g.Internal
{
    internal class Service
    {
        public string ServiceName { get; set; }
        public string ServiceVersion { get; set; }
        public string RelativeUrl { get; set; }
    }

    internal class SessionInfo
    {
        public bool Admin { get; set; }
        public string Login { get; set; }
        public int TimeToLive { get; set; }
        public string PublicBaseUrl { get; set; }
        public string PrivateBaseUrl { get; set; }
        public string CreationDate { get; set; }
        public List<Service> Services { get; set; }
        public string ExpirationDate { get; set; }
    }

    internal class SessionAccount : IAccount
    {
        public string LoginName { get; set; }

        public string O2GUserLoginName { get; set; }

        public bool IsGoingToExpired { get; set; }
    }

    class KeepAlive : CancelableTask
    {
        private readonly int value;
        private readonly Action action;

        public KeepAlive(int keepAliveValue, Action keepAliveAction)
        {
            value = keepAliveValue;
            action = keepAliveAction;
        }

        protected async override Task CancelableRun()
        {
            while (!Token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(value), Token);
                action();
            }
        }

        internal async Task Cancel()
        {
            CancelTask();
            await RunningTask;
        }
    }

    internal class SessionImpl : Session
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ServiceFactory serviceFactory;

        private ChunkEventing chunkEventing = null;

        private KeepAlive keepAlive = null;

        private string subscriptionId = null;

        internal SessionInfo Info { get; private set; }

        public string LoginName { get; init; }

        public bool Admin => Info.Admin;

        public SessionAccount _account = null;
        public IAccount Account => _account;

        public ITelephony TelephonyService => serviceFactory.GetTelephonyService();
        public IUsers UsersService => serviceFactory.GetUsersService();
        public IRouting RoutingService => serviceFactory.GetRoutingService();
        public IMessaging MessagingService => serviceFactory.GetMessagingService();
        public IMaintenance MaintenanceService => serviceFactory.GetMaintenanceService();
        public IDirectory DirectoryService => serviceFactory.GetDirectoryService();
        public IEventSummary EventSummaryService => serviceFactory.GetEventSummaryService();
        public IPbxManagement PbxManagementService => serviceFactory.GetPbxManagementService();
        public ICommunicationLog CommunicationLogService => serviceFactory.GetCommunicationLogService();
        public IPhoneSetProgramming PhoneSetProgrammingService => serviceFactory.GetPhoneSetProgrammingService();
        public ICallCenterAgent CallCenterAgentService => serviceFactory.GetCallCenterAgentService();
        public ICallCenterPilot CallCenterPilotService => serviceFactory.GetCallCenterPilotService();
        public ICallCenterRealtime CallCenterRealtimeService => serviceFactory.GetCallCenterRealtimeService();

        //        public ICallCenterRsi CallCenterRsiService => serviceFactory.GetCallCenterRsiService();
        public IAnalytics AnalyticsService => serviceFactory.GetAnalyticsService();
        public IUserManagement UserManagementService => serviceFactory.GetUserManagementService();
        public IRecording RecordingService => serviceFactory.GetRecordingService();

        internal SessionImpl(ServiceFactory serviceFactory, SessionInfo info, string loginName, bool passwordIsGoingToExpire)
        {
            this.serviceFactory = serviceFactory;
            Info = info;

            // From O2G 2.7.4 external login can be used, so application must access real O2G login
            _account = new();
            _account.LoginName = loginName;
            _account.IsGoingToExpired = passwordIsGoingToExpire;

            if (info.Login != null)
            {
                LoginName = info.Login;
                _account.O2GUserLoginName = info.Login;
            }
            else
            {
                LoginName = loginName;
                _account.O2GUserLoginName = loginName;
            }

            StartKeepAlive();
        }

        private void StartKeepAlive()
        {
            keepAlive = new(this.Info.TimeToLive, () =>
            {
                logger.Trace("Send Keep Alive");
                ISessions sessionService = serviceFactory.GetSessionsService();
                sessionService.SendKeepAlive();
            });
            keepAlive.Start();
        }

        public async Task ListenEvents(Subscription subscriptionRequest)
        {
            if (subscriptionRequest != null)
            {
                // Need to subscribe to eventing
                await StartEventing((SubscriptionImpl)subscriptionRequest);
            }
        }

        private async Task StopEventing()
        {
            logger.Trace("Stop chunk");
            if (chunkEventing != null)
            {
                await chunkEventing.Stop();
            }

            logger.Trace("Delete Subsription");
            ISubscriptions subscriptionsService = serviceFactory.GetSubscriptionService();
            await subscriptionsService.Delete(subscriptionId);
            logger.Trace("Subsription Deleted");

            // Subscription is cancelled
            subscriptionId = null;
        }

        private async Task StartEventing(SubscriptionImpl subscription)
        {
            ISubscriptions subscriptionsService = serviceFactory.GetSubscriptionService();
            SubscriptionResult subscriptionResult = await subscriptionsService.Create(subscription);

            if ((subscriptionResult != null) && (subscriptionResult.Status == "ACCEPTED"))
            {
                subscriptionId = subscriptionResult.SubscriptionId;

                logger.Trace("Subscription has been accepted.");

                Uri chunkUri;
                if (serviceFactory.AccessMode == AccessMode.Private)
                {
                    chunkUri = new UriBuilder(subscriptionResult.PrivatePollingUrl).Uri;
                }
                else
                {
                    chunkUri = new UriBuilder(subscriptionResult.PublicPollingUrl).Uri;
                }

                // The default handler is NOT used.
                chunkEventing = new(chunkUri, null);
                chunkEventing.Start();

                logger.Info("Eventing is started.");
            }
            else
            {
                logger.Fatal("Subscription has been refused. Fix the subscription request.");
                if (subscriptionResult == null)
                {
                    throw new O2GException("Subscription Refused");
                }
                else
                {
                    throw new O2GException("Subscription Refused : " + subscriptionResult.Message);
                }
            }
        }

        public async Task Close()
        {
            // First stop eventing if eventing exist
            if (subscriptionId != null)
            {
                await StopEventing();
            }

            // Stop Keep Alive
            if (keepAlive != null)
            {
                await keepAlive.Cancel();
            }

            // Close the session
            ISessions sessionService = serviceFactory.GetSessionsService();
            await sessionService.Close();

            logger.Info("Session is closed.");
        }
    }
}
