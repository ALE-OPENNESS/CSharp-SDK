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
using o2g.Types.CallCenterRealtimeNS;

namespace o2g_sdk.Tests.Types.CallCenterRealtime
{
    public class RtiObjectsTests : JsonTestBase
    {
        #region JSON fixtures

        private const string RtiObjectJson = """
        {
            "agents": [
                { "number" : "12000", "name": "ag12000", "firstName" : "ag-fist12000" },
                { "number" : "12001", "name": "ag12001", "firstName" : "ag-fist12001" },
                { "number" : "12002", "name": "ag12002" }
            ],
            "pilots": [
                { "number" : "15000", "name" : "pil15000" },
                { "number" : "15001" }
            ],
            "queues": [
                { "number" : "16000" },
                { "number" : "16001" }
            ],
            "pgAgents": [
                { "number" : "17000" },
                { "number" : "17001" }
            ],
            "pgOthers": [
                { "number" : "17002" },
                { "number" : "17003" }
            ]
        }
        """;

        private const string RtiObjectMissingQueuesJson = """
        {
            "agents": [
                { "number" : "12000", "name": "ag12000", "firstName" : "ag-fist12000" },
                { "number" : "12001", "name": "ag12001", "firstName" : "ag-fist12001" },
                { "number" : "12002", "name": "ag12002" }
            ],
            "pilots": [
                { "number" : "15000", "name" : "pil15000" },
                { "number" : "15001" }
            ]
        }
        """;

        #endregion

        [Fact]
        public void Deserialize_RtiObjectMissingQueuesJson()
        {
            var rtiObjects = Deserialize<RtiObjects>(RtiObjectMissingQueuesJson);

            rtiObjects.Should().NotBeNull();

            rtiObjects.Pilots.Should().NotBeNull();
            rtiObjects.Pilots.Count.Should().Be(2);

            rtiObjects.Agents.Should().NotBeNull();
            rtiObjects.Agents.Count.Should().Be(3);

            rtiObjects.Queues.Should().BeNull();
            rtiObjects.AgentProcessingGroups.Should().BeNull();
            rtiObjects.OtherProcessingGroups.Should().BeNull();
        }

        [Fact]
        public void Deserialize_RtiObjectJson()
        {
            var rtiObjects = Deserialize<RtiObjects>(RtiObjectJson);

            rtiObjects.Should().NotBeNull();

            rtiObjects.Pilots.Should().NotBeNull();
            rtiObjects.Pilots.Count.Should().Be(2);

            rtiObjects.Pilots[0].Number.Should().Be("15000");
            rtiObjects.Pilots[0].Name.Should().Be("pil15000");
            rtiObjects.Pilots[1].Number.Should().Be("15001");
            rtiObjects.Pilots[1].Name.Should().BeNull();

            rtiObjects.Agents.Should().NotBeNull();
            rtiObjects.Agents.Count.Should().Be(3);

            rtiObjects.Agents[0].Number.Should().Be("12000");
            rtiObjects.Agents[0].Name.Should().Be("ag12000");
            rtiObjects.Agents[0].FirstName.Should().Be("ag-fist12000");
            rtiObjects.Agents[1].Number.Should().Be("12001");
            rtiObjects.Agents[1].Name.Should().Be("ag12001");
            rtiObjects.Agents[1].FirstName.Should().Be("ag-fist12001");

            rtiObjects.Queues.Should().NotBeNull();
            rtiObjects.Queues.Count.Should().Be(2);

            rtiObjects.Queues[0].Number.Should().Be("16000");
            rtiObjects.Queues[0].Name.Should().BeNull();

            rtiObjects.AgentProcessingGroups.Should().NotBeNull();
            rtiObjects.AgentProcessingGroups.Count.Should().Be(2);

            rtiObjects.AgentProcessingGroups[0].Number.Should().Be("17000");
            rtiObjects.AgentProcessingGroups[0].Name.Should().BeNull();

            rtiObjects.OtherProcessingGroups.Should().NotBeNull();
            rtiObjects.OtherProcessingGroups.Count.Should().Be(2);

            rtiObjects.OtherProcessingGroups[0].Number.Should().Be("17002");
            rtiObjects.OtherProcessingGroups[0].Name.Should().BeNull();
        }
    }
}
