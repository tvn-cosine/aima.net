using aima.net.probability.api;
using aima.net.probability.bayes.api;

namespace aima.net.probability.bayes 
{
    /// <summary> 
    /// Default implementation of the FiniteNode interface that uses a fully
    /// specified Conditional Probability Table to represent the Node's conditional
    /// distribution.
    /// </summary>
    public class FullCPTNode : AbstractNode, IFiniteNode
    {
        private IConditionalProbabilityTable cpt = null;

        public FullCPTNode(IRandomVariable var, double[] distribution)
            : this(var, distribution, (INode[])null)
        { }

        public FullCPTNode(IRandomVariable var, double[] values, params INode[] parents)
            : base(var, parents)
        {
            IRandomVariable[] conditionedOn = new IRandomVariable[GetParents().Size()];
            int i = 0;
            foreach (INode p in GetParents())
            {
                conditionedOn[i++] = p.GetRandomVariable();
            }

            cpt = new CPT(var, values, conditionedOn);
        }

        public override IConditionalProbabilityDistribution GetCPD()
        {
            return GetCPT();
        }

        public virtual IConditionalProbabilityTable GetCPT()
        {
            return cpt;
        }
    }
}
