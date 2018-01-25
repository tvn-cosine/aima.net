using aima.net;
using aima.net.api;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.util;
using aima.net.util.api;

namespace aima.net.search.local
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.5, page
     * 126.<br>
     * <br>
     * 
     * <pre>
     * function SIMULATED-ANNEALING(problem, schedule) returns a solution state
     *                    
     *   current &lt;- MAKE-NODE(problem.INITIAL-STATE)
     *   for t = 1 to INFINITY do
     *     T &lt;- schedule(t)
     *     if T = 0 then return current
     *     next &lt;- a randomly selected successor of current
     *     /\E &lt;- next.VALUE - current.value
     *     if /\E &gt; 0 then current &lt;- next
     *     else current &lt;- next only with probability e&circ;(/\E/T)
     * </pre>
     * 
     * Figure 4.5 The simulated annealing search algorithm, a version of stochastic
     * hill climbing where some downhill moves are allowed. Downhill moves are
     * accepted readily early in the annealing schedule and then less often as time
     * goes on. The schedule input determines the value of the temperature T as a
     * function of time.
     * 
     * @author Ravi Mohan
     * @author Mike Stampone
     * @author Ruediger Lunde
     */
    public class SimulatedAnnealingSearch<S, A> : ISearchForActions<S, A>, ISearchForStates<S, A>
    {
        public enum SearchOutcome
        {
            FAILURE, SOLUTION_FOUND
        }

        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_TEMPERATURE = "temp";
        public const string METRIC_NODE_VALUE = "nodeValue";

        private readonly IToDoubleFunction<Node<S, A>> h;
        private readonly Scheduler scheduler;
        private readonly NodeExpander<S, A> nodeExpander;

        private SearchOutcome outcome = SearchOutcome.FAILURE;
        private S lastState;
        private Metrics metrics = new Metrics();

        /**
         * Constructs a simulated annealing search from the specified heuristic
         * function and a default scheduler.
         * 
         * @param h
         *            a heuristic function
         */
        public SimulatedAnnealingSearch(IToDoubleFunction<Node<S, A>> h)
            : this(h, new Scheduler())
        { }

        /**
         * Constructs a simulated annealing search from the specified heuristic
         * function and scheduler.
         * 
         * @param h
         *            a heuristic function
         * @param scheduler
         *            a mapping from time to "temperature"
         */
        public SimulatedAnnealingSearch(IToDoubleFunction<Node<S, A>> h, Scheduler scheduler)
            : this(h, scheduler, new NodeExpander<S, A>())
        { }

        public SimulatedAnnealingSearch(IToDoubleFunction<Node<S, A>> h, Scheduler scheduler, NodeExpander<S, A> nodeExpander)
        {
            this.h = h;
            this.scheduler = scheduler;
            this.nodeExpander = nodeExpander;
            nodeExpander.addNodeListener((node) => metrics.incrementInt(METRIC_NODES_EXPANDED));
        }


        public ICollection<A> findActions(IProblem<S, A> p)
        {
            nodeExpander.useParentLinks(true);
            return SearchUtils.toActions(findNode(p));
        }


        public S findState(IProblem<S, A> p)
        {
            nodeExpander.useParentLinks(false);
            return SearchUtils.toState(findNode(p));
        }


        private bool currIsCancelled;

        public void SetCurrIsCancelled(bool value)
        {
            currIsCancelled = value;
        }

        public bool GetCurrIsCancelled()
        {
            return currIsCancelled;
        }

        // function SIMULATED-ANNEALING(problem, schedule) returns a solution state
        public Node<S, A> findNode(IProblem<S, A> p)
        {
            clearMetrics();
            outcome = SearchOutcome.FAILURE;
            lastState = default(S);
            // current <- MAKE-NODE(problem.INITIAL-STATE)
            Node<S, A> current = nodeExpander.createRootNode(p.getInitialState());
            // for t = 1 to INFINITY do
            int timeStep = 0;
            while (!currIsCancelled)
            {
                // temperature <- schedule(t)
                double temperature = scheduler.getTemp(timeStep);
                timeStep++;
                lastState = current.getState();
                // if temperature = 0 then return current
                if (temperature == 0.0)
                {
                    if (p.testSolution(current))
                        outcome = SearchOutcome.SOLUTION_FOUND;
                    return current;
                }

                updateMetrics(temperature, getValue(current));
                ICollection<Node<S, A>> children = nodeExpander.expand(current, p);
                if (children.Size() > 0)
                {
                    // next <- a randomly selected successor of current
                    Node<S, A> next = Util.selectRandomlyFromList(children);
                    // /\E <- next.VALUE - current.value
                    double deltaE = getValue(next) - getValue(current);

                    if (shouldAccept(temperature, deltaE))
                    {
                        current = next;
                    }
                }
            }
            return null;
        }

        /**
         * Returns <em>e</em><sup>&delta<em>E / T</em></sup>
         * 
         * @param temperature
         *            <em>T</em>, a "temperature" controlling the probability of
         *            downward steps
         * @param deltaE
         *            VALUE[<em>next</em>] - VALUE[<em>current</em>]
         * @return <em>e</em><sup>&delta<em>E / T</em></sup>
         */
        public double probabilityOfAcceptance(double temperature, double deltaE)
        {
            return System.Math.Exp(deltaE / temperature);
        }

        public SearchOutcome getOutcome()
        {
            return outcome;
        }

        /**
         * Returns the last state from which the simulated annealing search found a
         * solution state.
         * 
         * @return the last state from which the simulated annealing search found a
         *         solution state.
         */
        public object getLastSearchState()
        {
            return lastState;
        }

        /**
         * Returns all the search metrics.
         */

        public Metrics getMetrics()
        {
            return metrics;
        }

        private void updateMetrics(double temperature, double value)
        {
            metrics.set(METRIC_TEMPERATURE, temperature);
            metrics.set(METRIC_NODE_VALUE, value);
        }

        /**
         * Sets all metrics to zero.
         */
        private void clearMetrics()
        {
            metrics.set(METRIC_NODES_EXPANDED, 0);
            metrics.set(METRIC_TEMPERATURE, 0);
            metrics.set(METRIC_NODE_VALUE, 0);
        }


        public void addNodeListener(Consumer<Node<S, A>> listener)
        {
            nodeExpander.addNodeListener(listener);
        }


        public bool removeNodeListener(Consumer<Node<S, A>> listener)
        {
            return nodeExpander.removeNodeListener(listener);
        }

        //
        // PRIVATE METHODS
        //

        // if /\E > 0 then current <- next
        // else current <- next only with probability e^(/\E/T)
        private bool shouldAccept(double temperature, double deltaE)
        {
            return (deltaE > 0.0)
                    || (CommonFactory.CreateRandom().NextDouble() <= probabilityOfAcceptance(temperature, deltaE));
        }

        private double getValue(Node<S, A> n)
        {
            // assumption greater heuristic value =>
            // HIGHER on hill; 0 == goal state;
            // SA deals with gradient DESCENT
            return -1 * h.applyAsDouble(n);
        }
    }
}
