using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.bayes.api;

namespace aima.net.probability.bayes 
{

    /// <summary>
    /// Default implementation of the BayesianNetwork interface.
    /// </summary>
    public class BayesNet : IBayesianNetwork
    {
        protected ISet<INode> rootNodes = CollectionFactory.CreateSet<INode>();
        protected ICollection<IRandomVariable> variables = CollectionFactory.CreateQueue<IRandomVariable>();
        protected IMap<IRandomVariable, INode> varToNodeMap = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, INode>();

        public BayesNet(params INode[] rootNodes)
        {
            if (null == rootNodes)
            {
                throw new IllegalArgumentException("Root Nodes need to be specified.");
            }
            foreach (INode n in rootNodes)
            {
                this.rootNodes.Add(n);
            }
            if (this.rootNodes.Size() != rootNodes.Length)
            {
                throw new IllegalArgumentException("Duplicate Root Nodes Passed in.");
            }
            // Ensure is a DAG
            checkIsDAGAndCollectVariablesInTopologicalOrder();
            variables = CollectionFactory.CreateReadOnlyQueue<IRandomVariable>(variables);
        }

        //
        // START-BayesianNetwork

        public ICollection<IRandomVariable> GetVariablesInTopologicalOrder()
        {
            return variables;
        }


        public INode GetNode(IRandomVariable rv)
        {
            return varToNodeMap.Get(rv);
        }

        // END-BayesianNetwork
        //

        //
        // PRIVATE METHODS
        //
        private void checkIsDAGAndCollectVariablesInTopologicalOrder()
        {
            // Topological sort based on logic described at:
            // http://en.wikipedia.org/wiki/Topoligical_sorting
            ISet<INode> seenAlready = CollectionFactory.CreateSet<INode>();
            IMap<INode, ICollection<INode>> incomingEdges = CollectionFactory.CreateMap<INode, ICollection<INode>>();
            ICollection<INode> s = CollectionFactory.CreateFifoQueueNoDuplicates<INode>();
            foreach (INode n in this.rootNodes)
            {
                walkNode(n, seenAlready, incomingEdges, s);
            }
            while (!s.IsEmpty())
            {
                INode n = s.Pop();
                variables.Add(n.GetRandomVariable());
                varToNodeMap.Put(n.GetRandomVariable(), n);
                foreach (INode m in n.GetChildren())
                {
                    ICollection<INode> edges = incomingEdges.Get(m);
                    edges.Remove(n);
                    if (edges.IsEmpty())
                    {
                        s.Add(m);
                    }
                }
            }

            foreach (ICollection<INode> edges in incomingEdges.GetValues())
            {
                if (!edges.IsEmpty())
                {
                    throw new IllegalArgumentException("Network contains at least one cycle in it, must be a DAG.");
                }
            }
        }

        private void walkNode(INode n, ISet<INode> seenAlready,
                IMap<INode, ICollection<INode>> incomingEdges, ICollection<INode> rootNodes)
        {
            if (!seenAlready.Contains(n))
            {
                seenAlready.Add(n);
                // Check if has no incoming edges
                if (n.IsRoot())
                {
                    rootNodes.Add(n);
                }
                incomingEdges.Put(n, CollectionFactory.CreateQueue<INode>(n.GetParents()));
                foreach (INode c in n.GetChildren())
                {
                    walkNode(c, seenAlready, incomingEdges, rootNodes);
                }
            }
        }
    }

}
