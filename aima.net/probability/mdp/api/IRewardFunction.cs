namespace aima.net.probability.mdp.api
{ 
    /// <summary>
    /// An interface for MDP reward functions.
    /// </summary>
    /// <typeparam name="S">the state type. </typeparam>
    public interface IRewardFunction<S>
    { 
        /// <summary>
        /// Get the reward associated with being in state s.
        /// </summary>
        /// <param name="s">the state whose award is sought.</param>
        /// <returns>the reward associated with being in state s.</returns>
        double reward(S s);
    }
}
