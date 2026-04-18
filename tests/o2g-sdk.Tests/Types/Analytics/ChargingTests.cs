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
using o2g.Internal.Types.Analytics;
using o2g.Tests.Helpers;
using o2g.Types.AnalyticsNS;
using System;
using Xunit;

namespace o2g.Tests.Types.Analytics
{
    public class ChargingTests : JsonTestBase
    {
        private const string FullCharging = """
            {
                "caller": "1234",
                "name": "John Doe",
                "called": "5678",
                "initialDialledNumber": "91011",
                "callNumber": 2,
                "chargingUnits": 50,
                "cost": 12.5,
                "startDate": "20240226 15:30:00",
                "duration": 120,
                "callType": "LocalNode",
                "effectiveCallDuration": 115,
                "actingExtensionNumberNode": 42,
                "internalFacilities": { "facilities": ["BasicCall", "CallForwardingOnBusy"] },
                "externalFacilities": { "facilities": ["CallWaiting"] }
            }
            """;

        private const string MinimalCharging = """
            {
                "caller": "1234",
                "duration": 60,
                "effectiveCallDuration": 55,
                "actingExtensionNumberNode": 1
            }
            """;

        #region Scalar properties

        [Fact]
        public void ToCharging_FullCharging_MapsScalarProperties()
        {
            var charging = Deserialize<O2GCharging>(FullCharging).ToCharging();

            charging.Caller.Should().Be("1234");
            charging.Name.Should().Be("John Doe");
            charging.Called.Should().Be("5678");
            charging.InitialDialedNumber.Should().Be("91011");
            charging.CallNumber.Should().Be(2);
            charging.ChargingUnits.Should().Be(50);
            charging.Cost.Should().BeApproximately(12.5f, 0.001f);
            charging.Duration.Should().Be(120);
            charging.EffectiveCallDuration.Should().Be(115);
            charging.ActingExtensionNumberNode.Should().Be(42);
        }

        [Fact]
        public void ToCharging_FullCharging_MapsStartDate()
        {
            var charging = Deserialize<O2GCharging>(FullCharging).ToCharging();

            charging.StartDate.Should().Be(new DateTime(2024, 2, 26, 15, 30, 0));
        }

        [Fact]
        public void ToCharging_FullCharging_MapsCallType()
        {
            var charging = Deserialize<O2GCharging>(FullCharging).ToCharging();

            charging.CallType.Should().Be(CallType.LocalNode);
        }

        #endregion

        #region Facilities

        [Fact]
        public void ToCharging_FullCharging_MapsInternalFacilities()
        {
            var charging = Deserialize<O2GCharging>(FullCharging).ToCharging();

            charging.InternalFacilities.Should().HaveCount(2);
            charging.InternalFacilities.Should().Contain(TelFacility.BasicCall);
            charging.InternalFacilities.Should().Contain(TelFacility.CallForwardingOnBusy);
        }

        [Fact]
        public void ToCharging_FullCharging_MapsExternalFacilities()
        {
            var charging = Deserialize<O2GCharging>(FullCharging).ToCharging();

            charging.ExternalFacilities.Should().HaveCount(1);
            charging.ExternalFacilities.Should().Contain(TelFacility.CallWaiting);
        }

        #endregion

        #region Minimal / null properties

        [Fact]
        public void ToCharging_MinimalCharging_NullablePropertiesAreNull()
        {
            var charging = Deserialize<O2GCharging>(MinimalCharging).ToCharging();

            charging.Name.Should().BeNull();
            charging.Called.Should().BeNull();
            charging.StartDate.Should().BeNull();
            charging.InitialDialedNumber.Should().BeNull();
            charging.InternalFacilities.Should().BeNull();
            charging.ExternalFacilities.Should().BeNull();
        }

        [Fact]
        public void ToCharging_MinimalCharging_NumericDefaultsAreZero()
        {
            var charging = Deserialize<O2GCharging>(MinimalCharging).ToCharging();

            charging.CallNumber.Should().Be(0);
            charging.ChargingUnits.Should().Be(0);
            charging.Cost.Should().Be(0f);
        }

        #endregion

        #region Enum fallback

        [Fact]
        public void ToCharging_UnknownCallType_FallsBackToUnspecified()
        {
            var json = """
                {
                    "caller": "1234",
                    "duration": 60,
                    "callType": "SOME_FUTURE_VALUE"
                }
                """;

            var charging = Deserialize<O2GCharging>(json).ToCharging();

            charging.CallType.Should().Be(CallType.Unspecified);
        }

        [Fact]
        public void ToCharging_UnknownTelFacility_FallsBackToNone()
        {
            var json = """
                {
                    "caller": "1234",
                    "duration": 60,
                    "internalFacilities": { "facilities": ["BasicCall", "SOME_FUTURE_VALUE"] }
                }
                """;

            var charging = Deserialize<O2GCharging>(json).ToCharging();

            charging.InternalFacilities.Should().HaveCount(2);
            charging.InternalFacilities.Should().Contain(TelFacility.BasicCall);
            charging.InternalFacilities.Should().Contain(TelFacility.None);
        }

        #endregion
    }
}

