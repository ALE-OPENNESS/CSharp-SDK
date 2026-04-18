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
using o2g.Events.Rsi;
using o2g.Internal.Services;
using o2g.Types.RsiNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>IRsi</c> provides access to the RSI (Routing Service Intelligence) point features:
    /// <list type="bullet">
    /// <item><description>Makes route selection.</description></item>
    /// <item><description>Makes digit collection.</description></item>
    /// <item><description>Plays voice guides or tones.</description></item>
    /// <item><description>Plays announcements (prompts and/or digits).</description></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// To be able to receive route requests from the OmniPCX Enterprise, the application must first subscribe to
    /// RSI events and then enable the RSI point.
    /// <para>
    /// Using this service requires having a <b>CONTACTCENTER_RSI</b> license.
    /// </para>
    /// </remarks>
    public interface IRsi : IService
    {
        /// <summary>
        /// Raised when a digit collection session has ended.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnDigitCollectedEvent>> DigitCollected;

        /// <summary>
        /// Raised from an RSI point when a tone generation starts.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnToneGeneratedStartEvent>> ToneGeneratedStart;

        /// <summary>
        /// Raised from an RSI point when a tone generation stops.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnToneGeneratedStopEvent>> ToneGeneratedStop;

        /// <summary>
        /// Raised from a routing point to close a route session (the routing Crid is no longer valid).
        /// </summary>
        public event EventHandler<O2GEventArgs<OnRouteEndEvent>> RouteEnd;

        /// <summary>
        /// Raised from a routing point to request a route selection.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnRouteRequestEvent>> RouteRequest;

        /// <summary>
        /// Gets the configured RSI points.
        /// </summary>
        /// <returns>
        /// A list of <see cref="RsiPoint"/> representing all the declared RSI points.
        /// </returns>
        Task<List<RsiPoint>> GetRsiPointsAsync();

        /// <summary>
        /// Enables the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Returns <see langword="false"/> if the RSI point is already enabled.
        /// </remarks>
        Task<bool> EnableRsiPointAsync(string rsiNumber);

        /// <summary>
        /// Disables the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Returns <see langword="false"/> if the RSI point is already disabled.
        /// </remarks>
        Task<bool> DisableRsiPointAsync(string rsiNumber);

        /// <summary>
        /// Starts a digits collection on the specified RSI point, for the specified call.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which to collect the digits.</param>
        /// <param name="numChars">Optional number of digits to collect; the digit collection stops when this number is reached.</param>
        /// <param name="flushChar">Optional character that stops the digit collection when pressed.</param>
        /// <param name="timeout">Optional timeout in seconds; the digit collection stops when this delay elapses.</param>
        /// <param name="additionalCriteria">Extension criteria used to collect digits.</param>
        /// <returns>
        /// A unique identifier (Crid) for this digit collection session, or <see langword="null"/> in case of error.
        /// </returns>
        /// <seealso cref="StopCollectDigitsAsync(string, string)"/>
        /// <seealso cref="OnDigitCollectedEvent"/>
        Task<string> StartCollectDigitsAsync(string rsiNumber, string callRef, int? numChars, char? flushChar = null, int? timeout = null, AdditionalDigitCollectionCriteria additionalCriteria = null);

        /// <summary>
        /// Stops the specified digit collection on the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="collCrid">The digit collection identifier returned by <see cref="StartCollectDigitsAsync(string, string, int?, char?, int?, AdditionalDigitCollectionCriteria)"/>.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="StartCollectDigitsAsync(string, string, int?, char?, int?, AdditionalDigitCollectionCriteria)"/>
        /// <seealso cref="OnDigitCollectedEvent"/>
        Task<bool> StopCollectDigitsAsync(string rsiNumber, string collCrid);

        /// <summary>
        /// Plays the specified tone on the specified call.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which the tone will be played.</param>
        /// <param name="tone">The tone type.</param>
        /// <param name="duration">The duration the tone is played, in seconds.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="CancelToneAsync(string, string)"/>
        /// <seealso cref="OnToneGeneratedStartEvent"/>
        Task<bool> PlayToneAsync(string rsiNumber, string callRef, Tones tone, int duration);

        /// <summary>
        /// Cancels the tone currently playing on the specified call.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which the tone is playing.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="PlayToneAsync(string, string, Tones, int)"/>
        /// <seealso cref="OnToneGeneratedStopEvent"/>
        Task<bool> CancelToneAsync(string rsiNumber, string callRef);

        /// <summary>
        /// Plays the specified voice guide on the specified call.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which the voice guide will be played.</param>
        /// <param name="guideNumber">The voice guide number as defined in the OmniPCX Enterprise.</param>
        /// <param name="duration">An optional duration for the voice guide, in seconds.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="OnToneGeneratedStartEvent"/>
        Task<bool> PlayVoiceGuideAsync(string rsiNumber, string callRef, int guideNumber, int? duration = null);

        /// <summary>
        /// Ends a route session, indicating that no route will be selected.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="routeCrid">The routing session unique identifier.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="OnRouteRequestEvent"/>
        Task<bool> RouteEndAsync(string rsiNumber, string routeCrid);

        /// <summary>
        /// Selects a route as a response to a route request.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="routeCrid">The routing session unique identifier.</param>
        /// <param name="selectedRoute">The selected route number.</param>
        /// <param name="callingLine">Optional calling line number presented to the called party.</param>
        /// <param name="associatedData">Optional correlator data to attach to the call.</param>
        /// <param name="routeToVoiceMail"><see langword="true"/> if the selected route is the voice mail; <see langword="false"/> otherwise.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <c>callingLine</c> can be used to change the identity of the calling number presented to the called party.
        /// </remarks>
        /// <seealso cref="OnRouteRequestEvent"/>
        Task<bool> RouteSelectAsync(string rsiNumber, string routeCrid, string selectedRoute, string callingLine = null, string associatedData = null, bool? routeToVoiceMail = null);

        /// <summary>
        /// Gets the list of existing route sessions for the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <returns>
        /// A list of <see cref="RouteSession"/> objects representing the route sessions in progress for this RSI point.
        /// </returns>
        Task<List<RouteSession>> GetRouteSessionsAsync(string rsiNumber);

        /// <summary>
        /// Returns the specified route session.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="routeCrid">The routing session unique identifier.</param>
        /// <returns>
        /// A <see cref="RouteSession"/> object representing the route session, or <see langword="null"/> in case of error or if no such route session exists.
        /// </returns>
        Task<RouteSession> GetRouteSessionAsync(string rsiNumber, string routeCrid);

    }
}
