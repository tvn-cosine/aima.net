using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.bayes.api;

namespace aima.net.probability.bayes 
{ 
    /// <summary>
    /// Abstract base implementation of the Node interface.
    /// </summary>
    public abstract class AbstractNode : INode
    { 
        private IRandomVariable variable = null;
        private ISet<INode> parents = null;
        private ISet<INode> children = null;

        public AbstractNode(IRandomVariable var)
                : this(var, (INode[])null)
        { }

        public AbstractNode(IRandomVariable var, params INode[] parents)
        {
            if (null == var)
            {
                throw new IllegalArgumentException("Random Variable for Node must be specified.");
            }
            this.variable = var;
            this.parents = CollectionFactory.CreateSet<INode>();
            if (null != parents)
            {
                foreach (INode p in parents)
                {
                    ((AbstractNode)p).addChild(this);
                    this.parents.Add(p);
                }
            }
            this.parents = CollectionFactory.CreateReadOnlySet<INode>(this.parents);
            this.children = CollectionFactory.CreateReadOnlySet<INode>(CollectionFactory.CreateSet<INode>());
        }
         
        public IRandomVariable GetRandomVariable()
        {
            return variable;
        }


        public bool IsRoot()
        {
            return 0 == GetParents().Size();
        }


        public ISet<INode> GetParents()
        {
            return parents;
        }


        public ISet<INode> GetChildren()
        {
            return children;
        }


        public ISet<INode> GetMarkovBlanket()
        {
            ISet<INode> mb = CollectionFactory.CreateSet<INode>();
            // Given its parents,
            mb.AddAll(GetParents());
            // children,
            mb.AddAll(GetChildren());
            // and children's parents
            foreach (INode cn in GetChildren())
            {
                mb.AddAll(cn.GetParents());
            }

            return mb;
        }

        public abstract IConditionalProbabilityDistribution GetCPD();
         
        public override string ToString()
        {
            return GetRandomVariable().getName();
        }
         
        public override bool Equals(object o)
        {
            if (null == o)
            {
                return false;
            }
            if (o == this)
            {
                return true;
            }

            if (o is INode)
            {
                INode n = (INode)o;

                return GetRandomVariable().Equals(n.GetRandomVariable());
            }

            return false;
        } 

        public override int GetHashCode()
        {
            return variable.GetHashCode();
        }
         
        protected void addChild(INode childNode)
        {
            children = CollectionFactory.CreateSet<INode>(children);

            children.Add(childNode);

            children = CollectionFactory.CreateReadOnlySet<INode>(children);
        }
    } 
}
