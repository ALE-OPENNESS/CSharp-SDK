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
        private OnEvent _onEventDelegate;

#pragma warning disable CS0067, CS0649
        [Injection]
        private EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public ChunkEventDispatcher(BlockingCollection<O2GEventDescriptor> eventQueue, OnEvent onEventDelegate) : base(eventQueue)
        {
            this._onEventDelegate = onEventDelegate;
        }

        protected override Task CancelableRun()
        {
            while (true)
            {
                O2GEventDescriptor o2gEventDescriptor = Get();
                Token.ThrowIfCancellationRequested();

                // Try to send event to the event handlers
                if (!_eventHandlers.Throw(o2gEventDescriptor))
                {
                    // And finally on default handler
                    if (_onEventDelegate != null)
                    {
                        _onEventDelegate(o2gEventDescriptor.Event);
                    }
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
 
        public ChunkEventListener(BlockingCollection<O2GEventDescriptor> eventQueue, Uri uri, InfoSemaphore signalReady): base(eventQueue)
        {
            this.uri = uri;
            this.signalReady = signalReady;
        }


        private async Task ReadChuncks(HttpResponseMessage response)
        {
            logger.Trace("Event channel is established.");

            Stream chunkedStream = await response.Content.ReadAsStreamAsync(Token);
            StreamReader reader = new(chunkedStream);

            try
            {
                while (true)
                {
                    string sEvent = reader.ReadLine();
                    if (sEvent != null)
                    {
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
                                if (channelInfoEvent.Text == "keepalive")
                                {
                                    // Do nothing
                                }
                                else
                                {
                                    // Signal the channel has been established
                                    signalReady.Success();
                                }
                            }

                            // Push event for dispatching
                            Add(eventDescriptor);
                        }
                    }
                    else
                    {
                        logger.Error("Read a null string");
                    }
                }
            }
            catch (IOException)
            {
                // Chunk is closed => exit and restart except if cancellation is requested
                logger.Trace("Event channel has been closed.");
                Token.ThrowIfCancellationRequested();
            }
            catch (Exception ee)
            {
                logger.Error(ee, "An exception has been thrown !! ");
            }
        }


        protected override async Task CancelableRun()
        {
            try
            {
                logger.Trace("Enter in chunk loop");

                while (true)
                {
                    try
                    {
                        logger.Trace("Start eventing channel on {uri}", uri);

                        HttpRequestMessage request = new(HttpMethod.Post, uri);
                        HttpResponseMessage response =  await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Token);
                        bool ChannelIsOpen = response.IsSuccessStatusCode;
                        if (!ChannelIsOpen)
                        {
                            // We have a probleme with the eventing
                            throw new O2GException("Fail to open chunk event channel");
                        }
                        else
                        {
                            logger.Trace("Channel established, read");
                            await ReadChuncks(response);
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        logger.Error("Unable to request {uri} {error}", uri, e.Message);
                        signalReady.Fail(e);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.Error("OperationCanceledException receive");
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

        public ChunkEventing(Uri chunkUri, OnEvent onEventDelegate)
        {
            BlockingCollection<O2GEventDescriptor> eventQueue = new();

            _chunkEventDispatcher = DependancyResolver.Resolve<ChunkEventDispatcher>(new(eventQueue, onEventDelegate));
            _chunkEventListener = new(eventQueue, chunkUri, signalReady);
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
