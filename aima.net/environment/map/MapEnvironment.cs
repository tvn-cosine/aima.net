using aima.net.agent.api;
using aima.net.agent;

namespace aima.net.environment.map
{
    /**
     * Represents the environment a SimpleMapAgent can navigate.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class MapEnvironment : EnvironmentBase
    {
        private Map map = null;
        private MapEnvironmentState state = new MapEnvironmentState();

        public MapEnvironment(Map map)
        {
            this.map = map;
        }

        public void addAgent(IAgent a, string startLocation)
        {
            // Ensure the agent state information is tracked before
            // adding to super, as super will notify the registered
            // EnvironmentViews that is was added.
            state.setAgentLocationAndTravelDistance(a, startLocation, 0.0);
            base.AddAgent(a);
        }

        public string getAgentLocation(IAgent a)
        {
            return state.getAgentLocation(a);
        }

        public double getAgentTravelDistance(IAgent a)
        {
            return state.getAgentTravelDistance(a);
        }

        public override void executeAction(IAgent agent, IAction a)
        {
            if (!a.IsNoOp())
            {
                MoveToAction act = (MoveToAction)a;

                string currLoc = getAgentLocation(agent);
                double? distance = map.getDistance(currLoc, act.getToLocation());
                if (distance != null)
                {
                    double currTD = getAgentTravelDistance(agent);
                    state.setAgentLocationAndTravelDistance(agent,
                            act.getToLocation(), currTD + distance.Value);
                }
            }
        }

        public override IPercept getPerceptSeenBy(IAgent anAgent)
        {
            return new DynamicPercept(DynAttributeNames.PERCEPT_IN, getAgentLocation(anAgent));
        }

        public Map getMap()
        {
            return map;
        }
    }
}
