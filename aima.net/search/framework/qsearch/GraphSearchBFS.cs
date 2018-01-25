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
     * {@link QueueSearch#findNode(Problem, Queue)} of the superclass and
     * provides implementations for the needed primitive operations. It is the most
     * efficient variant of graph search for breadth first search. But don't expect
     * shortest paths in combination with priority queue frontiers.
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class GraphSearchBFS<S, A> : QueueSearch<S, A>
    { 
        private ISet<S> explored = CollectionFactory.CreateSet<S>();
        private ISet<S> frontierStates = CollectionFactory.CreateSet<S>();

        public GraphSearchBFS()
            : this(new NodeExpander<S, A>())
        {  }

        public GraphSearchBFS(NodeExpander<S, A> nodeExpander)
            : base(nodeExpander)
        {  }


        /**
         * Clears the set of explored states and calls the search implementation of
         * <code>QueSearch</code>
         */

        public override Node<S, A> findNode(IProblem<S, A> problem, ICollection<Node<S, A>> frontier)
        {
            // Initialize the explored set to be empty
            explored.Clear();
            frontierStates.Clear();
            return base.findNode(problem, frontier);
        }

        /**
         * Inserts the node at the tail of the frontier if the corresponding state
         * is not already a frontier state and was not yet explored.
         */

        protected override void addToFrontier(Node<S, A> node)
        {
            if (!explored.Contains(node.getState()) && !frontierStates.Contains(node.getState()))
            {
                frontier.Add(node);
                frontierStates.Add(node.getState());
                updateMetrics(frontier.Size());
            }
        }

        /**
         * Removes the node at the head of the frontier, adds the corresponding
         * state to the explored set, and returns the node.
         * 
         * @return the node at the head of the frontier.
         */

        protected override Node<S, A> removeFromFrontier()
        {
            Node<S, A> result = frontier.Pop();
            explored.Add(result.getState());
            frontierStates.Remove(result.getState());
            updateMetrics(frontier.Size());
            return result;
        }

        /**
         * Checks whether there are still some nodes left.
         */

        protected override bool isFrontierEmpty()
        {
            return frontier.IsEmpty();
        }
    }

}
