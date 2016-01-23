using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LevenshteinAutomaton.Test
{
	[TestFixture()]
	public class Lev2TAutomata_AcceptWord
	{
		[Test()]
		public void Atlas ()
		{
			var automata = new LevTAutomataImitation ("atlas", 2);
			Assert.True (automata.AcceptWord ("atlas"));
			Assert.True (automata.AcceptWord ("xatlxs"));
			Assert.False (automata.AcceptWord ("otter"));
			Assert.False (automata.AcceptWord (""));
			Assert.True (automata.AcceptWord ("aaatlas"));
			Assert.False (automata.AcceptWord ("aaaatlas"));
			Assert.False (automata.AcceptWord ("last"));
			Assert.True (automata.AcceptWord ("latas"));
			Assert.True (automata.AcceptWord ("taals"));
			Assert.False (automata.AcceptWord ("taalss"));
			Assert.False (automata.AcceptWord ("taasl"));
			Assert.False (automata.AcceptWord ("salomon"));
		}

		[Test()]
		public void Otter ()
		{
			var automata = new LevTAutomataImitation ("otter", 2);
			Assert.True (automata.AcceptWord ("xotter"));
			Assert.True (automata.AcceptWord ("xyotter"));
			Assert.False (automata.AcceptWord ("xyzotter"));
			Assert.True (automata.AcceptWord ("oxtter"));
			Assert.True (automata.AcceptWord ("oxtyter"));
			Assert.False (automata.AcceptWord ("oxtyterz"));
			Assert.False (automata.AcceptWord (""));
			Assert.True (automata.AcceptWord ("toter"));
			Assert.True (automata.AcceptWord ("toetr"));
			Assert.False (automata.AcceptWord ("toert"));
			Assert.True (automata.AcceptWord ("oter"));
			Assert.True (automata.AcceptWord ("ter"));
			Assert.False (automata.AcceptWord ("er"));
			Assert.False (automata.AcceptWord ("tre"));
		}

		[Test()]
		public void Abcde ()
		{
			var automata = new LevTAutomataImitation ("abcde", 2);
			Assert.True (automata.AcceptWord ("aabcde"));
			Assert.True (automata.AcceptWord ("aaabcde"));
			Assert.False (automata.AcceptWord ("aaaabcde"));
			Assert.True (automata.AcceptWord ("abced"));
			Assert.True (automata.AcceptWord ("acbed"));
			Assert.False (automata.AcceptWord ("aacbed"));
			Assert.True (automata.AcceptWord ("abde"));
			Assert.True (automata.AcceptWord ("abd"));
			Assert.False (automata.AcceptWord ("ad"));
			Assert.False (automata.AcceptWord ("bd"));
			Assert.True (automata.AcceptWord ("xbcde"));
			Assert.True (automata.AcceptWord ("xbcxe"));
			Assert.False (automata.AcceptWord ("xxcdx"));
			Assert.True (automata.AcceptWord ("bacdx"));
			Assert.True (automata.AcceptWord ("bade"));
			Assert.True (automata.AcceptWord ("xbdce"));
			Assert.False (automata.AcceptWord ("xbdxe"));
			Assert.False (automata.AcceptWord ("xbdcx"));
		}

		[Test()]
		public void A ()
		{
			var automata = new LevTAutomataImitation ("a", 2);
			Assert.True (automata.AcceptWord ("a"));
			Assert.True (automata.AcceptWord (""));
			Assert.True (automata.AcceptWord ("b"));
			Assert.True (automata.AcceptWord ("ab"));
			Assert.True (automata.AcceptWord ("ba"));
			Assert.True (automata.AcceptWord ("abc"));
			Assert.True (automata.AcceptWord ("bac"));
			Assert.True (automata.AcceptWord ("bca"));
			Assert.False (automata.AcceptWord ("abcd"));
		}

		[Test()]
		public void Array ()
		{
			//Misspelled word
			string wordToSearch = "fulzy";
			//Your dictionary
			string[] dictionaryAsArray = new string[] { "fuzzy", "fully", "funny", "fast"};
			//Maximum Damerau-Levenstein distance
			const int editDistance = 2;
			//Constructing automaton
			LevTAutomataImitation automaton = new LevTAutomataImitation (wordToSearch, editDistance);
			//List of possible corrections
			IEnumerable<string> corrections = dictionaryAsArray.Where(str => automaton.AcceptWord(str));

			Assert.AreEqual (3, corrections.Count());
		}
	}
}

