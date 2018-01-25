using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.search.framework
{
    /// <summary>
    /// Provides several useful static methods for implementing search.
    /// </summary>
    public class SearchUtils
    {
        /**
         * Returns the path from the root node to this node.
         *
         * @return the path from the root node to this node.
         */
        public static ICollection<Node<S, A>> getPathFromRoot<S, A>(Node<S, A> node)
        {
            ICollection<Node<S, A>> path = CollectionFactory.CreateQueue<Node<S, A>>();
            while (!node.isRootNode())
            {
                path.Insert(0, node);
                node = node.getParent();
            }
            // ensure the root node is added
            path.Insert(0, node);
            return path;
        }

        /**
         * Returns the list of actions which corresponds to the complete path to the
         * given node. The list is empty, if the node is the root node of the search
         * tree.
         */
        public static ICollection<A> getSequenceOfActions<S, A>(Node<S, A> node)
        {
            ICollection<A> actions = CollectionFactory.CreateQueue<A>();
            while (node != null && !node.isRootNode())
            {
                actions.Insert(0, node.getAction());
                node = node.getParent();
            }
            return actions;
        }

        public static ICollection<A> toActions<S, A>(Node<S, A> node)
        {
            return getSequenceOfActions(node);
        }


        public static S toState<S, A>(Node<S, A> node)
        {
            return node == null ? default(S) : node.getState();
        }
    }
}
