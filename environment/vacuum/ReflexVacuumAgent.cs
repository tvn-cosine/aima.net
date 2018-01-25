using aima.net.agent.api;
using aima.net.agent;

namespace aima.net.environment.vacuum
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.8, page 48.<br>
     * <br>
     * 
     * <pre>
     * function REFLEX-VACUUM-AGENT([location, status]) returns an action
     *   
     *   if status = Dirty then return Suck
     *   else if location = A then return Right
     *   else if location = B then return Left
     * </pre>
     * 
     * Figure 2.8 The agent program for a simple reflex agent in the two-state
     * vacuum environment. This program : the action function tabulated in
     * Figure 2.3.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ReflexVacuumAgent : DynamicAgent
    {
        class ReflexVacuumAgentProgram : IAgentProgram
        {
            // function REFLEX-VACUUM-AGENT([location, status]) returns an
            // action
            public IAction Execute(IPercept percept)
            {
                LocalVacuumEnvironmentPercept vep = (LocalVacuumEnvironmentPercept)percept;

                // if status = Dirty then return Suck
                if (VacuumEnvironment.LocationState.Dirty == vep
                        .getLocationState())
                {
                    return VacuumEnvironment.ACTION_SUCK;
                    // else if location = A then return Right
                }
                else if (VacuumEnvironment.LOCATION_A.Equals(vep
                      .getAgentLocation()))
                {
                    return VacuumEnvironment.ACTION_MOVE_RIGHT;
                }
                else if (VacuumEnvironment.LOCATION_B.Equals(vep
                      .getAgentLocation()))
                {
                    // else if location = B then return Left
                    return VacuumEnvironment.ACTION_MOVE_LEFT;
                }

                // Note: This should not be returned if the
                // environment is correct
                return DynamicAction.NO_OP;
            }
        }

        public ReflexVacuumAgent()
            : base(new ReflexVacuumAgentProgram())
        { }
    }
}
