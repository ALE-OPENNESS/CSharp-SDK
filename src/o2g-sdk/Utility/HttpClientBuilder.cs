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

using o2g.Internal.Utility;
using System.Net.Http;

namespace o2g.Utility
{
    /// <summary>
    /// <c>HttpClientBuilder</c> is used internal by the SDK to build the <see cref="HttpClient"/> used to invoke REST services.
    /// It's possible to activate REST traces and to disable the SSL certificate verification. 
    /// <b>Do not use this option on a production environment.</b>
    /// </summary>
    public class HttpClientBuilder
    {
        static HttpClientBuilder()
        {
            DisableSSValidation = false;
            TraceREST = false;
        }

        /// <summary>
        /// Disable the SSL certificate validation.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the SSL certificate validation is disabled; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// <para>
        /// It can be useful to disable the O2G server certificate validation on a test environment, when the O2G server is for exemple a self-signed certificate.
        /// <b>Do not disable the SSL certificate validation on a production environment!</b>
        /// </para>
        /// <para>
        /// Disabling the SSL certificate validation must be done before the creation of the <see cref="HttpClient"/>, so before the call to the <see cref="O2G.Application.LoginAsync(string, string)"/> method.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        ///    class Program
        ///    {
        ///        static async Task Main(string[] args)
        ///        {
        ///            HttpClientBuilder.DisableSSValidation = true;
        ///            HttpClientBuilder.TraceREST = true;
        ///
        ///            O2G.Application myApp = new("MyApplication");
        ///
        ///            myApp.SetHost(new()
        ///            {
        ///                 PrivateAddress = "o2g.private.vlab2.com",
        ///                 PublicAddress = "o2g.public.vlab2.com"
        ///            });
        ///
        ///            await myApp.LoginAsync("oxe1000", "1234");
        ///
        ///            ...
        ///        }
        /// </code>
        /// </example>
        public static bool DisableSSValidation { get; set; }


        /// <summary>
        /// Enable tracing the REST request.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the tracing is activated; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// <para>
        /// For debuging purpose, it can be interesting to log the REST messages exchanged.
        /// </para>
        /// <para>
        /// Activating REST traces must be done before the creation of the <see cref="HttpClient"/>, so before the call to the <see cref="O2G.Application.LoginAsync(string, string)"/> method.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        ///    class Program
        ///    {
        ///        static async Task Main(string[] args)
        ///        {
        ///            HttpClientBuilder.DisableSSValidation = true;
        ///            HttpClientBuilder.TraceREST = true;
        ///
        ///            O2G.Application myApp = new("MyApplication");
        ///
        ///            myApp.SetHost(new()
        ///            {
        ///                 PrivateAddress = "o2g.private.vlab2.com",
        ///                 PublicAddress = "o2g.public.vlab2.com"
        ///            });
        ///
        ///            await myApp.LoginAsync("oxe1000", "1234");
        ///
        ///            ...
        ///        }
        /// </code>
        /// </example>
        public static bool TraceREST { get; set; }

        internal static HttpClient Build()
        {
            if (!TraceREST && !DisableSSValidation)
            {
                return new();
            }
            else
            {
                var httpClientHandler = new HttpClientHandler();

                if (DisableSSValidation)
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback += HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                }

                if (TraceREST)
                {
                    return new(new LoggingHandler(httpClientHandler), true);
                }
                else
                {
                    return new(httpClientHandler, true);
                }
            }
        }

        internal static HttpClient BuildChunk()
        {
            if (!DisableSSValidation)
            {
                return new();
            }
            else 
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback += HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                
                return new(httpClientHandler, true);
            }
        }
    }
}
