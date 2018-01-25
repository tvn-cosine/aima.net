using aima.net.collections.api;
using aima.net.probability.api;

namespace aima.net.probability.bayes.api
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 590. 
    /// <para />
    /// A dynamic Bayesian network or DBN, is a Bayesian network that
    /// represents a temporal probability model. In general, each slice of a DBN can
    /// have any number of state variables Xt and evidence
    /// variables Et. For simplicity, we assume that the variables
    /// and their links are exactly replicated from slice to slice and that the DBN
    /// represents a first-order Markov process, so that each variable can have
    /// parents only in its own slice or the immediately preceding slice.
    /// </summary>
    public interface IDynamicBayesianNetwork : IBayesianNetwork
    {
        /// <summary>
        /// Return a Bayesian Network containing just the nodes representing the prior distribution (layer 0) of the dynamic bayesian network.
        /// </summary>
        /// <returns>a Bayesian Network containing just the nodes representing the prior distribution (layer 0) of the dynamic bayesian network.</returns>
        IBayesianNetwork GetPriorNetwork();

        /// <summary>
        /// Return the set of state variables representing the prior distribution.
        /// </summary>
        /// <returns>the set of state variables representing the prior distribution.</returns>
        ISet<IRandomVariable> GetX_0();

        /// <summary>
        /// Return the set of state variables representing the first posterior slice
        /// of the DBN. This along with <b>X</b><sub>0</sub> should represent
        /// the transition model P(X1 | X0).
        /// </summary>
        /// <returns>
        /// the set of state variables representing the first posterior slice
        /// of the DBN. This along with <b>X</b><sub>0</sub> should represent
        /// the transition model P(X1 | X0).
        /// </returns>
        ISet<IRandomVariable> GetX_1();

        /// <summary>
        /// Return the X_1 variables in topological order.
        /// </summary>
        /// <returns>the X_1 variables in topological order.</returns>
        ICollection<IRandomVariable> GetX_1_VariablesInTopologicalOrder();

        /// <summary>
        /// Return a Map indicating equivalent variables between X0 and X1.
        /// </summary>
        /// <returns>a Map indicating equivalent variables between X0 and X1.</returns>
        IMap<IRandomVariable, IRandomVariable> GetX_0_to_X_1();

        /// <summary>
        /// Return a Map indicating equivalent variables between X1 and X0.
        /// </summary>
        /// <returns>a Map indicating equivalent variables between X1 and X0.</returns>
        IMap<IRandomVariable, IRandomVariable> GetX_1_to_X_0();

        /// <summary>
        /// Return the set of state variables representing the evidence variables
        /// for the DBN. This along with X1 should
        /// represent the sensor model P(E1 | X1).
        /// </summary>
        /// <returns>
        /// the set of state variables representing the evidence variables
        /// for the DBN. This along with X1 should
        /// represent the sensor model P(E1 | X1).        
        /// </returns>
        ISet<IRandomVariable> GetE_1();
    } 
}
