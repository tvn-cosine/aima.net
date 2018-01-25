using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.util;

namespace aima.net.probability.bayes.approximate
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 531.<br>
     * <br>
     * 
     * <pre>
     * function PRIOR-SAMPLE(bn) returns an event sampled from the prior specified by bn
     *   inputs: bn, a Bayesian network specifying joint distribution <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
     * 
     *   x <- an event with n elements
     *   foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
     *      x[i] <- a random sample from <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
     *   return x
     * </pre>
     * 
     * Figure 14.13 A sampling algorithm that generates events from a Bayesian
     * network. Each variable is sampled according to the conditional distribution
     * given the values already sampled for the variable's parents.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class PriorSample
    {
        private IRandom randomizer = null;

        public PriorSample()
           : this(CommonFactory.CreateRandom())
        { }

        public PriorSample(IRandom r)
        {
            this.randomizer = r;
        }

        // function PRIOR-SAMPLE(bn) returns an event sampled from the prior
        // specified by bn
        /**
         * The PRIOR-SAMPLE algorithm in Figure 14.13. A sampling algorithm that
         * generates events from a Bayesian network. Each variable is sampled
         * according to the conditional distribution given the values already
         * sampled for the variable's parents.
         * 
         * @param bn
         *            a Bayesian network specifying joint distribution
         *            <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
         * @return an event sampled from the prior specified by bn
         */
        public IMap<IRandomVariable, object> priorSample(IBayesianNetwork bn)
        {
            // x <- an event with n elements
            IMap<IRandomVariable, object> x = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();
            // foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
            foreach (IRandomVariable Xi in bn.GetVariablesInTopologicalOrder())
            {
                // x[i] <- a random sample from
                // <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
                x.Put(Xi, ProbUtil.randomSample(bn.GetNode(Xi), x, randomizer));
            }
            // return x
            return x;
        }
    } 
}
