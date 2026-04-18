/*
* Copyright 2022 ALE International
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


using o2g.Types.CommonNS;

namespace o2g.Events.CallCenterRealtimeNS
{
    /// <summary>
    /// <c>OnPGOtherRtiChangedEvent</c> represents the event that is triggered
    /// periodically when the processing group set defined in the <see cref="o2g.Types.CallCenterRealtimeNS.RtiContext">RtiContext</see> has real-time information updates.
    /// </summary>
    /// <para>
    /// It contains details about the CCD processing group that are monitored in the real-time
    /// context and whose state or attributes have changed.
    /// </para>
    /// <remarks>
    /// <para>Since: 2.7.4</para>
    /// </remarks>   
    public class OnPGOtherRtiChangedEvent : O2GEvent
    {
        /// <summary>
        /// Gets the name of the processing group.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the directory number of the processing group.
        /// </summary>
        public string Number { get; init; }

        /// <summary>
        /// Gets the type of the processing group.
        /// </summary>
        public OtherProcessingGroupType Type { get; init; }

        /// <summary>
        /// Gets the current service state of the processing group.
        /// </summary>
        public ServiceState State { get; init; }

        /// <summary>
        /// Gets the number of ACD calls in the processing group.
        /// </summary>
        public int NbOfACDCalls { get; init; }

        /// <summary>
        /// Gets the number of incoming calls during the last minute.
        /// </summary>
        public int IncomingTraffic { get; init; }

        /// <summary>
        /// Gets the CCD key.
        /// </summary>
        public int AfeKey { get; init; }
    }
}
