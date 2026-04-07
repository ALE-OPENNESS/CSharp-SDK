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

using System;
using System.Threading;
using System.Threading.Tasks;

namespace o2g.Internal.Utility
{
    internal abstract class CancelableTask
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private CancellationTokenSource cancellationTokenSource;
        protected CancellationToken Token { get; private set; }
        protected Task RunningTask { get; private set; }

        public CancelableTask()
        {
        }

        protected abstract Task CancelableRun();

        public void Start()
        {
            cancellationTokenSource = new();
            Token = cancellationTokenSource.Token;

            RunningTask = Task.Run(async () =>
            {
                try
                {
                    await CancelableRun();
                }
                catch (OperationCanceledException)
                {
                    logger.Debug("Task is terminated");
                }
            }, Token);
        }

        protected void CancelTask()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
