using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.nondeterministic;
using aima.net.search.nondeterministic.api;

namespace aima.net.environment.vacuum
{
    /**
     * Contains useful functions for the vacuum cleaner world.
     *
     * @author Ruediger Lunde
     * @author Andrew Brown
     */
    public class VacuumWorldFunctions
    {
        /**
         * Map fully observable state percepts to their corresponding state
         * representation.
         * 
         * @author Andrew Brown
         */
        public static object FullyObservableVacuumEnvironmentPerceptToStateFunction(IPercept p)
        {
            // Note: VacuumEnvironmentState implements
            // FullyObservableVacuumEnvironmentPercept
            return (VacuumEnvironmentState)p;
        }

        public class ActionsFunction : IActionsFunction<VacuumEnvironmentState, IAction>
        {
            public ICollection<IAction> apply(VacuumEnvironmentState state)
            {
                ICollection<IAction> actions = CollectionFactory.CreateQueue<IAction>();
                actions.Add(VacuumEnvironment.ACTION_SUCK);
                actions.Add(VacuumEnvironment.ACTION_MOVE_LEFT);
                actions.Add(VacuumEnvironment.ACTION_MOVE_RIGHT);
                // Ensure cannot be modified.
                return CollectionFactory.CreateReadOnlyQueue<IAction>(actions);
            }
        }

        /**
         * Specifies the actions available to the agent at state s
         */
        public static IActionsFunction<VacuumEnvironmentState, IAction> getActionsFunction()
        {
            return new ActionsFunction();
        }

        public static bool testGoal(VacuumEnvironmentState state)
        {
            return state.getLocationState(VacuumEnvironment.LOCATION_A) == VacuumEnvironment.LocationState.Clean
                    && state.getLocationState(VacuumEnvironment.LOCATION_B) == VacuumEnvironment.LocationState.Clean;
        }

        public static IResultsFunction<VacuumEnvironmentState, IAction>
            createResultsFunction(IAgent agent)
        {
            return new VacuumWorldResults(agent);
        }

        /**
         * Returns possible results.
         */
        private class VacuumWorldResults : IResultsFunction<VacuumEnvironmentState, IAction>
        {
            private IAgent agent;

            public VacuumWorldResults(IAgent agent)
            {
                this.agent = agent;
            }

            /**
             * Returns a list of possible results for a given state and action.
             */
            public ICollection<VacuumEnvironmentState> results(VacuumEnvironmentState state, IAction action)
            {
                ICollection<VacuumEnvironmentState> results = CollectionFactory.CreateQueue<VacuumEnvironmentState>();
                // add clone of state to results, modify later...
                VacuumEnvironmentState s = state.Clone();
                results.Add(s);

                string currentLocation = state.getAgentLocation(agent);
                string adjacentLocation = (currentLocation.Equals(VacuumEnvironment.LOCATION_A))
                          ? VacuumEnvironment.LOCATION_B : VacuumEnvironment.LOCATION_A;

                if (action == VacuumEnvironment.ACTION_MOVE_RIGHT)
                {
                    s.setAgentLocation(agent, VacuumEnvironment.LOCATION_B);

                }
                else if (action == VacuumEnvironment.ACTION_MOVE_LEFT)
                {
                    s.setAgentLocation(agent, VacuumEnvironment.LOCATION_A);

                }
                else if (action == VacuumEnvironment.ACTION_SUCK)
                {
                    if (state.getLocationState(currentLocation) == VacuumEnvironment.LocationState.Dirty)
                    {
                        // always clean current
                        s.setLocationState(currentLocation, VacuumEnvironment.LocationState.Clean);
                        // sometimes clean adjacent as well
                        VacuumEnvironmentState s2 = s.Clone();
                        s2.setLocationState(adjacentLocation, VacuumEnvironment.LocationState.Clean);
                        if (!s2.Equals(s))
                            results.Add(s2);
                    }
                    else
                    {
                        // sometimes do nothing (-> s unchanged)
                        // sometimes deposit dirt
                        VacuumEnvironmentState s2 = s.Clone();
                        s2.setLocationState(currentLocation, VacuumEnvironment.LocationState.Dirty);
                        if (!s2.Equals(s))
                            results.Add(s2);
                    }
                }
                return results;
            }
        }
    }
}
