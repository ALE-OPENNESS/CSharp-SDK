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
using o2g.Internal.Utility;
using o2g.Types;
using o2g.Utility;
using System.Threading.Tasks;

namespace o2g.Internal
{
    internal class ServiceEndPointImpl : ServiceEndPoint
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ServiceFactory serviceFactory;
        
        public ServerInfo ServerInfo { get; private set; }

        internal ServiceEndPointImpl(ServiceFactory serviceFactory, ServerInfo serverInfo)
        {
            this.serviceFactory = serviceFactory;
            ServerInfo = serverInfo;
        }

        async Task<Session> ServiceEndPoint.OpenSession(Credential credential, string applicationName)
        {
            AssertUtil.NotNullOrEmpty(credential.Login, "Login");
            AssertUtil.NotNullOrEmpty(credential.Password, "Password");
            AssertUtil.NotNullOrEmpty(applicationName, "ApplicationName");

            // Get the authentication service and authenticate the given credential
            logger.Trace("OpenSession -> Authenticate user {login}", credential.Login);

            IAuthentication authenticationService = serviceFactory.GetAuthenticationService();
            O2GAuthenticateResult authenticateResult = await authenticationService.Authenticate(credential);
            logger.Debug("Authentication done.");

            serviceFactory.SetSessionUris(authenticateResult.PrivateUrl, authenticateResult.PublicUrl);

            // Now open the session and retrieve other services
            logger.Trace("OpenSession -> OpenSession {application}", applicationName);

            ISessions sessionsService = serviceFactory.GetSessionsService();
            SessionInfo sessionInfo = await sessionsService.Open(applicationName);
            serviceFactory.SetServices(sessionInfo);
            logger.Debug("Session opened: TimeToLive = {timeToLive}", sessionInfo.TimeToLive);

            // Create the session
            SessionImpl session = new(serviceFactory, sessionInfo, credential.Login, authenticateResult.Expired);

            // OK, create the session
            return session;
        }
    }
}
