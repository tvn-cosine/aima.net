using aima.net.learning.reinforcement.api;

namespace aima.net.learning.reinforcement.api
{   
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 832.<para />
    /// 
    /// A percept that supplies both the current state and the reward received in
    /// that state.
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    public interface IPerceptStateReward<S> : IRewardPercept
    {
        /// <summary>
        /// The current state associated with the percept.
        /// </summary>
        /// <returns>the current state associated with the percept.</returns>
        S state();
    } 
}
