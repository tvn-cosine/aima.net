using aima.net.api;
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
     * 
     * <br>
     * This implementation is based on the template method
     * {@link #findNode(Problem, Queue)} of the superclass and provides
     * implementations for the needed primitive operations. It : a special
     * version of graph search which keeps the frontier short by focusing on the
     * best node for each state only. It should only be used in combination with
     * priority queue frontiers. If a node is added to the frontier, this
     * implementation checks whether another node for the same state already exists
     * and decides whether to replace it or ignore the new node depending on the
     * node's costs (comparator of priority queue is used, if available).
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class GraphSearchReducedFrontier<S, A> : QueueSearch<S, A>
    {
        private ISet<S> explored = CollectionFactory.CreateSet<S>();
        private IMap<S, Node<S, A>> frontierNodeLookup = CollectionFactory.CreateInsertionOrderedMap<S, Node<S, A>>();
        private IComparer<Node<S, A>> nodeComparator = null;

        public GraphSearchReducedFrontier()
            : this(new NodeExpander<S, A>())
        { }

        public GraphSearchReducedFrontier(NodeExpander<S, A> nodeExpander)
            : base(nodeExpander)
        { }

        /**
         * Sets the comparator if a priority queue is used, resets explored list and
         * state map and calls the inherited version of search.
         */ 
        public override Node<S, A> findNode(IProblem<S, A> problem, ICollection<Node<S, A>> frontier)
        {
            // initialize the explored set to be empty
            if (frontier is PriorityQueue<Node<S, A>>)
                nodeComparator = ((PriorityQueue<Node<S, A>>)frontier).GetComparer();
            explored.Clear();
            frontierNodeLookup.Clear();
            return base.findNode(problem, frontier);
        }

        public IComparer<Node<S, A>> getNodeComparator()
        {
            return nodeComparator;
        }

        /**
         * Inserts the node into the frontier if the node's state is not yet
         * explored and not present in the frontier. If a second node for the same
         * state is already part of the frontier, it is checked, which node is
         * better (with respect to priority). Depending of the result, the existing
         * node is replaced or the new node is dropped.
         */

        protected override void addToFrontier(Node<S, A> node)
        {
            if (!explored.Contains(node.getState()))
            {
                Node<S, A> frontierNode = frontierNodeLookup.Get(node.getState());
                if (frontierNode == null)
                {
                    // child.STATE is not in frontier and not yet explored
                    frontier.Add(node);
                    frontierNodeLookup.Put(node.getState(), node);
                    updateMetrics(frontier.Size());
                }
                else if (nodeComparator != null && nodeComparator.Compare(node, frontierNode) < 0)
                {
                    // child.STATE is in frontier with higher cost
                    // replace that frontier node with child
                    if (frontier.Remove(frontierNode))
                        frontierNodeLookup.Remove(frontierNode.getState());
                    frontier.Add(node);
                    frontierNodeLookup.Put(node.getState(), node);
                }
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
            frontierNodeLookup.Remove(result.getState());
            // add the node to the explored set
            explored.Add(result.getState());
            updateMetrics(frontier.Size());
            return result;
        }

        /**
         * Checks whether the frontier contains not yet expanded nodes.
         */

        protected override bool isFrontierEmpty()
        {
            return frontier.IsEmpty();
        }
    }
}
