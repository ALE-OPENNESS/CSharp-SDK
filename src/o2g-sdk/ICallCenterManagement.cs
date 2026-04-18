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

using o2g.Internal.Services;
using o2g.Types.CallCenterManagementNS;
using o2g.Types.CallCenterManagementNS.CalendarNS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>ICallCenterManagement</c> provides operations to configure and manage CCD pilots
    /// and their associated calendars.
    /// <para>
    /// Usage of this service requires a <b>CONTACTCENTER_SERVICE</b> license in CAPEX mode,
    /// or 40 api-tel-f subscriptions in OPEX mode (Purple On Demand).
    /// </para>
    /// <para>
    /// Each CCD pilot has two types of calendars:
    /// <list type="bullet">
    /// <item><description>
    /// <b>Normal calendar</b> — defines standard pilot behaviour for each day of the week.
    /// Each day can have up to 10 transitions (time slots).
    /// </description></item>
    /// <item><description>
    /// <b>Exceptional calendar</b> — defines special days that override the normal calendar
    /// (e.g. holidays). Each exceptional day can also have up to 10 transitions.
    /// </description></item>
    /// </list>
    /// </para>
    /// <para>
    /// A <see cref="Transition"/> represents a time slot in a calendar, including the start
    /// time, the pilot rule to apply, and the pilot operating mode.
    /// </para>
    /// </summary>
    public interface ICallCenterManagement : IService
    {
        /// <summary>
        /// Returns all CCD pilots configured on the specified node.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <returns>
        /// A list of <see cref="Pilot"/> objects, or <see langword="null"/> if no pilots
        /// are configured.
        /// </returns>
        Task<IReadOnlyList<Pilot>> GetPilotsAsync(int nodeId);

        /// <summary>
        /// Returns the CCD pilot with the specified directory number.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <returns>
        /// The <see cref="Pilot"/>, or <see langword="null"/> if not found.
        /// </returns>
        Task<Pilot> GetPilotAsync(int nodeId, string pilotNumber);

        /// <summary>
        /// Returns the full calendar associated with the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <returns>
        /// The <see cref="Calendar"/> of the pilot, or <see langword="null"/> if not found.
        /// </returns>
        Task<Calendar> GetCalendarAsync(int nodeId, string pilotNumber);

        /// <summary>
        /// Returns the exceptional calendar for the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <returns>
        /// The <see cref="ExceptionCalendar"/>, or <see langword="null"/> if not found.
        /// </returns>
        Task<ExceptionCalendar> GetExceptionCalendarAsync(int nodeId, string pilotNumber);

        /// <summary>
        /// Adds a new transition to the exceptional calendar of the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <param name="date">The exceptional day.</param>
        /// <param name="transition">The <see cref="Transition"/> to add.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>Up to 10 transitions can be defined per exceptional day.</remarks>
        Task<bool> AddExceptionTransitionAsync(int nodeId, string pilotNumber,
            DateTime date, Transition transition);

        /// <summary>
        /// Removes a transition from the exceptional calendar of the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <param name="date">The exceptional day.</param>
        /// <param name="transitionIndex">The zero-based index of the transition to remove.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> DeleteExceptionTransitionAsync(int nodeId, string pilotNumber,
            DateTime date, int transitionIndex);

        /// <summary>
        /// Modifies a transition in the exceptional calendar of the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <param name="date">The exceptional day.</param>
        /// <param name="transitionIndex">The zero-based index of the transition to modify.</param>
        /// <param name="transition">The new <see cref="Transition"/> value.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> SetExceptionTransitionAsync(int nodeId, string pilotNumber,
            DateTime date, int transitionIndex, Transition transition);

        /// <summary>
        /// Returns the normal calendar for the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <returns>
        /// The <see cref="NormalCalendar"/>, or <see langword="null"/> if not found.
        /// </returns>
        Task<NormalCalendar> GetNormalCalendarAsync(int nodeId, string pilotNumber);

        /// <summary>
        /// Adds a new transition to the normal calendar of the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <param name="day">The day of the week to which the transition applies.</param>
        /// <param name="transition">The <see cref="Transition"/> to add.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <remarks>Up to 10 transitions can be defined per day.</remarks>
        Task<bool> AddNormalTransitionAsync(int nodeId, string pilotNumber,
            DayOfWeek day, Transition transition);

        /// <summary>
        /// Removes a transition from the normal calendar of the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <param name="day">The day of the week from which to remove the transition.</param>
        /// <param name="transitionIndex">The zero-based index of the transition to remove.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> DeleteNormalTransitionAsync(int nodeId, string pilotNumber,
            DayOfWeek day, int transitionIndex);

        /// <summary>
        /// Modifies a transition in the normal calendar of the specified CCD pilot.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <param name="day">The day of the week of the transition.</param>
        /// <param name="transitionIndex">The zero-based index of the transition to modify.</param>
        /// <param name="transition">The new <see cref="Transition"/> value.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        Task<bool> SetNormalTransitionAsync(int nodeId, string pilotNumber,
            DayOfWeek day, int transitionIndex, Transition transition);

        /// <summary>
        /// Forces the specified CCD pilot into the open state, regardless of its calendar schedule.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ClosePilotAsync(int, string)"/>
        Task<bool> OpenPilotAsync(int nodeId, string pilotNumber);

        /// <summary>
        /// Forces the specified CCD pilot into the closed state, regardless of its calendar schedule.
        /// </summary>
        /// <param name="nodeId">The OmniPCX Enterprise node identifier.</param>
        /// <param name="pilotNumber">The pilot directory number.</param>
        /// <returns><see langword="true"/> on success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="OpenPilotAsync(int, string)"/>
        Task<bool> ClosePilotAsync(int nodeId, string pilotNumber);
    }
}
