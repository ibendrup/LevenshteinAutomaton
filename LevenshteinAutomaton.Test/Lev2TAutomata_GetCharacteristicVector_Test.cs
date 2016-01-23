using NUnit.Framework;
using System;

namespace LevenshteinAutomaton.Test
{
	[TestFixture()]
	public class Lev2TAutomata_GetCharacteristicVector_Test
	{
		[Test()]
		public void ExistngLetter ()
		{
			var automata = new LevTAutomataImitation ("atlas", 2);
			//10010
			Assert.AreEqual (18, automata.GetCharacteristicVector ('a', 0));
			//00100
			Assert.AreEqual (4, automata.GetCharacteristicVector ('a', 1));
			//01000
			Assert.AreEqual (8, automata.GetCharacteristicVector ('a', 2));
			//10000
			Assert.AreEqual (16, automata.GetCharacteristicVector ('a', 3));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('a', 4));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('a', 5));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('a', 6));
		}

		[Test()]
		public void NonExistingLetter ()
		{
			var automata = new LevTAutomataImitation ("atlas", 2);
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('x', 0));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('x', 1));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('x', 2));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('x', 3));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('x', 4));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('x', 5));
			//00000
			Assert.AreEqual (0, automata.GetCharacteristicVector ('x', 6));
		}
	}
}

