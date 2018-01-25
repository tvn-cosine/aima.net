using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.search.framework.qsearch
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 90.<br>
     * <br>
     * Bidirectional search.<br>
     * <br>
     * The strategy of this search implementation is inspired by the description of
     * the bidirectional search algorithm i.e. 'Bidirectional search is implemented
     * by replacing the goal test with a check to see whether the frontiers of the
     * two searches intersect;'. But to gain some worst-case guarantees with respect
     * to solution quality (see below), the goal test of the original and the
     * reverse problem are replaced by a check, whether the node's state was already
     * explored in the other problem. Only one frontier is used which allows to use
     * the same queue search interface as known from other search implementations.
     * This implementation can be combined with many abstractions of search, e.g.
     * BreadthFirstSearch, UniformCostSearch, or even AStarSearch.
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     */
    public class BidirectionalSearch<S, A> : QueueSearch<S, A>
    {
        private const int ORG_P_IDX = 0;
        private const int REV_P_IDX = 1;
        private bool isCancelled;

        /**
         * Controls whether all actions of the reverse problem are tested to be
         * reversible. This shouldn't be necessary for a correctly implemented
         * bidirectional problem. But in case this is not guaranteed, the test is
         * helpful to avoid failures.
         */
        private bool isReverseActionTestEnabled = true;

        // index 0: original problem, index 2: reverse problem
        private ICollection<IMap<S, ExtendedNode>> explored;
        private ExtendedNode goalStateNode;

        public BidirectionalSearch()
            : this(new NodeExpander<S, A>())
        { }

        public BidirectionalSearch(NodeExpander<S, A> nodeExpander)
            : base(nodeExpander)
        {
            explored = CollectionFactory.CreateQueue<IMap<S, ExtendedNode>>();
            explored.Add(CollectionFactory.CreateInsertionOrderedMap<S, ExtendedNode>());
            explored.Add(CollectionFactory.CreateInsertionOrderedMap<S, ExtendedNode>());
        }

        public bool GetIsCancelled()
        {
            return isCancelled;
        }

        public void SetIsCancelled(bool isCancelled)
        {
            this.isCancelled = isCancelled;
        }

        /**
         * Implements an approximation algorithm for bidirectional problems with
         * exactly one initial and one goal state. The algorithm guarantees the
         * following: If the queue is ordered by path costs (uniform cost search),
         * the path costs of the solution will be less or equal to the costs of the
         * best solution multiplied with two. Especially, if all step costs are
         * equal and the reverse problem provides reverse actions for all actions of
         * the original problem, the path costs of the result will exceed the
         * optimal path by the costs of one step at maximum.
         * 
         * @param problem
         *            a bidirectional search problem
         * @param frontier
         *            the data structure to be used to decide which node to be
         *            expanded next
         * 
         * @return a list of actions to the goal if the goal was found, a list
         *         containing a single NoOp Action if already at the goal, or an
         *         empty list if the goal could not be found.
         */
        public override Node<S, A> findNode(IProblem<S, A> problem, ICollection<Node<S, A>> frontier)
        {
            if (!(problem is IBidirectionalProblem<S, A>))
            {
                throw new IllegalArgumentException("problem is not a BidirectionalProblem<S, A>");
            }
            this.isCancelled = false;
            nodeExpander.useParentLinks(true); // bidirectional search needs parents!
            this.frontier = frontier;
            clearMetrics();
            explored.Get(ORG_P_IDX).Clear();
            explored.Get(REV_P_IDX).Clear();

            IProblem<S, A> orgP = ((IBidirectionalProblem<S, A>)problem).getOriginalProblem();
            IProblem<S, A> revP = ((IBidirectionalProblem<S, A>)problem).getReverseProblem();
            ExtendedNode initStateNode;
            initStateNode = new ExtendedNode(nodeExpander.createRootNode(orgP.getInitialState()), ORG_P_IDX);
            goalStateNode = new ExtendedNode(nodeExpander.createRootNode(revP.getInitialState()), REV_P_IDX);

            if (orgP.getInitialState().Equals(revP.getInitialState()))
                return getSolution(orgP, initStateNode, goalStateNode);

            // initialize the frontier using the initial state of the problem
            addToFrontier(initStateNode);
            addToFrontier(goalStateNode);

            while (!isFrontierEmpty() && !this.isCancelled)
            {
                // choose a leaf node and remove it from the frontier
                ExtendedNode nodeToExpand = (ExtendedNode)removeFromFrontier();
                ExtendedNode nodeFromOtherProblem;

                // if the node contains a goal state then return the
                // corresponding solution
                if (!earlyGoalTest && (nodeFromOtherProblem = getCorrespondingNodeFromOtherProblem(nodeToExpand)) != null)
                    return getSolution(orgP, nodeToExpand, nodeFromOtherProblem);

                // expand the chosen node, adding the resulting nodes to the
                // frontier
                foreach (Node<S, A> s in nodeExpander.expand(nodeToExpand, problem))
                {
                    ExtendedNode successor = new ExtendedNode(s, nodeToExpand.getProblemIndex());
                    if (!isReverseActionTestEnabled || nodeToExpand.getProblemIndex() == ORG_P_IDX
                            || getReverseAction(orgP, successor) != null)
                    {

                        if (earlyGoalTest
                                && (nodeFromOtherProblem = getCorrespondingNodeFromOtherProblem(successor)) != null)
                            return getSolution(orgP, successor, nodeFromOtherProblem);

                        addToFrontier(successor);
                    }
                }
            }
            // if the frontier is empty then return failure
            return null;
        }

        /**
         * Enables a check for all actions offered by the reverse problem whether
         * there exists a corresponding action of the original problem. Default
         * value is true.
         */
        public void setReverseActionTestEnabled(bool b)
        {
            isReverseActionTestEnabled = b;
        }

        /**
         * Inserts the node at the tail of the frontier if the corresponding state
         * is not yet explored.
         */

        protected override void addToFrontier(Node<S, A> node)
        {
            if (!isExplored(node))
            {
                frontier.Add(node);
                updateMetrics(frontier.Size());
            }
        }

        /**
         * Cleans up the head of the frontier, removes the first node of a
         * non-explored state from the head of the frontier, adds it to the
         * corresponding explored map, and returns the node.
         * 
         * @return A node of a not yet explored state.
         */

        protected override Node<S, A> removeFromFrontier()
        {
            cleanUpFrontier(); // not really necessary because isFrontierEmpty
                               // should be called before...
            Node<S, A> result = frontier.Pop();
            updateMetrics(frontier.Size());
            // add the node to the explored set of the corresponding problem
            setExplored(result);
            return result;
        }

        /**
         * Pops nodes of already explored states from the head of the frontier and
         * checks whether there are still some nodes left.
         */

        protected override bool isFrontierEmpty()
        {
            cleanUpFrontier();
            updateMetrics(frontier.Size());
            return frontier.IsEmpty();
        }

        /**
         * Helper method which removes nodes of already explored states from the
         * head of the frontier.
         */
        private void cleanUpFrontier()
        {
            while (!frontier.IsEmpty() && isExplored(frontier.Peek()))
                frontier.Pop();
        }

        /**
         * Computes a node whose sequence of recursive parents corresponds to a
         * sequence of actions which leads from the initial state of the original
         * problem to the state of node1 and then to the initial state of the
         * reverse problem, following reverse actions to parents of node2. Note that
         * both nodes must be linked to the same state. Success is not guaranteed if
         * some actions cannot be reversed.
         */
        private Node<S, A> getSolution(IProblem<S, A> orgP, ExtendedNode node1, ExtendedNode node2)
        {
            if (!node1.getState().Equals(node2.getState()))
            {
                throw new NotSupportedException("states not equal");
            }

            Node<S, A> orgNode = node1.getProblemIndex() == ORG_P_IDX ? node1 : node2;
            Node<S, A> revNode = node1.getProblemIndex() == REV_P_IDX ? node1 : node2;

            while (revNode.getParent() != null)
            {
                A action = getReverseAction(orgP, revNode);
                if (action != null)
                {
                    S nextState = revNode.getParent().getState();
                    double stepCosts = orgP.getStepCosts(revNode.getState(), action, nextState);
                    orgNode = nodeExpander.createNode(nextState, orgNode, action, stepCosts);
                    revNode = revNode.getParent();
                }
                else
                {
                    return null;
                }
            }
            metrics.set(METRIC_PATH_COST, orgNode.getPathCost());
            return orgNode;
        }

        /**
         * Returns the action which leads from the state of <code>node</code> to the
         * state of the node's parent, if such an action exists in problem
         * <code>orgP</code>.
         */
        private A getReverseAction(IProblem<S, A> orgP, Node<S, A> node)
        {
            S currState = node.getState();
            S nextState = node.getParent().getState();

            foreach (A action in orgP.getActions(currState))
            {
                S aResult = orgP.getResult(currState, action);
                if (nextState.Equals(aResult))
                    return action;
            }
            return default(A);
        }


        private bool isExplored(Node<S, A> node)
        {
            ExtendedNode eNode = (ExtendedNode)node;
            return explored.Get(eNode.getProblemIndex()).ContainsKey(eNode.getState());
        }


        private void setExplored(Node<S, A> node)
        {
            ExtendedNode eNode = (ExtendedNode)node;
            explored.Get(eNode.getProblemIndex()).Put(eNode.getState(), eNode);
        }

        private ExtendedNode getCorrespondingNodeFromOtherProblem(ExtendedNode node)
        {
            ExtendedNode result = explored.Get(1 - node.getProblemIndex()).Get(node.getState());

            // Caution: The goal test of the original problem should always include
            // the root node of the reverse problem as that node might not yet have
            // been explored yet. This is important if the reverse problem does not
            // provide reverse actions for all original problem actions.
            if (result == null && node.getProblemIndex() == ORG_P_IDX && node.getState().Equals(goalStateNode.getState()))
                result = goalStateNode;
            return result;
        }

        /**
         * Maintains the usual node data and additionally the index of the problem
         * to which the node belongs.
         * 
         * @author Ruediger Lunde
         *
         */
        class ExtendedNode : Node<S, A>
        {
            int problemIndex;

            public ExtendedNode(Node<S, A> node, int problemIndex)
                  : base(node.getState(), node.getParent(), node.getAction(), node.getPathCost())
            {
                this.problemIndex = problemIndex;
            }

            public int getProblemIndex()
            {
                return problemIndex;
            }

            public override string ToString()
            {
                return "[" + getState() + ":" + problemIndex + "]";
            }
        }
    }
}
