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
using o2g.Internal.Utility;
using o2g.Utility;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace o2g.Internal.Events
{
    class ChunkEventDispatcher : CancelableQueueTask<O2GEventDescriptor>
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly OnEvent _onEventDelegate;
        private readonly SessionMonitoringPolicy _policy;

#pragma warning disable CS0067, CS0649
        [Injection]
        private EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public ChunkEventDispatcher(BlockingCollection<O2GEventDescriptor> eventQueue, OnEvent onEventDelegate, SessionMonitoringPolicy policy) : base(eventQueue)
        {
            _onEventDelegate = onEventDelegate;
            _policy = policy;
        }

        protected override Task CancelableRun()
        {
            while (true)
            {
                O2GEventDescriptor o2gEventDescriptor = Get();
                Token.ThrowIfCancellationRequested();

                try
                {
                    if (!_eventHandlers.Throw(o2gEventDescriptor))
                    {
                        _onEventDelegate?.Invoke(o2gEventDescriptor.Event);
                    }
                }
                catch (Exception e)
                {
                    _policy.OnEventException(e);
                }
            }
        }

        public async Task Stop()
        {
            CancelTask();
            await RunningTask;
        }
    }


    class ChunkEventListener : CancelableQueueTask<O2GEventDescriptor>
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly HttpClient httpClient = HttpClientBuilder.BuildChunk();
        private readonly Uri uri;
        private readonly InfoSemaphore signalReady;
        private readonly SessionMonitoringPolicy _policy;
        private readonly Action<string> _onSessionLost;
        private bool _startupSignalSent = false;

        public ChunkEventListener(BlockingCollection<O2GEventDescriptor> eventQueue, Uri uri, InfoSemaphore signalReady, SessionMonitoringPolicy policy, Action<string> onSessionLost) : base(eventQueue)
        {
            this.uri = uri;
            this.signalReady = signalReady;
            _policy = policy;
            _onSessionLost = onSessionLost;
        }

        // Sends the startup signal (once) or routes to the session-lost callback.
        private void OnStartupSignalOrLost(Exception e, string lostReason)
        {
            if (!_startupSignalSent)
            {
                _startupSignalSent = true;
                signalReady.Fail(e);
            }
            else
            {
                _onSessionLost(lostReason);
            }
        }

        // Returns null on clean EOF, returns the IOException on an unexpected close.
        private async Task<IOException> ReadChuncks(HttpResponseMessage response)
        {
            logger.Trace("Event channel is established.");

            Stream chunkedStream = await response.Content.ReadAsStreamAsync(Token);
            StreamReader reader = new(chunkedStream);

            while (true)
            {
                string sEvent;
                try
                {
                    sEvent = reader.ReadLine();
                }
                catch (IOException e)
                {
                    logger.Trace("Event channel closed unexpectedly.");
                    Token.ThrowIfCancellationRequested();
                    return e;
                }

                if (sEvent == null)
                {
                    logger.Trace("Event stream ended (EOF).");
                    return null;
                }

                O2GEventDescriptor eventDescriptor = EventBuilder.Get(sEvent);
                if (eventDescriptor == null)
                {
                    logger.Error("Unable to create Event from {event}", sEvent);
                }
                else
                {
                    O2GEvent o2gEvent = eventDescriptor.Event;
                    if (o2gEvent is OnChannelInformationEvent channelInfoEvent)
                    {
                        if (channelInfoEvent.Text != "keepalive")
                        {
                            // Channel established — notify policy and unblock ChunkEventing.Start() once
                            _policy.OnChunkChannelEstablished();
                            if (!_startupSignalSent)
                            {
                                _startupSignalSent = true;
                                signalReady.Success();
                            }
                        }
                    }
                    Add(eventDescriptor);
                }
            }
        }

        protected override async Task CancelableRun()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        logger.Trace("Start eventing channel on {uri}", uri);

                        HttpRequestMessage request = new(HttpMethod.Post, uri);
                        HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Token);

                        if (!response.IsSuccessStatusCode)
                        {
                            int status = (int)response.StatusCode;
                            logger.Error("Chunk channel HTTP error {status}", status);
                            _policy.OnChunkChannelFatalError(status);
                            OnStartupSignalOrLost(new O2GException($"Chunk channel HTTP error {status}"), $"chunk fatal HTTP error {status}");
                            return;
                        }

                        IOException channelCloseEx = await ReadChuncks(response);

                        if (channelCloseEx != null)
                        {
                            // Unexpected close — ask the policy how to react
                            var b = _policy.OnChunkChannelFailure(channelCloseEx);
                            if (b.IsAbort)
                            {
                                OnStartupSignalOrLost(channelCloseEx, "chunk channel closed: " + channelCloseEx.Message);
                                return;
                            }
                            if (b.DelayMs > 0)
                                await Task.Delay(b.DelayMs, Token);
                        }
                        // null means clean EOF — reconnect immediately (no policy call)
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (HttpRequestException e)
                    {
                        logger.Error("Unable to request {uri}: {error}", uri, e.Message);
                        var b = _policy.OnChunkChannelFailure(e);
                        if (b.IsAbort)
                        {
                            OnStartupSignalOrLost(e, "chunk network error: " + e.Message);
                            return;
                        }
                        if (b.DelayMs > 0)
                            await Task.Delay(b.DelayMs, Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.Trace("Chunk listener cancelled.");
                throw;
            }
        }

        public void Stop()
        {
            CancelTask();
        }
    }

    internal class ChunkEventing
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly InfoSemaphore signalReady = new();

        private readonly ChunkEventListener _chunkEventListener = null;
        private readonly ChunkEventDispatcher _chunkEventDispatcher = null;

        public ChunkEventing(Uri chunkUri, OnEvent onEventDelegate, SessionMonitoringPolicy policy, Action<string> onSessionLost)
        {
            BlockingCollection<O2GEventDescriptor> eventQueue = new();

            _chunkEventDispatcher = DependancyResolver.Resolve<ChunkEventDispatcher>(new(eventQueue, onEventDelegate, policy));
            _chunkEventListener = new(eventQueue, chunkUri, signalReady, policy, onSessionLost);
        }

        internal void Start()
        {
            _chunkEventDispatcher.Start();
            _chunkEventListener.Start();

            signalReady.Wait();
        }

        internal async Task Stop()
        {
            await _chunkEventDispatcher.Stop();
            _chunkEventListener.Stop();
        }
    }
}
