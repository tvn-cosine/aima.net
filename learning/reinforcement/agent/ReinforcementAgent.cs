using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.learning.reinforcement.api;

namespace aima.net.learning.reinforcement.agent
{
    /**
     * An abstract base class for creating reinforcement based agents.
     * 
     * @param <S>
     *            the state type.
     * @param <A>
     *            the action type. 
     */
    public abstract class ReinforcementAgent<S, A> : DynamicAgent
        where A : IAction
    {

        /**
         * Default Constructor.
         */
        public ReinforcementAgent()
        {
        }

        /**
         * Map the given percept to an Agent action.
         * 
         * @param percept
         *            a percept indicating the current state s' and reward signal r'
         * @return the action to take.
         */
        public abstract A execute(IPerceptStateReward<S> percept);

        /**
         * Get a vector of the currently calculated utilities for states of type S
         * in the world.
         * 
         * @return a Map of the currently learned utility values for the states in
         *         the environment (Note: this map may not contain all of the states
         *         in the environment, i.e. the agent has not seen them yet).
         */
        public abstract IMap<S, double> getUtility();

        /**
         * Reset the agent back to its initial state before it has learned anything
         * about its environment.
         */
        public abstract void reset();


        public override IAction Execute(IPercept p)
        {
            if (p is IPerceptStateReward<S>)
            {
                IAction a = execute((IPerceptStateReward<S>)p);
                if (null == a)
                {
                    a = DynamicAction.NO_OP;
                    SetAlive(false);
                }
                return a;
            }
            throw new IllegalArgumentException("Percept passed in must be a PerceptStateReward");
        }
    } 
}
