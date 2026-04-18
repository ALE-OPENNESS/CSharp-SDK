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

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterAgentNS
{
    /// <summary>
    /// <c>OperatorState</c> represents the state of a CCD operator. 
    /// </summary>
    public class OperatorState
    {
        /// <summary>
        /// <c>OperatorMainState</c> represents the login, logoff status of a CCD operator.
        /// </summary>
        /// <seealso cref="ICallCenterAgent.LogonAsync(string, string, bool, string)"/>
        /// <seealso cref="ICallCenterAgent.LogoffAsync(string)"/>
        [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: OperatorMainState.Unknown)]
        public enum OperatorMainState
        {
            /// <summary>
            /// The O2G server is unable to get the operator main state.
            /// </summary>
            [EnumMember(Value = "UNKNOWN")]
            Unknown,

            /// <summary>
            /// The operator is logged on a pro-acd set.
            /// </summary>
            [EnumMember(Value = "LOG_ON")]
            Logon,

            /// <summary>
            /// The operator is logged off.
            /// </summary>
            [EnumMember(Value = "LOG_OFF")]
            Logoff,

            /// <summary>
            /// Error status.
            /// </summary>
            [EnumMember(Value = "ERROR")]
            Error
        }

        /// <summary>
        /// <c>AgentDynamicState</c> represents the CCD operator dynamic state.
        /// </summary>
        [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: OperatorDynamicState.Unknown)]
        public enum OperatorDynamicState
        {
            /// <summary>
            /// The operator is ready.
            /// </summary>
            [EnumMember(Value = "READY")]
            Ready,

            /// <summary>
            /// The operator is logged but out of an agent group.
            /// </summary>
            [EnumMember(Value = "OUT_OF_PG")]
            OutOfProcessingGroup,

            /// <summary>
            /// The operator is busy.
            /// </summary>
            [EnumMember(Value = "BUSY")]
            Busy,

            /// <summary>
            /// The operator is in the transaction code phase.
            /// </summary>
            [EnumMember(Value = "TRANSACTION_CODE_INPUT")]
            TransactionCodeInput,

            /// <summary>
            /// The operator is in the automatic wrapup phase.
            /// </summary>
            [EnumMember(Value = "WRAPUP")]
            Wrapup,

            /// <summary>
            /// The operator is in pause.
            /// </summary>
            [EnumMember(Value = "PAUSE")]
            Pause,

            /// <summary>
            /// The operator is in withdraw from the call distribution.
            /// </summary>
            [EnumMember(Value = "WITHDRAW")]
            Withdraw,

            /// <summary>
            /// The operator is in withdraw from the call distribution because he is treating an IM.
            /// </summary>
            [EnumMember(Value = "WRAPUP_IM")]
            WrapupIm,

            /// <summary>
            /// The operator is in withdraw from the call distribution because he is treating an email.
            /// </summary>
            [EnumMember(Value = "WRAPUP_EMAIL")]
            WrapupEmail,

            /// <summary>
            /// The operator is in withdraw from the call distribution because he is treating an email, 
            /// nevertheless, a CCD call can be distributed on this operator.
            /// </summary>
            [EnumMember(Value = "WRAPUP_EMAIL_INTERRUPTIBLE")]
            WrapupEmailInterruptible,

            /// <summary>
            /// The operator is in wrapup after an outbound call.
            /// </summary>
            [EnumMember(Value = "WRAPUP_OUTBOUND")]
            WrapupOutbound,

            /// <summary>
            /// The operator is in wrapup after a callback call.
            /// </summary>
            [EnumMember(Value = "WRAPUP_CALLBACK")]
            WrapupCallback,

            /// <summary>
            /// Unknown operator dynamic state.
            /// </summary>
            Unknown
        }

        /// <summary>
        /// The static login/logoff state of this operator.
        /// </summary>
        /// <value>
        /// An <see cref="OperatorMainState"/> value indicating whether the operator
        /// is logged on, logged off, or in error state.
        /// </value>
        /// <seealso cref="ICallCenterAgent.LogonAsync(string, string, bool, string)"/>
        /// <seealso cref="ICallCenterAgent.LogoffAsync(string)"/>
        public OperatorMainState MainState { get; init; }

        /// <summary>
        /// The dynamic activity state of this operator.
        /// </summary>
        /// <value>
        /// An <see cref="OperatorDynamicState"/> value indicating the operator's current
        /// activity (ready, busy, wrapup, pause, etc.), or <see langword="null"/> if the
        /// operator is logged off.
        /// </value>
        public OperatorDynamicState? SubState { get; init; }

        /// <summary>
        /// The pro-ACD station extension number this operator is logged on.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the pro-ACD device extension number,
        /// or <see langword="null"/> if the operator is logged off.
        /// </value>
        public string ProAcdDeviceNumber { get; init; }

        /// <summary>
        /// The processing group this operator is currently entered in.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the processing group number,
        /// or <see langword="null"/> if the operator is not entered in any group.
        /// </value>
        public string PgNumber { get; init; }

        /// <summary>
        /// The index of the withdraw reason currently applied to this operator.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that is the withdraw reason index within the
        /// agent group's configured withdraw reasons, or <see langword="null"/> if
        /// the operator is not in withdraw state.
        /// </value>
        /// <seealso cref="ICallCenterAgent.GetWithdrawReasonsAsync(string, string)"/>
        /// <seealso cref="ICallCenterAgent.SetWithdrawAsync(WithdrawReason, string)"/>
        public int? WithdrawReason { get; init; }

        /// <summary>
        /// Whether the operator is currently withdrawn from call distribution.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the operator is in withdraw state;
        /// <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="ICallCenterAgent.SetWithdrawAsync(WithdrawReason, string)"/>
        public bool Withdraw { get; init; }
    }
}
