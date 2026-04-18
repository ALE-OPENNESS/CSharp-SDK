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
    /// Defines the set of statistical attributes for a CCD agent with respect to a specific pilot.
    /// Enum member names match the JSON API field names exactly.
    /// </summary>
#pragma warning disable CS1591
    public enum AgentByPilotAttributes
    {
        nbCallsReceived,
        nbCallsTransfIn,
        nbCallsServed,
        nbCallsServedTooQuickly,
        nbCallsWithEnquiry,
        nbCallsWithHelp,
        nbCallsTransf,
        nbCallsTransfToAgent,
        nbCallsInWrapup,
        maxCallProcDur,
        maxConvDur,
        maxWrapupDur,
        callProcTDur,
        callProcADur,
        convTDur,
        convADur,
        wrapupTDur,
        wrapupADur,
        convInWrapupTDur,
        busyTimeInWrapupTDur,
        onHoldTDur,
        onHoldADur,
        transTDur,
        transADur,
        pauseTDur,
        pauseADur,
        ALL
    }
#pragma warning restore CS1591
}
