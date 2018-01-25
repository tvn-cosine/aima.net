using aima.net.collections.api;

namespace aima.net.search.adversarial.api
{ 
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 165. 
    /// <para />
    /// A game can be formally defined as a kind of search problem with the following
    /// elements: <para />
    ///  
    /// * S0: The initial state, which specifies how the game is set up at the
    /// start.<para />
    /// * PLAYER(s): Defines which player has the move in a state.<para />
    /// * ACTIONS(s): Returns the set of legal moves in a state.<para />
    /// * RESULT(s, a): The transition model, which defines the result of a move.<para />
    /// * TERMINAL-TEST(s): A terminal test, which is true when the game is over
    /// and false TERMINAL STATES otherwise. States where the game has ended are
    /// called terminal states.<para />
    /// * UTILITY(s, p): A utility function (also called an objective function or
    /// payoff function), defines the final numeric value for a game that ends in
    /// terminal state s for a player p. In chess, the outcome is a win, loss, or
    /// draw, with values +1, 0, or 1/2 . Some games have a wider variety of possible
    /// outcomes; the payoffs in backgammon range from 0 to +192. A zero-sum game is
    /// (confusingly) defined as one where the total payoff to all players is the
    /// same for every instance of the game. Chess is zero-sum because every game has
    /// payoff of either 0 + 1, 1 + 0 or 1/2 + 1/2 . "Constant-sum" would have been a
    /// better term, but zero-sum is traditional and makes sense if you imagine each
    /// player is charged an entry fee of 1/2.<para />
    /// </summary>
    /// <typeparam name="S">Type which is used for states in the game.</typeparam>
    /// <typeparam name="A">Type which is used for actions in the game.</typeparam>
    /// <typeparam name="P">Type which is used for players in the game.</typeparam>
    public interface IGame<S, A, P>
    { 
        S GetInitialState(); 
        P[] GetPlayers(); 
        P GetPlayer(S state); 
        ICollection<A> GetActions(S state); 
        S GetResult(S state, A action); 
        bool IsTerminal(S state); 
        double GetUtility(S state, P player);
    } 
}
