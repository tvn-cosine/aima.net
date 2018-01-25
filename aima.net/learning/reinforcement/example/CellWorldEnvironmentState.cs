using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.cellworld;

namespace aima.net.learning.reinforcement.example
{
    /// <summary>
    /// An implementation of the EnvironmentState interface for a Cell World.
    /// </summary>
    public class CellWorldEnvironmentState : IEnvironmentState
    { 
        private IMap<IAgent, CellWorldPercept> agentLocations = CollectionFactory.CreateInsertionOrderedMap<IAgent, CellWorldPercept>();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public CellWorldEnvironmentState()
        { }

        /// <summary>
        /// Reset the environment state to its default state.
        /// </summary>
        public void reset()
        {
            agentLocations.Clear();
        }
         
        /// <summary>
        /// Set an agent's location within the cell world environment.
        /// </summary>
        /// <param name="anAgent">the agents whose location is to be tracked.</param>
        /// <param name="location">the location for the agent in the cell world environment.</param>
        public void setAgentLocation(IAgent anAgent, Cell<double> location)
        {
            CellWorldPercept percept = agentLocations.Get(anAgent);
            if (null == percept)
            {
                percept = new CellWorldPercept(location);
                agentLocations.Put(anAgent, percept);
            }
            else
            {
                percept.setCell(location);
            }
        }
         
        /// <summary>
        /// Get the location of an agent within the cell world environment.
        /// </summary>
        /// <param name="anAgent">the agent whose location is being queried.</param>
        /// <returns>the location of the agent within the cell world environment.</returns>
        public Cell<double> getAgentLocation(IAgent anAgent)
        {
            return agentLocations.Get(anAgent).getCell();
        }

        /// <summary>
        /// Get a percept for an agent, representing what it senses within the cell world environment.
        /// </summary>
        /// <param name="anAgent">the agent a percept is being queried for.</param>
        /// <returns>a percept for the agent, representing what it senses within the cell world environment.</returns>
        public CellWorldPercept getPerceptFor(IAgent anAgent)
        {
            return agentLocations.Get(anAgent);
        }
    }
}
