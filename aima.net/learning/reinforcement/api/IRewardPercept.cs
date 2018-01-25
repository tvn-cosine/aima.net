using aima.net.agent.api;

namespace aima.net.learning.reinforcement.api
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 830.<para />
    /// 
    /// Our framework for agents regards the reward as part of the input percept, but
    /// the agent must be "hardwired" to recognize that part as a reward rather than
    /// as just another sensory input. 
    /// </summary>
    public interface IRewardPercept : IPercept
    {
        /// <summary>
        /// The reward part of the percept ('hardwired').
        /// </summary>
        /// <returns>the reward part of the percept ('hardwired').</returns>
        double reward();
    }
}
