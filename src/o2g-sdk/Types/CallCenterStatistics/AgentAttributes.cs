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

using System.Runtime.Serialization;

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Defines the set of statistical attributes available for a CCD agent.
    /// Enum member names match the JSON API field names exactly.
    /// </summary>
#pragma warning disable CS1591
    public enum AgentAttributes
    {
        nbRotating,
        nbPickedUp,
        nbPickup,
        nbLocalOutNonAcd,
        [EnumMember(Value = "nbExtOutNonacd")]
        nbExtOutNonAcd,
        nbRingAcd,
        nbHelp,
        [EnumMember(Value = "nbLocInNonacd")]
        nbLocInNonAcd,
        [EnumMember(Value = "nbExtInNonacdDirect")]
        nbExtInNonAcdDirect,
        [EnumMember(Value = "nbExtInNonacdTransferred")]
        nbExtInNonAcdTransferred,
        nbServedWOCode,
        nbServedWCode,
        nbAcdQuickServed,
        [EnumMember(Value = "nbExtinNonacdServed")]
        nbExtInNonAcdServed,
        [EnumMember(Value = "nbExtinNonacdQuickServed")]
        nbExtInNonAcdQuickServed,
        nbOutAcd,
        nbOutAcdAnswered,
        nbOnWrapup,
        ringAcdServedTDur,
        ringAcdServedADur,
        ringInNonAcdExtServedTDur,
        ringInNonAcdExtServedADur,
        ringAcdTDur,
        ringAcdADur,
        ringInNonAcdExtTDur,
        ringInNonAcdExtADur,
        ringTDur,
        ringADur,
        convAcdTDur,
        convAcdADur,
        wrapupAcdTDur,
        [EnumMember(Value = "convLocOutNonacdTDur")]
        convLocOutNonAcdTDur,
        [EnumMember(Value = "convLocOutNonacdADur")]
        convLocOutNonAcdADur,
        convExtOutTDur,
        convExtOutADur,
        [EnumMember(Value = "convLocInNonacdTDur")]
        convLocInNonAcdTDur,
        [EnumMember(Value = "convLocInNonacdADur")]
        convLocInNonAcdADur,
        [EnumMember(Value = "convExtInNonacdTDur")]
        convExtInNonAcdTDur,
        [EnumMember(Value = "convExtInNonacdADur")]
        convExtInNonAcdADur,
        outAcdCommTDur,
        outAcdCommADur,
        outAcdConvTDur,
        outAcdConvADur,
        outAcdTransactTDur,
        outAcdTransactADur,
        outAcdWrapupTDur,
        outAcdWrapupADur,
        outAcdPauseTDur,
        outAcdPauseADur,
        wrapUpIdleTDur,
        callOnWrapupTDur,
        busyOnWrapupTDur,
        busyTDur,
        loggedOutPerTime,
        notAssignedPerTime,
        assignedPerTime,
        withdrawPerTime,
        withdrawPerTimeCause1,
        withdrawPerTimeCause2,
        withdrawPerTimeCause3,
        withdrawPerTimeCause4,
        withdrawPerTimeCause5,
        withdrawPerTimeCause6,
        withdrawPerTimeCause7,
        withdrawPerTimeCause8,
        withdrawPerTimeCause9,
        nbPilots,
        nbAcdServedCalls,
        nbAcdInServedCalls,
        nbInCallsReceivedByPilot,
        nbAcdOutServedCalls,
        nbTotNonServedCalls,
        nbInNonServedCalls,
        [EnumMember(Value = "nbPickedupCalls")]
        nbPickedUpCalls,
        nbRefusedCalls,
        nbAcdOutNonServedCalls,
        [EnumMember(Value = "nbTotNonAcdreceivedCalls")]
        nbTotNonAcdReceivedCalls,
        nbInNonAcdCalls,
        nbOutNonAcdCalls,
        assignedNotWithdrawDur,
        withdrawDur,
        manuWrapupDur,
        unreachableDur,
        nonAcdWorkTDur,
        nonAcdWorkADur,
        acdWorkTDur,
        acdWorkADur,
        acdWorkInTDur,
        acdWorkInADur,
        acdWorkInConvTDur,
        acdWorkInConvADur,
        acdWorkInRingTDur,
        acdWorkInRingADur,
        acdWorkInWrapupTDur,
        acdWorkInWrapupADur,
        acdWorkOutTDur,
        acdWorkOutADur,
        acdWorkOutConvTDur,
        acdWorkOutConvADur,
        acdWorkOutWrapupTDur,
        acdWorkOutWrapupADur,
        acdInConvTDur,
        acdInConvADur,
        acdOutConvTDur,
        acdOutConvADur,
        ALL
    }
#pragma warning restore CS1591
}
