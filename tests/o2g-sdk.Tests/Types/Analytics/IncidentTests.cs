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

namespace o2g.Tests.Types.Analytics
{
    public class IncidentTests : JsonTestBase
    {
        private const string FullIncident = """
            {
                "date": "26/02/24",
                "hour": "15:30:00",
                "severity": 2,
                "value": "1042",
                "type": "Link failure",
                "nbOccurs": 3,
                "node": "1",
                "main": true,
                "rack": "R1",
                "board": "B2",
                "equipement": "E3",
                "termination": "T4"
            }
            """;

        private const string MinimalIncident = """
            {
                "date": "01/03/24",
                "hour": "08:00:00",
                "severity": 0,
                "value": "500",
                "type": "Minor fault",
                "nbOccurs": 1,
                "node": "2",
                "main": false
            }
            """;

        #region Deserialization

        [Fact]
        public void Deserialize_FullIncident_MapsAllProperties()
        {
            var o2g = Deserialize<O2GIncident>(FullIncident);

            o2g.Date.Should().Be("26/02/24");
            o2g.Hour.Should().Be("15:30:00");
            o2g.Severity.Should().Be(2);
            o2g.Value.Should().Be("1042");
            o2g.Type.Should().Be("Link failure");
            o2g.NbOccurs.Should().Be(3);
            o2g.Node.Should().Be("1");
            o2g.Main.Should().BeTrue();
            o2g.Rack.Should().Be("R1");
            o2g.Board.Should().Be("B2");
            o2g.Equipement.Should().Be("E3");
            o2g.Termination.Should().Be("T4");
        }

        [Fact]
        public void Deserialize_MinimalIncident_NullablePropertiesAreNull()
        {
            var o2g = Deserialize<O2GIncident>(MinimalIncident);

            o2g.Rack.Should().BeNull();
            o2g.Board.Should().BeNull();
            o2g.Equipement.Should().BeNull();
            o2g.Termination.Should().BeNull();
        }

        #endregion

        #region ToIncident

        [Fact]
        public void ToIncident_MapsAllProperties()
        {
            var incident = Deserialize<O2GIncident>(FullIncident).ToIncident();

            incident.Id.Should().Be(1042);
            incident.Severity.Should().Be(2);
            incident.Description.Should().Be("Link failure");
            incident.NbOccurs.Should().Be(3);
            incident.Node.Should().Be(1);
            incident.Main.Should().BeTrue();
            incident.Rack.Should().Be("R1");
            incident.Board.Should().Be("B2");
            incident.Equipment.Should().Be("E3");
            incident.Termination.Should().Be("T4");
        }

        [Fact]
        public void ToIncident_ParsesDateCorrectly()
        {
            var incident = Deserialize<O2GIncident>(FullIncident).ToIncident();

            incident.Date.Should().Be(new DateTime(2024, 2, 26, 15, 30, 0));
        }

        [Fact]
        public void ToIncident_DateWithLeadingSpaces_ParsesCorrectly()
        {
            var json = """
                {
                    "date": " 26/02/24",
                    "hour": " 15:30:00",
                    "severity": 1,
                    "value": "100",
                    "type": "Test",
                    "nbOccurs": 1,
                    "node": "1",
                    "main": false
                }
                """;

            var incident = Deserialize<O2GIncident>(json).ToIncident();

            incident.Date.Should().Be(new DateTime(2024, 2, 26, 15, 30, 0));
        }

        [Fact]
        public void ToIncident_MinimalIncident_NullablePropertiesAreNull()
        {
            var incident = Deserialize<O2GIncident>(MinimalIncident).ToIncident();

            incident.Rack.Should().BeNull();
            incident.Board.Should().BeNull();
            incident.Equipment.Should().BeNull();
            incident.Termination.Should().BeNull();
        }

        [Fact]
        public void ToIncident_MidnightHour_ParsesCorrectly()
        {
            var json = """
                {
                    "date": "01/01/24",
                    "hour": "00:00:00",
                    "severity": 0,
                    "value": "1",
                    "type": "Test",
                    "nbOccurs": 1,
                    "node": "1",
                    "main": false
                }
                """;

            var incident = Deserialize<O2GIncident>(json).ToIncident();

            incident.Date.Should().Be(new DateTime(2024, 1, 1, 0, 0, 0));
        }

        #endregion
    }
}