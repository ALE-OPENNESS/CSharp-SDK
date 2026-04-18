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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
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
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly int _intervalSeconds;
        private readonly Func<Task<bool>> _sendKeepAlive;
        private readonly SessionMonitoringPolicy _policy;
        private readonly Action<string> _onSessionLost;

        public KeepAlive(int intervalSeconds, Func<Task<bool>> sendKeepAlive, SessionMonitoringPolicy policy, Action<string> onSessionLost)
        {
            _intervalSeconds = intervalSeconds;
            _sendKeepAlive = sendKeepAlive;
            _policy = policy;
            _onSessionLost = onSessionLost;
        }

        protected override async Task CancelableRun()
        {
            while (!Token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), Token);

                try
                {
                    logger.Trace("Send Keep Alive");
                    bool ok = await _sendKeepAlive();
                    if (ok)
                    {
                        _policy.OnKeepAliveDone();
                    }
                    else
                    {
                        logger.Warn("Keep-alive rejected by server");
                        _policy.OnKeepAliveFatalError();
                        _onSessionLost("keep-alive rejected by server");
                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (HttpRequestException e)
                {
                    logger.Error(e, "Keep-alive network error");
                    var b = _policy.OnKeepAliveFailure(e);
                    if (b.IsAbort)
                    {
                        _onSessionLost("keep-alive network failure: " + e.Message);
                        return;
                    }
                    if (b.DelayMs > 0)
                        await Task.Delay(b.DelayMs, Token);
                }
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
        private readonly SessionMonitoringPolicy _policy;

        private ChunkEventing chunkEventing = null;
        private IWebHook _webHook = null;
        private ChunkEventDispatcher _webHookDispatcher = null;

        private KeepAlive keepAlive = null;

        private string subscriptionId = null;

        private readonly TaskCompletionSource<string> _sessionLostTcs = new();

        internal Task<string> WaitForLossAsync() => _sessionLostTcs.Task;

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
        public ICallCenterManagement CallCenterManagementService => serviceFactory.GetCallCenterManagementService();

        //        public ICallCenterRsi CallCenterRsiService => serviceFactory.GetCallCenterRsiService();
        public IAnalytics AnalyticsService => serviceFactory.GetAnalyticsService();
        public IUserManagement UserManagementService => serviceFactory.GetUserManagementService();
        public IRecording RecordingService => serviceFactory.GetRecordingService();
        public ICallCenterStatistics CallCenterStatisticsService => serviceFactory.GetCallCenterStatisticsService();

        internal SessionImpl(ServiceFactory serviceFactory, SessionInfo info, string loginName, bool passwordIsGoingToExpire, SessionMonitoringPolicy policy)
        {
            this.serviceFactory = serviceFactory;
            _policy = policy;
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

        private void SignalSessionLost(string reason)
        {
            logger.Warn("Session lost: {reason}", reason);
            _sessionLostTcs.TrySetResult(reason);
        }

        private void StartKeepAlive()
        {
            ISessions sessionService = serviceFactory.GetSessionsService();
            keepAlive = new(
                Info.TimeToLive,
                () => sessionService.SendKeepAlive(),
                _policy,
                SignalSessionLost);
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
            if (chunkEventing != null)
            {
                logger.Trace("Stop chunk");
                await chunkEventing.Stop();
                chunkEventing = null;
            }

            if (_webHookDispatcher != null)
            {
                logger.Trace("Stop webhook dispatcher");
                await _webHookDispatcher.Stop();
                _webHookDispatcher = null;
            }

            _webHook = null;

            logger.Trace("Delete Subsription");
            ISubscriptions subscriptionsService = serviceFactory.GetSubscriptionService();
            await subscriptionsService.Delete(subscriptionId);
            logger.Trace("Subsription Deleted");

            // Subscription is cancelled
            subscriptionId = null;
        }

        // Stops all local background activity without calling the server.
        // Used during session recovery when the server is unreachable.
        internal async Task CloseLocalAsync()
        {
            if (chunkEventing != null)
            {
                await chunkEventing.Stop();
                chunkEventing = null;
            }

            if (_webHookDispatcher != null)
            {
                await _webHookDispatcher.Stop();
                _webHookDispatcher = null;
            }

            _webHook = null;
            subscriptionId = null;

            if (keepAlive != null)
            {
                await keepAlive.Cancel();
                keepAlive = null;
            }
        }

        private async Task StartEventing(SubscriptionImpl subscription)
        {
            ISubscriptions subscriptionsService = serviceFactory.GetSubscriptionService();
            SubscriptionResult subscriptionResult = await subscriptionsService.Create(subscription);

            if ((subscriptionResult != null) && (subscriptionResult.Status == "ACCEPTED"))
            {
                subscriptionId = subscriptionResult.SubscriptionId;

                logger.Trace("Subscription has been accepted.");

                if (subscription.WebHook != null)
                {
                    // Webhook eventing: queue events from the HTTP handler, dispatch from a background thread
                    BlockingCollection<O2GEventDescriptor> eventQueue = new();

                    _webHookDispatcher = DependancyResolver.Resolve(new ChunkEventDispatcher(eventQueue, null, _policy));
                    _webHookDispatcher.Start();

                    subscription.WebHook.ConnectProcessor(new WebHookEventProcessor(eventQueue));
                    _webHook = subscription.WebHook;

                    logger.Info("Webhook eventing is configured.");
                }
                else
                {
                    // Chunk eventing
                    Uri chunkUri;
                    if (serviceFactory.AccessMode == AccessMode.Private)
                    {
                        chunkUri = new UriBuilder(subscriptionResult.PrivatePollingUrl).Uri;
                    }
                    else
                    {
                        chunkUri = new UriBuilder(subscriptionResult.PublicPollingUrl).Uri;
                    }

                    chunkEventing = new(chunkUri, null, _policy, SignalSessionLost);
                    chunkEventing.Start();

                    logger.Info("Chunk eventing is started.");
                }
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
                keepAlive = null;
            }

            // Close the session
            ISessions sessionService = serviceFactory.GetSessionsService();
            await sessionService.Close();

            logger.Info("Session is closed.");
        }
    }
}
