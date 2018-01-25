using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;

namespace aima.net.environment.map
{
    public class MapEnvironmentState : IEnvironmentState
    {

        private IMap<IAgent, Pair<string, double>> agentLocationAndTravelDistance;

        public MapEnvironmentState()
        {
            agentLocationAndTravelDistance = CollectionFactory.CreateInsertionOrderedMap<IAgent, Pair<string, double>>();
        }

        public string getAgentLocation(IAgent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance.Get(a);
            if (null == locAndTDistance)
            {
                return null;
            }
            return locAndTDistance.GetFirst();
        }

        public double getAgentTravelDistance(IAgent a)
        {
            Pair<string, double> locAndTDistance = agentLocationAndTravelDistance.Get(a);
            if (null == locAndTDistance)
            {
                return 0D;
            }
            return locAndTDistance.getSecond();
        }

        public void setAgentLocationAndTravelDistance(IAgent a, string location,
                double travelDistance)
        {
            agentLocationAndTravelDistance.Put(a, new Pair<string, double>(
                    location, travelDistance));
        }
    }
}
