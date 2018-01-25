using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.proposition;
using aima.net.probability.proposition.api;
using aima.net.probability.util;

namespace aima.net.probability.full
{
    /**
     * An implementation of the FiniteProbabilityModel API using a full joint
     * distribution as the underlying model.
     * 
     * @author Ciaran O'Reilly
     */
    public class FullJointDistributionModel : IFiniteProbabilityModel
    {
        private ProbabilityTable distribution = null;
        private ISet<IRandomVariable> representation = null;

        public FullJointDistributionModel(double[] values, params IRandomVariable[] vars)
        {
            if (null == vars)
            {
                throw new IllegalArgumentException("Random Variables describing the model's representation of the World need to be specified.");
            }

            distribution = new ProbabilityTable(values, vars);

            representation = CollectionFactory.CreateSet<IRandomVariable>();
            for (int i = 0; i < vars.Length;++i)
            {
                representation.Add(vars[i]);
            }
            representation = CollectionFactory.CreateReadOnlySet<IRandomVariable>(representation);
        }

        //
        // START-ProbabilityModel
        public bool isValid()
        {
            // Handle rounding
            return System.Math.Abs(1 - distribution.getSum()) <= ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;
        }

        public double prior(params IProposition[] phi)
        {
            return probabilityOf(ProbUtil.constructConjunction(phi));
        }

        public double posterior(IProposition phi, params IProposition[] evidence)
        {
            IProposition conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            IProposition aAndB = new ConjunctiveProposition(phi, conjEvidence);
            double probabilityOfEvidence = prior(conjEvidence);
            if (0 != probabilityOfEvidence)
            {
                return prior(aAndB) / probabilityOfEvidence;
            }

            return 0;
        }

        public ISet<IRandomVariable> getRepresentation()
        {
            return representation;
        }

        public ICategoricalDistribution priorDistribution(params IProposition[] phi)
        {
            return jointDistribution(phi);
        }

        public ICategoricalDistribution posteriorDistribution(IProposition phi,
                params IProposition[] evidence)
        {

            IProposition conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            ICategoricalDistribution dAandB = jointDistribution(phi, conjEvidence);
            ICategoricalDistribution dEvidence = jointDistribution(conjEvidence);

            return dAandB.divideBy(dEvidence);
        }

        class ProbabilityTableIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private IProposition conjProp;
            private ProbabilityTable ud;
            private object[] values;
            private ISet<IRandomVariable> vars;

            public ProbabilityTableIterator(IProposition conjProp, ProbabilityTable ud, object[] values, ISet<IRandomVariable> vars)
            {
                this.conjProp = conjProp;
                this.ud = ud;
                this.values = values;
                this.vars = vars;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                if (conjProp.holds(possibleWorld))
                {
                    int i = 0;
                    foreach (IRandomVariable rv in vars)
                    {
                        values[i] = possibleWorld.Get(rv);
                       ++i;
                    }
                    int dIdx = ud.getIndex(values);
                    ud.setValue(dIdx, ud.getValues()[dIdx] + probability);
                }
            }
        }

        public ICategoricalDistribution jointDistribution(params IProposition[] propositions)
        {
            ProbabilityTable d = null;
            IProposition conjProp = ProbUtil.constructConjunction(propositions);
            ISet<IRandomVariable> vars = CollectionFactory.CreateSet<IRandomVariable>(conjProp.getUnboundScope());

            if (vars.Size() > 0)
            {
                IRandomVariable[] distVars = vars.ToArray();

                ProbabilityTable ud = new ProbabilityTable(distVars);
                object[] values = new object[vars.Size()];

                ProbabilityTable.ProbabilityTableIterator di = new ProbabilityTableIterator(conjProp, ud, values, vars);

                distribution.iterateOverTable(di);

                d = ud;
            }
            else
            {
                // No Unbound Variables, therefore just return
                // the singular probability related to the proposition.
                d = new ProbabilityTable();
                d.setValue(0, prior(propositions));
            }
            return d;
        }
         
        class ProbabilityTableIteratorImpl2 : ProbabilityTable.ProbabilityTableIterator
        {
            private IProposition phi;
            private double[] probSum;

            public ProbabilityTableIteratorImpl2(double[] probSum, IProposition phi)
            {
                this.probSum = probSum;
                this.phi = phi;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                if (phi.holds(possibleWorld))
                {
                    probSum[0] += probability;
                }
            }
        }
        
        private double probabilityOf(IProposition phi)
        {
            double[] probSum = new double[1];
            ProbabilityTable.ProbabilityTableIterator di = new ProbabilityTableIteratorImpl2(probSum, phi);

            distribution.iterateOverTable(di);

            return probSum[0];
        }
    } 
}
