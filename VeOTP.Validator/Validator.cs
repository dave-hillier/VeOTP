using System;
using System.Text;
using System.Threading;
using VeOTP.Common;

namespace VeOTP.Validator
{
	class Validator
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
				Console.Write("Password to test: ");
				var codeToTest = Console.ReadLine();
				var code = Topt.GetCode(key, DateTime.UtcNow, ValidityDuration, PasswordDigits);

				if (code == codeToTest)
				{
					Console.WriteLine("Valid");
				}
				else 
				{
					Console.WriteLine("Incorrect Password");
				}
			}
		}
	}
}
