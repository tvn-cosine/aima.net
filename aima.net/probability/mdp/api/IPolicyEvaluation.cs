using aima.net.agent.api;
using aima.net.collections.api;

namespace aima.net.probability.mdp.api
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 656.<para />
    /// Given a policy πi, calculate
    /// U>i=Uπi, the utility of each state if
    /// πi were to be executed.
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    /// <typeparam name="A">the action type.</typeparam>
    public interface IPolicyEvaluation<S, A> where A : IAction
    {
        /// <summary>
        /// Policy evaluation: given a policy πi, calculate
        /// Ui=Uπi, the utility of each state if
        /// πi were to be executed.
        /// </summary>
        /// <param name="pi_i">a policy vector indexed by state</param>
        /// <param name="U">a vector of utilities for states in S</param>
        /// <param name="mdp"></param>
        /// <returns>
        /// an MDP with states S, actions A(s), transition model P(s'|s,a)
        /// Ui=Uπi, the utility of each state if
        /// πi were to be executed
        /// </returns>
        IMap<S, double> evaluate(IMap<S, A> pi_i, IMap<S, double> U, IMarkovDecisionProcess<S, A> mdp);
    }

}
