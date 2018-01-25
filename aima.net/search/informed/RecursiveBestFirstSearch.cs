using aima.net;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.informed.api;
using aima.net.util;
using aima.net.util.api;

namespace aima.net.search.informed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.26, page
     * 99.<br>
     * <br>
     * <p>
     * <pre>
     * function RECURSIVE-BEST-FIRST-SEARCH(problem) returns a solution, or failure
     *   return RBFS(problem, MAKE-NODE(problem.INITIAL-STATE), infinity)
     *
     * function RBFS(problem, node, f_limit) returns a solution, or failure and a new f-cost limit
     *   if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
     *   successors &lt;- []
     *   for each action in problem.ACTION(node.STATE) do
     *       add CHILD-NODE(problem, node, action) into successors
     *   if successors is empty then return failure, infinity
     *   for each s in successors do // update f with value from previous search, if any
     *     s.f &lt;- max(s.g + s.h, node.f)
     *   repeat
     *     best &lt;- the lowest f-value node in successors
     *     if best.f &gt; f_limit then return failure, best.f
     *     alternative &lt;- the second-lowest f-value among successors
     *     result, best.f &lt;- RBFS(problem, best, min(f_limit, alternative))
     *     if result != failure then return result
     * </pre>
     * <p>
     * Figure 3.26 The algorithm for recursive best-first search.
     * <p>
     * <br>
     * This version additionally provides an option to avoid loops. States on the
     * current path are stored in a hash set if the loop avoidance option is enabled.
     *
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * @author Ruediger Lunde
     */
    public class RecursiveBestFirstSearch<S, A> : ISearchForActions<S, A>, IInformed<S, A>
    { 
        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_MAX_RECURSIVE_DEPTH = "maxRecursiveDepth";
        public const string METRIC_PATH_COST = "pathCost";

        private const double INFINITY = double.MaxValue;

        private readonly IToDoubleFunction<Node<S, A>> evalFn;
        private bool avoidLoops;
        private readonly NodeExpander<S, A> nodeExpander;

        // stores the states on the current path if avoidLoops is true.
        private ISet<S> explored = CollectionFactory.CreateSet<S>();
        private Metrics metrics;

        public RecursiveBestFirstSearch(IToDoubleFunction<Node<S, A>> evalFn)
            : this(evalFn, false)
        { }

        /**
         * Constructor which allows to enable the loop avoidance strategy.
         */
        public RecursiveBestFirstSearch(IToDoubleFunction<Node<S, A>> evalFn, bool avoidLoops)
            : this(evalFn, avoidLoops, new NodeExpander<S, A>())
        { }

        public RecursiveBestFirstSearch(IToDoubleFunction<Node<S, A>> evalFn, bool avoidLoops,
                                        NodeExpander<S, A> nodeExpander)
        {
            this.evalFn = evalFn;
            this.avoidLoops = avoidLoops;
            this.nodeExpander = nodeExpander;
            nodeExpander.addNodeListener((node) => metrics.incrementInt(METRIC_NODES_EXPANDED));
            metrics = new Metrics();
        }

        /**
         * Modifies the evaluation function if it is a {@link HeuristicEvaluationFunction}.
         */
          
        public void setHeuristicFunction(IToDoubleFunction<Node<S, A>> h)
        {
            if (evalFn is HeuristicEvaluationFunction<S, A>)
                ((HeuristicEvaluationFunction<S, A>)evalFn).setHeuristicFunction(h);
        }

        // function RECURSIVE-BEST-FIRST-SEARCH(problem) returns a solution, or failure 
        public ICollection<A> findActions(IProblem<S, A> p)
        {
            explored.Clear();
            clearMetrics();

            // RBFS(problem, MAKE-NODE(INITIAL-STATE[problem]), infinity)
            Node<S, A> n = nodeExpander.createRootNode(p.getInitialState());
            SearchResult sr = rbfs(p, n, evalFn.applyAsDouble(n), INFINITY, 0);
            if (sr.hasSolution())
            {
                Node<S, A> s = sr.getSolutionNode();
                metrics.set(METRIC_PATH_COST, s.getPathCost());
                return SearchUtils.getSequenceOfActions(s);
            }
            return null;
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
            metrics.set(METRIC_MAX_RECURSIVE_DEPTH, 0);
            metrics.set(METRIC_PATH_COST, 0.0);
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
        // function RBFS(problem, node, f_limit) returns a solution, or failure and
        // a new f-cost limit
        private SearchResult rbfs(IProblem<S, A> p, Node<S, A> node, double node_f, double fLimit, int recursiveDepth)
        {
            updateMetrics(recursiveDepth);

            // if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
            if (p.testSolution(node))
                return getResult(null, node, fLimit);

            // successors <- []
            // for each action in problem.ACTION(node.STATE) do
            // add CHILD-NODE(problem, node, action) into successors
            ICollection<Node<S, A>> successors = expandNode(node, p);

            // if successors is empty then return failure, infinity
            if (successors.IsEmpty())
                return getResult(node, null, INFINITY);

            double[] f = new double[successors.Size()];
            // for each s in successors do
            // update f with value from previous search, if any
            int size = successors.Size();
            for (int s = 0; s < size; s++)
            {
                // s.f <- max(s.g + s.h, node.f)
                f[s] = System.Math.Max(evalFn.applyAsDouble(successors.Get(s)), node_f);
            }

            // repeat
            while (true)
            {
                // best <- the lowest f-value node in successors
                int bestIndex = getBestFValueIndex(f);
                // if best.f > f_limit then return failure, best.f
                if (f[bestIndex] > fLimit)
                {
                    return getResult(node, null, f[bestIndex]);
                }
                // if best.f > f_limit then return failure, best.f
                int altIndex = getNextBestFValueIndex(f, bestIndex);
                // result, best.f <- RBFS(problem, best, min(f_limit, alternative))
                SearchResult sr = rbfs(p, successors.Get(bestIndex), f[bestIndex], System.Math.Min(fLimit, f[altIndex]), recursiveDepth + 1);
                f[bestIndex] = sr.getFCostLimit();
                // if result != failure then return result
                if (sr.hasSolution())
                {
                    return getResult(node, sr.getSolutionNode(), sr.getFCostLimit());
                }
            }
        }

        // the lowest f-value node
        private int getBestFValueIndex(double[] f)
        {
            int lidx = 0;
            double lowestSoFar = INFINITY;

            for (int i = 0; i < f.Length;++i)
            {
                if (f[i] < lowestSoFar)
                {
                    lowestSoFar = f[i];
                    lidx = i;
                }
            }

            return lidx;
        }

        // the second-lowest f-value
        private int getNextBestFValueIndex(double[] f, int bestIndex)
        {
            // Array may only contain 1 item (i.e. no alternative),
            // therefore default to bestIndex initially
            int lidx = bestIndex;
            double lowestSoFar = INFINITY;

            for (int i = 0; i < f.Length;++i)
            {
                if (i != bestIndex && f[i] < lowestSoFar)
                {
                    lowestSoFar = f[i];
                    lidx = i;
                }
            }

            return lidx;
        }

        private ICollection<Node<S, A>> expandNode(Node<S, A> node, IProblem<S, A> problem)
        {
            ICollection<Node<S, A>> result = nodeExpander.expand(node, problem);
            if (avoidLoops)
            {
                explored.Add(node.getState());
                foreach (Node<S, A> n in nodeExpander.expand(node, problem))
                {
                    if (explored.Contains(n.getState()))
                    {
                        result.Remove(n);
                    }
                }
            }
            return result;
        }

        private SearchResult getResult(Node<S, A> currNode, Node<S, A> solutionNode, double fCostLimit)
        {
            if (avoidLoops && currNode != null)
                explored.Remove(currNode.getState());
            return new SearchResult(solutionNode, fCostLimit);
        }

        /**
         * Increases the maximum recursive depth if the specified depth is greater
         * than the current maximum.
         *
         * @param recursiveDepth the depth of the current path
         */
        private void updateMetrics(int recursiveDepth)
        {
            int maxRdepth = metrics.getInt(METRIC_MAX_RECURSIVE_DEPTH);
            if (recursiveDepth > maxRdepth)
            {
                metrics.set(METRIC_MAX_RECURSIVE_DEPTH, recursiveDepth);
            }
        }


        private class SearchResult
        { 
            private Node<S, A> solNode;
            private readonly double fCostLimit;

            public SearchResult(Node<S, A> solutionNode, double fCostLimit)
            {
                this.solNode = solutionNode;
                this.fCostLimit = fCostLimit;
            }

            public bool hasSolution()
            {
                return solNode != null;
            }

            public Node<S, A> getSolutionNode()
            {
                return solNode;
            }

            public double getFCostLimit()
            {
                return fCostLimit;
            }
        }
    }

}
