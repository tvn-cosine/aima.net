using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.search.framework.qsearch
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.7, page 77.
     * <br>
     * 
     * <pre>
     * function GRAPH-SEARCH(problem) returns a solution, or failure
     *   initialize the frontier using the initial state of problem
     *   initialize the explored set to be empty
     *   loop do
     *     if the frontier is empty then return failure
     *     choose a leaf node and remove it from the frontier
     *     if the node contains a goal state then return the corresponding solution
     *     add the node to the explored set
     *     expand the chosen node, adding the resulting nodes to the frontier
     *       only if not in the frontier or explored set
     * </pre>
     * 
     * Figure 3.7 An informal description of the general graph-search algorithm.
     * <br>
     * This implementation is based on the template method
     * {@link QueueSearch#findNode(Problem, Queue)} of the superclass and provides
     * implementations for the needed primitive operations. In contrast to the code
     * above, here, nodes resulting from node expansion are added to the frontier
     * even if nodes for the same states already exist there. This makes it possible
     * to use the implementation also in combination with priority queue frontiers.
     * This implementation avoids linear costs for frontier node removal (compared
     * to {@link GraphSearchReducedFrontier}) and gets by without node comparator
     * knowledge.
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     */
    public class GraphSearch<S, A> : QueueSearch<S, A>
    {
        private ISet<S> explored = CollectionFactory.CreateSet<S>();

        public GraphSearch()
            : this(new NodeExpander<S, A>())
        { }

        public GraphSearch(NodeExpander<S, A> nodeExpander)
                : base(nodeExpander)
        {  }

        /**
         * Clears the set of explored states and calls the search implementation of
         * {@link QueueSearch}.
         */

        public override Node<S, A> findNode(IProblem<S, A> problem, ICollection<Node<S, A>> frontier)
        {
            // initialize the explored set to be empty
            explored.Clear();
            return base.findNode(problem, frontier);
        }

        /**
         * Inserts the node at the tail of the frontier if the corresponding state
         * was not yet explored.
         */

        protected override void addToFrontier(Node<S, A> node)
        {
            if (!explored.Contains(node.getState()))
            {
                frontier.Add(node);
                updateMetrics(frontier.Size());
            }
        }

        /**
         * Removes the node at the head of the frontier, adds the corresponding
         * state to the explored set, and returns the node. Leading nodes of already
         * explored states are dropped. So the resulting node state will always be
         * unexplored yet.
         * 
         * @return the node at the head of the frontier.
         */

        protected override Node<S, A> removeFromFrontier()
        {
            cleanUpFrontier(); // not really necessary because isFrontierEmpty
                               // should be called before...
            Node<S, A> result = frontier.Pop();
            explored.Add(result.getState());
            updateMetrics(frontier.Size());
            return result;
        }

        /**
         * Pops nodes of already explored states from the head of the frontier
         * and checks whether there are still some nodes left.
         */

        protected override bool isFrontierEmpty()
        {
            cleanUpFrontier();
            updateMetrics(frontier.Size());
            return frontier.IsEmpty();
        }

        /**
         * Helper method which removes nodes of already explored states from the head
         * of the frontier.
         */
        private void cleanUpFrontier()
        {
            while (!frontier.IsEmpty() && explored.Contains(frontier.Peek().getState()))
                frontier.Pop();
        }
    }
}
