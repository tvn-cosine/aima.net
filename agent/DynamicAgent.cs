using aima.net.agent.api;

namespace aima.net.agent
{
    public class DynamicAgent : IAgent
    {
        protected readonly IAgentProgram program;
        private bool alive = true;

        public DynamicAgent()
        { }

        /// <summary>
        /// Constructs an Agent with the specified AgentProgram.
        /// </summary>
        /// <param name="aProgram">the Agent's program, which maps any given percept sequences to an action.</param>
        public DynamicAgent(IAgentProgram aProgram)
        {
            program = aProgram;
        }

        public virtual IAction Execute(IPercept p)
        {
            if (null != program)
            {
                return program.Execute(p);
            }
            return DynamicAction.NO_OP;
        }

        public virtual bool IsAlive()
        {
            return alive;
        }

        public virtual void SetAlive(bool alive)
        {
            this.alive = alive;
        }
    }
}
