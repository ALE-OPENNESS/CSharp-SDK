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

using o2g.Types.TelephonyNS.CallNS;
using System;
using System.Text;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// Represents correlator data attached to a call.
    /// <para>
    /// Correlator data is application-provided information (limited to 32 bytes)
    /// that travels with a call. It is typically used to carry application context
    /// from one party to another across telephony operations such as transfer.
    /// </para>
    /// <para>
    /// For example, user A receives an external call and attaches correlator data
    /// to it. When user A transfers the call to user B, user B receives an
    /// <see cref="ITelephony"/> call created event whose <see cref="CallData"/> contains the
    /// same correlator data, allowing user B's application to retrieve the original
    /// context.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// // Attach correlator data to a call
    /// var data = new CorrelatorData("transactionId=abc123");
    /// await telephony.MakeCallAsync("1234", "5678", autoAnswer: true, correlatorData: data);
    ///
    /// // Read correlator data from a received call event
    /// telephony.CallCreated += (sender, e) =>
    /// {
    ///     var correlator = e.Event.CallData?.CorrelatorData;
    ///     if (correlator != null)
    ///     {
    ///         Console.WriteLine($"Context: {correlator.AsString()}");
    ///     }
    /// };
    /// </code>
    /// </example>
    public class CorrelatorData
    {
        private readonly byte[] _value;

        /// <summary>
        /// Creates a new <see cref="CorrelatorData"/> from a string, encoding it as UTF-8.
        /// </summary>
        /// <param name="value">The correlator data as a string.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">if <paramref name="value"/> contains the byte <c>0x00</c> once encoded.</exception>
        public CorrelatorData(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _value = Encoding.UTF8.GetBytes(value);
            ValidateNoNullByte();
        }

        /// <summary>
        /// Creates a new <see cref="CorrelatorData"/> from a byte array.
        /// </summary>
        /// <param name="value">The correlator data as a byte array.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">if <paramref name="value"/> contains the byte <c>0x00</c>.</exception>
        public CorrelatorData(byte[] value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _value = value;
            ValidateNoNullByte();
        }

        /// <summary>
        /// Returns the correlator data as a byte array.
        /// </summary>
        /// <returns>A <see langword="byte"/>[] containing the raw correlator data.</returns>
        public byte[] AsByteArray() => _value;

        /// <summary>
        /// Returns the correlator data as a UTF-8 decoded string.
        /// </summary>
        /// <returns>A <see langword="string"/> representation of the correlator data.</returns>
        public string AsString() => Encoding.UTF8.GetString(_value);

        private void ValidateNoNullByte()
        {
            if (Array.IndexOf(_value, (byte)0x00) >= 0)
            {
                throw new ArgumentException("Byte 0x00 is not authorized in correlator data.");
            }
        }
    }
}
