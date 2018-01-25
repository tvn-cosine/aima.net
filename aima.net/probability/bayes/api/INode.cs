using aima.net.collections.api;
using aima.net.probability.api;

namespace aima.net.probability.bayes.api
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 511. 
    /// <para />
    /// A node is annotated with quantitative probability information. Each node
    /// corresponds to a random variable, which may be discrete or continuous. If
    /// there is an arrow from node X to node Y in a Bayesian Network, X is said to
    /// be a parent of Y and Y is a child of X. Each node X<sub>i</sub> has a
    /// conditional probability distribution P(X<sub>i</sub> |
    /// Parents(X<sub>i</sub>)) that quantifies the effect of the parents on the
    /// node. 
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Return the Random Variable this Node is for/on.
        /// </summary>
        /// <returns>the Random Variable this Node is for/on.</returns>
        IRandomVariable GetRandomVariable();
         
        /// <summary>
        /// Return true if this Node has no parents
        /// </summary>
        /// <returns>true if this Node has no parents</returns>
        bool IsRoot();

        /// <summary>
        /// Return the parent Nodes for this Node.
        /// </summary>
        /// <returns>the parent Nodes for this Node.</returns>
        ISet<INode> GetParents();

        /// <summary>
        /// Return the children Nodes for this Node.
        /// </summary>
        /// <returns>the children Nodes for this Node.</returns>
        ISet<INode> GetChildren();

        /// <summary>
        /// Get this Node's Markov Blanket:<para />
        /// 'A node is conditionally independent of all other nodes in the network,
        /// given its parents, children, and children's parents - that is, given its
        /// MARKOV BLANKET (AIMA3e pg, 517).
        /// </summary>
        /// <returns>this Node's Markov Blanket.</returns>
        ISet<INode> GetMarkovBlanket();
         
        /// <summary>
        /// Return the Conditional Probability Distribution associated with this Node.
        /// </summary>
        /// <returns>the Conditional Probability Distribution associated with this Node.</returns>
        IConditionalProbabilityDistribution GetCPD();
    }
}
