using aima.net;
using aima.net.api;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.informed;
using aima.net.search.informed.api;
using aima.net.util;
using aima.net.util.api;

namespace aima.net.search.local
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.2, page
     * 122.<br>
     * <br>
     * <p>
     * <pre>
     * function HILL-CLIMBING(problem) returns a state that is a local maximum
     *
     *   current &lt;- MAKE-NODE(problem.INITIAL-STATE)
     *   loop do
     *     neighbor &lt;- a highest-valued successor of current
     *     if neighbor.VALUE &lt;= current.VALUE then return current.STATE
     *     current &lt;- neighbor
     * </pre>
     * <p>
     * Figure 4.2 The hill-climbing search algorithm, which is the most basic local
     * search technique. At each step the current node is replaced by the best
     * neighbor; in this version, that means the neighbor with the highest VALUE,
     * but if a heuristic cost estimate h is used, we would find the neighbor with
     * the lowest h.
     *
     * @author Ravi Mohan
     * @author Mike Stampone
     * @author Ruediger Lunde
     */
    public class HillClimbingSearch<S, A> : ISearchForActions<S, A>, ISearchForStates<S, A>, IInformed<S, A>
    {
        public enum SearchOutcome
        {
            FAILURE, SOLUTION_FOUND
        }

        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_NODE_VALUE = "nodeValue";

        private IToDoubleFunction<Node<S, A>> h = null;
        private NodeExpander<S, A> nodeExpander;
        private SearchOutcome outcome = SearchOutcome.FAILURE;
        private S lastState = default(S);
        private Metrics metrics = new Metrics();

        /**
         * Constructs a hill-climbing search from the specified heuristic function.
         *
         * @param h a heuristic function
         */
        public HillClimbingSearch(IToDoubleFunction<Node<S, A>> h)
            : this(h, new NodeExpander<S, A>())
        { }

        public HillClimbingSearch(IToDoubleFunction<Node<S, A>> h, NodeExpander<S, A> nodeExpander)
        {
            this.h = h;
            this.nodeExpander = nodeExpander;
            nodeExpander.addNodeListener((node) => metrics.incrementInt(METRIC_NODES_EXPANDED));
        }


        public void setHeuristicFunction(IToDoubleFunction<Node<S, A>> h)
        {
            this.h = h;
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
        /**
         * Returns a node corresponding to a local maximum or empty if the search was
         * cancelled by the user.
         *
         * @param p the search problem
         * @return a node or empty
         */
        // function HILL-CLIMBING(problem) returns a state that is a local maximum
        public Node<S, A> findNode(IProblem<S, A> p)
        {
            clearMetrics();
            outcome = SearchOutcome.FAILURE;
            // current <- MAKE-NODE(problem.INITIAL-STATE)
            Node<S, A> current = nodeExpander.createRootNode(p.getInitialState());
            Node<S, A> neighbor;
            // loop do
            while (!currIsCancelled)
            {
                lastState = current.getState();
                metrics.set(METRIC_NODE_VALUE, getValue(current));
                ICollection<Node<S, A>> children = nodeExpander.expand(current, p);
                // neighbor <- a highest-valued successor of current
                neighbor = getHighestValuedNodeFrom(children);

                // if neighbor.VALUE <= current.VALUE then return current.STATE
                if (neighbor == null || getValue(neighbor) <= getValue(current))
                {
                    if (p.testSolution(current))
                        outcome = SearchOutcome.SOLUTION_FOUND;
                    return current;
                }
                // current <- neighbor
                current = neighbor;
            }
            return null;
        }

        /**
         * Returns SOLUTION_FOUND if the local maximum is a goal state, or FAILURE
         * if the local maximum is not a goal state.
         *
         * @return SOLUTION_FOUND if the local maximum is a goal state, or FAILURE
         * if the local maximum is not a goal state.
         */
        public SearchOutcome getOutcome()
        {
            return outcome;
        }

        /**
         * Returns the last state from which the hill climbing search found the
         * local maximum.
         *
         * @return the last state from which the hill climbing search found the
         * local maximum.
         */
        public S getLastSearchState()
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

        /**
         * Sets all metrics to zero.
         */
        private void clearMetrics()
        {
            metrics.set(METRIC_NODES_EXPANDED, 0);
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

        private Node<S, A> getHighestValuedNodeFrom(ICollection<Node<S, A>> children)
        {
            double highestValue = double.NegativeInfinity;
            Node<S, A> nodeWithHighestValue = null;
            foreach (Node<S, A> child in children)
            {
                double value = getValue(child);
                if (value > highestValue)
                {
                    highestValue = value;
                    nodeWithHighestValue = child;
                }
            }
            return nodeWithHighestValue;
        }

        private double getValue(Node<S, A> n)
        {
            // assumption greater heuristic value =>
            // HIGHER on hill; 0 == goal state;
            return -1 * h.applyAsDouble(n);
        }
    }
}
