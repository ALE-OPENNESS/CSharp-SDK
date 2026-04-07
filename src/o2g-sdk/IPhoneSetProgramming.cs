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
using o2g.Types.PhoneSetProgrammableNS;
using o2g.Types.UsersNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>IPhoneSetProgramming</c> service allows a user to manage its device states: lock/campon/programmable keys.
    /// Using this service requires having a <b>API_PHONESETPROG</b> license.
    /// </summary>
    public interface IPhoneSetProgramming : IService
    {
        /// <summary>
        /// Get the device of the specified user.
        /// </summary>
        /// <param name="loginName">The user login name</param>
        /// <returns>
        /// A list of <see cref="Device"/> that represents the user's devices.
        /// </returns>
        Task<List<Device>> GetDevicesAsync(string loginName);

        /// <summary>
        /// Return the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns>
        /// A <see cref="Device"/> that represents the device with the specified number in case of success, of <see langword="null"/> in case of error of if
        /// the user does not have a device with this number.
        /// </returns>
        Task<Device> GetDeviceAsync(string loginName, string deviceId);

        /// <summary>
        /// Get the programmable keys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns>
        /// A list of <see cref="ProgrammableKey"/> that represents this device programmable keys in case of success, or a <see langword="null"/> value in case of error.
        /// </returns>
        Task<List<ProgrammableKey>> GetProgrammableKeysAsync(string loginName, string deviceId);


        /// <summary>
        /// Get the already programmed keys associated to the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns>
        /// A list of <see cref="ProgrammableKey"/> that represents this device already programmed keys in case of success, or a <see langword="null"/> value in case of error.
        /// </returns>
        Task<List<ProgrammableKey>> GetProgrammedKeysAsync(string loginName, string deviceId);

        /// <summary>
        /// Set the programmable keys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="key">The programmable key.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// The key position must be configured in the <c>key</c> object.
        /// </remarks>
        Task<bool> SetProgrammableKeyAsync(string loginName, string deviceId, ProgrammableKey key);

        /// <summary>
        /// Delete the programmable keys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="position">The programmable key position.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        Task<bool> DeleteProgrammableKeyAsync(string loginName, string deviceId, int position);


        /// <summary>
        /// Get the softkeys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns>
        /// A list of <see cref="SoftKey"/> that represents this device softkeys in case of success, or a <see langword="null"/> value in case of error.
        /// </returns>
        Task<List<SoftKey>> GetSoftKeys(string loginName, string deviceId);

        /// <summary>
        /// Set the softkeys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="key">The softkey.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// The key position must be configured in the <c>key</c> object.
        /// </remarks>
        Task<bool> SetSoftKeyAsync(string loginName, string deviceId, SoftKey key);

        /// <summary>
        /// Delete the softkeys of the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="position">The softkey position.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        Task<bool> DeleteSoftKeyAsync(string loginName, string deviceId, int position);

        /// <summary>
        /// Lock the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// This method does nothing and return <see langword="true"/> if the the device is already locked.
        /// </remarks>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> LockDeviceAsync(string loginName, string deviceId);

        /// <summary>
        /// UnLock the specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// This method does nothing and return <see langword="true"/> if the the device is already unlocked.
        /// </remarks>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> UnLockDeviceAsync(string loginName, string deviceId);


        /// <summary>
        /// Enable the camp on for specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// This method does nothing and return <see langword="true"/> f the the camp on is already enabled.
        /// </remarks>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> EnableCamponAsync(string loginName, string deviceId);

        /// <summary>
        /// Disable the camp on for specified device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <remarks>
        /// This method does nothing and return <see langword="true"/> if the the camp on is already disabled.
        /// </remarks>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> DisableCamponAsync(string loginName, string deviceId);

        /// <summary>
        /// Return the PIN code associated to this device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns>
        /// A <see cref="Pin"/> object that represents the PIN code.
        /// </returns>
        Task<Pin> GetPinCodeAsync(string loginName, string deviceId);

        /// <summary>
        /// Set the PIN code for this device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="code">The PIN code.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        Task<bool> SetPinCodeAsync(string loginName, string deviceId, Pin code);

        /// <summary>
        /// Return the device dynamic state.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <returns>
        /// A <see cref="DynamicState"/> object that represent the device dynamic state.
        /// </returns>
        /// <seealso cref="EnableCamponAsync(string, string)"/>
        /// <seealso cref="LockDeviceAsync(string, string)"/>
        Task<DynamicState> GetDynamicStateAsync(string loginName, string deviceId);

        /// <summary>
        /// Set the associate phone number.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The device phone number.</param>
        /// <param name="associate">The associate number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <seealso cref="GetDynamicStateAsync(string, string)"/>
        Task<bool> SetAssociateAsync(string loginName, string deviceId, string associate);

        /// <summary>
        /// Activate the associated remote extension device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The remote extension phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <seealso cref="IRouting.ActivateRemoteExtensionAsync(string)"/>
        Task<bool> ActivateRemoteExtensionAsync(string loginName, string deviceId);

        /// <summary>
        /// Deactivate the associated remote extension device.
        /// </summary>
        /// <param name="loginName">The user login name.</param>
        /// <param name="deviceId">The remote extension phone number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <seealso cref="IRouting.DeactivateRemoteExtensionAsync(string)"/>
        Task<bool> DeactivateRemoteExtensionAsync(string loginName, string deviceId);
    }
}
