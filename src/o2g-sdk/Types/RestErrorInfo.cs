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

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace o2g.Types
{
    /// <summary>
    /// <c>ErrorInfo</c> gives the detail information on an error raised during a service invocation.
    /// </summary>
    /// <remarks>
    /// Most of the O2G services return the following code in case of error:
    /// <list type="table">
    ///     <listheader><term>Code</term><description>Description</description></listheader>
    ///     <item><term>400</term><description>Bad Request</description></item>
    ///     <item><term>403</term><description>Forbidden</description></item>
    ///     <item><term>404</term><description>Not Found</description></item>
    ///     <item><term>500</term><description>Internal Server Error</description></item>
    ///     <item><term>503</term><description>Service Unavailable</description></item>
    /// </list>
    /// </remarks>
    public class RestErrorInfo
    {
        /// <summary>
        /// <c>RoutingErrorInfo</c> class provides complementary information in case of an error in the <see cref="IRouting"/> service.
        /// </summary>
        public class RoutingErrorInfo
        {
            /// <summary>
            /// <c>RoutingErrorType</c> represents the different type of routing errors.
            /// </summary>
            [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: RoutingErrorType.Unknown)]
            public enum RoutingErrorType
            {
                /// <summary>
                /// Unknown routing error.
                /// </summary>
                [EnumMember(Value = "UNKNOWN")]
                Unknown,

                /// <summary>
                /// The routing service has rejected the request for a bad or missing parameter reason.
                /// </summary>
                [EnumMember(Value = "BAD_PARAMETER_VALUE")]
                BadParameterValue,

                /// <summary>
                /// Routing service request has been rejected because of limitation configured on the concerned user. For examples:
                /// <list type="bullet">
                /// <item><description>Overflow on busy is not allowed (barring limitation).</description></item>
                /// <item><description>Cancel overflow is not allowed: (barring limitation).</description></item>
                /// <item><description>Phone number to other destination is not authorized (dial plan limitation).</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "UNAUTHORIZED")]
                Unauthorized,

                /// <summary>
                /// Requested operation is not supported by the routing service.
                /// </summary>
                [EnumMember(Value = "INVALID_OPERATION")]
                InvalidOperation,

                /// <summary>
                /// The provided phone number can not be fully resolved in the current dial plan. For example:
                /// <list type="bullet">
                /// <item><description>Setting route with a destination type Other, containing a partially matching number (e.g. 3253 instead of 32535).</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "INCOMPLETE_PHONE_NUMBER")]
                IncompletePhoneNumber,

                /// <summary>
                /// The provided phone number can not be resolved in the current dial plan. For example:
                /// <list type="bullet">
                /// <item><description>Setting route with a destination type NUMBER, containing an unknown number.</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "UNKNOWN_PHONE_NUMBER")]
                UnknownPhoneNumber
            }

            /// <summary>
            /// <c>RoutingErrorCause</c> represents the differents routing error causes.
            /// </summary>
            [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: RoutingErrorCause.Unknown)]
            public enum RoutingErrorCause
            {
                /// <summary>
                /// Unknown routing error cause.
                /// </summary>
                [EnumMember(Value = "UNKNOWN")]
                Unknown,

                /// <summary>
                /// The phone number does not comply with formating rules. For examples:
                /// <list type="bullet">
                /// <item><description>The phone number does not respect the following regular expression: [+]?[0-9A-D*#\\(\\) ]{1,49}.</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "BAD_PHONE_NUMBER_FORMAT")]
                BadPhoneNumberFormat,

                /// <summary>
                /// The given device number as current device is not valid. For examples:
                /// <list type="bullet">
                /// <item><description>The device is not in an acceptable state to be used.</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "INVALID_CURRENT_DEVICE")]
                InvalidCurrentDevice,

                /// <summary>
                /// The given forward route is not valid. For examples:
                /// <list type="bullet">
                /// <item><description>The forwarded destination type is not one of VOICEMAIL or NUMBER.</description></item>
                /// <item><description>The forwarded destination is not acceptable.</description></item>
                /// <item><description>A loop in the forward chain has been detected.</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "INVALID_FORWARD_ROUTE")]
                InvalidForwardRoute,

                /// <summary>
                /// The given overflow route is not valid. For examples:
                /// <list type="bullet">
                /// <item><description>The overflow destination is not acceptable.</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "INVALID_OVERFLOW_ROUTE")]
                InvalidOverflowRoute,

                /// <summary>
                /// Parameter must not be null nor empty.
                /// </summary>
                [EnumMember(Value = "NULL_OR_EMPTY_PARAMETER")]
                NullOrEmptyParameter,

                /// <summary>
                /// Parameter must not be null.
                /// </summary>
                [EnumMember(Value = "NULL_PARAMETER")]
                NullParameter,

                /// <summary>
                /// Cancel overflow has been rejected because of barring rules.
                /// </summary>
                [EnumMember(Value = "UNAUTHORIZED_CANCEL_OVERFLOW")]
                UnauthorizedCancelOverflow,

                /// <summary>
                /// The destination type is set to USER, but the number does not correspond to a user.
                /// </summary>
                [EnumMember(Value = "UNAUTHORIZED_NOT_A_USER")]
                UnauthorizedNotAUser,

                /// <summary>
                /// Overflow has been rejected because of barring rules.
                /// </summary>
                [EnumMember(Value = "UNAUTHORIZED_OVERFLOW")]
                UnauthorizedOverflow,

                /// <summary>
                /// The given phone number is unauthorized. For examples:
                /// <list type="bullet">
                /// <item><description>Barring as rejected the number.</description></item>
                /// <item><description>The destination can not be a service number.</description></item>
                /// <item><description>The destination is a voicemail, but user has no rights to use it.</description></item>
                /// </list>
                /// </summary>
                [EnumMember(Value = "UNAUTHORIZED_PHONE_NUMBER")]
                UnauthorizedPhoneNumber,
            }

            /// <summary>
            /// Return the routing error type.
            /// </summary>
            /// <value>
            /// A <see cref="RoutingErrorType"/> value that represents the routing error type.
            /// </value>
            public RoutingErrorType ErrorType { get; init; }

            /// <summary>
            /// Return the probable routing error cause.
            /// </summary>
            /// <value>
            /// A <see cref="RoutingErrorCause"/> value that represents the routing error cause.
            /// </value>
            public RoutingErrorCause ErrorCause { get; init; }

            /// <summary>
            /// Return an additional textual error information provided by the routing service.
            /// </summary>
            /// <value>
            /// A <see langword="string"/> message providing additional information.
            /// </value>
            public string Message { get; init; }
        }

        /// <summary>
        /// <c>TelephonyErrorInfo</c> class provides complementary information in case of an error in the <see cref="ITelephony"/> service.
        /// </summary>
        public class TelephonyErrorInfo
        {
            /// <summary>
            /// <c>TelephonyErrorType</c> represents the telephony error types.
            /// </summary>
            [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: TelephonyErrorType.Unknown)]
            public enum TelephonyErrorType
            {
                /// <summary>
                /// Unknown error, the O2G is unable to identify the root cause of the error.
                /// </summary>
                [EnumMember(Value = "UNKNOWN")]
                Unknown,
                /// <summary>
                /// The specified call reference in invalid.
                /// </summary>
                [EnumMember(Value = "CALL_REFERENCE_NOT_FOUND")]
                CallReferenceNotFound,
                /// <summary>
                /// The telephony service can not execute the request because the specified leg can not be found.
                /// <para>For exemple: request an online recording while there is no active leg.</para>
                /// </summary>
                [EnumMember(Value = "LEG_NOT_FOUND")]
                MegNotFound,
                /// <summary>
                /// Some parameters attached to the request are not correct.
                /// </summary>
                [EnumMember(Value = "BAD_PARAMETER_VALUE")]
                BasParameterValue,
                /// <summary>
                /// The telephony service can not execute the request because the current telephony state does not permit such operation.
                /// </summary>
                [EnumMember(Value = "INCOMPATIBLE_WITH_STATE")]
                IncompatibleWithState,
                /// <summary>
                /// The telephony service can not execute the request because the corresponding service is not provided in this context.
                /// </summary>
                [EnumMember(Value = "SERVICE_NOT_PROVIDED")]
                ServiceNotProvided,
                /// <summary>
                /// The telephony service can not execute the request because it relies on another service which is unavailable.
                /// <para>For exemple: Redirecting to voicemail is requested, but user has no voicemail.</para>
                /// </summary>
                [EnumMember(Value = "SERVICE_UNAVAILABLE")]
                ServiceUnavailable,
                /// <summary>
                /// The telephony service is starting up and has not finished its initialization.
                /// </summary>
                [EnumMember(Value = "INITIALIZATION")]
                Initialization,
                /// <summary>
                /// Telephony service request has been rejected.
                /// <para>For exemple: The user has already a call ongoing and he has no Advanced Telephony license.</para>
                /// </summary>
                [EnumMember(Value = "UNAUTHORIZED")]
                Unauthorized,
                /// <summary>
                /// Telephony service request failed because of an error generated by the call server. More details can be provided by <see cref="TelephonyErrorCause"/>
                /// </summary>
                [EnumMember(Value = "CALL_SERVER_ERROR")]
                CallserverError,
                /// <summary>
                /// An operation invoked on the telephony service has exceeded the expected response timeout (default 5 seconds).
                /// </summary>
                [EnumMember(Value = "REQUEST_TIMEOUT")]
                RequestTimeout
            }

            /// <summary>
            /// <c>TelephonyErrorCause</c> represents the possible errors raised by the OmniPCX Enterprise call server.
            /// </summary>
            [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: TelephonyErrorCause.Unknown)]
            public enum TelephonyErrorCause
            {
                /// <summary>
                /// Unknown telephony error cause.
                /// </summary>
                [EnumMember(Value = "UNKNOWN")]
                Unknown,
                /// <summary>
                /// The call server can not process the request because the calling device is not an acceptable one.
                /// </summary>
                [EnumMember(Value = "INVALID_CALLING")]
                InvalidCalling,
                /// <summary>
                /// Destination is not a correct number.
                /// </summary>
                [EnumMember(Value = "INVALID_DESTINATION")]
                InvalidDestination,
                /// <summary>
                /// The referenced call reference is not valid.
                /// </summary>
                [EnumMember(Value = "INVALID_CALL_ID")]
                InvalidCallId,
                /// <summary>
                /// The current state of the call does not permit the operation.
                /// </summary>
                [EnumMember(Value = "INVALID_CONNECTION_STATE")]
                InvalidConnectionState,
                /// <summary>
                /// The device is out of service.
                /// </summary>
                [EnumMember(Value = "DEVICE_OUT_OF_SERVICE")]
                DeviceOutOfService,
                /// <summary>
                /// The device is not in valid.
                /// </summary>
                [EnumMember(Value = "INVALID_DEVICE")]
                InvalidDevice,
                /// <summary>
                /// The device state is incompatible with the request.
                /// </summary>
                [EnumMember(Value = "INVALID_DEVICE_STATE")]
                InvalidDeviceState,
                /// <summary>
                /// The data parameter has invalid value.
                /// </summary>
                [EnumMember(Value = "INVALID_DATA")]
                InvalidData,
                /// <summary>
                /// The destination is busy. All the user phone lines are already engaged.
                /// </summary>
                [EnumMember(Value = "RESOURCE_BUSY")]
                ResourceBusy
            }

            /// <summary>
            /// An <see cref="TelephonyErrorType"/> value that represent the error type.
            /// </summary>
            public TelephonyErrorType ErrorType { get; set; }
            /// <summary>
            /// An <see cref="TelephonyErrorCause"/> value that represent the error cause.
            /// </summary>
            public TelephonyErrorCause ErrorCause { get; set; }
            /// <summary>
            /// Give a textual information about the error
            /// </summary>
            public string Message { get; set; }
        }

        /// <summary>
        /// <c>UserPreferencesErrorInfo</c> class provides complementary information in case of an error in the <see cref="IUsers"/> service.
        /// </summary>
        public class UserPreferencesErrorInfo
        {
            /// <summary>
            /// <c>UserPreferencesErrorType</c> represents the possible error types for the user preferences.
            /// </summary>
            [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: UserPreferencesErrorType.Unknown)]
            public enum UserPreferencesErrorType
            {
                /// <summary>
                /// Unexpected error.
                /// </summary>
                [EnumMember(Value = "UNKNOWN")]
                Unknown,
                /// <summary>
                /// Value not expected.
                /// </summary>
                [EnumMember(Value = "WRONG_VALUE")]
                WrongValue,
                /// <summary>
                /// Wrong number format.
                /// </summary>
                [EnumMember(Value = "WRONG_NUMBER_FORMAT")]
                WrongNumberFormat
            }

            /// <summary>
            /// <c>UserPreferenceParameter</c> represents the possible preferences.
            /// </summary>
            [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: UserPreferenceParameter.Unknown)]
            public enum UserPreferenceParameter
            {
                /// <summary>
                /// User GUI language.
                /// </summary>
                [EnumMember(Value = "GUI_LANGUAGE")]
                GuiLanguage,

                /// <summary>
                /// Unknown property
                /// </summary>
                Unknown
            }

            /// <summary>
            /// Return the information on the error.
            /// </summary>
            /// <value>
            /// A <see cref="UserPreferencesErrorType"/> value that represents the error.
            /// </value>
            [JsonPropertyName("userPreferencesErrorTypeDTO")]
            public UserPreferencesErrorType UserPreferencesError { get; init; }

            /// <summary>
            /// Return the parameter that cause the error.
            /// </summary>
            /// <value>
            /// A <see cref="UserPreferenceParameter"/> value that represents the parameter.
            /// </value>
            [JsonPropertyName("userPreferencesParameterDTO")]
            public UserPreferenceParameter UserPreferencesParameter { get; init; }
        }


        /// <summary>
        /// The Http status as define in <see href="https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html"/>.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the http error code.
        /// </value>
        public string HttpStatus { get; init; }

        /// <summary>
        /// Return the REST API error code. It allows to quickly identify the possible error.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the REST API error code.
        /// </value>
        public int Code { get; init; }

        /// <summary>
        /// Return a help message, associated with the <see cref="Type"/> and <see cref="Code"/>, providing a more detailed cause of the error.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the help message.
        /// </value>
        public string HelpMessage { get; init; }

        /// <summary>
        /// Return the REST API error type, which group all possible underlying errors in a finite number of possibilities.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the REST API error type.
        /// </value>
        public string Type { get; init; }

        /// <summary>
        /// Return a message containing relevant information to help an administrator or support team to find the cause of the problem.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the inner message.
        /// </value>
        public string InnerMessage { get; init; }

        /// <summary>
        /// <see langword="true"/> if the error can be solved by modifying the request; <see langword="false"/> if it is a server side error.
        /// </summary>
        /// <value>
        /// A <see langword="bool"/> value that indicates if the error can be fixed.
        /// </value>
        public bool CanRetry { get; init; }

        /// <summary>
        /// Return additional information when the error is raised by the <see cref="IRouting"/> service.
        /// </summary>
        /// <value>
        /// A <see cref="RoutingErrorInfo"/> object that represents the routing error.
        /// </value>
        public RoutingErrorInfo Routing { get; init; }

        /// <summary>
        /// Return additional information when the error is raised by the <see cref="ITelephony"/> service.
        /// </summary>
        /// <value>
        /// A <see cref="TelephonyErrorInfo"/> object that represents the telephony error.
        /// </value>
        public TelephonyErrorInfo Telephony { get; init; }

        /// <summary>
        /// Return additional information when the error is raised by the <see cref="IUsers"/> service.
        /// </summary>
        /// <value>
        /// A <see cref="UserPreferencesErrorInfo"/> object that represents the user's preference error.
        /// </value>
        public UserPreferencesErrorInfo UserPreferences { get; init; }
    }
}
