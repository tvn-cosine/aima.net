using aima.net.agent.api;
using aima.net.text.api;

namespace aima.net.environment.vacuum
{
    public class VacuumEnvironmentViewActionTracker : IEnvironmentView
    { 
        private IStringBuilder actions = null;

        public VacuumEnvironmentViewActionTracker(IStringBuilder envChanges)
        {
            this.actions = envChanges;
        }
         
        public void Notify(string msg)
        {
            // Do nothing by default.
        }

        public void AgentAdded(IAgent agent, IEnvironment source)
        {
            // Do nothing by default.
        }

        public void AgentActed(IAgent agent, IPercept percept, IAction action, IEnvironment source)
        {
            actions.Append(action);
        } 
    } 
}
