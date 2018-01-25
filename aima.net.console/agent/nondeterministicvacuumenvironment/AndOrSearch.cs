using aima.net;
using aima.net.agent.api;
using aima.net.environment.vacuum;
using aima.net.search.framework.problem;
using aima.net.search.nondeterministic;
using aima.net.api;
using aima.net.text.api;
using aima.net.text;

namespace aima.net.demo.agent.nondeterministicvacuumenvironment
{
    public class AndOrSearch
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine("NON-DETERMINISTIC-VACUUM-ENVIRONMENT DEMO");
            System.Console.WriteLine("");
            startAndOrSearch();
        }

        private static void startAndOrSearch()
        {
            System.Console.WriteLine("AND-OR-GRAPH-SEARCH");

            NondeterministicVacuumAgent
                agent = new NondeterministicVacuumAgent(
                    VacuumWorldFunctions.FullyObservableVacuumEnvironmentPerceptToStateFunction);
            // create state: both rooms are dirty and the vacuum is in room A
            VacuumEnvironmentState state = new VacuumEnvironmentState();
            state.setLocationState(VacuumEnvironment.LOCATION_A, VacuumEnvironment.LocationState.Dirty);
            state.setLocationState(VacuumEnvironment.LOCATION_B, VacuumEnvironment.LocationState.Dirty);
            state.setAgentLocation(agent, VacuumEnvironment.LOCATION_A);
            // create problem
            NondeterministicProblem<VacuumEnvironmentState, IAction> problem
                = new NondeterministicProblem<VacuumEnvironmentState, IAction>(
                    state,
                    VacuumWorldFunctions.getActionsFunction(),
                    VacuumWorldFunctions.createResultsFunction(agent),
                    VacuumWorldFunctions.testGoal,
                    new DefaultStepCostFunction<VacuumEnvironmentState, IAction>());
            // set the problem and agent
            //   agent.setProblem(problem);

            // create world
            NondeterministicVacuumEnvironment
                world = new NondeterministicVacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            world.addAgent(agent, VacuumEnvironment.LOCATION_A);

            // execute and show plan
            System.Console.WriteLine("Initial Plan: " + agent.getContingencyPlan());
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            world.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(sb));
            world.StepUntilDone();
            System.Console.WriteLine("Remaining Plan: " + agent.getContingencyPlan());
            System.Console.WriteLine("Actions Taken: " + sb);
            System.Console.WriteLine("Final State: " + world.getCurrentState());
        }
    }
}
