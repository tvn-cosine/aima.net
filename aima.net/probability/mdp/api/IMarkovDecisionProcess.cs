using aima.net.agent.api;
using aima.net.collections.api;

namespace aima.net.probability.mdp.api
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 647.<para />
    ///  
    /// A sequential decision problem for a fully observable, stochastic environment
    /// with a Markovian transition model and additive rewards is called a Markov
    /// decision process, or MDP, and consists of a set of states (with an
    /// initial state s0; a set ACTIONS(s) of actions in each state; a
    /// transition model P(s' | s, a); and a reward function R(s).<para />
    /// Note: Some definitions of MDPs allow the reward to depend on the
    /// action and outcome too, so the reward function is R(s, a, s'). This
    /// simplifies the description of some environments but does not change the
    /// problem in any fundamental way.
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    /// <typeparam name="A">the action type.</typeparam>
    public interface IMarkovDecisionProcess<S, A>
        where A : IAction
    {
        /// <summary>
        /// Get the set of states associated with the Markov decision process.
        /// </summary>
        /// <returns>the set of states associated with the Markov decision process.</returns>
        ISet<S> states();
         
        /// <summary>
        /// Get the initial state s<sub>0</sub> for this instance of a Markov decision process.
        /// </summary>
        /// <returns>the initial state s0.</returns>
        S getInitialState();
        
        /// <summary>
        /// Get the set of actions for state s.
        /// </summary>
        /// <param name="s">the state.</param>
        /// <returns>the set of actions for state s.</returns>
        ISet<A> actions(S s);
        
        /// <summary>
        /// Return the probability of going from state s using action a to s' based
        /// on the underlying transition model P(s' | s, a).
        /// </summary>
        /// <param name="sDelta">the state s' being transitioned to.</param>
        /// <param name="s">the state s being transitions from.</param>
        /// <param name="a">the action used to move from state s to s'.</param>
        /// <returns>the probability of going from state s using action a to s'.</returns>
        double transitionProbability(S sDelta, S s, A a);
         
        /// <summary>
        /// Get the reward associated with being in state s.
        /// </summary>
        /// <param name="s">the state whose award is sought.</param>
        /// <returns>the reward associated with being in state s.</returns>
        double reward(S s);
    }
}
