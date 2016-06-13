using System;
using System.Text;
using VeOTP.Common;

namespace VeOTP.Validator
{
	class Validator
	{
		public static void Main(string[] args)
		{
			TimeSpan validityDuration;
			int passwordDigits;
			string secret;
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
			Console.Clear();

			while (!isExiting)
			{
				Console.Write("Password to test: ");
				var codeToTest = Console.ReadLine().Trim();

				var code = Topt.GetCode(key, DateTime.UtcNow, validityDuration, passwordDigits);
				Console.WriteLine((code == codeToTest) ? "✅  Valid " : "⛔ ️ Incorrect Password");
				Console.WriteLine(code);
			}
		}
	}
}
