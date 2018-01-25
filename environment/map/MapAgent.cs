using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.problem;
using aima.net.search.informed;
using aima.net.util;
using aima.net.collections;
using aima.net.search.framework.api;
using aima.net.search.framework.problem.api;
using aima.net.search.informed.api;
using aima.net.util.api;

namespace aima.net.environment.map
{
    /**
     * Variant of {@link aima.core.environment.map.SimpleMapAgent} which works
     * correctly also for A* and other best-first search implementations. It can be
     * extended also for scenarios, in which the agent faces unforeseen events. When
     * using informed search and more then one goal, make sure, that a heuristic
     * function factory is provided!
     *
     * @author Ruediger Lunde
     */
    public class MapAgent : ProblemSolvingAgent<string, MoveToAction>
    {
        protected readonly Map map;
        protected readonly DynamicState state = new DynamicState();
        protected readonly ICollection<string> goals = CollectionFactory.CreateQueue<string>();
        protected int currGoalIdx = -1;

        // possibly null...
        protected IEnvironmentViewNotifier notifier = null;
        private ISearchForActions<string, MoveToAction> _search = null;
        private Function<string, IToDoubleFunction<Node<string, MoveToAction>>> hFnFactory;

        public MapAgent(Map map, ISearchForActions<string, MoveToAction> search, string goal)
        {
            this.map = map;
            this._search = search;
            goals.Add(goal);
        }

        public MapAgent(Map map, ISearchForActions<string, MoveToAction> search,
            string goal, IEnvironmentViewNotifier notifier)
            : this(map, search, goal)
        {
            this.notifier = notifier;
        }

        public MapAgent(Map map, ISearchForActions<string, MoveToAction> search,
            ICollection<string> goals)
        {
            this.map = map;
            this._search = search;
            this.goals.AddAll(goals);
        }

        public MapAgent(Map map, ISearchForActions<string, MoveToAction> search,
            ICollection<string> goals,
            IEnvironmentViewNotifier notifier)
            : this(map, search, goals)
        {

            this.notifier = notifier;
        }

        /**
         * Constructor.
         * @param map Information about the environment
         * @param search Search strategy to be used
         * @param goals List of locations to be visited
         * @param notifier Gets informed about decisions of the agent
         * @param hFnFactory Factory, mapping goals to heuristic functions. When using
         *                   informed search, the agent must be able to estimate remaining costs for
         *                   the goals he has selected.
         */
        public MapAgent(Map map,
            ISearchForActions<string, MoveToAction> search, ICollection<string> goals,
            IEnvironmentViewNotifier notifier,
            Function<string, IToDoubleFunction<Node<string, MoveToAction>>> hFnFactory)
            : this(map, search, goals, notifier)
        {
            this.hFnFactory = hFnFactory;
        }

        //
        // PROTECTED METHODS
        // 
        protected override void updateState(IPercept p)
        {
            DynamicPercept dp = (DynamicPercept)p;
            state.SetAttribute(DynAttributeNames.AGENT_LOCATION, dp.GetAttribute(DynAttributeNames.PERCEPT_IN));
        }

        protected override object formulateGoal()
        {
            string goal = null;
            if (currGoalIdx < goals.Size() - 1)
            {
                goal = goals.Get(++currGoalIdx);
                if (hFnFactory != null && _search is IInformed<string, MoveToAction>)
                    ((IInformed<string, MoveToAction>)_search)
                        .setHeuristicFunction(hFnFactory(goal));

                if (notifier != null)
                    notifier.NotifyViews("Current location: In(" + state.GetAttribute(DynAttributeNames.AGENT_LOCATION)
                            + "), Goal: In(" + goal + ")");
            }
            return goal;
        }

        protected override IProblem<string, MoveToAction> formulateProblem(object goal)
        {
            return new BidirectionalMapProblem(map, 
                (string)state.GetAttribute(DynAttributeNames.AGENT_LOCATION),
                    (string)goal);
        }
         
    protected override ICollection<MoveToAction> search(IProblem<string, MoveToAction> problem)
        {
            ICollection<MoveToAction> result = _search.findActions(problem);
            notifyViewOfMetrics();
            return result;
        }

        protected void notifyViewOfMetrics()
        {
            if (notifier != null)
                notifier.NotifyViews("Search metrics: " + _search.getMetrics());
        }
    }
}
