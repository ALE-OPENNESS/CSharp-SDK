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

using o2g.Types.CommonNS;

namespace o2g.Types.CommunicationLogNS
{
    /// <summary>
    /// <c>ComRecordParticipant</c> represents a participant referenced in a com record.
    /// </summary>
    /// <remarks>
    /// <h3>Record content</h3>
    /// <para>
    /// For a <b>simple call</b> (user A calls user B), the call record on each party will contain <u>both the participant A and 
    /// the participant B</u>: (no guaranty on the order of the participants on 2 successive responses).
    /// <list type="table">
    /// <listheader><term>Side</term><description>Com record content</description></listheader>
    /// <item>
    /// <term>On A side</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>Participant A (the owner of the record) with <c>role=Caller</c> and the answered status (whether the call has been answered or not)</description></item>
    /// <item><description>Participant B : neither the role nor the state is provided</description></item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <term>On B side</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>Participant A : neither the role nor the state is provided</description></item>
    /// <item><description>Participant B (the owner of the record) with <c>role=Callee</c> and the answered status (whether the call has been answered or not)</description></item>
    /// </list>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// For a <b>re-routed call</b> (user A calls user B, the call is re-routed on user C, caused by overflow, redirection or pickup), 
    /// the record generated on each side including the user B side (the "victim" of the re-routing) will not contain as participant 
    /// the user B itself because he was not the last destination of the call:
    /// <list type="table">
    /// <listheader><term>Side</term><description>Com record content</description></listheader>
    /// <item>
    /// <term>On A side</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>Participant A : with <c>role=Caller</c>, the answered status, initialCalled=B</description></item>
    /// <item><description>Participant C</description></item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <term>On B side</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>Participant A : neither the role nor the state is provided</description></item>
    /// <item><description>Participant C with <c>role=Callee</c>, the answered status (whether the call has been answered or not) and initialCalled=B 
    /// which tells B that he was called by A but the call has been rerouted to C(with a reason) which answered or not to the call</description></item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <term>On C side</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>Participant A : neither the role nor the state is provided</description></item>
    /// <item><description>Participant C with <c>role=Callee</c>, the answered status (whether the call has been answered or not) and initialCalled=B</description></item>
    /// </list>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// Furthermore, for a multi-parties call using addParticipant, the already connected users in the call will also received a call record which
    /// will contain the answered status of the added participant: this information is provided to distinguish the added participants which have 
    /// really answered and the other which decline the call.
    /// </para>
    /// 
    /// <h3>Identification of the participant</h3>
    /// <para>
    /// In the comlog notification events, the participant owner is identified only by its loginName (in order to reduce the event call flow), 
    /// the other participants are identified with their full identity (loginName, phoneNumber).
    /// </para>
    /// <para>
    /// In a <see cref="QueryResult"/> result:
    /// <list type="bullet">
    /// <item><description>
    /// if no optimization is asked, all the participants are identified with their full identity.
    /// </description></item>
    /// <item><description>
    /// If the <c>optimized</c> parameter is set to <see langword="true"/>, only the first occurence of a participant (owner or other) is identified with its 
    /// full identity. The following occurences are identified only with the phonenumber.
    /// </description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ComRecordParticipant
    {
        /// <summary>
        /// Return whether the participant was the caller party, a called party or a masterconf.
        /// </summary>
        /// <value>
        /// A <see cref="Role"/> value that represents the participant role.
        /// </value>
        /// <remarks>
        /// In a conference, a participant can be both a called party and a caller (consultation call plus conference following an incoming call).
        /// </remarks>
        public Role Role { get; init; }

        /// <summary>
        /// Return whether the participant has really entered in the conversation.
        /// </summary>
        /// <value>
        /// A <see langword="bool"/> value that indicates if the participant has answered the call, or a <see langword="null"/> value 
        /// if it has been impossible to identify wheter the participant has answered.
        /// </value>
        public bool? Answered { get; init; }

        /// <summary>
        /// Return this participant identity.
        /// </summary>
        /// <value>
        /// A <see cref="PartyInfo"/> that is the participant identity.
        /// </value>
        public PartyInfo Identity { get; init; }

        /// <summary>
        /// Return whether the participant identity is secret.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the participant is anonymous; <see langword="false"/> otherwise.
        /// </value>
        public bool Anonymous { get; init; }

        /// <summary>
        /// Return the number that has been initially called when this participant has been entered in the call.
        /// </summary>
        /// <value>
        /// A <see cref="PartyInfo"/> that is the initial participant identity.
        /// </value>
        public PartyInfo InitialCalled { get; init; }

        /// <summary>
        /// Return the reason why the call has been established, rerouted, terminated.
        /// </summary>
        /// <value>
        /// A <see cref="Reason"/> value that represents the reason the call has been established, rerouted, terminated.
        /// </value>
        public Reason Reason { get; init; }
    }
}
