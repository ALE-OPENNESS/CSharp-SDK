using o2g;
using o2g.Events.EventSummary;
using o2g.Events.Telephony;
using o2g.Types;
using o2g.Utility;
using System.Net;

namespace o2g_sdk.Sample
{
    // ---------------------------------------------------------------------------
    // IWebHook implementation — exposes an HTTP endpoint and forwards POST bodies
    // to the SDK event processor.
    // ---------------------------------------------------------------------------
    class SampleWebHook : IWebHook
    {
        private IEventProcessor _processor;
        private readonly HttpListener _listener = new();
        private Task _listenTask;

        public string Url { get; }

        public SampleWebHook(string url)
        {
            Url = url;
            _listener.Prefixes.Add(url.TrimEnd('/') + "/");
        }

        public void ConnectProcessor(IEventProcessor processor)
        {
            _processor = processor;
            _listener.Start();
            _listenTask = Task.Run(ListenAsync);
            Console.WriteLine($"[WebHook] Listening on {Url}");
        }

        private async Task ListenAsync()
        {
            while (_listener.IsListening)
            {
                try
                {
                    HttpListenerContext ctx = await _listener.GetContextAsync();
                    using StreamReader reader = new(ctx.Request.InputStream);
                    string body = await reader.ReadToEndAsync();

                    _processor.Process(body);

                    ctx.Response.StatusCode = 200;
                    ctx.Response.Close();
                }
                catch (HttpListenerException)
                {
                    // Listener was stopped — exit gracefully
                    break;
                }
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listenTask?.Wait();
        }
    }

    // ---------------------------------------------------------------------------

    internal class Program
    {
        // ---------------------------------------------------------------------------
        // Configuration
        // ---------------------------------------------------------------------------
        const string ServerHost  = "localhost:8443";
        const string LoginName   = "internal29";
        const string Password    = "alcatel29";
        const string WebHookUrl  = "http://localhost:8080/o2g/events/";
        // ---------------------------------------------------------------------------

        static async Task Main(string[] args)
        {
            HttpClientBuilder.DisableSSValidation = true;

            O2G.Application app = new("o2g-sdk-sample");

            app.SetHost(new Host
            {
                PrivateAddress = ServerHost,
                PublicAddress  = ServerHost
            });

            SampleWebHook webHook = new(WebHookUrl);

            try
            {
                Console.WriteLine($"Connecting to '{ServerHost}' as '{LoginName}'...");
                await app.LoginAsync(LoginName, Password);
                Console.WriteLine("Login successful.");

                // Register telephony event handlers
                app.EventSummaryService.EventSummaryUpdated += (_, e) =>
                {
                    OnEventSummaryUpdatedEvent ev = e.Event;
                    Console.WriteLine($"[EVENT] OnEventSummaryUpdatedEvent");
                };

                Subscription subscription = Subscription.Builder
                    .AddEventSummaryEvents(["*"])
                    .SetWebHook(webHook)
                    .Build();

                await app.SubscribeAsync(subscription);

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey(intercept: true);
            }
            catch (O2GException ex) when (ex.Message.Contains("401") || ex.Message.Contains("Unauthorized") || ex.Message.Contains("authenticate"))
            {
                Console.WriteLine("Authentication failed — check login and password.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                webHook.Stop();
                await app.ShutdownAsync();
                Console.WriteLine("Session closed.");
            }
        }
    }
}
