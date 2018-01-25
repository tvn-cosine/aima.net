using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.proposition;

namespace aima.net.probability.bayes.approximate.api
{
    /// <summary>
    /// General interface to be implemented by approximate Bayesian Inference algorithms.
    /// </summary>
    public interface IBayesSampleInference
    {

        /// <summary>
        /// Return an estimate of P(X|e).
        /// </summary>
        /// <param name="X">the query variables.</param>
        /// <param name="observedEvidence">observed values for variables E.</param>
        /// <param name="bn">a Bayes net with variables {X} n E n Y /* Y = hidden variables</param>
        /// <param name="N">the total number of samples to be generated</param>
        /// <returns>an estimate of P(X|e).</returns>
        ICategoricalDistribution Ask(IRandomVariable[] X,
               AssignmentProposition[] observedEvidence,
               IBayesianNetwork bn, int N);
    }
}
