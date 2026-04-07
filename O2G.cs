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
using o2g.Events.Common;
using o2g.Internal;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types;
using o2g.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>O2G</c> is the basic class an application must use create an O2G application. 
    /// </summary>
    /// <remarks>
    /// <c>O2G</c> class is a singleton class used to bootstrap the SDK and create an <see cref="Application"/> application.
    /// </remarks>
    /// <example>
    /// <code>
    ///     // Create an application
    ///     O2G.Application myApp = new("MyApplication");
    ///     
    ///     // Set the O2G host
    ///     myApp.SetHost(new()
    ///     {
    ///         PrivateAddress = "o2g-vl30.aledsppublic.com",
    ///         PublicAddress = "o2g-vl30.aledspprivate.com"
    ///     });
    /// </code>
    /// </example>
    public class O2G
    {
        static O2G()
        {
            ApiVersion = "1.0";
            SdkVersion = "2.7.4";
        }

        /// <summary>
        /// Represent the API version. By default this value is <c>"1.0"</c>. 
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the O2G API version.
        /// </value>
        /// <remarks>
        /// This value can be changed at startup to specify using another version of the REST API. 
        /// </remarks>
        public static string ApiVersion { get; set; }


        /// <summary>
        /// Return the version of this sdk.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the SDK version.
        /// </value>
        public static string SdkVersion { get; private set; }


        private static ConnectionPolicy _connectionPolicy = new DefaultConnectionPolicy();


        /// <summary>
        /// Class Application represents an O2G application.
        /// <para>
        /// A typical O2G application is built by creating an O2G.Application instance and then 
        /// call the <see cref="o2g.O2G.Application.LoginAsync(string, string)"/> method.
        /// Then, if required, the application subscribes a set of events.
        /// </para>
        /// </summary>
        /// <example>
        /// <code>
        ///     
        ///     O2G.Application myApplication = new("ApplicationName");
        ///     
        ///     try
        ///     {
        ///         await myApplication.LoginAsync(myLoginName, myPassword);
        ///         
        ///         // Create subscription
        ///         Subscription subscription = Subscription.Builder
        ///                                         .AddRoutingEvents()         // Add routing service events
        ///                                         .AddTelephonyEvents()       // Add telephony service events
        ///                                         .AddEventSummaryEvents()    // Add event summary events
        ///                                         .Build();                   // Build the subscription
        ///         
        ///         // Add an event handler on the ChannelInformation event.
        ///         myApplication.ChannelInformation += (source, ev) =>
        ///         {
        ///             // Called when the event channel is established
        ///             Console.WriteLine("Eventing channel is established.");
        ///             
        ///             // Use the routing service
        ///             IRouting routingService = myApplication.RoutingService;
        ///             await routingService.ForwardOnVoiceMailAsync(Forward.ForwardCondition.Immediate);
        ///             
        ///             ...
        ///         };
        /// 
        ///         // Suscribe to events using the built subscription
        ///         await myApplication.SubscribeAsync(subscription);
        ///         
        ///         ...
        ///     }
        ///     catch (O2GAuthenticateException e)
        ///     {
        ///         Console.WriteLine("Unable to authenticate the user.");
        ///     }
        /// 
        /// </code>
        /// </example>
        public class Application
        {
            private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            /// <summary>
            /// <c>EventSummaryUpdated</c> event is raised each time the user's counters have changed..
            /// </summary>
            public event EventHandler<O2GEventArgs<OnChannelInformationEvent>> ChannelInformation
            {
                add => _eventHandlers.ChannelInformation += value;
                remove => _eventHandlers.ChannelInformation -= value;
            }

            private HostManager _hostManager = null;

            private Session _session = null;
            private readonly EventHandlers _eventHandlers = new();

            /// <summary>
            /// Return the application name.
            /// </summary>
            /// <value>
            /// A <see langword="string"/> value that represents the application name.
            /// </value>
            /// <remarks>
            /// The application name is given when the <see cref="O2G.Application"/> is created. It is used for statistics on the O2G server.
            /// </remarks>
            public string ApplicationName { get; init; }

            /// <summary>
            /// Return the account corresponding to the connected user
            /// </summary>
            public IAccount Account => _session.Account;

            /// <summary>
            /// Create a new O2G application with the specified name.
            /// </summary>
            /// <param name="applicationName">The applicationName is an identifier for logging and statistic purpose</param>
            public Application(string applicationName)
            {
                ApplicationName = applicationName;
                DependancyResolver.RegisterService(HttpClientBuilder.Build());
                DependancyResolver.RegisterService(_eventHandlers);
            }

            /// <summary>
            /// Set the O2G <see cref="Host"/> on which the application wants to connect to.
            /// </summary>
            /// <param name="host1">The main O2G <see cref="Host">Host</see></param>
            /// <param name="host2">The secondary O2G <see cref="Host">Host</see></param>
            public void SetHost(Host host1, Host host2 = null)
            {
                _hostManager = new HostManager(host1, host2);
            }

            /// <summary>
            /// Connect the application to the O2G service endpoint, using the specified login and password to authenticate the user.
            /// </summary>
            /// <param name="loginName">The user login name</param>
            /// <param name="password">The user password</param>
            public async Task LoginAsync(string loginName, string password)
            {
                // First connect to the right service endpoint
                ServiceEndPoint serviceEndPoint = await Connect();

                // And open a session
                _session = await serviceEndPoint.OpenSession(new()
                {
                    Login = loginName,
                    Password = password
                },
                ApplicationName);
            }

            /// <summary>
            /// Subscribe to events from the O2G server. The requested events are specified using a <see cref="Subscription"/> object.
            /// </summary>
            /// <param name="subscription">The <see cref="Subscription"/> describing the events to receive.</param>
            public async Task SubscribeAsync(Subscription subscription)
            {
                await _session.ListenEvents(subscription);
            }

            /// <summary>
            /// Terminate the application. Dispose all used ressources.
            /// </summary>
            public async Task ShutdownAsync()
            {
                // session can be null if the login has failed.
                if (_session != null)
                {
                    await _session.Close();
                }
            }

            // Apply the connection policy and try to connect on provided hosts
            private async Task<ServiceEndPointImpl> Connect()
            {
                for (int connectTry = 0; ; connectTry++)
                {
                    try
                    {
                        ServiceFactory serviceFactory = new(ApiVersion);
                        ServerInfo serverInfo = await serviceFactory.Bootstrap(_hostManager.Host1);

                        return new ServiceEndPointImpl(serviceFactory, serverInfo);
                    }
                    catch (Exception)
                    {
                        logger.Error("Unable to connect on {address}", _hostManager.Host1);

                        // The connection failed
                        if (_hostManager.Host2 != null)
                        {
                            try
                            {
                                ServiceFactory serviceFactory = new(ApiVersion);
                                ServerInfo serverInfo = await serviceFactory.Bootstrap(_hostManager.Host2);

                                return new ServiceEndPointImpl(serviceFactory, serverInfo);
                            }
                            catch (Exception)
                            {
                                logger.Error("Unable to connect on {address}", _hostManager.Host2);
                            }
                        }
                    }

                    if ((_connectionPolicy.NbRetry == -1) || (connectTry < _connectionPolicy.NbRetry))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(_connectionPolicy.Interval));
                    }
                    else
                    {
                        logger.Error("Unable to connect: Connection policy : Failed");
                        throw new O2GException("Unable to connect: Connection policy : Failed");
                    }
                }                
            }

            /// <summary>
            /// Return the maintenance service.
            /// </summary>
            /// <value>
            /// A <see cref="IMaintenance"/> object that provides maintenance services.
            /// </value>
            public IMaintenance MaintenanceService => _session.MaintenanceService;

            /// <summary>
            /// Return the routing service.
            /// </summary>
            /// <value>
            /// A <see cref="IRouting"/> object that provides routing services.
            /// </value>
            public IRouting RoutingService => _session.RoutingService;

            /// <summary>
            /// Return the telephony service.
            /// </summary>
            /// <value>
            /// A <see cref="ITelephony"/> object that provides telephony services.
            /// </value>
            public ITelephony TelephonyService => _session.TelephonyService;

            /// <summary>
            /// Return the event summary service.
            /// </summary>
            /// <value>
            /// A <see cref="IEventSummary"/> object that provides event summary services.
            /// </value>
            public IEventSummary EventSummaryService => _session.EventSummaryService;

            /*
            /// <summary>
            /// Return the cal center rsi service.
            /// </summary>
            /// <value>
            /// A <see cref="ICallCenterRsi"/> object that provides call center ris services.
            /// </value>
            public ICallCenterRsi CallCenterRsiService => _session.CallCenterRsiService;
            */

            /// <summary>
            /// Return the messaging service.
            /// </summary>
            /// <value>
            /// A <see cref="IMessaging"/> object that provides messaging services.
            /// </value>
            public IMessaging MessagingService => _session.MessagingService;

            /// <summary>
            /// Return the users service.
            /// </summary>
            /// <value>
            /// A <see cref="IUsers"/> object that provides users services.
            /// </value>
            public IUsers UsersService => _session.UsersService;

            /// <summary>
            /// Return the pbx management service.
            /// </summary>
            /// <value>
            /// A <see cref="IPbxManagement"/> object that provides pbx management services.
            /// </value>
            public IPbxManagement PbxManagementService => _session.PbxManagementService;

            /// <summary>
            /// Return the analytics service.
            /// </summary>
            /// <value>
            /// A <see cref="IAnalytics"/> object that provides analytics services.
            /// </value>
            public IAnalytics AnalyticsService => _session.AnalyticsService;

            /// <summary>
            /// Return the communication log service.
            /// </summary>
            /// <value>
            /// A <see cref="ICommunicationLog"/> object that provides communication log services.
            /// </value>
            public ICommunicationLog CommunicationLogService => _session.CommunicationLogService;

            /// <summary>
            /// Return the phone set programming service.
            /// </summary>
            /// <value>
            /// A <see cref="IPhoneSetProgramming"/> object that provides phone set programming services.
            /// </value>
            public IPhoneSetProgramming PhoneSetProgrammingService => _session.PhoneSetProgrammingService;

            /// <summary>
            /// Return the call center agent service.
            /// </summary>
            /// <value>
            /// A <see cref="ICallCenterAgent"/> object that provides call center agent services.
            /// </value>
            public ICallCenterAgent CallCenterAgentService => _session.CallCenterAgentService;

            /// <summary>
            /// Return the call center pilot service.
            /// </summary>
            /// <value>
            /// A <see cref="ICallCenterPilot"/> object that provides call center pilot services.
            /// </value>
            public ICallCenterPilot CallCenterPilotService => _session.CallCenterPilotService;

            /// <summary>
            /// Return the call center pilot service.
            /// </summary>
            /// <value>
            /// A <see cref="ICallCenterRealtime"/> object that provides call center real time services.
            /// </value>
            public ICallCenterRealtime CallCenterRealtimeService => _session.CallCenterRealtimeService;

            /// <summary>
            /// Return the user management service.
            /// </summary>
            /// <value>
            /// A <see cref="IUserManagement"/> object that provides user management services.
            /// </value>
            public IUserManagement UserManagementService => _session.UserManagementService;

            /// <summary>
            /// Return the recording service.
            /// </summary>
            /// <value>
            /// A <see cref="IRecording"/> object that provides recording services.
            /// </value>
            public IRecording RecordingService => _session.RecordingService;
        }
    }
}
