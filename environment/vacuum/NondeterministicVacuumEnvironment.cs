using aima.net.agent.api;
using aima.net;
using aima.net.api;

namespace aima.net.environment.vacuum
{
    /**
     * Create the erratic vacuum world from page 134, AIMA3e. In the erratic vacuum
     * world, the Suck action works as follows: 1) when applied to a dirty square
     * the action cleans the square and sometimes cleans up dirt in an adjacent
     * square too; 2) when applied to a clean square the action sometimes deposits
     * dirt on the carpet.
     *
     * @author Andrew Brown
     */
    public class NondeterministicVacuumEnvironment : VacuumEnvironment
    {
        IRandom random = CommonFactory.CreateRandom();
        /**
         * Construct a vacuum environment with two locations, in which dirt is
         * placed at random.
         */
        public NondeterministicVacuumEnvironment()
            : base()
        {

        }

        /**
         * Construct a vacuum environment with two locations, in which dirt is
         * placed as specified.
         *
         * @param locAState the initial state of location A, which is either
         * <em>Clean</em> or <em>Dirty</em>.
         * @param locBState the initial state of location B, which is either
         * <em>Clean</em> or <em>Dirty</em>.
         */
        public NondeterministicVacuumEnvironment(LocationState locAState, LocationState locBState)
            : base(locAState, locBState)
        {

        }

        /**
         * Execute the agent action
         */
        public override void executeAction(IAgent a, IAction action)
        {
            if (ACTION_MOVE_RIGHT == action)
            {
                envState.setAgentLocation(a, LOCATION_B);
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_MOVE_LEFT == action)
            {
                envState.setAgentLocation(a, LOCATION_A);
                updatePerformanceMeasure(a, -1);
            }
            else if (ACTION_SUCK == action)
            {
                // case: square is dirty
                if (VacuumEnvironment.LocationState.Dirty == envState.getLocationState(envState.getAgentLocation(a)))
                {
                    string currentLocation = envState.getAgentLocation(a);
                    string adjacentLocation = (currentLocation.Equals("A")) ? "B" : "A";
                    // always clean current square
                    envState.setLocationState(currentLocation, VacuumEnvironment.LocationState.Clean);
                    // possibly clean adjacent square
                    if (CommonFactory.CreateRandom().NextDouble() > 0.5)
                    {
                        envState.setLocationState(adjacentLocation, VacuumEnvironment.LocationState.Clean);
                    }
                } // case: square is clean
                else if (VacuumEnvironment.LocationState.Clean == envState.getLocationState(envState.getAgentLocation(a)))
                {
                    // possibly dirty current square
                    if (random.NextBoolean())
                    {
                        envState.setLocationState(envState.getAgentLocation(a), VacuumEnvironment.LocationState.Dirty);
                    }
                }
            }
            else if (action.IsNoOp())
            {
                _isDone = true;
            }
        }
    }
}
