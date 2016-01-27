using System;
using System.Collections.Generic;
using System.Linq;

namespace LevenshteinAutomaton
{
	/// <summary>
	/// Spell Checker uses the Levenshtein automaton and FB-Trie algorithm to find possible corrections for the given garbled word
	/// </summary>
	public class SpellChecker
	{
		/// <summary>
		/// Search state
		/// </summary>
		struct SpellCheckerState {
			/// <summary>
			/// Current trie node
			/// </summary>
			public TrieNode Node;
			/// <summary>
			/// The state of the Levenshtein automaton.
			/// </summary>
			public int AutomataState;
			/// <summary>
			/// The offset of the Levenshtein automaton.
			/// </summary>
			public int AutomataOffset;

			public SpellCheckerState(TrieNode node, int state, int offset) {
				Node = node;
				AutomataState = state;
				AutomataOffset = offset;
			}

			public override bool Equals (object obj)
			{
				SpellCheckerState? other = obj as SpellCheckerState?;
				if (!other.HasValue)
					return false;
				return other.Value.AutomataOffset == this.AutomataOffset 
					&& other.Value.AutomataState == this.AutomataState 
						&& other.Value.Node == this.Node;
			}

			public override int GetHashCode ()
			{
				return AutomataState ^ AutomataOffset ^ Node.GetHashCode();
			}
		}

		/// <summary>
		/// Dictionary as the Trie
		/// </summary>
		Trie ForwardDictionary = new Trie();
		/// <summary>
		/// Backward dictionary as the Trie
		/// </summary>
		Trie BackwardDictionary = new Trie();

		/// <summary>
		/// Initializes a new instance for empty lexicon
		/// </summary>
		public SpellChecker ()
		{
		}

		/// <summary>
		/// Initializes a new instance for the given lexicon
		/// </summary>
		public SpellChecker (IEnumerable<string> words)
		{
			foreach (string word in words) {
				if (string.IsNullOrEmpty (word))
					continue;
				ForwardDictionary.AddWord (word);
				BackwardDictionary.AddWord (new string(word.Reverse().ToArray()));
			}
		}	

		/// <summary>
		/// Append new word to the dictionary
		/// </summary>
		/// <param name="word">Word.</param>
		public void AddWord(string word) {
			ForwardDictionary.AddWord (word);
			BackwardDictionary.AddWord (new string(word.Reverse ().ToArray ()));
		}

		/// <summary>
		/// Gets the corrections for edit distance 2 with transpositions.
		/// </summary>
		/// <param name="typo">Garbled word.</param>
		public IList<string> GetCorrections2T(string typo) {
			var corrections = new List<string> ();

			if (string.IsNullOrEmpty (typo)) {
				return corrections;
			}

			//For short words use basic algoritm
			if (typo.Length <= 2) {
				return GetCorrectionStrings (typo, ForwardDictionary.root, 2).ToList();
			}

			//Prepare substrings
			string left;
			string right;
			string rleft;
			string rright;
			int llen = PrepareSubstrings (typo, out left, out right, out rleft, out rright);

			//May be d(S1, W1) = 0 and d(S2, W2) <= 2?
			TrieNode lnode = ForwardDictionary.GetNode (left);
			if (lnode != null) {
				corrections.AddRange(GetCorrectionStrings(right, lnode, 2));
			}

			//May be 1 <= d(S1, W1) <= 2 and d(S2, W2) = 0?
			TrieNode rnode = BackwardDictionary.GetNode (rright);
			if (rnode != null) {
				corrections.AddRange(GetCorrectionStrings(rleft, rnode, 2).Select(x => new string(x.Reverse().ToArray())));
			}

			//May be d(S1, W1) = 1 and d(S2, W2) = 1?
			foreach (TrieNode node in GetCorrectionNodes(left, ForwardDictionary.root, 1, false)) {
				corrections.AddRange(GetCorrectionStrings(right, node, 1));
			}

			//May be there is a transposition in the middle?..
			char[] temp = typo.ToCharArray ();
			char c = temp [llen - 1];
			temp [llen - 1] = temp [llen];
			temp [llen] = c;

			string transposedTypo = new string (temp);

			llen = PrepareSubstrings (transposedTypo, out left, out right, out rleft, out rright);

			//...and d(S1, W1') = 0 and d(S2, W2') <= 1?..
			lnode = ForwardDictionary.GetNode (left);
			if (lnode != null) {
				corrections.AddRange(GetCorrectionStrings(right, lnode, 1));
			}
			//...or d(S1, W1') = 1 and d(S2, W2') = 0
			rnode = BackwardDictionary.GetNode (rright);
			if (rnode != null) {
				corrections.AddRange(GetCorrectionStrings(rleft, rnode, 1).Select(x => new string(x.Reverse().ToArray())));
			}

			//There may be duplicates
			return corrections.Distinct().ToList();
		}	


