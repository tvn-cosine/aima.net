using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.approximate.api;
using aima.net.probability.proposition;
using aima.net.probability.util;

namespace aima.net.probability.bayes.approximate
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 534.<br>
     * <br>
     * 
     * <pre>
     * function LIKELIHOOD-WEIGHTING(X, e, bn, N) returns an estimate of <b>P</b>(X|e)
     *   inputs: X, the query variable
     *           e, observed values for variables E
     *           bn, a Bayesian network specifying joint distribution <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
     *           N, the total number of samples to be generated
     *   local variables: W, a vector of weighted counts for each value of X, initially zero
     *   
     *   for j = 1 to N do
     *       <b>x</b>,w <- WEIGHTED-SAMPLE(bn,e)
     *       W[x] <- W[x] + w where x is the value of X in <b>x</b>
     *   return NORMALIZE(W)
     * --------------------------------------------------------------------------------------
     * function WEIGHTED-SAMPLE(bn, e) returns an event and a weight
     *   
     *    w <- 1; <b>x</b> <- an event with n elements initialized from e
     *    foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
     *        if X<sub>i</sub> is an evidence variable with value x<sub>i</sub> in e
     *            then w <- w * P(X<sub>i</sub> = x<sub>i</sub> | parents(X<sub>i</sub>))
     *            else <b>x</b>[i] <- a random sample from <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
     *    return <b>x</b>, w
     * </pre>
     * 
     * Figure 14.15 The likelihood-weighting algorithm for inference in Bayesian
     * networks. In WEIGHTED-SAMPLE, each nonevidence variable is sampled according
     * to the conditional distribution given the values already sampled for the
     * variable's parents, while a weight is accumulated based on the likelihood for
     * each evidence variable.<br>
     * <br>
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables. <br>
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class LikelihoodWeighting : IBayesSampleInference
    {
        private IRandom randomizer = null;

        public LikelihoodWeighting()
                : this(CommonFactory.CreateRandom())
        { }

        public LikelihoodWeighting(IRandom r)
        {
            this.randomizer = r;
        }

        // function LIKELIHOOD-WEIGHTING(X, e, bn, N) returns an estimate of
        // <b>P</b>(X|e)
        /**
         * The LIKELIHOOD-WEIGHTING algorithm in Figure 14.15. For answering queries
         * given evidence in a Bayesian Network.
         * 
         * @param X
         *            the query variables
         * @param e
         *            observed values for variables E
         * @param bn
         *            a Bayesian network specifying joint distribution
         *            <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
         * @param N
         *            the total number of samples to be generated
         * @return an estimate of <b>P</b>(X|e)
         */
        public ICategoricalDistribution likelihoodWeighting(IRandomVariable[] X, AssignmentProposition[] e, IBayesianNetwork bn, int N)
        {
            // local variables: W, a vector of weighted counts for each value of X,
            // initially zero
            double[] W = new double[ProbUtil.expectedSizeOfCategoricalDistribution(X)];

            // for j = 1 to N do
            for (int j = 0; j < N; j++)
            {
                // <b>x</b>,w <- WEIGHTED-SAMPLE(bn,e)
                Pair<IMap<IRandomVariable, object>, double> x_w = weightedSample(bn, e);
                // W[x] <- W[x] + w where x is the value of X in <b>x</b>
                W[ProbUtil.indexOf(X, x_w.GetFirst())] += x_w.getSecond();
            }
            // return NORMALIZE(W)
            return new ProbabilityTable(W, X).normalize();
        }

        // function WEIGHTED-SAMPLE(bn, e) returns an event and a weight
        /**
         * The WEIGHTED-SAMPLE function in Figure 14.15.
         * 
         * @param e
         *            observed values for variables E
         * @param bn
         *            a Bayesian network specifying joint distribution
         *            <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
         * @return return <b>x</b>, w - an event with its associated weight.
         */
        public Pair<IMap<IRandomVariable, object>, double> weightedSample(IBayesianNetwork bn, AssignmentProposition[] e)
        {
            // w <- 1;
            double w = 1.0;
            // <b>x</b> <- an event with n elements initialized from e
            IMap<IRandomVariable, object> x = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();
            foreach (AssignmentProposition ap in e)
            {
                x.Put(ap.getTermVariable(), ap.getValue());
            }

            // foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
            foreach (IRandomVariable Xi in bn.GetVariablesInTopologicalOrder())
            {
                // if X<sub>i</sub> is an evidence variable with value x<sub>i</sub>
                // in e
                if (x.ContainsKey(Xi))
                {
                    // then w <- w * P(X<sub>i</sub> = x<sub>i</sub> |
                    // parents(X<sub>i</sub>))
                    w *= bn.GetNode(Xi)
                            .GetCPD()
                            .GetValue(ProbUtil.getEventValuesForXiGivenParents(bn.GetNode(Xi), x));
                }
                else
                {
                    // else <b>x</b>[i] <- a random sample from
                    // <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
                    x.Put(Xi, ProbUtil.randomSample(bn.GetNode(Xi), x, randomizer));
                }
            }
            // return <b>x</b>, w
            return new Pair<IMap<IRandomVariable, object>, double>(x, w);
        }
         
        public ICategoricalDistribution Ask(IRandomVariable[] X,
                AssignmentProposition[] observedEvidence,
                IBayesianNetwork bn, int N)
        {
            return likelihoodWeighting(X, observedEvidence, bn, N);
        } 
    } 
}
