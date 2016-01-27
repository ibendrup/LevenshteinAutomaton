using NUnit.Framework;
using System;

namespace LevenshteinAutomaton.Test
{
	[TestFixture()]
	public class Lev1TAutomata_AcceptWord
	{
		[Test()]
		public void A ()
		{
			var automata = new LevTAutomataImitation ("a", 1);
			Assert.True (automata.AcceptWord ("a"));
			Assert.True (automata.AcceptWord (""));
			Assert.True (automata.AcceptWord ("b"));
			Assert.True (automata.AcceptWord ("ab"));
			Assert.True (automata.AcceptWord ("ba"));
			Assert.False (automata.AcceptWord ("abc"));
			Assert.False (automata.AcceptWord ("bac"));
			Assert.False (automata.AcceptWord ("bca"));
			Assert.False (automata.AcceptWord ("abcd"));
		}

		[Test()]
		public void Abcde ()
		{
			var automata = new LevTAutomataImitation ("abcde", 1);
			Assert.True (automata.AcceptWord ("aabcde"));
			Assert.False (automata.AcceptWord ("aaabcde"));
			Assert.False (automata.AcceptWord ("aaaabcde"));
			Assert.True (automata.AcceptWord ("abced"));
			Assert.False (automata.AcceptWord ("acbed"));
			Assert.False (automata.AcceptWord ("aacbed"));
			Assert.True (automata.AcceptWord ("abde"));
			Assert.False (automata.AcceptWord ("abd"));
			Assert.False (automata.AcceptWord ("ad"));
			Assert.False (automata.AcceptWord ("bd"));
			Assert.True (automata.AcceptWord ("xbcde"));
			Assert.False (automata.AcceptWord ("xbcxe"));
			Assert.False (automata.AcceptWord ("xxcdx"));
			Assert.False (automata.AcceptWord ("bacdx"));
			Assert.False (automata.AcceptWord ("bade"));
			Assert.False (automata.AcceptWord ("xbdce"));
			Assert.False (automata.AcceptWord ("xbdxe"));
			Assert.False (automata.AcceptWord ("xbdcx"));
		}

		[Test()]
		public void Fuzzy ()
		{
			const string word = "fuzzy";
			var automata = new LevTAutomataImitation (word, 1);
			foreach (string typo in FullNeighborhood.GenerateNeighbors1(word)) {
				Assert.True(automata.AcceptWord(typo));
			}
		}

		[Test()]
		public void Atlas ()
		{
			const string word = "atlas";
			var automata = new LevTAutomataImitation (word, 1);
			foreach (string typo in FullNeighborhood.GenerateNeighbors1(word)) {
				Assert.True(automata.AcceptWord(typo));
			}
		}

		[Test()]
		public void Ababa ()
		{
			const string word = "ababa";
			var automata = new LevTAutomataImitation (word, 1);
			foreach (string typo in FullNeighborhood.GenerateNeighbors1(word)) {
				Assert.True(automata.AcceptWord(typo));
			}
		}
	}
}