		/// <summary>
		/// Gets the corrections for edit distance 1 with transpositions.
		/// </summary>
		/// <param name="typo">Garbled word.</param>
		public IList<string> GetCorrections1T(string typo) {
			//For short strings use simple algorithm
			if (typo.Length <= 2) {
				return GetCorrectionStrings (typo, ForwardDictionary.root, 1).ToList();
			}
			var corrections = new List<string> ();

			//Prepare substrings

			//Left substring
			string left;
			//Right substring
			string right;
			//Reversed left substring
			string rleft;
			//Reversed right substring
			string rright;
			//Length of the left substring
			int llen = PrepareSubstrings (typo, out left, out right, out rleft, out rright);

			//May be for some S d(S1, W1) = 0 and d(S2, W2) <= 1?
			TrieNode lnode = ForwardDictionary.GetNode (left);
			if (lnode != null) {
				corrections.AddRange(GetCorrectionStrings(right, lnode, 1));
			}

			//May be for some S d(S1, W1) = 1 and d(S2, W2) = 0?
			TrieNode rnode = BackwardDictionary.GetNode (rright);
			if (rnode != null) {
				corrections.AddRange(GetCorrectionStrings(rleft, rnode, 1).Select(x => new string(x.Reverse().ToArray())));
			}

			//May be there is a transposition in the middle?
			char[] temp = typo.ToCharArray ();
			char c = temp [llen - 1];
			temp [llen - 1] = temp [llen];
			temp [llen] = c;

			TrieNode n = ForwardDictionary.GetWord (new string(temp));
			if (n != null) {
				corrections.Add (n.Key);
			}

			//There may be duplicates
			return corrections.Distinct().ToList();
		}

		/// <summary>
		/// Find all words in dictionary such that Levenshtein distance to typo less or equal to editDistance
		/// </summary>
		/// <returns>Set of possible corrections</returns>
		/// <param name="typo">Garbled word.</param>
		/// <param name="start">Start search on this node</param>
		/// <param name="editDistance">Edit distance.</param>
		IEnumerable<string> GetCorrectionStrings(string typo, TrieNode start, int editDistance) {
			return GetCorrectionNodes (typo, start, editDistance).Select(x => x.Key);
		}

		/// <summary>
		/// Basic Schulz and Mihov algoritm
		/// </summary>
		/// <returns>The correction nodes.</returns>
		/// <param name="typo">Typo.</param>
		/// <param name="start">Start.</param>
		/// <param name="editDistance">Edit distance.</param>
		/// <param name="includeOnlyWords">If set to <c>true</c> include only words.</param>
		IList<TrieNode> GetCorrectionNodes(string typo, TrieNode start, int editDistance, bool includeOnlyWords = true) {
			var corrections = new List<TrieNode> ();

			if (string.IsNullOrEmpty (typo)) {
				return corrections;
			}
			LevTAutomataImitation automata = new LevTAutomataImitation (typo, editDistance);

			Stack<SpellCheckerState> stack = new Stack<SpellCheckerState> ();
			stack.Push (new SpellCheckerState () {
				Node = start,
				AutomataState = 0,
				AutomataOffset = 0,
			});

			while (stack.Count > 0) {
				SpellCheckerState state = stack.Pop();

				automata.LoadState (state.AutomataState, state.AutomataOffset);
				AutomatonState nextZeroState = automata.GetNextState (0);

				foreach (char c in state.Node.Children.Keys) {
					AutomatonState nextState = null;

					if ((state.AutomataOffset < typo.Length && typo[state.AutomataOffset] == c)
					    || (state.AutomataOffset < typo.Length - 1 && typo[state.AutomataOffset + 1] == c)
					    || (state.AutomataOffset < typo.Length - 2 && typo[state.AutomataOffset + 2] == c)) {
						nextState = automata.GetNextState (automata.GetCharacteristicVector(c, state.AutomataOffset));
					} else {
						nextState = nextZeroState;
					}

					if (nextState != null) {
						TrieNode nextNode = state.Node.Children [c];
						if (nextNode.Children.Count > 0) {
							stack.Push (new SpellCheckerState () {
								Node = nextNode,
								AutomataState = nextState.State,
								AutomataOffset = nextState.Offset
							});
						}
						if ((nextNode.IsWord || !includeOnlyWords) && automata.IsAcceptState (nextState.State, nextState.Offset)) {
							corrections.Add (nextNode);
						}
					}
				}				
			}

			return corrections;
		}

		/// <summary>
		/// Splits the typo into two substrings of similar length
		/// </summary>
		/// <returns>Length of the left substring.</returns>
		/// <param name="typo">Typo.</param>
		/// <param name="left">Left substring</param>
		/// <param name="right">Right substring.</param>
		/// <param name="rleft">Reversed left substring</param>
		/// <param name="rright">Reversed right substring</param>
		static int PrepareSubstrings (string typo, out string left, out string right, out string rleft, out string rright)
		{
			int rlen = typo.Length / 2;
			int llen = typo.Length - rlen;
			left = typo.Substring (0, llen);
			right = typo.Substring (llen);
			rleft = new string (left.Reverse ().ToArray ());
			rright = new string (right.Reverse ().ToArray ());
			return llen;
		}
	}
}

