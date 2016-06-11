using System;
using System.Text;
using System.Threading;
using VeOTP.Common;

namespace VeOTP
{
	class Generator
	{
		static TimeSpan ValidityDuration = TimeSpan.FromSeconds(10);
		static int PasswordDigits = 8;

		public static void Main(string[] args)
		{
			// Shared secret.
			var secret = "Lorem ipsum dolor sit amet, consectetur adipiscing elit posuere.";
			var key = Encoding.ASCII.GetBytes(secret);

			bool isExiting = false;
			Console.CancelKeyPress += (s, e) => isExiting = true;
			while (!isExiting)
			{
				Console.SetCursorPosition(0, 0);
				Console.WriteLine("OTP: {0}", Topt.GetCode(key, DateTime.UtcNow, ValidityDuration, PasswordDigits));
				Thread.Sleep(500);
			}
		}
	}
}
