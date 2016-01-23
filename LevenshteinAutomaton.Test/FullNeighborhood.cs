using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LevenshteinAutomaton.Test
{
	public class FullNeighborhood
	{
		public static readonly char[] Alphabet = new char[] { 
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 
			'u', 'v', 'w', 'x', 'y', 'z'
		};

		public static IEnumerable<string> GenerateNeighbors1(string word)
		{
			try
			{
				if (string.IsNullOrEmpty(word)) return new List<string>();

				var res = new List<string>();

				char[] w = word.ToCharArray();
				char[] n = new char[w.Length + 1];

				for (int i = 0; i < w.Length + 1; i++)
				{
					if (i > 0) n[i - 1] = w[i - 1];
					if (i < w.Length) n[i] = w[i]; 

					//Deleteions
					if (word.Length > 1 && i < w.Length)
					{
						for (int j = i; j < w.Length - 1; j++)
						{
							n[j] = w[j + 1];
						}
						res.Add(new String(n, 0, w.Length - 1));
					}				

					//Insertions        
					for (int k = i + 1; k < n.Length; k++)
					{
						n[k] = w[k - 1];
					}
					foreach (char c in Alphabet)
					{
						n[i] = c;
						res.Add(new String(n, 0, w.Length + 1));
					}

					//Substitutions
					if (i < w.Length)
					{
						for (int k = i + 1; k < w.Length; k++)
						{
							n[k] = w[k];
						}
						foreach (char c1 in Alphabet)
						{
							if (c1 != w[i])
							{
								n[i] = c1;
								res.Add(new String(n, 0, w.Length));
							}
						}
					}

					//Transpostitions
					if (i > w.Length - 2 || w[i] == w[i + 1]) continue;
					for (int k = i + 2; k < w.Length; k++)
					{
						n[k] = w[k];
					}
					n[i] = w[i + 1];
					n[i + 1] = w[i];
					res.Add(new String(n, 0, w.Length));
				}

				return res;
			}
			catch
			{
				return new string[0];
			}
		}
	}
}

