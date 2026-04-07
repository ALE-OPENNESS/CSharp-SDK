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
using o2g.Events.CallCenterRsi;
using o2g.Internal.Services;
using o2g.Types.CallCenterRsiNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>ContactCenterRsi</c> service provides the capabilities on a RSI (Routing Service Intelligence) point:
    /// <list type="bullet">
    /// <item><description>To make route selection.</description></item>
    /// <item><description>To make digit collection.</description></item>
    /// <item><description>To play voice guide or tone.</description></item>
    /// <item><description>To play announcements (prompts and/or digits).</description></item>
    /// </list>
    /// <para>
    /// To be able to receive the RouteRequest from the OmniPCX Enterprise, the first action is subscribe to rsi events 
    /// and the second action is to enable the RSI point.
    /// </para>
    /// Using this service requires having a <b>CONTACTCENTER_RSI</b> license.
    /// </summary>
    public interface ICallCenterRsi : IService
    {
        /// <summary>
        /// Occurs when a data collection has ended.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnDigitCollectedEvent>> DigitCollected;

        /// <summary>
        /// Raised from a RSI point when a tone generation is started.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnToneGeneratedStartEvent>> ToneGeneratedStart;

        /// <summary>
        /// Raised from a RSI point when a tone generation is stopped.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnToneGeneratedStopEvent>> ToneGeneratedStop;

        /// <summary>
        /// Raised from a Routing point to close a route session (routing crid is no longer valid).
        /// </summary>
        public event EventHandler<O2GEventArgs<OnRouteEndEvent>> RouteEnd;

        /// <summary>
        /// Raised from a Routing point to request a route.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnRouteRequestEvent>> RouteRequest;

        /// <summary>
        /// Get the configured RSI points.
        /// </summary>
        /// <returns>
        /// A list of <see cref="RsiPoint"/> that represents all the declared RSI points.
        /// </returns>
        Task<List<RsiPoint>> GetRsiPointsAsync();

        /// <summary>
        /// Enable the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The method return <see langword="false"/> if the RSI point is already enabled.
        /// </remarks>
        Task<bool> EnableRsiPointAsync(string rsiNumber);

        /// <summary>
        /// Disable the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The method return <see langword="false"/> if the RSI point is already disabled.
        /// </remarks>
        Task<bool> DisableRsiPointAsync(string rsiNumber);

        /// <summary>
        /// Start a digits collection on the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which to collect the digits.</param>
        /// <param name="numChars">Optional number of digit to collect. The digit collection is stopped when this number is reached.</param>
        /// <param name="flushChar">Optional char used to stop the digit collection when pressed.</param>
        /// <param name="timeout">Optional timeout in second. Stop the digit collection after this time elapses.</param>
        /// <param name="additionalCriteria">Extension criteria used to collect digits.</param>
        /// <returns>
        /// A <see langword="string"/> thats represents the digits collection Crid. A unique identifier for the collection.
        /// </returns>
        /// <seealso cref="StopCollectDigitsAsync(string, string)"/>
        /// <seealso cref="OnDigitCollectedEvent"/>
        Task<string> StartCollectDigitsAsync(string rsiNumber, string callRef, int? numChars, char? flushChar = null, int? timeout = null, AdditionalDigitCollectionCriteria additionalCriteria = null);

        /// <summary>
        /// Stop the specified digit collection on the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="collCrid">The digit collection identifier.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="OnDigitCollectedEvent"/>
        /// <seealso cref="StopCollectDigitsAsync(string, string)"/>
        Task<bool> StopCollectDigitsAsync(string rsiNumber, string collCrid);

        /// <summary>
        /// Play the specified tone.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which to tone will be played.</param>
        /// <param name="tone">The tone type.</param>
        /// <param name="duration">The duration the tone is played (in second)</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="CancelToneAsync(string, string)"/>
        /// <seealso cref="OnToneGeneratedStartEvent"/>
        Task<bool> PlayToneAsync(string rsiNumber, string callRef, Tones tone, int duration);

        /// <summary>
        /// Cancel playing a tone.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which to tone will be played.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="PlayToneAsync(string, string, Tones, int)"/>
        /// <seealso cref="OnToneGeneratedStopEvent"/>
        Task<bool> CancelToneAsync(string rsiNumber, string callRef);

        /// <summary>
        /// Play the specified voice guide.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="callRef">The reference of the call on which to voice guide will be played.</param>
        /// <param name="guideNumber">The voice guide number as defined in the OmniPcx Enterprise.</param>
        /// <param name="duration">An optional duration for the voice guide</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="OnToneGeneratedStartEvent"/>
        Task<bool> PlayVoiceGuideAsync(string rsiNumber, string callRef, int guideNumber, int? duration = null);

        /// <summary>
        /// End a route session.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="routeCrid">The routing session unique identifier.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> RouteEndAsync(string rsiNumber, string routeCrid);

        /// <summary>
        /// Selects a route as a response to a RouteRequest.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="routeCrid">The routing session unique identifier.</param>
        /// <param name="selectedRoute">The selected route number.</param>
        /// <param name="callingLine">Optional calling line number.</param>
        /// <param name="associatedData">Correlator data to add to the call.</param>
        /// <param name="routeToVoiceMail"><see langword="true"/> if the selected route is the voice mail.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <paramref name="callingLine"/> can be used to change the identity of the calling number to present on the called party.
        /// </remarks>
        /// <seealso cref="OnRouteRequestEvent"/>
        Task<bool> RouteSelectAsync(string rsiNumber, string routeCrid, string selectedRoute, string callingLine = null, string associatedData = null, bool? routeToVoiceMail = null);

        /// <summary>
        /// Return the list of existing route request session for the specified RSI point.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <returns>
        /// A list of <see cref="RouteSession"/> objets that represents the route session in progress for this RSI point.
        /// </returns>
        Task<List<RouteSession>> GetRouteSessionsAsync(string rsiNumber);

        /// <summary>
        /// Return the specified route session.
        /// </summary>
        /// <param name="rsiNumber">The RSI point extension number.</param>
        /// <param name="routeCrid">The routing session unique identifier.</param>
        /// <returns>
        /// A <see cref="RouteSession"/> objet that represents the route session.
        /// </returns>
        Task<RouteSession> GetRouteSessionAsync(string rsiNumber, string routeCrid);

    }
}
