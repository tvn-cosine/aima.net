using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.adversarial;
using aima.net.search.adversarial.api;

namespace aima.net.environment.connectfour
{
    /**
     * Implements an iterative deepening Minimax search with alpha-beta pruning and
     * a special action ordering optimized for the Connect Four game.
     * 
     * @author Ruediger Lunde
     */
    public class ConnectFourAIPlayer
        : IterativeDeepeningAlphaBetaSearch<ConnectFourState, int, string>
    {
        public ConnectFourAIPlayer(IGame<ConnectFourState, int, string> game, int time)
            : base(game, 0.0, 1.0, time)
        { }

        protected override bool isSignificantlyBetter(double newUtility, double utility)
        {
            return newUtility - utility > (utilMax - utilMin) * 0.4;
        }

        protected override bool hasSafeWinner(double resultUtility)
        {
            return System.Math.Abs(resultUtility - (utilMin + utilMax) / 2) > 0.4 * utilMax - utilMin;
        }

        /**
         * Modifies the super implementation by making safe winner values even more
         * attractive if depth is small.
         */
        protected override double eval(ConnectFourState state, string player)
        {
            double value = base.eval(state, player);
            if (hasSafeWinner(value))
            {
                if (value > (utilMin + utilMax) / 2)
                    value -= state.getMoves() / 1000.0;
                else
                    value += state.getMoves() / 1000.0;
            }
            return value;
        }

        /**
         * Orders actions with respect to the number of potential win positions
         * which profit from the action.
         */
        public override ICollection<int> orderActions(ConnectFourState state,
               ICollection<int> actions, string player, int depth)
        {
            ICollection<int> result = actions;
            if (depth == 0)
            {
                ICollection<ActionValuePair<int>> actionEstimates
                    = CollectionFactory.CreateQueue<ActionValuePair<int>>();
                foreach (int action in actions)
                    actionEstimates.Add(ActionValuePair<int>.createFor(action,
                            state.analyzePotentialWinPositions(action)));
                actionEstimates.Sort(new List<ActionValuePair<int>>.Comparer());
                result = CollectionFactory.CreateQueue<int>();
                foreach (ActionValuePair<int> pair in actionEstimates)
                    result.Add(pair.getAction());
            }
            return result;
        }
    }
}
