/*
* Copyright 2026 ALE International
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

namespace o2g
{
    /// <summary>
    /// Controls how the SDK reacts to connection and session failures, and receives
    /// notifications about session lifecycle changes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Override the virtual methods you need to customize. Methods that return a
    /// <see cref="Behavior"/> control whether the SDK retries or aborts after a failure.
    /// Notification methods (void) are called to inform the application of state changes.
    /// </para>
    /// <para>
    /// Set an instance on the application before calling
    /// <see cref="O2G.Application.LoginAsync"/> via
    /// <see cref="O2G.Application.SetSessionMonitoringPolicy"/>.
    /// If no policy is set, the built-in default policy is used.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// class MyPolicy : SessionMonitoringPolicy
    /// {
    ///     public override Behavior OnConnectFailure(Exception e)
    ///     {
    ///         Console.WriteLine($"Connect failed: {e.Message} — retrying in 10 s");
    ///         return Behavior.RetryAfter(10_000);
    ///     }
    ///
    ///     public override void OnSessionLost(string reason)
    ///         => Console.WriteLine($"Session lost ({reason}) — SDK is recovering...");
    ///
    ///     public override void OnSessionRecovered()
    ///         => Console.WriteLine("Session recovered.");
    /// }
    ///
    /// app.SetSessionMonitoringPolicy(new MyPolicy());
    /// </code>
    /// </example>
    public abstract class SessionMonitoringPolicy
    {
        /// <summary>
        /// Represents the action the SDK should take after a recoverable failure.
        /// </summary>
        public sealed class Behavior
        {
            private readonly int _delayMs;

            private Behavior(int delayMs) => _delayMs = delayMs;

            /// <summary>
            /// Retry immediately without waiting.
            /// </summary>
            public static Behavior Retry() => new(0);

            /// <summary>
            /// Retry after the specified delay.
            /// </summary>
            /// <param name="milliseconds">Delay in milliseconds before the next attempt.</param>
            public static Behavior RetryAfter(int milliseconds) => new(Math.Max(0, milliseconds));

            /// <summary>
            /// Abort the current operation and trigger session recovery.
            /// </summary>
            public static Behavior Abort() => new(-1);

            internal bool IsAbort => _delayMs < 0;
            internal int DelayMs  => _delayMs;
        }

        // -----------------------------------------------------------------------
        // Failure callbacks — return how the SDK should react
        // -----------------------------------------------------------------------

        /// <summary>
        /// Called when an attempt to connect or reconnect to the O2G server fails.
        /// </summary>
        /// <param name="e">The exception that caused the failure.</param>
        /// <returns>
        /// <see cref="Behavior.RetryAfter"/> to wait and try again;
        /// <see cref="Behavior.Abort"/> to stop and throw.
        /// </returns>
        public virtual Behavior OnConnectFailure(Exception e)
            => Behavior.RetryAfter(5_000);

        /// <summary>
        /// Called when the chunk event channel encounters a network error.
        /// </summary>
        /// <param name="e">The exception that caused the failure.</param>
        /// <returns>
        /// <see cref="Behavior.RetryAfter"/> to reconnect the chunk channel;
        /// <see cref="Behavior.Abort"/> to signal session loss and start recovery.
        /// </returns>
        public virtual Behavior OnChunkChannelFailure(Exception e)
            => Behavior.Abort();

        /// <summary>
        /// Called when the keep-alive request fails with a network error.
        /// </summary>
        /// <param name="e">The exception that caused the failure.</param>
        /// <returns>
        /// <see cref="Behavior.RetryAfter"/> to continue keep-alive attempts;
        /// <see cref="Behavior.Abort"/> to signal session loss and start recovery.
        /// </returns>
        public virtual Behavior OnKeepAliveFailure(Exception e)
            => Behavior.Abort();

        // -----------------------------------------------------------------------
        // Session lifecycle notifications
        // -----------------------------------------------------------------------

        /// <summary>
        /// Called when the session is lost due to a server failure or network outage.
        /// </summary>
        /// <param name="reason">A short string describing the cause of the loss.</param>
        /// <remarks>
        /// The SDK starts automatic recovery immediately after this call.
        /// </remarks>
        public virtual void OnSessionLost(string reason) { }

        /// <summary>
        /// Called when the session has been successfully recovered after a loss.
        /// </summary>
        /// <remarks>
        /// The event subscription (if any) has been re-established at this point.
        /// </remarks>
        public virtual void OnSessionRecovered() { }

        // -----------------------------------------------------------------------
        // Success notifications
        // -----------------------------------------------------------------------

        /// <summary>
        /// Called when the chunk event channel is successfully established.
        /// </summary>
        public virtual void OnChunkChannelEstablished() { }

        /// <summary>
        /// Called when a keep-alive request is successfully acknowledged by the server.
        /// </summary>
        public virtual void OnKeepAliveDone() { }

        // -----------------------------------------------------------------------
        // Fatal error notifications
        // -----------------------------------------------------------------------

        /// <summary>
        /// Called when the chunk channel receives a fatal HTTP error (e.g. 401, 403).
        /// </summary>
        /// <param name="httpStatus">The HTTP status code returned by the server.</param>
        public virtual void OnChunkChannelFatalError(int httpStatus) { }

        /// <summary>
        /// Called when the server rejects the keep-alive, indicating the session has
        /// expired server-side.
        /// </summary>
        public virtual void OnKeepAliveFatalError() { }

        /// <summary>
        /// Called when an exception is thrown inside an application event handler.
        /// </summary>
        /// <param name="e">The exception thrown by the application handler.</param>
        public virtual void OnEventException(Exception e) { }
    }
}
