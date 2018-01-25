using aima.net.agent.api;

namespace aima.net.agent
{
    /// <summary>
    /// Simple environment view which uses the standard output stream to inform about relevant events.
    /// </summary>
    public class SimpleEnvironmentView : IEnvironmentView
    {
        public void Notify(string msg)
        {
            System.Console.WriteLine("Message: " + msg);
        }

        public void AgentAdded(IAgent agent, IEnvironment source)
        {
            int agentId = source.GetAgents().IndexOf(agent) + 1;
            System.Console.WriteLine("Agent " + agentId + " added.");
        }

        public void AgentActed(IAgent agent, IPercept percept, IAction action, IEnvironment source)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            int agentId = source.GetAgents().IndexOf(agent) + 1;
            builder.Append("Agent ").Append(agentId).Append(" acted.");
            builder.Append("\n   Percept: ").Append(percept.ToString());
            builder.Append("\n   Action: ").Append(action.ToString());
            System.Console.WriteLine(builder);
        }
    } 
}
