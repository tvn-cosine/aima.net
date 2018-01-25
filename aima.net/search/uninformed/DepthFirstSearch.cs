using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.qsearch;

namespace aima.net.search.uninformed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 85.<br>
     * <br>
     * Depth-first search always expands the deepest node in the current frontier of
     * the search tree. <br>
     * <br>
     * <b>Note:</b> Supports TreeSearch, GraphSearch, and BidirectionalSearch. Just
     * provide an instance of the desired QueueSearch implementation to the
     * constructor!
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * 
     */
    public class DepthFirstSearch<S, A> : QueueBasedSearch<S, A>
    {
        public DepthFirstSearch(QueueSearch<S, A> impl)
                : base(impl, CollectionFactory.CreateLifoQueue<Node<S, A>>())
        {

        }
    }
}
