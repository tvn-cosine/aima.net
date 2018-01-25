using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.qsearch;

namespace aima.net.search.uninformed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.14, page
     * 84.<br>
     * <br>
     * 
     * <pre>
     * function UNIFORM-COST-SEARCH(problem) returns a solution, or failure
     *   node &lt;- a node with STATE = problem.INITIAL-STATE, PATH-COST = 0
     *   frontier &lt;- a priority queue ordered by PATH-COST, with node as the only element
     *   explored &lt;- an empty set
     *   loop do
     *      if EMPTY?(frontier) then return failure
     *      node &lt;- POP(frontier) // chooses the lowest-cost node in frontier
     *      if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
     *      add node.STATE to explored
     *      for each action in problem.ACTIONS(node.STATE) do
     *          child &lt;- CHILD-NODE(problem, node, action)
     *          if child.STATE is not in explored or frontier then
     *             frontier &lt;- INSERT(child, frontier)
     *          else if child.STATE is in frontier with higher PATH-COST then
     *             replace that frontier node with child
     * </pre>
     * 
     * Figure 3.14 Uniform-cost search on a graph. The algorithm is identical to the
     * general graph search algorithm in Figure 3.7, except for the use of a
     * priority queue and the addition of an extra check in case a shorter path to a
     * frontier state is discovered.
     * 
     * </br>
     * This implementation is more general. It supports TreeSearch, GraphSearch, and
     * BidirectionalSearch by delegating the search space exploration to an instance
     * of a QueueSearch implementation.
     *
     * @author Ruediger Lunde
     * @author Ciaran O'Reilly
     */
    public class UniformCostSearch<S, A> : QueueBasedSearch<S, A>
    {
        class UniformCostSearchComparer : IComparer<Node<S, A>>
        {
            private readonly System.Collections.Generic.Comparer<double> comparer = System.Collections.Generic.Comparer<double>.Default;
            public int Compare(Node<S, A> x, Node<S, A> y)
            {
                return comparer.Compare(x.getPathCost(), y.getPathCost());
            }
        }
        /** Creates a UniformCostSearch instance using GraphSearch */
        public UniformCostSearch()
                : this(new GraphSearch<S, A>())
        { }

        /**
         * Combines UniformCostSearch queue definition with the specified
         * search space exploration strategy.
         */
        public UniformCostSearch(QueueSearch<S, A> impl)
                : base(impl, CollectionFactory.CreatePriorityQueue<Node<S, A>>(new UniformCostSearchComparer()))
        { }
    }
}
