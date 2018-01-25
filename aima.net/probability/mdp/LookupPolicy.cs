using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability.mdp.api;

namespace aima.net.probability.mdp
{
    /// <summary>
    /// Default implementation of the Policy interface using an underlying Map to look up an action associated with a state.
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    /// <typeparam name="A">the action type.</typeparam>
    public class LookupPolicy<S, A> : IPolicy<S, A> where A : IAction
    {
        private IMap<S, A> policy = CollectionFactory.CreateInsertionOrderedMap<S, A>();

        public LookupPolicy(IMap<S, A> aPolicy)
        {
            policy.PutAll(aPolicy);
        }

        public A action(S s)
        {
            return policy.Get(s);
        }
    }
}
