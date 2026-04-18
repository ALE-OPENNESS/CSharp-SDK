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

using o2g.Internal.Services;
using o2g.Types.CommonNS;
using o2g.Types.PhoneSetProgrammingNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>IPhoneSetProgramming</c> allows managing the programmable keys, soft keys, and device settings
    /// of the phone sets assigned to a user.
    /// Using this service requires having a <b>API_PHONESETPROG</b> license.
    /// </summary>
    /// <remarks>
    /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, but it is mandatory if the session has been opened by an administrator.
    /// </remarks>
    public interface IPhoneSetProgramming : IService
    {
        /// <summary>
        /// Gets the list of devices assigned to the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <returns>
        /// A list of <see cref="Device"/> objects representing the user's devices, or <see langword="null"/> in case of error.
        /// </returns>
        Task<List<Device>> GetDevicesAsync(string loginName);

        /// <summary>
        /// Gets the information of a specific device assigned to the specified user.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns>
        /// A <see cref="Device"/> representing the specified device, or <see langword="null"/> in case of error or if the user does not have a device with this number.
        /// </returns>
        Task<Device> GetDeviceAsync(string loginName, string deviceId);

        /// <summary>
        /// Gets all programmable keys of the specified device, including unassigned positions.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns>
        /// A list of <see cref="ProgrammableKey"/> representing all key positions on the device, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// Use this method when you need to know the full layout including empty positions.
        /// To retrieve only the assigned keys, use <see cref="GetProgrammedKeysAsync(string, string)"/>.
        /// </remarks>
        /// <seealso cref="GetProgrammedKeysAsync(string, string)"/>
        Task<List<ProgrammableKey>> GetProgrammableKeysAsync(string loginName, string deviceId);


        /// <summary>
        /// Gets only the programmed (assigned) programmable keys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns>
        /// A list of assigned <see cref="ProgrammableKey"/> objects, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// Use this method when you only need the keys that have been assigned.
        /// To retrieve the full layout including unassigned positions, use <see cref="GetProgrammableKeysAsync(string, string)"/>.
        /// </remarks>
        /// <seealso cref="GetProgrammableKeysAsync(string, string)"/>
        Task<List<ProgrammableKey>> GetProgrammedKeysAsync(string loginName, string deviceId);

        /// <summary>
        /// Assigns or updates a programmable key on the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <param name="key">The programmable key configuration to set. The position must be configured in the <c>key</c> object.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="DeleteProgrammableKeyAsync(string, string, int)"/>
        Task<bool> SetProgrammableKeyAsync(string loginName, string deviceId, ProgrammableKey key);

        /// <summary>
        /// Deletes the programmable key at the specified position on the given device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <param name="position">The position of the programmable key to delete.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="SetProgrammableKeyAsync(string, string, ProgrammableKey)"/>
        Task<bool> DeleteProgrammableKeyAsync(string loginName, string deviceId, int position);


        /// <summary>
        /// Gets the soft keys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns>
        /// A list of <see cref="SoftKey"/> objects representing the device soft keys, or <see langword="null"/> in case of error.
        /// </returns>
        Task<List<SoftKey>> GetSoftKeysAsync(string loginName, string deviceId);

        /// <summary>
        /// Assigns or updates a soft key on the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <param name="key">The soft key configuration to set. The position must be configured in the <c>key</c> object.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="DeleteSoftKeyAsync(string, string, int)"/>
        Task<bool> SetSoftKeyAsync(string loginName, string deviceId, SoftKey key);

        /// <summary>
        /// Deletes the soft key at the specified position on the given device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <param name="position">The position of the soft key to delete.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="SetSoftKeyAsync(string, string, SoftKey)"/>
        Task<bool> DeleteSoftKeyAsync(string loginName, string deviceId, int position);

        /// <summary>
        /// Locks the specified device, preventing it from being used to place or receive calls.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Returns <see langword="true"/> without any action if the device is already locked.
        /// </remarks>
        /// <seealso cref="UnLockDeviceAsync(string, string)"/>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> LockDeviceAsync(string loginName, string deviceId);

        /// <summary>
        /// Unlocks the specified device, restoring normal call capabilities.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Returns <see langword="true"/> without any action if the device is already unlocked.
        /// </remarks>
        /// <seealso cref="LockDeviceAsync(string, string)"/>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> UnLockDeviceAsync(string loginName, string deviceId);


        /// <summary>
        /// Enables the camp-on feature on the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// When camp-on is enabled, the user is automatically connected when a busy destination becomes available.
        /// Returns <see langword="true"/> without any action if camp-on is already enabled.
        /// </remarks>
        /// <seealso cref="DisableCamponAsync(string, string)"/>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> EnableCamponAsync(string loginName, string deviceId);

        /// <summary>
        /// Disables the camp-on feature on the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// Returns <see langword="true"/> without any action if camp-on is already disabled.
        /// </remarks>
        /// <seealso cref="EnableCamponAsync(string, string)"/>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> DisableCamponAsync(string loginName, string deviceId);

        /// <summary>
        /// Gets the PIN code configuration of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns>
        /// A <see cref="Pin"/> object representing the PIN code configuration, or <see langword="null"/> in case of error.
        /// </returns>
        /// <seealso cref="SetPinCodeAsync(string, string, Pin)"/>
        Task<Pin> GetPinCodeAsync(string loginName, string deviceId);

        /// <summary>
        /// Sets the PIN code on the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <param name="code">The PIN configuration to set.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="GetPinCodeAsync(string, string)"/>
        Task<bool> SetPinCodeAsync(string loginName, string deviceId, Pin code);

        /// <summary>
        /// Gets the dynamic state of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <returns>
        /// A <see cref="DynamicState"/> object representing the device dynamic state, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// The dynamic state reflects runtime settings such as the associated device and remote extension activation status.
        /// </remarks>
        /// <seealso cref="EnableCamponAsync(string, string)"/>
        /// <seealso cref="LockDeviceAsync(string, string)"/>
        Task<DynamicState> GetDynamicStateAsync(string loginName, string deviceId);

        /// <summary>
        /// Associates an additional device with the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (phone number).</param>
        /// <param name="associate">The phone number of the device to associate.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// The associate feature allows calls to ring simultaneously on both devices, which is useful for example
        /// to have a mobile phone ring alongside a desk phone.
        /// </remarks>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> SetAssociateAsync(string loginName, string deviceId, string associate);

        /// <summary>
        /// Activates the remote extension on the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (remote extension phone number).</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// When activated, the device operates as a remote extension, allowing the user to use an off-site phone as if it were connected to the PBX.
        /// </remarks>
        /// <seealso cref="DeactivateRemoteExtensionAsync(string, string)"/>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        /// <seealso cref="IRouting.ActivateRemoteExtensionAsync(string)"/>
        Task<bool> ActivateRemoteExtensionAsync(string loginName, string deviceId);

        /// <summary>
        /// Deactivates the remote extension on the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device identifier (remote extension phone number).</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <seealso cref="ActivateRemoteExtensionAsync(string, string)"/>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        /// <seealso cref="IRouting.DeactivateRemoteExtensionAsync(string)"/>
        Task<bool> DeactivateRemoteExtensionAsync(string loginName, string deviceId);
    }
}
