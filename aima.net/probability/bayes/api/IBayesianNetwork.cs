using aima.net.collections.api;
using aima.net.probability.api;

namespace aima.net.probability.bayes.api
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 510. 
    /// <para />
    /// Bayesian Networks are used to represent the dependencies among Random
    /// Variables. They can represent essentially any full joint probability
    /// distribution and in many cases can do so very concisely. A Bayesian network
    /// is a directed graph in which each node is annotated with quantitative
    /// probability information. The full specification is as follows:
    /// <para />
    /// 1. Each node corresponds to a random variable, which may be discrete or
    /// continuous.
    /// <para />
    /// 2. A set of directed links or arrows connects pairs of nodes. If there is an
    /// arrow from node X to node Y, X is said to be a parent of Y. The graph has no
    /// directed cycles (and hence is a directed acyclic graph, or DAG.
    /// <para />
    /// 3. Each node X<sub>i</sub> has a conditional probability distribution
    /// P(Xi | Parents(Xi)) that quantifies the effect of the
    /// parents on the node.
    /// <para />
    /// The topology of the network - the set of nodes and links - specifies the
    /// conditional independence relationships that hold in the domain.
    /// <para />
    /// A network with both discrete and continuous variables is called a hybrid
    /// Bayesian network.
    /// <para />
    /// Note(1): "Bayesian Network" is the most common name used, but there
    /// are many synonyms, including "belief network", "probabilistic network",
    /// "causal network", and "knowledge map".
    /// </summary>
    public interface IBayesianNetwork
    {
        /// <summary>
        /// Return a list of the Random Variables, in topological order, contained within the network.
        /// </summary>
        /// <returns>a list of the Random Variables, in topological order, contained within the network.</returns>
        ICollection<IRandomVariable> GetVariablesInTopologicalOrder();
         
        /// <summary>
        /// Return the Node associated with the random variable in this Bayesian Network.
        /// </summary>
        /// <param name="rv">the RandomVariable whose corresponding Node is to be retrieved.</param>
        /// <returns>the Node associated with the random variable in this Bayesian Network.</returns>
        INode GetNode(IRandomVariable rv);
    } 
}
