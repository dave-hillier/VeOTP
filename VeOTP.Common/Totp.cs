using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace VeOTP.Common
{
	/// <summary>
	/// Time-Based One-Time Password generator. 
	///  See: https://tools.ietf.org/html/rfc6238 and https://tools.ietf.org/html/rfc4226
	/// </summary>
	public class Topt
	{
		/// <summary>
		/// Generates an OTP 
		/// </summary>
		/// <returns>An OTP for the given time window.</returns>
		/// <param name="sharedKey">Shared key; must be know on both generator and validator.</param>
		/// <param name="clock">Used in generating in the OTP.</param>
		/// <param name="validity">The length of the validity period of the OTP.</param>
		/// <param name="passwordLength">The length of the OTP to be generated.</param>
		public static string GetCode(byte[] sharedKey, DateTime clock, TimeSpan validity, int passwordLength)
		{
			var c = GetCounter(clock, validity);
			var hash = ComputeHash(sharedKey, c);
			return TruncateRfc4226(hash, passwordLength);
		}

		static string TruncateRfc4226(byte[] hash, int passwordLength)
		{
			// Similar to http://tools.ietf.org/html/rfc4226#page-29
			int offset = hash[hash.Length - 1] & 0xf;
			int number = (hash[offset] & 0x7f) << 24
				| (hash[offset + 1] & 0xff) << 16
				| (hash[offset + 2] & 0xff) << 8
				| (hash[offset + 3] & 0xff);
			return (number % Math.Pow(10, passwordLength)).ToString().PadLeft(passwordLength, '0'); // Pad the front with zeros if its too small
		}

		static byte[] ComputeHash(byte[] sharedSecret, int clock)
		{
			var hmac = new HMACSHA256(sharedSecret);
			var stream = new MemoryStream(BitConverter.GetBytes(clock));
			var hash = hmac.ComputeHash(stream);
			return hash;
		}

		static int GetCounter(DateTime clock, TimeSpan validity)
		{
			var epochTime = clock.Subtract(new DateTime(1970, 1, 1));
			return (int)Math.Floor(epochTime.TotalSeconds / validity.TotalSeconds);
		}
	}
}

