using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.exceptions;
using aima.net.search.adversarial;
using aima.net.search.adversarial.api;

namespace aima.net.environment.tictactoe
{
    /**
     * Provides an implementation of the Tic-tac-toe game which can be used for
     * experiments with the Minimax algorithm.
     * 
     * @author Ruediger Lunde
     * 
     */
    public class TicTacToeGame : IGame<TicTacToeState, XYLocation, string>
    { 
        private TicTacToeState initialState = new TicTacToeState();

        public TicTacToeState GetInitialState()
        {
            return initialState;
        }

        public string[] GetPlayers()
        {
            return new string[] { TicTacToeState.X, TicTacToeState.O };
        }

        public string GetPlayer(TicTacToeState state)
        {
            return state.getPlayerToMove();
        }

        public ICollection<XYLocation> GetActions(TicTacToeState state)
        {
            return state.getUnMarkedPositions();
        }

        public TicTacToeState GetResult(TicTacToeState state, XYLocation action)
        {
            TicTacToeState result = state.Clone();
            result.mark(action);
            return result;
        }

        public bool IsTerminal(TicTacToeState state)
        {
            return state.getUtility() != -1;
        }

        public double GetUtility(TicTacToeState state, string player)
        {
            double result = state.getUtility();
            if (result != -1)
            {
                if (player.Equals(TicTacToeState.O))
                    result = 1 - result;
            }
            else
            {
                throw new IllegalArgumentException("State is not terminal.");
            }
            return result;
        }
    }
}
