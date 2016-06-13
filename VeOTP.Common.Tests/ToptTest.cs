using NUnit.Framework;
using System;
using System.Security.Cryptography;

namespace VeOTP.Common.Tests
{
	[TestFixture]
	public class ToptTest
	{
		byte[] sharedKey = new byte[64];
		RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
		int digits = 6;
		TimeSpan validity = TimeSpan.FromSeconds(30);

		[SetUp]
		public void Setup()
		{
			rng.GetBytes(sharedKey);
		}

		[Test]
		public void SameTimeGeneratesSameCode()
		{
			var now = DateTime.UtcNow;
			var code1 = Topt.GetCode(sharedKey, now, validity, digits);
			var code2 = Topt.GetCode(sharedKey, now, validity, digits);
			Assert.That(code1, Is.EqualTo(code2));
		}

		[Test]
		public void WithinThirtySecondSameCode()
		{
			var time = DateTime.Today;
			var code1 = Topt.GetCode(sharedKey, time, validity, digits);
			var code2 = Topt.GetCode(sharedKey, time + validity - TimeSpan.FromMilliseconds(1), validity, digits);
			Assert.That(code1, Is.EqualTo(code2));
		}

		[Test]
		public void MoreThanThirtySecondDifferent()
		{
			var time = DateTime.Today;
			var code1 = Topt.GetCode(sharedKey, time, validity, digits);
			var code2 = Topt.GetCode(sharedKey, time + validity, validity, digits);
			Assert.That(code1, Is.Not.EqualTo(code2));
		}

		[Test]
		public void ShorterValidityDifferent()
		{
			var shorterValidity = TimeSpan.FromSeconds(5);
			var time = DateTime.Today;
			var code1 = Topt.GetCode(sharedKey, time, validity, digits);
			var code2 = Topt.GetCode(sharedKey, time + shorterValidity, shorterValidity, digits);
			Assert.That(code1, Is.Not.EqualTo(code2));
		}

		[Test]
		public void SameTimeDifferentKey()
		{
			var time = DateTime.Today;
			var code1 = Topt.GetCode(sharedKey, time, validity, digits);
			var key = new byte[64];
			rng.GetBytes(key);
			var code2 = Topt.GetCode(key, time, validity, digits);
			Assert.That(code1, Is.Not.EqualTo(code2));
		}

		[Test]
		public void DigitsControlsLength()
		{
			var time = DateTime.Today;
			var code1 = Topt.GetCode(sharedKey, time, validity, digits);
			var code2 = Topt.GetCode(sharedKey, time, validity, digits + 1);
			var code3 = Topt.GetCode(sharedKey, time, validity, digits + 2);
			Assert.That(code1.Length, Is.EqualTo(digits));
			Assert.That(code2.Length, Is.EqualTo(digits + 1));
			Assert.That(code3.Length, Is.EqualTo(digits + 2));
		}

		[Test]
		public void EnsureLengthIsCorrect()
		{
			var time = new DateTime(2016, 6, 13);
			var code1 = Topt.GetCode(sharedKey, time, validity, digits); // Happens to generate a short code. Not the only example, but one for bug elimination
			Assert.That(code1.Length, Is.EqualTo(digits));
		}
	}
}

