using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.search.framework.problem;
using aima.net.util;
using aima.net.collections;
using aima.net.search.framework.api;
using aima.net.search.framework.problem.api;

namespace aima.net.search.framework.agent
{   
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public class SearchAgent<S, A> : DynamicAgent
        where A : IAction
    { 
        private ICollection<A> actionList; 
        private Metrics searchMetrics;

        public SearchAgent(IProblem<S, A> p, ISearchForActions<S, A> search)
        {
            ICollection<A> actions = search.findActions(p);
            actionList = CollectionFactory.CreateQueue<A>();
            if (null != actions)
                actionList.AddAll(actions);

            //   actionIterator = actionList.iterator();
            searchMetrics = search.getMetrics();
        }

        public override IAction Execute(IPercept p)
        {
            if (!actionList.IsEmpty())
                return actionList.Pop();
            return DynamicAction.NO_OP; // no success or at goal
        }

        public bool isDone()
        {
            return actionList.IsEmpty();
        }

        public ICollection<A> getActions()
        {
            return actionList;
        }

        public Properties getInstrumentation()
        {
            Properties result = new Properties();
            foreach (string key in searchMetrics.keySet())
            {
                string value = searchMetrics.get(key);
                result.setProperty(key, value);
            }
            return result;
        }
    }
}
