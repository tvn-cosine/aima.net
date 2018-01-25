using aima.net;
using aima.net.collections.api;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.framework.qsearch;

namespace aima.net.search.framework
{
    /**
     * Base class for all search algorithms which use a queue to manage not yet
     * explored nodes. Subclasses are responsible for node prioritization. They
     * define the concrete queue to be used as frontier in their constructor.
     * Search space exploration control is always delegated to some
     * <code>QueueSearch</code> implementation.
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     */
    public abstract class QueueBasedSearch<S, A> : ISearchForActions<S, A>, ISearchForStates<S, A>
    {
        protected readonly QueueSearch<S, A> impl;
        private readonly ICollection<Node<S, A>> frontier;

        protected QueueBasedSearch(QueueSearch<S, A> impl, ICollection<Node<S, A>> queue)
        {
            this.impl = impl;
            this.frontier = queue;
        }

        public virtual ICollection<A> findActions(IProblem<S, A> p)
        {
            impl.getNodeExpander().useParentLinks(true);
            frontier.Clear();
            Node<S, A> node = impl.findNode(p, frontier);
            return SearchUtils.toActions(node);
        }

        public virtual S findState(IProblem<S, A> p)
        {
            impl.getNodeExpander().useParentLinks(false);
            frontier.Clear();
            Node<S, A> node = impl.findNode(p, frontier);
            return SearchUtils.toState(node);
        }

        public virtual Metrics getMetrics()
        {
            return impl.getMetrics();
        }

        public virtual void addNodeListener(Consumer<Node<S, A>> listener)
        {
            impl.getNodeExpander().addNodeListener(listener);
        }

        public virtual bool removeNodeListener(Consumer<Node<S, A>> listener)
        {
            return impl.getNodeExpander().removeNodeListener(listener);
        }
    }
}
