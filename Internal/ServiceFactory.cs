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

using o2g.Internal.Rest;
using o2g.Internal.Services;
using o2g.Internal.Utility;
using o2g.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace o2g.Internal
{
    internal enum AccessMode
    {
        Public, Private
    }

    internal class O2GService
    {
        private O2GService(string value) { Value = value; }

        public string Value { get; private set; }

        public static O2GService Get(string name)
        {
            return new O2GService(name.ToLower());
        }

        public static O2GService Authentication { get { return new O2GService("authenticate"); } }
        public static O2GService Sessions { get { return new O2GService("sessions"); } }
        public static O2GService O2G { get { return new O2GService("O2G"); } }
        public static O2GService Subscriptions { get { return new O2GService("subscriptions"); } }
        public static O2GService EventSummary { get { return new O2GService("eventsummary"); } }
        public static O2GService Telephony { get { return new O2GService("telephony"); } }
        public static O2GService Users { get { return new O2GService("users"); } }
        public static O2GService Routing { get { return new O2GService("routing"); } }
        public static O2GService Messaging { get { return new O2GService("voicemail"); } }
        public static O2GService Maintenance { get { return new O2GService("maintenance"); } }
        public static O2GService Directory { get { return new O2GService("directory"); } }
        public static O2GService PbxManagement { get { return new O2GService("pbxmanagement"); } }
        public static O2GService CommunicationLog { get { return new O2GService("comlog"); } }
        public static O2GService PhoneSetProgramming { get { return new O2GService("phonesetprogramming"); } }
        public static O2GService CallCenterAgent { get { return new O2GService("acdagent"); } }
        public static O2GService CallCenterPilot { get { return new O2GService("acdpilotmonitoring"); } }
        public static O2GService CallCenterRealtime { get { return new O2GService("acdrealtime"); } }
        //        public static O2GService CallCenterRsi { get { return new O2GService("acdrsi"); } }
        public static O2GService Analytics { get { return new O2GService("analytics"); } }
        public static O2GService UserManagement { get { return new O2GService("usermanagement"); } }
        public static O2GService Recording { get { return new O2GService("oxr recording"); } }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return Value == ((O2GService)obj).Value;
            }
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    internal class ServiceFactory
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Dictionary<O2GService, IService> services = new();
        private readonly Dictionary<O2GService, Uri> servicesUri = new();

        public string ApiVersion { get; private set; }

        public AccessMode AccessMode { get; private set; }


        public ServiceFactory(string apiVersion)
        {
            ApiVersion = apiVersion;
        }

        public async Task<ServerInfo> Bootstrap(Host host)
        {
            RoxeRestApiDescriptor descriptor = null;

            // Check connection using private URL if it exist
            if (host.PrivateAddress != null)
            {
                try
                {
                    // Initialize the server Uri
                    SetO2GServiceUri(host.PrivateAddress);

                    // Get the server information
                    descriptor = await GetO2GService().Get();
                    AccessMode = AccessMode.Private;
                }
                catch (HttpRequestException e)
                {
                    logger.Debug("Unable to bootstrap on {address} return code = {httpCode}", host.PrivateAddress, e.StatusCode);
                    if (host.PublicAddress == null)
                    {
                        // In case there is no public address, bootstrap definitively failed !
                        ThrowUnableToConnect(host, e);
                    }
                }
            }

            if (descriptor == null)
            {
                try
                {
                    // Initialize the server Uri
                    SetO2GServiceUri(host.PublicAddress);

                    // Get the server information
                    descriptor = await GetO2GService().Get();
                    AccessMode = AccessMode.Public;
                }
                catch (HttpRequestException e)
                {
                    logger.Debug("Unable to bootstrap on {address} return code = {httpCode}", host.PublicAddress, e.StatusCode);
                    ThrowUnableToConnect(host, e);
                }
            }

            // Here we should have a valid descriptor
            // Check the requested API version
            Version version;
            if (ApiVersion != null)
            {
                version = descriptor.Get(ApiVersion);
                if (version == null)
                {
                    // We have a problem...
                    throw new O2GException("The requested API version [" + ApiVersion + "] is not supported.");
                }
            }
            else
            {
                version = descriptor.GetCurrent();
                ApiVersion = version.Id;
            }

            // And initialize the authentication URL received
            if (AccessMode == AccessMode.Private)
            {
                servicesUri.Add(O2GService.Authentication, new UriBuilder(version.InternalUrl).Uri);
            }
            else
            {
                servicesUri.Add(O2GService.Authentication, new UriBuilder(version.PublicUrl).Uri);
            }

            return descriptor.ServerInfo;
        }


        private void SetO2GServiceUri(string address)
        {
            if (servicesUri.ContainsKey(O2GService.O2G))
            {
                servicesUri.Remove(O2GService.O2G);
                services.Remove(O2GService.O2G);
            }
            servicesUri.Add(O2GService.O2G, new UriBuilder("https://" + address + "/api/rest").Uri);
        }



        private static void ThrowUnableToConnect(Host host, HttpRequestException e)
        {
            if ((host.PrivateAddress != null) && (host.PublicAddress != null))
            {
                throw new O2GException(String.Format("Unable to bootstrap on O2G [{0}, {1}]", host.PrivateAddress, host.PublicAddress), e);
            }
            else if (host.PrivateAddress != null)
            {
                throw new O2GException(String.Format("Unable to bootstrap on O2G [{0}]", host.PrivateAddress), e);
            }
            else
            {
                throw new O2GException(String.Format("Unable to bootstrap on O2G [{0}]", host.PublicAddress), e);
            }
        }

        internal O2GRest GetO2GService()
        {
            return GetOrCreate<O2GRest>(O2GService.O2G);
        }

        internal EventSummaryRest GetEventSummaryService()
        {
            return GetOrCreate<EventSummaryRest>(O2GService.EventSummary);
        }

        internal SessionsRest GetSessionsService()
        {
            return GetOrCreate<SessionsRest>(O2GService.Sessions);
        }

        internal AuthenticationRest GetAuthenticationService()
        {
            return GetOrCreate<AuthenticationRest>(O2GService.Authentication);
        }

        internal SubscriptionsRest GetSubscriptionService()
        {
            return GetOrCreate<SubscriptionsRest>(O2GService.Subscriptions);
        }

        internal CommunicationLogRest GetCommunicationLogService()
        {
            return GetOrCreate<CommunicationLogRest>(O2GService.CommunicationLog);
        }
        internal PhoneSetProgrammingRest GetPhoneSetProgrammingService()
        {
            return GetOrCreate<PhoneSetProgrammingRest>(O2GService.PhoneSetProgramming);
        }

        internal TelephonyRest GetTelephonyService()
        {
            return GetOrCreate<TelephonyRest>(O2GService.Telephony);
        }
        internal UsersRest GetUsersService()
        {
            return GetOrCreate<UsersRest>(O2GService.Users);
        }
        internal RoutingRest GetRoutingService()
        {
            return GetOrCreate<RoutingRest>(O2GService.Routing);
        }
        internal MessagingRest GetMessagingService()
        {
            return GetOrCreate<MessagingRest>(O2GService.Messaging);
        }
        internal MaintenanceRest GetMaintenanceService()
        {
            return GetOrCreate<MaintenanceRest>(O2GService.Maintenance);
        }
        internal PbxManagementRest GetPbxManagementService()
        {
            return GetOrCreate<PbxManagementRest>(O2GService.PbxManagement);
        }

        internal DirectoryRest GetDirectoryService()
        {
            return GetOrCreate<DirectoryRest>(O2GService.Directory);
        }

        internal CallCenterAgentRest GetCallCenterAgentService()
        {
            return GetOrCreate<CallCenterAgentRest>(O2GService.CallCenterAgent);
        }

        internal CallCenterPilotRest GetCallCenterPilotService()
        {
            return GetOrCreate<CallCenterPilotRest>(O2GService.CallCenterPilot);
        }

        internal CallCenterRealtimeRest GetCallCenterRealtimeService()
        {
            return GetOrCreate<CallCenterRealtimeRest>(O2GService.CallCenterRealtime);
        }

        /*
        internal CallCenterRsiRest GetCallCenterRsiService()
        {
            return GetOrCreate<CallCenterRsiRest>(O2GService.CallCenterRsi);
        }
        */

        internal AnalyticsRest GetAnalyticsService()
        {
            return GetOrCreate<AnalyticsRest>(O2GService.Analytics);
        }

        internal UserManagementRest GetUserManagementService()
        {
            return GetOrCreate<UserManagementRest>(O2GService.UserManagement);
        }

        internal RecordingRest GetRecordingService()
        {
            return GetOrCreate<RecordingRest>(O2GService.Recording);
        }

        private T GetOrCreate<T>(O2GService serviceName) where T : IService
        {
            if (servicesUri.ContainsKey(serviceName))
            {
                IService service = GetService(serviceName);
                if (service == null)
                {
                    service = (IService)Activator.CreateInstance(typeof(T), servicesUri[serviceName]);
                    services.Add(serviceName, DependancyResolver.Resolve(service));
                }

                return (T)service;
            }
            else
            {
                // The service is not available, probably a license issue or a right (admin / users)
                throw new O2GException(string.Format("Service: {0} is not available in this context. It may be because it is only " +
                    "available for an administrator account or because you don't have the appropriate license.", serviceName.Value));
            }
        }

        private IService GetService(O2GService serviceName)
        {
            if (services.ContainsKey(serviceName))
            {
                return services[serviceName];
            }
            else
            {
                return null;
            }
        }

        internal void SetSessionUris(string privateUrl, string publicUrl)
        {
            if (AccessMode == AccessMode.Private)
            {
                servicesUri.Add(O2GService.Sessions, new UriBuilder(privateUrl).Uri);
            }
            else
            {
                servicesUri.Add(O2GService.Sessions, new UriBuilder(publicUrl).Uri);
            }
        }

        internal void SetServices(SessionInfo sessionInfo)
        {
            string baseUrl;

            // get the right URL
            if (AccessMode == AccessMode.Private)
            {
                baseUrl = sessionInfo.PrivateBaseUrl;
            }
            else
            {
                baseUrl = sessionInfo.PublicBaseUrl;
            }

            foreach (Service service in sessionInfo.Services)
            {
                O2GService serviceName = O2GService.Get(service.ServiceName);
                if (service.RelativeUrl.StartsWith("/telephony"))
                {
                    // Patch: check the principle
                    serviceName = O2GService.Telephony;
                }

                if (!servicesUri.ContainsKey(serviceName))
                {
                    if (service.RelativeUrl.StartsWith("/telephony"))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Register service: Telephony");
                        }

                        servicesUri.Add(serviceName, new UriBuilder(baseUrl + "/telephony").Uri);
                    }
                    else
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Register service: {0}", service.ServiceName);
                        }

                        servicesUri.Add(serviceName, new UriBuilder(baseUrl + service.RelativeUrl).Uri);
                    }
                }
            }

            servicesUri.Add(O2GService.UserManagement, new UriBuilder(baseUrl + "/usermanagement").Uri);
        }
    }
}
