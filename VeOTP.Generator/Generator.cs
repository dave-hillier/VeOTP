using System;
using System.Text;
using System.Threading;
using VeOTP.Common;

namespace VeOTP
{
	class Generator
	{
		public static void Main(string[] args)
		{
			TimeSpan validityDuration = TimeSpan.FromSeconds(30);
			int passwordDigits = 8;
			string secret = "";

			if (args.Length < 3)
			{
				Console.WriteLine("usage: shared_secret password_validity_duration_seconds password_length");
				return;
			}
			else
			{
				secret = args[0];
				// Note: not validated.
				validityDuration = TimeSpan.FromSeconds(int.Parse(args[1]));
				passwordDigits = int.Parse(args[2]);
			}

			var key = Encoding.UTF8.GetBytes(secret);
			
			bool isExiting = false;
			Console.CancelKeyPress += (s, e) => isExiting = true;
			while (!isExiting)
			{
				Console.Clear();
				Console.SetCursorPosition(0, 0);
				Console.Write("OTP: {0}", Topt.GetCode(key, DateTime.UtcNow, validityDuration, passwordDigits));
				ShowCountdown(validityDuration);
				Thread.Sleep(500);
			}
		}

		// Note: Tightly coupled to the counter calculation
		static void ShowCountdown(TimeSpan validityDuration)
		{
			var epochTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1));
			var counter = epochTime.TotalSeconds / validityDuration.TotalSeconds;
			var remaining = (int)Math.Floor(10 * (counter - Math.Floor(counter)));

			Console.Write(" -- ╢");
			for (int i = 0; i < 10; ++i)
			{
				Console.Write(i < remaining ? "░" : "█");
			}
			Console.WriteLine("╟");
		}
}
}
