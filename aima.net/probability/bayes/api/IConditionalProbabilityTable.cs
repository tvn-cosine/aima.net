using aima.net.probability.api;
using aima.net.probability.proposition;

namespace aima.net.probability.bayes.api
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 512. 
    /// <para />
    /// A Conditional Probability Table, or CPT, can be used for representing
    /// conditional probabilities for discrete (finite) random variables. Each row in
    /// a CPT contains the conditional probability of each node value for a
    /// conditioning case.
    /// </summary>
    public interface IConditionalProbabilityTable : IConditionalProbabilityDistribution
    {
        new ICategoricalDistribution GetConditioningCase(params object[] parentValues); 
        new ICategoricalDistribution GetConditioningCase(params AssignmentProposition[] parentValues);
       
        /// <summary>
        /// Construct a Factor consisting of the Random Variables from the
        /// Conditional Probability Table that are not part of the evidence (see
        /// AIMA3e pg. 524).
        /// </summary>
        /// <param name="evidence">evidence</param>
        /// <returns>a Factor for the Random Variables from the Conditional Probability Table that are not part of the evidence.</returns>
        IFactor GetFactorFor(params AssignmentProposition[] evidence);
    } 
}
