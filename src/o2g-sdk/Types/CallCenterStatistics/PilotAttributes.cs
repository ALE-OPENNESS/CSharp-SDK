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

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Defines the set of statistical attributes available for a CCD pilot.
    /// Enum member names match the JSON API field names exactly.
    /// </summary>
#pragma warning disable CS1591
    public enum PilotAttributes
    {
        nbCallsOpen,
        nbCallsBlocked,
        nbCallsForward,
        nbCallsByTransfer,
        nbCallsByMutualAid,
        maxNbSimultCalls,
        nbOverflowInQueue,
        nbOverflowInRinging,
        nbCallsWOQueuing,
        nbCallsAfterQueuing,
        nbCallsSentInMutualAidQueue,
        nbCallsRedirectedOutACDArea,
        nbCallsDissuaded,
        nbCallsDissuadedAfterTryingMutualAid,
        nbCallsVGTypePG,
        nbCallsSentToPG,
        nbCallsRejectedLackOfRes,
        nbCallsServedByAgent,
        nbCallsServedInTime,
        nbCallsServedTooQuick,
        nbCallsWithoutTransCode,
        nbCallsWithTransCode,
        nbCallsRedistrib,
        nbCallsBeforeTS1,
        percentCallsBeforeTS1,
        nbCallsBeforeTS2,
        percentCallsBeforeTS2,
        nbCallsBeforeTS3,
        percentCallsBeforeTS3,
        nbCallsBeforeTS4,
        percentCallsBeforeTS4,
        nbCallsAfterTS4,
        percentCallsAfterTS4,
        nbAbandonsOnGreetingsVG,
        nbAbandonsOn1WaitingVG,
        nbAbandonsOn2WaitingVG,
        nbAbandonsOn3WaitingVG,
        nbAbandonsOn4WaitingVG,
        nbAbandonsOn5WaitingVG,
        nbAbandonsOn6WaitingVG,
        nbAbandonsOnRinging,
        nbAbandonsOnGenFwdVG,
        nbAbandonsOnBlockedVG,
        nbAbandonsOnAgentBusy,
        nbAbandons,
        nbAbandonsBeforeTS1,
        percentAbandonsBeforeTS1,
        nbAbandonsBeforeTS2,
        percentAbandonsBeforeTS2,
        nbAbandonsBeforeTS3,
        percentAbandonsBeforeTS3,
        nbAbandonsBeforeTS4,
        percentAbandonsBeforeTS4,
        nbAbandonsAfterTS4,
        percentAbandonsAfterTS4,
        callProcTDur,
        callProcADur,
        greetingListenTDur,
        greetingListenADur,
        beforeQueuingTDur,
        waitServedCallsTDur,
        waitServedCallsADur,
        waitAbandonnedCallsTDur,
        waitAbandonnedCallsADur,
        ringingTDur,
        ringingADur,
        convTDur,
        convADur,
        holdCallsTDur,
        holdCallsADur,
        wrapupTDur,
        wrapupADur,
        longestWaitingDur,
        serviceLevel,
        efficiency,
        inServiceState,
        genFwdState,
        blockedState,
        dnbTotReceivedCalls,
        ALL
    }
#pragma warning restore CS1591
}
