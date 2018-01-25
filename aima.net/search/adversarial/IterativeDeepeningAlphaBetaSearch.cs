using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.search.adversarial.api;
using aima.net.search.framework;

namespace aima.net.search.adversarial
{
    /**
     * Implements an iterative deepening Minimax search with alpha-beta pruning and
     * action ordering. Maximal computation time is specified in seconds. The
     * algorithm is implemented as template method and can be configured and tuned
     * by subclassing.
     *
     * @param <S> Type which is used for states in the game.
     * @param <A> Type which is used for actions in the game.
     * @param <P> Type which is used for players in the game.
     * @author Ruediger Lunde
     */
    public class IterativeDeepeningAlphaBetaSearch<S, A, P> : IAdversarialSearch<S, A>
    {
        public const string METRICS_NODES_EXPANDED = "nodesExpanded";
        public const string METRICS_MAX_DEPTH = "maxDepth";

        protected IGame<S, A, P> game;
        protected double utilMax;
        protected double utilMin;
        protected int currDepthLimit;
        private bool heuristicEvaluationUsed; // indicates that non-terminal
                                              // nodes
                                              // have been evaluated.
        private Timer timer;
        private bool logEnabled;

        private Metrics metrics = new Metrics();

        /**
         * Creates a new search object for a given game.
         *
         * @param game    The game.
         * @param utilMin Utility value of worst state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param utilMax Utility value of best state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param time    Maximal computation time in seconds.
         */
        public static IterativeDeepeningAlphaBetaSearch<STATE, ACTION, PLAYER> createFor<STATE, ACTION, PLAYER>(
                IGame<STATE, ACTION, PLAYER> game, double utilMin, double utilMax, int time)
        {
            return new IterativeDeepeningAlphaBetaSearch<STATE, ACTION, PLAYER>(game, utilMin, utilMax, time);
        }

        /**
         * Creates a new search object for a given game.
         *
         * @param game    The game.
         * @param utilMin Utility value of worst state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param utilMax Utility value of best state for this player. Supports
         *                evaluation of non-terminal states and early termination in
         *                situations with a safe winner.
         * @param time    Maximal computation time in seconds.
         */
        public IterativeDeepeningAlphaBetaSearch(IGame<S, A, P> game, double utilMin, double utilMax, int time)
        {
            this.game = game;
            this.utilMin = utilMin;
            this.utilMax = utilMax;
            this.timer = new Timer(time);
        }

        public virtual void setLogEnabled(bool b)
        {
            logEnabled = b;
        }

        /**
         * Template method controlling the search. It is based on iterative
         * deepening and tries to make to a good decision in limited time. Credit
         * goes to Behi Monsio who had the idea of ordering actions by utility in
         * subsequent depth-limited search runs.
         */

        public virtual A makeDecision(S state)
        {
            metrics = new Metrics();
            IStringBuilder logText = null;
            P player = game.GetPlayer(state);
            ICollection<A> results = orderActions(state, game.GetActions(state), player, 0);
            timer.start();
            currDepthLimit = 0;
            do
            {
                incrementDepthLimit();
                if (logEnabled)
                    logText = TextFactory.CreateStringBuilder("depth " + currDepthLimit + ": ");
                heuristicEvaluationUsed = false;
                ActionStore newResults = new ActionStore();
                foreach (A action in results)
                {
                    double value = minValue(game.GetResult(state, action), player, double.NegativeInfinity,
                            double.PositiveInfinity, 1);
                    if (timer.timeOutOccurred())
                        break; // exit from action loop
                    newResults.add(action, value);
                    if (logEnabled)
                        logText.Append(action).Append("->").Append(value).Append(" ");
                }
                if (logEnabled)
                    System.Console.WriteLine(logText);
                if (newResults.size() > 0)
                {
                    results = newResults.actions;
                    if (!timer.timeOutOccurred())
                    {
                        if (hasSafeWinner(newResults.utilValues.Get(0)))
                            break; // exit from iterative deepening loop
                        else if (newResults.size() > 1
                                && isSignificantlyBetter(newResults.utilValues.Get(0), newResults.utilValues.Get(1)))
                            break; // exit from iterative deepening loop
                    }
                }
            } while (!timer.timeOutOccurred() && heuristicEvaluationUsed);
            return results.Get(0);
        }

