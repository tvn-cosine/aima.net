using aima.net.agent.api;
using aima.net.agent;
using aima.net.api;
using aima.net.collections.api;
using aima.net.util;
using aima.net;
using aima.net.collections;

namespace aima.net.environment.vacuum
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 58.<br>
     * <br>
     * Let the world contain just two locations. Each location may or may not
     * contain dirt, and the agent may be in one location or the other. There are 8
     * possible world states, as shown in Figure 3.2. The agent has three possible
     * actions in this version of the vacuum world: <em>Left</em>, <em>Right</em>,
     * and <em>Suck</em>. Assume for the moment, that sucking is 100% effective. The
     * goal is to clean up all the dirt.
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * @author Ruediger Lunde
     */
    public class VacuumEnvironment : EnvironmentBase
    {
        // Allowable Actions within the Vacuum Environment
        public static readonly IAction ACTION_MOVE_LEFT = new DynamicAction("Left");
        public static readonly IAction ACTION_MOVE_RIGHT = new DynamicAction("Right");
        public static readonly IAction ACTION_SUCK = new DynamicAction("Suck");
        public const string LOCATION_A = "A";
        public const string LOCATION_B = "B";

        public enum LocationState
        {
            Clean, Dirty
        }

        private readonly ICollection<string> locations;
        protected VacuumEnvironmentState envState = null;
        protected bool _isDone = false;

        /**
         * Constructs a vacuum environment with two locations A and B, in which dirt is
         * placed at random.
         */
        public VacuumEnvironment()
            : this(Util.randomBoolean() ? LocationState.Clean : LocationState.Dirty,
                 Util.randomBoolean() ? LocationState.Clean : LocationState.Dirty)
        { }

        /**
         * Constructs a vacuum environment with two locations A and B, in which dirt is
         * placed as specified.
         * 
         * @param locAState
         *            the initial state of location A, which is either
         *            <em>Clean</em> or <em>Dirty</em>.
         * @param locBState
         *            the initial state of location B, which is either
         *            <em>Clean</em> or <em>Dirty</em>.
         */
        public VacuumEnvironment(LocationState locAState, LocationState locBState)
            : this(CollectionFactory.CreateQueue<string>(new[] { LOCATION_A, LOCATION_B }), locAState, locBState)
        { }

        /**
         * Constructor which allows subclasses to define a vacuum environment with an arbitrary number
         * of squares. Two-dimensional grid environments can be defined by additionally overriding
         * {@link #getXDimension()} and {@link #getYDimension()}.
         */
        protected VacuumEnvironment(ICollection<string> locations, params LocationState[] locStates)
        {
            this.locations = locations;
            envState = new VacuumEnvironmentState();
            for (int i = 0; i < locations.Size() && i < locStates.Length;++i)
                envState.setLocationState(locations.Get(i), locStates[i]);
        }

        public ICollection<string> getLocations()
        {
            return locations;
        }

        public IEnvironmentState getCurrentState()
        {
            return envState;
        }

        public LocationState getLocationState(string location)
        {
            return envState.getLocationState(location);
        }

        public string getAgentLocation(IAgent a)
        {
            return envState.getAgentLocation(a);
        }

        public override void AddAgent(IAgent a)
        {
            int idx = CommonFactory.CreateRandom().Next(locations.Size());
            envState.setAgentLocation(a, locations.Get(idx));
            base.AddAgent(a);
        }

        public void addAgent(IAgent a, string location)
        {
            // Ensure the agent state information is tracked before
            // adding to super, as super will notify the registered
            // EnvironmentViews that is was added.
            envState.setAgentLocation(a, location);
            base.AddAgent(a);
        }

        public override IPercept getPerceptSeenBy(IAgent anAgent)
        {
            if (anAgent is NondeterministicVacuumAgent)
            {
                // This agent expects a fully observable environment. It gets a clone of the environment state.
                return envState.Clone();
            }
            // Other agents get a local percept.
            string loc = envState.getAgentLocation(anAgent);
            return new LocalVacuumEnvironmentPercept(loc, envState.getLocationState(loc));
        }

        public override void executeAction(IAgent a, IAction action)
        {
            string loc = getAgentLocation(a);
            if (ACTION_MOVE_RIGHT == action)
            {
                int x = getX(loc);
                if (x < getXDimension())
                    envState.setAgentLocation(a, getLocation(x + 1, getY(loc)));
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_MOVE_LEFT == action)
            {
                int x = getX(loc);
                if (x > 1)
                    envState.setAgentLocation(a, getLocation(x - 1, getY(loc)));
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_SUCK == action)
            {
                if (LocationState.Dirty == envState.getLocationState(envState
                        .getAgentLocation(a)))
                {
                    envState.setLocationState(envState.getAgentLocation(a),
                            LocationState.Clean);
                    updatePerformanceMeasure(a, 10);
                }
            }
            else if (action.IsNoOp())
            {
                // In the Vacuum Environment we consider things done if
                // the agent generates a NoOp.
                _isDone = true;
            }
        }

        public override bool IsDone()
        {
            return base.IsDone() || _isDone;
        }


        // Information for grid views...

        public int getXDimension()
        {
            return locations.Size();
        }

        public int getYDimension()
        {
            return 1;
        }

        // 1 means left
        public int getX(string location)
        {
            return getLocations().IndexOf(location) % getXDimension() + 1;
        }

        // 1 means bottom
        public int getY(string location)
        {
            return getYDimension() - getLocations().IndexOf(location) / getXDimension();
        }

        // (1, 1) is bottom left
        public string getLocation(int x, int y)
        {
            return locations.Get((getYDimension() - y) * getXDimension() + x - 1);
        }
    }
}
