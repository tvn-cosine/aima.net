﻿using aima.net.collections.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.search.framework.qsearch
{
    /**
     * Base class for queue-based search implementations, especially for
     * {@link TreeSearch}, {@link GraphSearch}, and {@link BidirectionalSearch}. It
     * provides a template method for controlling search execution and defines
     * primitive methods encapsulating frontier access. Tree search implementations
     * will implement frontier access straight-forward. Graph search implementations
     * will add node filtering mechanisms to avoid that nodes of already explored
     * states are selected for expansion.
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public abstract class QueueSearch<S, A>
    {
        public const string METRIC_NODES_EXPANDED = "nodesExpanded";
        public const string METRIC_QUEUE_SIZE = "queueSize";
        public const string METRIC_MAX_QUEUE_SIZE = "maxQueueSize";
        public const string METRIC_PATH_COST = "pathCost";

        protected NodeExpander<S, A> nodeExpander;
        protected ICollection<Node<S, A>> frontier;
        protected bool earlyGoalTest = false;
        protected Metrics metrics = new Metrics();

        /** Stores the provided node expander and adds a node listener to it. */
        protected QueueSearch(NodeExpander<S, A> nodeExpander)
        {
            this.nodeExpander = nodeExpander;
            nodeExpander.addNodeListener((node) => metrics.incrementInt(METRIC_NODES_EXPANDED));
        }

        /**
         * Receives a problem and a queue implementing the search strategy and
         * computes a node referencing a goal state, if such a state was found.
         * This template method provides a base for tree and graph search
         * implementations. It can be customized by overriding some primitive
         * operations, especially {@link #addToFrontier(Node)},
         * {@link #removeFromFrontier()}, and {@link #isFrontierEmpty()}.
         * 
         * @param problem
         *            the search problem
         * @param frontier
         *            the data structure for nodes that are waiting to be expanded
         * 
         * @return a node referencing a goal state, if the goal was found, otherwise empty;
         */
        public virtual Node<S, A> findNode(IProblem<S, A> problem, ICollection<Node<S, A>> frontier)
        {
            this.frontier = frontier;
            clearMetrics();
            // initialize the frontier using the initial state of the problem
            Node<S, A> root = nodeExpander.createRootNode(problem.getInitialState());
            addToFrontier(root);
            if (earlyGoalTest && problem.testSolution(root))
                return getSolution(root);

            while (!isFrontierEmpty() && !currIsCancelled)
            {
                // choose a leaf node and remove it from the frontier
                Node<S, A> nodeToExpand = removeFromFrontier();
                // only need to check the nodeToExpand if have not already
                // checked before adding to the frontier
                if (!earlyGoalTest && problem.testSolution(nodeToExpand))
                    // if the node contains a goal state then return the
                    // corresponding solution
                    return getSolution(nodeToExpand);

                // expand the chosen node, adding the resulting nodes to the
                // frontier
                foreach (Node<S, A> successor in nodeExpander.expand(nodeToExpand, problem))
                {
                    addToFrontier(successor);
                    if (earlyGoalTest && problem.testSolution(successor))
                        return getSolution(successor);
                }
            }
            // if the frontier is empty then return failure
            return null;
        }

        /**
         * Primitive operation which inserts the node at the tail of the frontier.
         */
        protected abstract void addToFrontier(Node<S, A> node);

        /**
         * Primitive operation which removes and returns the node at the head of the
         * frontier.
         * 
         * @return the node at the head of the frontier.
         */
        protected abstract Node<S, A> removeFromFrontier();

        /**
         * Primitive operation which checks whether the frontier contains not yet
         * expanded nodes.
         */
        protected abstract bool isFrontierEmpty();

        /**
         * Enables optimization for FIFO queue based search, especially breadth
         * first search.
         */
        public void setEarlyGoalTest(bool b)
        {
            earlyGoalTest = b;
        }

        public NodeExpander<S, A> getNodeExpander()
        {
            return nodeExpander;
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
        protected void clearMetrics()
        {
            metrics.set(METRIC_NODES_EXPANDED, 0);
            metrics.set(METRIC_QUEUE_SIZE, 0);
            metrics.set(METRIC_MAX_QUEUE_SIZE, 0);
            metrics.set(METRIC_PATH_COST, 0);
        }

        protected void updateMetrics(int queueSize)
        {
            metrics.set(METRIC_QUEUE_SIZE, queueSize);
            int maxQSize = metrics.getInt(METRIC_MAX_QUEUE_SIZE);
            if (queueSize > maxQSize)
            {
                metrics.set(METRIC_MAX_QUEUE_SIZE, queueSize);
            }
        }

        private Node<S, A> getSolution(Node<S, A> node)
        {
            metrics.set(METRIC_PATH_COST, node.getPathCost());
            return node;
        }

        private bool currIsCancelled;

        public bool GetCurrIsCancelled()
        {
            return currIsCancelled;
        }

        public void SetCurrIsCancelled(bool currIsCancelled)
        {
            this.currIsCancelled = currIsCancelled;
        }
    }
}
