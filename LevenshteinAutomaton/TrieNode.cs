using System;
using System.Collections.Generic;

namespace LevenshteinAutomaton
{
	/// <summary>
	/// Prefix tree node
	/// </summary>
	public class TrieNode
	{
		/// <summary>
		/// Node's key
		/// </summary>
		public readonly String Key;
		/// <summary>
		/// Children node list
		/// </summary>
		public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();        

		/// <summary>
		/// Init node with the given key
		/// </summary>
		/// <param name="key">Key</param>
		public TrieNode(String key) {
			Key = key;
		}

		/// <summary>
		/// Add child node
		/// </summary>
		/// <param name="c">Letter</param>
		/// <param name="child">Node to add</param>
		/// <returns></returns>
		public bool AddChild(char c, TrieNode child)
		{
			Children.Add(c, child);
			return true;
		}

		/// <summary>
		/// Check if node contains child for given letter
		/// </summary>
		public bool ContainsChildValue(char letter) {
			return Children.ContainsKey(letter);
		}	

		/// <summary>
		/// Get child node by letter
		/// </summary>
		public TrieNode GetChild(char letter)
		{
			if (Children.ContainsKey(letter))
				return Children[letter];
			else
				return null;
		}

		/// <summary>
		/// The node represents word
		/// </summary>
		public bool IsWord { get; set; }         

		/// <summary>
		/// Get node for given word or prefix if exists
		/// </summary>
		public TrieNode GetChild(String wordOrPrefix)
		{            
			return string.IsNullOrEmpty(wordOrPrefix) ? null : GetChild(wordOrPrefix.ToCharArray());
		}

		/// <summary>
		/// Get node for given char array
		/// </summary>
		public TrieNode GetChild(char[] letters)
		{
			TrieNode currentNode = this;             
			for (int i = 0; i < letters.Length && currentNode != null; i++)
			{
				currentNode = currentNode.GetChild(letters[i]);
				if (currentNode == null)
				{
					return null;
				}
			}
			return currentNode;
		}

		/// <summary>
		/// Get node for given word if exists
		/// </summary>
		public TrieNode GetWord(string word)
		{
			TrieNode node = GetChild(word);
			return node != null && node.IsWord ? node : null;
		}

		public override String ToString() {
			return Key;
		}
	}
}

