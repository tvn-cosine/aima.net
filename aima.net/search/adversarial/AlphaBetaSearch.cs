using aima.net.search.adversarial.api;
using aima.net.search.framework;

namespace aima.net.search.adversarial
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Page 173.<br>
     * <p>
     * <pre>
     * <code>
     * function ALPHA-BETA-SEARCH(state) returns an action
     *   v = MAX-VALUE(state, -infinity, +infinity)
     *   return the action in ACTIONS(state) with value v
     *
     * function MAX-VALUE(state, alpha, beta) returns a utility value
     *   if TERMINAL-TEST(state) then return UTILITY(state)
     *   v = -infinity
     *   for each a in ACTIONS(state) do
     *     v = MAX(v, MIN-VALUE(RESULT(s, a), alpha, beta))
     *     if v >= beta then return v
     *     alpha = MAX(alpha, v)
     *   return v
     *
     * function MIN-VALUE(state, alpha, beta) returns a utility value
     *   if TERMINAL-TEST(state) then return UTILITY(state)
     *   v = infinity
     *   for each a in ACTIONS(state) do
     *     v = MIN(v, MAX-VALUE(RESULT(s,a), alpha, beta))
     *     if v <= alpha then return v
     *     beta = MIN(beta, v)
     *   return v
     * </code>
     * </pre>
     * <p>
     * Figure 5.7 The alpha-beta search algorithm. Notice that these routines are
     * the same as the MINIMAX functions in Figure 5.3, except for the two lines in
     * each of MIN-VALUE and MAX-VALUE that maintain alpha and beta (and the
     * bookkeeping to pass these parameters along).
     *
     * @param <S> Type which is used for states in the game.
     * @param <A> Type which is used for actions in the game.
     * @param <P> Type which is used for players in the game.
     * @author Ruediger Lunde
     */
    public class AlphaBetaSearch<S, A, P> : IAdversarialSearch<S, A>
    {
        public const string METRICS_NODES_EXPANDED = "nodesExpanded";

        IGame<S, A, P> game;
        private Metrics metrics = new Metrics();

        /**
         * Creates a new search object for a given game.
         */
        public static AlphaBetaSearch<STATE, ACTION, PLAYER> createFor<STATE, ACTION, PLAYER>(IGame<STATE, ACTION, PLAYER> game)
        {
            return new AlphaBetaSearch<STATE, ACTION, PLAYER>(game);
        }

        public AlphaBetaSearch(IGame<S, A, P> game)
        {
            this.game = game;
        }
         
        public A makeDecision(S state)
        {
            metrics = new Metrics();
            A result = default(A);
            double resultValue = double.NegativeInfinity;
            P player = game.GetPlayer(state);
            foreach (A action in game.GetActions(state))
            {
                double value = minValue(game.GetResult(state, action), player,
                        double.NegativeInfinity, double.PositiveInfinity);
                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }
            return result;
        }

        public double maxValue(S state, P player, double alpha, double beta)
        {
            metrics.incrementInt(METRICS_NODES_EXPANDED);
            if (game.IsTerminal(state))
                return game.GetUtility(state, player);
            double value = double.NegativeInfinity;
            foreach (A action in game.GetActions(state))
            {
                value = System.Math.Max(value, minValue( //
                        game.GetResult(state, action), player, alpha, beta));
                if (value >= beta)
                    return value;
                alpha = System.Math.Max(alpha, value);
            }
            return value;
        }

        public double minValue(S state, P player, double alpha, double beta)
        {
            metrics.incrementInt(METRICS_NODES_EXPANDED);
            if (game.IsTerminal(state))
                return game.GetUtility(state, player);
            double value = double.PositiveInfinity;
            foreach (A action in game.GetActions(state))
            {
                value = System.Math.Min(value, maxValue( //
                        game.GetResult(state, action), player, alpha, beta));
                if (value <= alpha)
                    return value;
                beta = System.Math.Min(beta, value);
            }
            return value;
        }
         
        public Metrics getMetrics()
        {
            return metrics;
        }
    } 
}
