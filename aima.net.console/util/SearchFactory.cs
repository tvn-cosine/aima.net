using aima.net.search.framework;
using aima.net.search.framework.api;
using aima.net.search.framework.qsearch;
using aima.net.search.informed;
using aima.net.search.local;
using aima.net.search.uninformed;
using aima.net.util;
using aima.net.util.api;

namespace aima.net.demo.util
{
    /**
     * Useful factory for configuring search objects. Implemented as a singleton.
     * @author Ruediger Lunde
     */
    public class SearchFactory
    {

        /** Search strategy: Depth first search. */
        public const int DF_SEARCH = 0;
        /** Search strategy: Depth first search. */
        public const int BF_SEARCH = 1;
        /** Search strategy: Iterative deepening search. */
        public const int ID_SEARCH = 2;
        /** Search strategy: Uniform cost search. */
        public const int UC_SEARCH = 3;
        /** Search strategy: Greedy best first search. */
        public const int GBF_SEARCH = 4;
        /** Search strategy: A* search. */
        public const int ASTAR_SEARCH = 5;
        /** Search strategy: Recursive best first search. */
        public const int RBF_SEARCH = 6;
        /** Search strategy: Recursive best first search avoiding loops. */
        public const int RBF_AL_SEARCH = 7;
        /** Search strategy: Hill climbing search. */
        public const int HILL_SEARCH = 8;

        /** Queue search implementation: tree search. */
        public const int TREE_SEARCH = 0;
        /** Queue search implementation: graph search. */
        public const int GRAPH_SEARCH = 1;
        /** Queue search implementation: graph search with reduced frontier. */
        public const int GRAPH_SEARCH_RED_FRONTIER = 2;
        /** Queue search implementation: graph search for breadth first search. */
        public const int GRAPH_SEARCH_BFS = 3;
        /** Queue search implementation: bidirectional search. */
        public const int BIDIRECTIONAL_SEARCH = 4;

        /** Contains the only existing instance. */
        private static SearchFactory instance;

        /** Invisible constructor. */
        private SearchFactory() { }

        /** Provides access to the factory. Implemented with lazy instantiation. */
        public static SearchFactory getInstance()
        {
            if (instance == null)
                instance = new SearchFactory();
            return instance;
        }

        /**
         * Returns the names of all search strategies, which are supported by this
         * factory. The indices correspond to the parameter values of method
         * {@link #createSearch(int, int, ToDoubleFunction)}.
         */
        public string[] getSearchStrategyNames()
        {
            return new string[] { "Depth First", "Breadth First",
                "Iterative Deepening", "Uniform Cost", "Greedy Best First",
                "A*", "Recursive Best First", "Recursive Best First No Loops", "Hill Climbing" };
        }

        /**
         * Returns the names of all queue search implementation names, which are supported by this
         * factory. The indices correspond to the parameter values of method
         * {@link #createSearch(int, int, ToDoubleFunction)}.
         */
        public string[] getQSearchImplNames()
        {
            return new string[] { "Tree Search", "Graph Search", "Graph Search red Fr.",
                "Graph Search BFS", "Bidirectional Search" };
        }

        /**
         * Creates a search instance.
         * 
         * @param strategy
         *            search strategy. See static constants.
         * @param qSearchImpl
         *            queue search implementation: e.g. {@link #TREE_SEARCH}, {@link #GRAPH_SEARCH}
         * 
         */
        public ISearchForActions<S, A> createSearch<S, A>(int strategy, int qSearchImpl, IToDoubleFunction<Node<S, A>> h)
        {
            QueueSearch<S, A> qs = null;
            ISearchForActions<S, A> result = null;
            switch (qSearchImpl)
            {
                case TREE_SEARCH:
                    qs = new TreeSearch<S, A>();
                    break;
                case GRAPH_SEARCH:
                    qs = new GraphSearch<S, A>();
                    break;
                case GRAPH_SEARCH_RED_FRONTIER:
                    qs = new GraphSearchReducedFrontier<S, A>();
                    break;
                case GRAPH_SEARCH_BFS:
                    qs = new GraphSearchBFS<S, A>();
                    break;
                case BIDIRECTIONAL_SEARCH:
                    qs = new BidirectionalSearch<S, A>();
                    break;
            }
            switch (strategy)
            {
                case DF_SEARCH:
                    result = new DepthFirstSearch<S, A>(qs);
                    break;
                case BF_SEARCH:
                    result = new BreadthFirstSearch<S, A>(qs);
                    break;
                case ID_SEARCH:
                    result = new IterativeDeepeningSearch<S, A>();
                    break;
                case UC_SEARCH:
                    result = new UniformCostSearch<S, A>(qs);
                    break;
                case GBF_SEARCH:
                    result = new GreedyBestFirstSearch<S, A>(qs, h);
                    break;
                case ASTAR_SEARCH:
                    result = new AStarSearch<S, A>(qs, h);
                    break;
                case RBF_SEARCH:
                    result = new RecursiveBestFirstSearch<S, A>(new AStarSearch<S, A>.EvalFunction(h));
                    break;
                case RBF_AL_SEARCH:
                    result = new RecursiveBestFirstSearch<S, A>(new AStarSearch<S, A>.EvalFunction(h), true);
                    break;
                case HILL_SEARCH:
                    result = new HillClimbingSearch<S, A>(h);
                    break;
            }
            return result;
        }
    }

}
