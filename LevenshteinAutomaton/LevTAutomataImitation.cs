using System;

namespace LevenshteinAutomaton
{
	/// <summary>
	/// Universal deterministic Levenshtein automaton for Damerau-Levenshtein distance
	/// </summary>
	public class LevTAutomataImitation
	{
		/// <summary>
		/// Allowed modifications count
		/// </summary>
		public readonly int N;	

		/// <summary>
		/// Word length
		/// </summary>
		public readonly int W;

		/// <summary>
		/// Characteristic vector length
		/// </summary>
		public readonly int VectorLength;

		/// <summary>
		/// Possible characteristic vector value count
		/// </summary>
		public readonly int VectorCount;

		/// <summary>
		/// The word.
		/// </summary>
		public readonly string Word;

		/// <summary>
		/// The state count.
		/// </summary>
		public readonly int StateCount;

		/// <summary>
		/// Current state number
		/// </summary>
		public int CurrentState { get; private set; }

		/// <summary>
		/// Current automata offset
		/// </summary>
		public int CurrentOffset { get; private set; }


		/// <summary>
		/// Current state transitions matrix
		/// </summary>
		sbyte[,] stateTransitions = null;
		/// <summary>
		/// Current offset increment matrix
		/// </summary>
		sbyte[,] offsetIncrements = null;

		/// <summary>
		/// Initializes a new instance of the LevTAutomataImitation class.
		/// </summary>
		/// <param name="word">Word</param>
		/// <param name="n">Maximum edit distance</param>
		public LevTAutomataImitation(string word, int n) {
			if (string.IsNullOrEmpty (word)) {
				throw new ArgumentException ("Word should have one or more letters");
			}
			if (n < 0 || n > 2) {
				throw new ArgumentException ("Supported edit distances are 1 and 2");
			}

			W = word.Length;
			Word = word;
			N = n;
			VectorLength = 2 * N + 1;
			VectorCount = (int)Math.Pow (2, VectorLength);
			StateCount = n == 1 ? 6 : 42;

			UpdateMatricies ();
		}

		/// <summary>
		/// Gets a value indicating whether this automaton is in empty state.
		/// </summary>
		/// <value><c>true</c> if this instance is in empty state; otherwise, <c>false</c>.</value>
		public bool IsInEmptyState {
			get {
				return CurrentState < 0 || CurrentOffset < 0;
			}
		}

		/// <summary>
		/// Gets the characteristic vector for given letter and position.
		/// </summary>
		/// <returns>The characteristic vector.</returns>
		/// <param name="letter">Letter.</param>
		/// <param name="position">Position.</param>
		public int GetCharacteristicVector(char letter, int position) {
			int vector = 0;
			for (int i = position; i < Math.Min(Word.Length, position + VectorLength); i++) {
				if (Word[i] == letter)
				{
					vector |= 1 << (VectorLength - 1) - (i - position);
				}
			}
			return vector;
		}

		/// <summary>
		/// Check if given state is the accept state for given offset
		/// </summary>
		/// <param name="state">State.</param>
		/// <param name="offset">Offset.</param>
		public bool IsAcceptState(int state, int offset) {
			if (state < 0 || offset < 0) {
				return false;
			}
			int distanceToEnd = W - offset;
			if (distanceToEnd > VectorLength - 1)
				return false;
			return ParametricDescription.IsAcceptState [N - 1][state, distanceToEnd];
		}

		/// <summary>
		/// Сheck if automaton is in accept state
		/// </summary>
		/// <value><c>true</c> if this automaton is in accept state; otherwise, <c>false</c>.</value>
		public bool IsInAcceptState {
			get {
				return IsAcceptState (CurrentState, CurrentOffset);
			}
		}

		/// <summary>
		/// Select current state transition matrix and offset increment matrix
		/// </summary>
		protected void UpdateMatricies ()
		{
			int distanceToEnd = W - CurrentOffset;
			switch (distanceToEnd) {		
				case 0:
				stateTransitions = ParametricDescription.StateTransitions0[N-1];
				offsetIncrements = ParametricDescription.OffsetIncrements0[N-1];
				break;
				case 1:
				stateTransitions = ParametricDescription.StateTransitions1[N-1];
				offsetIncrements = ParametricDescription.OffsetIncrements1[N-1];
				break;
				case 2:
				stateTransitions = ParametricDescription.StateTransitions2[N-1];
				offsetIncrements = ParametricDescription.OffsetIncrements2[N-1];
				break;
				case 3:
				stateTransitions = ParametricDescription.StateTransitions3[N-1];
				offsetIncrements = ParametricDescription.OffsetIncrements3[N-1];
				break;
				case 4:
				stateTransitions = ParametricDescription.StateTransitions4[N-1];
				offsetIncrements = ParametricDescription.OffsetIncrements4[N-1];
				break;
				default:
				stateTransitions = ParametricDescription.StateTransitions5[N-1];
				offsetIncrements = ParametricDescription.OffsetIncrements5[N-1];
				break;
			}
		}

		/// <summary>
		/// Loads automaton state and offset.
		/// </summary>
		/// <param name="state">State.</param>
		/// <param name="offset">Offset.</param>
		public void LoadState(int state, int offset) {
			if (offset > W || offset < 0) {
				throw new ArgumentException ("Offset should be betweeen 0 and " + W);
			}
			if (state >= StateCount || state < 0) {
				throw new ArgumentException ("State should be betweeen 0 and 5");
			}
			CurrentState = state;
			CurrentOffset = offset;

			UpdateMatricies ();
		}		

		/// <summary>
		/// Evaluate next automaton state (without actual transition)
		/// </summary>
		/// <returns>The next state.</returns>
		/// <param name="vector">Characteristic vector.</param>
		public AutomatonState GetNextState(int vector) {
			if (vector >= VectorCount) {
				throw new ArgumentException ("Vector should be between 0 and " + (VectorCount - 1));
			}

			if (IsInEmptyState) {
				return null;
			}

			int nextState = stateTransitions [vector, CurrentState];

			if (nextState >= 0)
			{
				return new AutomatonState() { State = nextState, Offset = (CurrentOffset + offsetIncrements [vector, CurrentState]) };
			}
			else 
			{
				return null;
			}
		}

		/// <summary>
		/// Make transition to the next state for the given characteristic vector
		/// </summary>
		/// <param name="vector">Vector.</param>
		public void NextState(int vector)
		{
			AutomatonState s = GetNextState (vector);

			if (s != null)
			{
				CurrentOffset = s.Offset;
				CurrentState = s.State;

				UpdateMatricies ();
			}
			else 
			{
				CurrentState = -1;
				CurrentOffset = -1;
			}
		}	

		/// <summary>
		/// Make transition to the next state for the given letter
		/// </summary>
		/// <param name="letter">Letter.</param>
		public void NextState(char letter) {
			if (IsInEmptyState) {
				return;
			}
			int vector = GetCharacteristicVector (letter, CurrentOffset);
			NextState (vector);
		}

		/// <summary>
		/// Check if this automaton accepts given word (i.e. edit distance between Word and word <= N)
		/// </summary>
		/// <returns><c>true</c>, if word was accepted, <c>false</c> otherwise.</returns>
		/// <param name="word">Word to check</param>
		public bool AcceptWord(string word) {
			LoadState(0, 0);
			for (int i = 0; i < word.Length; i++) {
				NextState (word [i]);
			}
			return IsInAcceptState;
		}
	}
}