        // returns an utility value
        public virtual double maxValue(S state, P player, double alpha, double beta, int depth)
        {
            updateMetrics(depth);
            if (game.IsTerminal(state) || depth >= currDepthLimit || timer.timeOutOccurred())
            {
                return eval(state, player);
            }
            else
            {
                double value = double.NegativeInfinity;
                foreach (A action in orderActions(state, game.GetActions(state), player, depth))
                {
                    value = System.Math.Max(value, minValue(game.GetResult(state, action), //
                            player, alpha, beta, depth + 1));
                    if (value >= beta)
                        return value;
                    alpha = System.Math.Max(alpha, value);
                }
                return value;
            }
        }

        // returns an utility value
        public virtual double minValue(S state, P player, double alpha, double beta, int depth)
        {
            updateMetrics(depth);
            if (game.IsTerminal(state) || depth >= currDepthLimit || timer.timeOutOccurred())
            {
                return eval(state, player);
            }
            else
            {
                double value = double.PositiveInfinity;
                foreach (A action in orderActions(state, game.GetActions(state), player, depth))
                {
                    value = System.Math.Min(value, maxValue(game.GetResult(state, action), //
                            player, alpha, beta, depth + 1));
                    if (value <= alpha)
                        return value;
                    beta = System.Math.Min(beta, value);
                }
                return value;
            }
        }

        private void updateMetrics(int depth)
        {
            metrics.incrementInt(METRICS_NODES_EXPANDED);
            metrics.set(METRICS_MAX_DEPTH, System.Math.Max(metrics.getInt(METRICS_MAX_DEPTH), depth));
        }

        /**
         * Returns some statistic data from the last search.
         */

        public virtual Metrics getMetrics()
        {
            return metrics;
        }

        /**
         * Primitive operation which is called at the beginning of one depth limited
         * search step. This implementation increments the current depth limit by
         * one.
         */
        protected virtual void incrementDepthLimit()
        {
            currDepthLimit++;
        }

        /**
         * Primitive operation which is used to stop iterative deepening search in
         * situations where a clear best action exists. This implementation returns
         * always false.
         */
        protected virtual bool isSignificantlyBetter(double newUtility, double utility)
        {
            return false;
        }

        /**
         * Primitive operation which is used to stop iterative deepening search in
         * situations where a safe winner has been identified. This implementation
         * returns true if the given value (for the currently preferred action
         * result) is the highest or lowest utility value possible.
         */
        protected virtual bool hasSafeWinner(double resultUtility)
        {
            return resultUtility <= utilMin || resultUtility >= utilMax;
        }

        /**
         * Primitive operation, which estimates the value for (not necessarily
         * terminal) states. This implementation returns the utility value for
         * terminal states and <code>(utilMin + utilMax) / 2</code> for non-terminal
         * states. When overriding, first call the super implementation!
         */
        protected virtual double eval(S state, P player)
        {
            if (game.IsTerminal(state))
            {
                return game.GetUtility(state, player);
            }
            else
            {
                heuristicEvaluationUsed = true;
                return (utilMin + utilMax) / 2;
            }
        }

        /**
         * Primitive operation for action ordering. This implementation preserves
         * the original order (provided by the game).
         */
        public virtual ICollection<A> orderActions(S state, ICollection<A> actions, P player, int depth)
        {
            return actions;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // nested helper classes

        private class Timer
        {
            private IDateTime endTime;
            private long duration;

            public Timer(int maxSeconds)
            {
                this.duration = 1000 * maxSeconds;
            }

            public void start()
            {
                endTime = CommonFactory.Now().AddMilliseconds(duration);
            }

            public bool timeOutOccurred()
            {
                return CommonFactory.Now().BiggerThan(endTime);
            }
        }

        /**
         * Orders actions by utility.
         */
        private class ActionStore
        {
            public ICollection<A> actions = CollectionFactory.CreateQueue<A>();
            public ICollection<double> utilValues = CollectionFactory.CreateQueue<double>();

            public void add(A action, double utilValue)
            {
                int idx = 0;
                while (idx < actions.Size() && utilValue <= utilValues.Get(idx))
                    idx++;
                actions.Insert(idx, action);
                utilValues.Insert(idx, utilValue);
            }

            public int size()
            {
                return actions.Size();
            }
        }
    }

}
