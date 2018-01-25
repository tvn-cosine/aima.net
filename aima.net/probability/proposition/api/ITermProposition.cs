using aima.net.probability.api;

namespace aima.net.probability.proposition.api
{
    /// <summary>
    /// A proposition on a single variable term.
    /// 
    /// Note: The scope may be greater than a single variable as the term may be a
    /// derived variable (e.g. Total=Dice1+Dice2). 
    /// </summary>
    public interface ITermProposition : IProposition
    {
        /// <summary>
        /// The Term's Variable.
        /// </summary>
        /// <returns>The Term's Variable.</returns>
        IRandomVariable getTermVariable();
    }
}
