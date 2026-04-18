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

using FluentAssertions;
using o2g.Tests.Helpers;
using o2g.Types.CallCenterAgentNS;
using Xunit;

namespace o2g.Tests.Types.CallCenterAgent
{
    public class OperatorStateTests : JsonTestBase
    {
        private const string LoggedOnState = """
            {
                "mainState": "LOG_ON",
                "subState": "READY",
                "proAcdDeviceNumber": "1000",
                "pgNumber": "PG001",
                "withdraw": false
            }
            """;

        private const string WithdrawState = """
            {
                "mainState": "LOG_ON",
                "subState": "WITHDRAW",
                "proAcdDeviceNumber": "1000",
                "pgNumber": "PG001",
                "withdraw": true,
                "withdrawReason": 2
            }
            """;

        private const string LoggedOffState = """
            {
                "mainState": "LOG_OFF",
                "withdraw": false
            }
            """;

        #region MainState

        [Fact]
        public void Deserialize_LoggedOn_MapsMainState()
        {
            var state = Deserialize<OperatorState>(LoggedOnState);

            state.MainState.Should().Be(OperatorState.OperatorMainState.Logon);
        }

        [Fact]
        public void Deserialize_LoggedOff_MapsMainState()
        {
            var state = Deserialize<OperatorState>(LoggedOffState);

            state.MainState.Should().Be(OperatorState.OperatorMainState.Logoff);
        }

        [Fact]
        public void Deserialize_UnknownMainState_FallsBackToUnknown()
        {
            var json = """
                {
                    "mainState": "SOME_FUTURE_VALUE",
                    "withdraw": false
                }
                """;

            var state = Deserialize<OperatorState>(json);

            state.MainState.Should().Be(OperatorState.OperatorMainState.Unknown);
        }

        #endregion

        #region SubState

        [Fact]
        public void Deserialize_LoggedOn_MapsSubState()
        {
            var state = Deserialize<OperatorState>(LoggedOnState);

            state.SubState.Should().Be(OperatorState.OperatorDynamicState.Ready);
        }

        [Fact]
        public void Deserialize_LoggedOff_SubStateIsNull()
        {
            var state = Deserialize<OperatorState>(LoggedOffState);

            state.SubState.Should().BeNull();
        }

        [Fact]
        public void Deserialize_WrapupEmail_MapsSubState()
        {
            var json = """
                {
                    "mainState": "LOG_ON",
                    "subState": "WRAPUP_EMAIL",
                    "withdraw": false
                }
                """;

            var state = Deserialize<OperatorState>(json);

            state.SubState.Should().Be(OperatorState.OperatorDynamicState.WrapupEmail);
        }

        [Fact]
        public void Deserialize_UnknownSubState_FallsBackToUnknown()
        {
            var json = """
                {
                    "mainState": "LOG_ON",
                    "subState": "SOME_FUTURE_VALUE",
                    "withdraw": false
                }
                """;

            var state = Deserialize<OperatorState>(json);

            state.SubState.Should().Be(OperatorState.OperatorDynamicState.Unknown);
        }

        #endregion

        #region Scalar properties

        [Fact]
        public void Deserialize_LoggedOn_MapsAllProperties()
        {
            var state = Deserialize<OperatorState>(LoggedOnState);

            state.ProAcdDeviceNumber.Should().Be("1000");
            state.PgNumber.Should().Be("PG001");
            state.Withdraw.Should().BeFalse();
            state.WithdrawReason.Should().BeNull();
        }

        [Fact]
        public void Deserialize_WithdrawState_MapsWithdrawProperties()
        {
            var state = Deserialize<OperatorState>(WithdrawState);

            state.SubState.Should().Be(OperatorState.OperatorDynamicState.Withdraw);
            state.Withdraw.Should().BeTrue();
            state.WithdrawReason.Should().Be(2);
        }

        [Fact]
        public void Deserialize_LoggedOff_NullablePropertiesAreNull()
        {
            var state = Deserialize<OperatorState>(LoggedOffState);

            state.SubState.Should().BeNull();
            state.ProAcdDeviceNumber.Should().BeNull();
            state.PgNumber.Should().BeNull();
            state.WithdrawReason.Should().BeNull();
        }

        #endregion

        #region All dynamic states

        [Theory]
        [InlineData("READY", OperatorState.OperatorDynamicState.Ready)]
        [InlineData("OUT_OF_PG", OperatorState.OperatorDynamicState.OutOfProcessingGroup)]
        [InlineData("BUSY", OperatorState.OperatorDynamicState.Busy)]
        [InlineData("TRANSACTION_CODE_INPUT", OperatorState.OperatorDynamicState.TransactionCodeInput)]
        [InlineData("WRAPUP", OperatorState.OperatorDynamicState.Wrapup)]
        [InlineData("PAUSE", OperatorState.OperatorDynamicState.Pause)]
        [InlineData("WITHDRAW", OperatorState.OperatorDynamicState.Withdraw)]
        [InlineData("WRAPUP_IM", OperatorState.OperatorDynamicState.WrapupIm)]
        [InlineData("WRAPUP_EMAIL", OperatorState.OperatorDynamicState.WrapupEmail)]
        [InlineData("WRAPUP_EMAIL_INTERRUPTIBLE", OperatorState.OperatorDynamicState.WrapupEmailInterruptible)]
        [InlineData("WRAPUP_OUTBOUND", OperatorState.OperatorDynamicState.WrapupOutbound)]
        [InlineData("WRAPUP_CALLBACK", OperatorState.OperatorDynamicState.WrapupCallback)]
        public void Deserialize_AllDynamicStates_MappedCorrectly(
            string jsonValue, OperatorState.OperatorDynamicState expected)
        {
            var json = $$"""
                {
                    "mainState": "LOG_ON",
                    "subState": "{{jsonValue}}",
                    "withdraw": false
                }
                """;

            var state = Deserialize<OperatorState>(json);

            state.SubState.Should().Be(expected);
        }

        #endregion
    }
}
