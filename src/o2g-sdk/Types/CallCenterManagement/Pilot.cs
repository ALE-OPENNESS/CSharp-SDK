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

using o2g.Events.CallCenterRealtimeNS;
using o2g.Types.CommonNS;
using o2g.Types.TelephonyNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using System.Collections.Generic;

namespace o2g.Types.CallCenterManagementNS
{
    /// <summary>
    /// Represents an Automatic Call Distributor (ACD) pilot.
    /// <para>
    /// A pilot is a single entry point into the call center distribution system.
    /// It is used to organize and manage call distribution for a specific type
    /// of service, connecting pilots, queues, and resources (agents or agent groups)
    /// and allowing calls to be prioritized and routed according to configured rules.
    /// </para>
    /// </summary>
    /// <seealso cref="ICallCenterPilot"/>
    public class Pilot
    {
        /// <summary>
        /// The directory number of this pilot.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the pilot's directory number,
        /// or <see langword="null"/> if not available.
        /// </value>
        public string Number { get; init; }

        /// <summary>
        /// The display name of this pilot.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the pilot name,
        /// or <see langword="null"/> if not available.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// The current high-level service state of this pilot.
        /// </summary>
        /// <value>
        /// A <see cref="ServiceState"/> value indicating whether the pilot is
        /// open, closed, or blocked, or <see langword="null"/> if not available.
        /// </value>
        public ServiceState? State { get; init; }

        /// <summary>
        /// The detailed service status of this pilot.
        /// </summary>
        /// <value>
        /// A <see cref="PilotStatus"/> value providing a more granular view of
        /// the pilot state (e.g. blocked on rule, in general forwarding),
        /// or <see langword="null"/> if not available.
        /// </value>
        public PilotStatus? DetailedState { get; init; }

        /// <summary>
        /// The expected maximum waiting time for a call on this pilot.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> representing the waiting time in seconds.
        /// </value>
        public int WaitingTime { get; init; }

        /// <summary>
        /// Whether the call queues for this pilot are currently saturated.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the queues are saturated and unable to receive
        /// additional calls; <see langword="false"/> otherwise.
        /// </value>
        public bool Saturation { get; init; }

        /// <summary>
        /// The routing rules associated with this pilot.
        /// </summary>
        /// <value>
        /// A <see cref="PilotRuleSet"/> containing the routing rules that define
        /// how calls are distributed on this pilot, or <see langword="null"/> if
        /// no rules are configured.
        /// </value>
        /// <seealso cref="PilotRuleSet"/>
        public PilotRuleSet Rules { get; init; }

        /// <summary>
        /// Whether transferring a call to this pilot is possible.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if call transfer to this pilot is possible;
        /// <see langword="false"/> otherwise.
        /// </value>
        public bool PossibleTransfer { get; init; }

        /// <summary>
        /// Whether a supervised transfer to this pilot is allowed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if supervised transfer is possible;
        /// <see langword="false"/> otherwise.
        /// </value>
        /// <seealso cref="ITelephony.MakePilotOrRSISupervisedTransferCallAsync(string, string, CorrelatorData, CallProfile, string)"/>
        public bool SupervisedTransfer { get; init; }
    }
}
