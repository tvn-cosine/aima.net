using aima.net;
using aima.net.agent;
using aima.net.api;
using aima.net.text.api;
using aima.net.text;

namespace aima.net.environment.vacuum
{
    /**
     * Represents a local percept in the vacuum environment (i.e. details the
     * agent's location and the state of the square the agent is currently at).
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * @author Andrew Brown
     */
    public class LocalVacuumEnvironmentPercept : DynamicPercept, IStringable
    {
        public const string ATTRIBUTE_AGENT_LOCATION = "agentLocation";
        public const string ATTRIBUTE_STATE = "state";

        /**
         * Construct a vacuum environment percept from the agent's perception of the
         * current location and state.
         * 
         * @param agentLocation
         *            the agent's perception of the current location.
         * @param state
         *            the agent's perception of the current state.
         */
        public LocalVacuumEnvironmentPercept(string agentLocation,
                VacuumEnvironment.LocationState state)
        {
            SetAttribute(ATTRIBUTE_AGENT_LOCATION, agentLocation);
            SetAttribute(ATTRIBUTE_STATE, state);
        }

        /**
         * Return the agent's perception of the current location, which is either A
         * or B.
         * 
         * @return the agent's perception of the current location, which is either A
         *         or B.
         */
        public string getAgentLocation()
        {
            return (string)GetAttribute(ATTRIBUTE_AGENT_LOCATION);
        }

        /**
         * Return the agent's perception of the current state, which is either
         * <em>Clean</em> or <em>Dirty</em>.
         * 
         * @return the agent's perception of the current state, which is either
         *         <em>Clean</em> or <em>Dirty</em>.
         */
        public VacuumEnvironment.LocationState getLocationState()
        {
            return (VacuumEnvironment.LocationState)GetAttribute(ATTRIBUTE_STATE);
        }

        /**
         * Return string representation of this percept.
         *
         * @return a string representation of this percept.
         */
        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            sb.Append("[");
            sb.Append(getAgentLocation());
            sb.Append(", ");
            sb.Append(getLocationState());
            sb.Append("]");
            return sb.ToString();
        }
    }
}
