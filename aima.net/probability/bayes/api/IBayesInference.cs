using aima.net.probability.api;
using aima.net.probability.proposition;

namespace aima.net.probability.bayes.api
{
    /// <summary>
    /// General interface to be implemented by Bayesian Inference algorithms.
    /// </summary>
    public interface IBayesInference
    { 
        /// <summary>
        /// Return a distribution over the query variables.
        /// </summary>
        /// <param name="X">the query variables.</param>
        /// <param name="observedEvidence">observed values for variables E.</param>
        /// <param name="bn">a Bayes net with variables {X} n E n Y /* Y = hidden variables</param>
        /// <returns>a distribution over the query variables.</returns>
        ICategoricalDistribution Ask(IRandomVariable[] X,
                 AssignmentProposition[] observedEvidence,
                 IBayesianNetwork bn);
    }
}
