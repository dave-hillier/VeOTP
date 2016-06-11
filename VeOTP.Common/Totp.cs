using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace VeOTP.Common
{
	public class Topt
	{
		static string TruncateRfc4226(byte[] hash, int passwordLength)
		{
			// Similar to http://tools.ietf.org/html/rfc4226#page-29
			int offset = hash[hash.Length - 1] & 0xf;
			int number = (hash[offset] & 0x7f) << 24
				| (hash[offset + 1] & 0xff) << 16
				| (hash[offset + 2] & 0xff) << 8
				| (hash[offset + 3] & 0xff);
			return (number % Math.Pow(10, passwordLength)).ToString();
		}

		// Not required - but included 
		static string TruncateHashAlphaNum(byte[] hash, int passwordLength)
		{
			// An alternative implementation 
			string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var code = hash.Take(passwordLength).Select(c => characters[c % characters.Length]);
			return string.Concat(code);
		}

		static byte[] ComputeHash(byte[] sharedSecret, int clock)
		{
			var hmac = new HMACSHA256(sharedSecret);
			var stream = new MemoryStream(BitConverter.GetBytes(clock));
			var hash = hmac.ComputeHash(stream);
			return hash;
		}

		public static string GetCode(byte[] sharedKey, DateTime clock, TimeSpan validity, int digits)
		{
			var c = GetCounter(clock, validity);
			var hash = ComputeHash(sharedKey, c);
			return TruncateRfc4226(hash, digits);
		}

		static int GetCounter(DateTime clock, TimeSpan validity)
		{
			var epochTime = clock.Subtract(new DateTime(1970, 1, 1));
			return (int)Math.Floor(epochTime.TotalSeconds / validity.TotalSeconds);
		}
}
}

