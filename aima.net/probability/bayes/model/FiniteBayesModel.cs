using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.exact;
using aima.net.probability.proposition;
using aima.net.probability.proposition.api;
using aima.net.probability.util;

namespace aima.net.probability.bayes.model
{
    /**
     * Very simple implementation of the FiniteProbabilityModel API using a Bayesian
     * Network, consisting of FiniteNodes, to represent the underlying model.<br>
     * <br>
     * <b>Note:</b> The implementation currently doesn't take advantage of the use
     * of evidence values when calculating posterior values using the provided
     * Bayesian Inference implementation (e.g enumerationAsk). Instead it simply
     * creates a joint distribution over the scope of the propositions (i.e. using
     * the inference implementation with just query variables corresponding to the
     * scope of the propositions) and then iterates over this to get the appropriate
     * probability values. A smarter version, in the general case, would need to
     * dynamically create deterministic nodes to represent the outcome of logical
     * and derived propositions (e.g. conjuncts and summations over variables) in
     * order to be able to correctly calculate using evidence values.
     * 
     * @author Ciaran O'Reilly
     */
    public class FiniteBayesModel : IFiniteProbabilityModel
    {
        private IBayesianNetwork bayesNet = null;
        private ISet<IRandomVariable> representation = CollectionFactory.CreateSet<IRandomVariable>();
        private IBayesInference bayesInference = null;

        public FiniteBayesModel(IBayesianNetwork bn)
            : this(bn, new EnumerationAsk())
        { }

        public FiniteBayesModel(IBayesianNetwork bn, IBayesInference bi)
        {
            if (null == bn)
            {
                throw new IllegalArgumentException("Bayesian Network describing the model must be specified.");
            }
            this.bayesNet = bn;
            this.representation.AddAll(bn.GetVariablesInTopologicalOrder());
            setBayesInference(bi);
        }

        public virtual IBayesInference getBayesInference()
        {
            return bayesInference;
        }

        public virtual void setBayesInference(IBayesInference bi)
        {
            this.bayesInference = bi;
        }

        //
        // START-ProbabilityModel
        public virtual bool isValid()
        {
            // Handle rounding 
            int counter = 0;
            IProposition[] propositionArray = new IProposition[representation.Size()];
            foreach (IProposition prop in representation)
            {
                propositionArray[counter] = prop;
                ++counter;
            }
            return System.Math.Abs(1 - prior(propositionArray)) <= ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;
        }

        class CategoricalDistributionIteraorPrior : CategoricalDistributionIterator
        {
            private IProposition conjunct;
            private double[] probSum;

            public CategoricalDistributionIteraorPrior(IProposition conjunct, double[] probSum)
            {
                this.conjunct = conjunct;
                this.probSum = probSum;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                if (conjunct.holds(possibleWorld))
                {
                    probSum[0] += probability;
                }
            }
        }

        public virtual double prior(params IProposition[] phi)
        {
            // Calculating the prior, therefore no relevant evidence
            // just query over the scope of proposition phi in order
            // to get a joint distribution for these
            IProposition conjunct = ProbUtil.constructConjunction(phi);
            IRandomVariable[] X = conjunct.getScope().ToArray();
            ICategoricalDistribution d = bayesInference.Ask(X, new AssignmentProposition[0], bayesNet);

            // Then calculate the probability of the propositions phi
            // be seeing where they hold.
            double[] probSum = new double[1];
            CategoricalDistributionIterator di = new CategoricalDistributionIteraorPrior(conjunct, probSum);
            d.iterateOver(di);

            return probSum[0];
        }

        public virtual double posterior(IProposition phi, params IProposition[] evidence)
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

        public virtual ISet<IRandomVariable> getRepresentation()
        {
            return representation;
        }

        public virtual ICategoricalDistribution priorDistribution(params IProposition[] phi)
        {
            return jointDistribution(phi);
        }

        public virtual ICategoricalDistribution posteriorDistribution(IProposition phi, params IProposition[] evidence)
        {

            IProposition conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            ICategoricalDistribution dAandB = jointDistribution(phi, conjEvidence);
            ICategoricalDistribution dEvidence = jointDistribution(conjEvidence);

            ICategoricalDistribution rVal = dAandB.divideBy(dEvidence);
            // Note: Need to ensure normalize() is called
            // in order to handle the case where an approximate
            // algorithm is used (i.e. won't evenly divide
            // as will have calculated on separate approximate
            // runs). However, this should only be done
            // if the all of the evidences scope are bound (if not
            // you are returning in essence a set of conditional
            // distributions, which you do not want normalized).
            bool unboundEvidence = false;
            foreach (IProposition e in evidence)
            {
                if (e.getUnboundScope().Size() > 0)
                {
                    unboundEvidence = true;
                    break;
                }
            }
            if (!unboundEvidence)
            {
                rVal.normalize();
            }

            return rVal;
        }

        class CategoricalDistributionIteratorJointDistribution : CategoricalDistributionIterator
        {
            private IProposition conjProp;
            private ProbabilityTable ud;
            private object[] values;
            private ISet<IRandomVariable> vars;

            public CategoricalDistributionIteratorJointDistribution(IProposition conjProp, ISet<IRandomVariable> vars, ProbabilityTable ud, object[] values)
            {
                this.conjProp = conjProp;
                this.vars = vars;
                this.ud = ud;
                this.values = values;
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

        public virtual ICategoricalDistribution jointDistribution(params IProposition[] propositions)
        {
            ProbabilityTable d = null;
            IProposition conjProp = ProbUtil.constructConjunction(propositions);
            ISet<IRandomVariable> vars = CollectionFactory.CreateSet<IRandomVariable>(conjProp.getUnboundScope());

            if (vars.Size() > 0)
            {
                IRandomVariable[] distVars = new IRandomVariable[vars.Size()];
                int i = 0;
                foreach (IRandomVariable rv in vars)
                {
                    distVars[i] = rv;
                   ++i;
                }

                ProbabilityTable ud = new ProbabilityTable(distVars);
                object[] values = new object[vars.Size()];

                CategoricalDistributionIterator di = new CategoricalDistributionIteratorJointDistribution(conjProp, vars, ud, values);

                IRandomVariable[] X = conjProp.getScope().ToArray();
                bayesInference.Ask(X, new AssignmentProposition[0], bayesNet).iterateOver(di);

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
    }
}
