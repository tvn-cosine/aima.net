using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.search.adversarial.api;

namespace aima.net.environment.connectfour
{
    /// <summary>
    /// Provides an implementation of the ConnectFour game which can be used for
    /// experiments with the Minimax algorithm.
    /// </summary>
    public class ConnectFourGame : IGame<ConnectFourState, int, string>
    {
        private string[] players = new string[] { "red", "yellow" };
        private ConnectFourState initialState = new ConnectFourState(6, 7);

        public ConnectFourState GetInitialState()
        {
            return initialState;
        }

        public string[] GetPlayers()
        {
            return players;
        }

        public string GetPlayer(ConnectFourState state)
        {
            return GetPlayer(state.getPlayerToMove());
        }

        /// <summary>
        /// Returns the player corresponding to the specified player number. For
        /// efficiency reasons, ConnectFourState's use numbers
        /// instead of strings to identify players.
        /// </summary>
        /// <param name="playerNum"></param>
        /// <returns></returns>
        public string GetPlayer(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    return players[0];
                case 2:
                    return players[1];
            }
            return null;
        }

        /// <summary>
        /// Returns the player number corresponding to the specified player. For
        /// efficiency reasons, ConnectFourState's use numbers instead of
        /// strings to identify players.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public int GetPlayerNum(string player)
        {
            for (int i = 0; i < players.Length; ++i)
                if (players[i].Equals(player))
                    return i + 1;
            throw new IllegalArgumentException("Wrong player number.");
        }

        public ICollection<int> GetActions(ConnectFourState state)
        {
            ICollection<int> result = CollectionFactory.CreateQueue<int>();
            for (int i = 0; i < state.getCols(); ++i)
                if (state.getPlayerNum(0, i) == 0)
                    result.Add(i);
            return result;
        }

        public ConnectFourState GetResult(ConnectFourState state, int action)
        {
            ConnectFourState result = state.Clone();
            result.dropDisk(action);
            return result;
        }

        public bool IsTerminal(ConnectFourState state)
        {
            return state.getUtility() != -1;
        }

        public double GetUtility(ConnectFourState state, string player)
        {
            double result = state.getUtility();
            if (result != -1)
            {
                if (player.Equals(players[1]))
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
