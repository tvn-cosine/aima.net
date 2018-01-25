using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.util;

namespace aima.net.probability.bayes 
{
    /// <summary>
    /// Default implementation of the DynamicBayesianNetwork interface.
    /// </summary>
    public class DynamicBayesNet : BayesNet, IDynamicBayesianNetwork
    {
        private ISet<IRandomVariable> X_0 = CollectionFactory.CreateSet<IRandomVariable>();
        private ISet<IRandomVariable> X_1 = CollectionFactory.CreateSet<IRandomVariable>();
        private ISet<IRandomVariable> E_1 = CollectionFactory.CreateSet<IRandomVariable>();
        private IMap<IRandomVariable, IRandomVariable> X_0_to_X_1 = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, IRandomVariable>();
        private IMap<IRandomVariable, IRandomVariable> X_1_to_X_0 = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, IRandomVariable>();
        private IBayesianNetwork priorNetwork = null;
        private ICollection<IRandomVariable> X_1_VariablesInTopologicalOrder = CollectionFactory.CreateQueue<IRandomVariable>();

        public DynamicBayesNet(IBayesianNetwork priorNetwork,
                IMap<IRandomVariable, IRandomVariable> X_0_to_X_1,
                ISet<IRandomVariable> E_1, params INode[] rootNodes)
            : base(rootNodes)
        {
            foreach (var x0_x1 in X_0_to_X_1)
            {
                IRandomVariable x0 = x0_x1.GetKey();
                IRandomVariable x1 = x0_x1.GetValue();
                this.X_0.Add(x0);
                this.X_1.Add(x1);
                this.X_0_to_X_1.Put(x0, x1);
                this.X_1_to_X_0.Put(x1, x0);
            }
            this.E_1.AddAll(E_1);

            // Assert the X_0, X_1, and E_1 sets are of expected sizes
            ISet<IRandomVariable> combined = CollectionFactory.CreateSet<IRandomVariable>();
            combined.AddAll(X_0);
            combined.AddAll(X_1);
            combined.AddAll(E_1);
            if (SetOps.difference(CollectionFactory.CreateSet<IRandomVariable>(varToNodeMap.GetKeys()), combined).Size() != 0)
            {
                throw new IllegalArgumentException("X_0, X_1, and E_1 do not map correctly to the Nodes describing this Dynamic Bayesian Network.");
            }
            this.priorNetwork = priorNetwork;

            X_1_VariablesInTopologicalOrder.AddAll(GetVariablesInTopologicalOrder());
            X_1_VariablesInTopologicalOrder.RemoveAll(X_0);
            X_1_VariablesInTopologicalOrder.RemoveAll(E_1);
        }

        public IBayesianNetwork GetPriorNetwork()
        {
            return priorNetwork;
        }


        public ISet<IRandomVariable> GetX_0()
        {
            return X_0;
        }


        public ISet<IRandomVariable> GetX_1()
        {
            return X_1;
        }


        public ICollection<IRandomVariable> GetX_1_VariablesInTopologicalOrder()
        {
            return X_1_VariablesInTopologicalOrder;
        }


        public IMap<IRandomVariable, IRandomVariable> GetX_0_to_X_1()
        {
            return X_0_to_X_1;
        }


        public IMap<IRandomVariable, IRandomVariable> GetX_1_to_X_0()
        {
            return X_1_to_X_0;
        }


        public ISet<IRandomVariable> GetE_1()
        {
            return E_1;
        } 
    }
}
