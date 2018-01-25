using aima.net.agent.api;

namespace aima.net.probability.mdp.api
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 647.<para />
    /// A solution to a Markov decision process is called a policy. It
    /// specifies what the agent should do for any state that the agent might reach.
    /// It is traditional to denote a policy by π, and π(s) is the action
    /// recommended by the policy π for state s. If the agent has a complete
    /// policy, then no matter what the outcome of any action, the agent will always
    /// know what to do next.
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    /// <typeparam name="A">the action type.</typeparam>
    public interface IPolicy<S, A> where A : IAction
    { 
        /// <summary>
        /// π(s) is the action recommended by the policy π for state s.
        /// </summary>
        /// <param name="s">the state s</param>
        /// <returns>the action recommended by the policy π for state s.</returns>
        A action(S s);
    } 
}
