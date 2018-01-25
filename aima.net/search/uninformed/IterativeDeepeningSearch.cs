using aima.net;
using aima.net.api;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.search.uninformed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.18, page
     * 89.<br>
     * <br>
     * 
     * <pre>
     * function ITERATIVE-DEEPENING-SEARCH(problem) returns a solution, or failure
     *   for depth = 0 to infinity  do
     *     result &lt;- DEPTH-LIMITED-SEARCH(problem, depth)
     *     if result != cutoff then return result
     * </pre>
     * 
     * Figure 3.18 The iterative deepening search algorithm, which repeatedly
     * applies depth-limited search with increasing limits. It terminates when a
     * solution is found or if the depth- limited search returns failure, meaning
     * that no solution exists.
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class IterativeDeepeningSearch<S, A> : ISearchForActions<S, A>, ISearchForStates<S, A>
    {
        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_PATH_COST = "pathCost";

        private readonly NodeExpander<S, A> nodeExpander;
        private readonly Metrics metrics;

        public IterativeDeepeningSearch()
            : this(new NodeExpander<S, A>())
        { }

        public IterativeDeepeningSearch(NodeExpander<S, A> nodeExpander)
        {
            this.nodeExpander = nodeExpander;
            this.metrics = new Metrics();
        }


        // function ITERATIVE-DEEPENING-SEARCH(problem) returns a solution, or
        // failure

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
         * Returns a solution node if a solution was found, empty if no solution is reachable or the task was cancelled
         * by the user.
         * @param p
         * @return
         */
        private Node<S, A> findNode(IProblem<S, A> p)
        {
            clearMetrics();
            // for depth = 0 to infinity do
            for (int i = 0; !currIsCancelled;++i)
            {
                // result <- DEPTH-LIMITED-SEARCH(problem, depth)
                DepthLimitedSearch<S, A> dls = new DepthLimitedSearch<S, A>(i, nodeExpander);
                Node<S, A> result = dls.findNode(p);
                updateMetrics(dls.getMetrics());
                // if result != cutoff then return result
                if (!dls.isCutoffResult(result))
                    return result;
            }
            return null;
        }


        public Metrics getMetrics()
        {
            return metrics;
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

        /**
         * Sets the nodes expanded and path cost metrics to zero.
         */
        private void clearMetrics()
        {
            metrics.set(METRIC_NODES_EXPANDED, 0);
            metrics.set(METRIC_PATH_COST, 0);
        }

        private void updateMetrics(Metrics dlsMetrics)
        {
            metrics.set(METRIC_NODES_EXPANDED,
                    metrics.getInt(METRIC_NODES_EXPANDED) + dlsMetrics.getInt(METRIC_NODES_EXPANDED));
            metrics.set(METRIC_PATH_COST, dlsMetrics.getDouble(METRIC_PATH_COST));
        }
    }
}
