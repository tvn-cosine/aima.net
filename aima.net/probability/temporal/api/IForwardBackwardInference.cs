using aima.net.collections.api;
using aima.net.probability.api;
using aima.net.probability.proposition;

namespace aima.net.probability.temporal.api
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 576.<para />
    ///  
    /// Generic interface for calling different implementations of the
    /// forward-backward algorithm for smoothing: computing posterior probabilities
    /// of a sequence of states given a sequence of observations.
    /// </summary>
    public interface IForwardBackwardInference : IForwardStepInference, IBackwardStepInference
    { 
        /// <summary>
        /// The forward-backward algorithm for smoothing: computing posterior
        /// probabilities of a sequence of states given a sequence of observations.
        /// </summary>
        /// <param name="ev">a vector of evidence values for steps 1,...,t</param>
        /// <param name="prior">the prior distribution on the initial state, P(X0)</param>
        /// <returns>a vector of smoothed estimates for steps 1,...,t</returns>
        ICollection<ICategoricalDistribution> forwardBackward(ICollection<ICollection<AssignmentProposition>> ev, ICategoricalDistribution prior);
    }

}
