namespace aima.net.probability.bayes.api
{
    /// <summary>
    /// A node over a Random Variable that has a finite countable domain.
    /// </summary>
    public interface IFiniteNode : IDiscreteNode
    {
        /// <summary>
        /// Return the Conditional Probability Table detailing the finite set of probabilities for this Node.
        /// </summary>
        /// <returns>the Conditional Probability Table detailing the finite set of probabilities for this Node.</returns>
        IConditionalProbabilityTable GetCPT();
    }
}
