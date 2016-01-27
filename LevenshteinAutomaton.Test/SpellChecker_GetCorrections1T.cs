using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace LevenshteinAutomaton.Test
{
	[TestFixture()]
	public class SpellChecker_GetCorrections1T
	{
		SpellChecker corrector;

		[TestFixtureSetUp]
		public void Setup() {
			corrector = new SpellChecker (new string[] { "fuzzy", "fully", "funny", "fast" });
		}

		[Test()]
		public void GetCorrections1T ()
		{
			var c = corrector.GetCorrections1T ("fulzy");
			Assert.AreEqual (1, c.Count(s => s == "fully"));
			c = corrector.GetCorrections1T ("fulzy");
			Assert.AreEqual (1, c.Count(s => s == "fuzzy"));
			c = corrector.GetCorrections1T ("fulzy");
			Assert.AreEqual (0, c.Count(s => s == "funny"));
		}

		[Test()]
		public void GetCorrections2T ()
		{
			var c = corrector.GetCorrections2T ("fulzy");
			Assert.AreEqual (1, c.Count(s => s == "fully"));
			c = corrector.GetCorrections2T ("fulzy");
			Assert.AreEqual (1, c.Count(s => s == "fuzzy"));
			c = corrector.GetCorrections2T ("fulzy");
			Assert.AreEqual (1, c.Count(s => s == "funny"));
		}
	}
}

