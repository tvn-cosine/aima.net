using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.qsearch;

namespace aima.net.search.uninformed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.11, page
     * 82.<br>
     * <br>
     * 
     * <pre>
     * function BREADTH-FIRST-SEARCH(problem) returns a solution, or failure
     *   node &lt;- a node with STATE = problem.INITIAL-STATE, PATH-COST=0
     *   if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
     *   frontier &lt;- a FIFO queue with node as the only element
     *   explored &lt;- an empty set
     *   loop do
     *      if EMPTY?(frontier) then return failure
     *      node &lt;- POP(frontier) // chooses the shallowest node in frontier
     *      add node.STATE to explored
     *      for each action in problem.ACTIONS(node.STATE) do
     *          child &lt;- CHILD-NODE(problem, node, action)
     *          if child.STATE is not in explored or frontier then
     *              if problem.GOAL-TEST(child.STATE) then return SOLUTION(child)
     *              frontier &lt;- INSERT(child, frontier)
     * </pre>
     * 
     * Figure 3.11 Breadth-first search on a graph.<br>
     * <br>
     * <b>Note:</b> Supports TreeSearch, GraphSearch, and BidirectionalSearch. Just
     * provide an instance of the desired QueueSearch implementation to the
     * constructor!
     *
     * @author Ruediger Lunde
     * @author Ciaran O'Reilly
     */
    public class BreadthFirstSearch<S, A> : QueueBasedSearch<S, A>
    {
        public BreadthFirstSearch()
                : this(new GraphSearch<S, A>())
        {

        }

        public BreadthFirstSearch(QueueSearch<S, A> impl)
                : base(impl, CollectionFactory.CreateFifoQueue<Node<S, A>>())
        {

            // Goal test is to be applied to each node when it is generated
            // rather than when it is selected for expansion.
            impl.setEarlyGoalTest(true);
        }
    }
}
