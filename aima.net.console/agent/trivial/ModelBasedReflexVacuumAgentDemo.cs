using aima.net.agent.api;
using aima.net.agent;
using aima.net.environment.vacuum;

namespace aima.net.demo.agent.trivial
{
    public class ModelBasedReflexVacuumAgentDemo
    {
        static void Main(params string[] args)
        {
            // create environment with random state of cleaning.
            IEnvironment env = new VacuumEnvironment();
            IEnvironmentView view = new SimpleEnvironmentView();
            env.AddEnvironmentView(view);

            IAgent  a = new ModelBasedReflexVacuumAgent<object>(); 

            env.AddAgent(a);
            env.Step(16);
            env.NotifyViews("Performance=" + env.GetPerformanceMeasure(a));
        } 
    }
}
