using aima.net.search.adversarial.api;
using aima.net.search.framework;

namespace aima.net.search.adversarial
{
    /**
  * Artificial Intelligence A Modern Approach (3rd Edition): page 169.<br>
  * <p>
  * <pre>
  * <code>
  * function MINIMAX-DECISION(state) returns an action
  *   return argmax_[a in ACTIONS(s)] MIN-VALUE(RESULT(state, a))
  *
  * function MAX-VALUE(state) returns a utility value
  *   if TERMINAL-TEST(state) then return UTILITY(state)
  *   v = -infinity
  *   for each a in ACTIONS(state) do
  *     v = MAX(v, MIN-VALUE(RESULT(s, a)))
  *   return v
  *
  * function MIN-VALUE(state) returns a utility value
  *   if TERMINAL-TEST(state) then return UTILITY(state)
  *     v = infinity
  *     for each a in ACTIONS(state) do
  *       v  = MIN(v, MAX-VALUE(RESULT(s, a)))
  *   return v
  * </code>
  * </pre>
  * <p>
  * Figure 5.3 An algorithm for calculating minimax decisions. It returns the
  * action corresponding to the best possible move, that is, the move that leads
  * to the outcome with the best utility, under the assumption that the opponent
  * plays to minimize utility. The functions MAX-VALUE and MIN-VALUE go through
  * the whole game tree, all the way to the leaves, to determine the backed-up
  * value of a state. The notation argmax_[a in S] f(a) computes the element a of
  * set S that has the maximum value of f(a).
  *
  * @param <S> Type which is used for states in the game.
  * @param <A> Type which is used for actions in the game.
  * @param <P> Type which is used for players in the game.
  * @author Ruediger Lunde
  */
    public class MinimaxSearch<S, A, P> : IAdversarialSearch<S, A>
    {
        public const string METRICS_NODES_EXPANDED = "nodesExpanded";

        private IGame<S, A, P> game;
        private Metrics metrics = new Metrics();

        /**
         * Creates a new search object for a given game.
         */
        public static MinimaxSearch<S, A, P> createFor(IGame<S, A, P> game)
        {
            return new MinimaxSearch<S, A, P>(game);
        }

        public MinimaxSearch(IGame<S, A, P> game)
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
                double value = minValue(game.GetResult(state, action), player);
                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }
            return result;
        }

        public double maxValue(S state, P player)
        { // returns an utility
          // value
            metrics.incrementInt(METRICS_NODES_EXPANDED);
            if (game.IsTerminal(state))
                return game.GetUtility(state, player);
            double value = double.NegativeInfinity;
            foreach (A action in game.GetActions(state))
                value = System.Math.Max(value,
                        minValue(game.GetResult(state, action), player));
            return value;
        }

        public double minValue(S state, P player)
        { // returns an utility
          // value
            metrics.incrementInt(METRICS_NODES_EXPANDED);
            if (game.IsTerminal(state))
                return game.GetUtility(state, player);
            double value = double.PositiveInfinity;
            foreach (A action in game.GetActions(state))
                value = System.Math.Min(value,
                        maxValue(game.GetResult(state, action), player));
            return value;
        }

        public Metrics getMetrics()
        {
            return metrics;
        }
    }
}