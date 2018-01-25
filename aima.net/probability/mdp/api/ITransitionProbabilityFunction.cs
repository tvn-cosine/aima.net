using aima.net.agent.api;

namespace aima.net.probability.mdp.api
{
    /// <summary>
    /// An interface for MDP transition probability functions.
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    /// <typeparam name="A">the action type.</typeparam>
    public interface ITransitionProbabilityFunction<S, A> where A : IAction
    {
        /// <summary>
        /// Return the probability of going from state s using action a to s' based
        /// on the underlying transition model P(s' | s, a).
        /// </summary>
        /// <param name="sDelta">the state s' being transitioned to.</param>
        /// <param name="s">the state s being transitions from.</param>
        /// <param name="a">the action used to move from state s to s'.</param>
        /// <returns>the probability of going from state s using action a to s'.</returns>
        double probability(S sDelta, S s, A a);
    }
}
