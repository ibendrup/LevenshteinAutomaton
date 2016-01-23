using System;
using System.Collections.Generic;

namespace LevenshteinAutomaton
{
	/// <summary>
	/// Prefix tree
	/// </summary>
	public class Trie
	{
		/// <summary>
		/// Root of the tree
		/// </summary>
		public TrieNode root = new TrieNode("");

		/// <summary>
		/// Init empty prefix tree
		/// </summary>
		public Trie() { }

		/// <summary>
		/// Init prefix tree for given lexicon
		/// </summary>
		public Trie(IEnumerable<String> lexicon) {
			foreach (String word in lexicon) {
				AddWord(word);
			}
		}

		/// <summary>
		/// Append word to the prefix tree
		/// </summary>
		public void AddWord(String word) {
			char[] argChars = word.ToCharArray();
			TrieNode currentNode = root;

			for (int i = 0; i < argChars.Length; i++) {
				if (!currentNode.ContainsChildValue(argChars[i])) {
					currentNode.AddChild(argChars[i], new TrieNode(currentNode.Key + argChars[i]));
				}
				currentNode = currentNode.GetChild(argChars[i]);
			} 
			currentNode.IsWord = true;
		}

		/// <summary>
		/// Check if trie contains given prefix
		/// </summary>
		public bool СontainsPrefix(String prefix)
		{
			return Сontains(prefix, false);
		}

		/// <summary>
		/// Check if trie contains given word
		/// </summary>
		public bool СontainsWord(String word)
		{
			return Сontains(word, true);
		}

		/// <summary>
		/// Get node for given word if exists
		/// </summary>
		public TrieNode GetWord(String word)
		{
			TrieNode node = GetNode(word);
			return node != null && node.IsWord ? node : null;
		}

		/// <summary>
		/// Get node for given prefix if exists
		/// </summary>
		public TrieNode GetPrefix(String prefix)
		{
			return GetNode(prefix);
		}

		/// <summary>
		/// Check if trie contains given word or prefix
		/// </summary>
		private bool Сontains(String argString, bool word)
		{
			TrieNode node = GetNode(argString);
			return (node != null && node.IsWord && word) || (!word && node != null);
		}

		/// <summary>
		/// Get node for given word or prefix
		/// </summary>
		public TrieNode GetNode(String argString)
		{            
			return root.GetChild(argString);
		}
	}
}

