using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.domain;
using aima.net.probability.domain.api;
using aima.net.probability.proposition;
using aima.net.probability.util;
using aima.net.util;

namespace aima.net.probability.bayes.exact
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 14.9, page
     * 525.<br>
     * <br>
     * 
     * <pre>
     * function ENUMERATION-ASK(X, e, bn) returns a distribution over X
     *   inputs: X, the query variable
     *           e, observed values for variables E
     *           bn, a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden variables //
     *           
     *   Q(X) <- a distribution over X, initially empty
     *   for each value x<sub>i</sub> of X do
     *       Q(x<sub>i</sub>) <- ENUMERATE-ALL(bn.VARS, e<sub>x<sub>i</sub></sub>)
     *          where e<sub>x<sub>i</sub></sub> is e extended with X = x<sub>i</sub>
     *   return NORMALIZE(Q(X))
     *   
     * ---------------------------------------------------------------------------------------------------
     * 
     * function ENUMERATE-ALL(vars, e) returns a real number
     *   if EMPTY?(vars) then return 1.0
     *   Y <- FIRST(vars)
     *   if Y has value y in e
     *       then return P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e)
     *       else return &sum;<sub>y</sub> P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e<sub>y</sub>)
     *           where e<sub>y</sub> is e extended with Y = y
     * </pre>
     * 
     * Figure 14.9 The enumeration algorithm for answering queries on Bayesian
     * networks. <br>
     * <br>
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables. <br>
     * 
     * @author Ciaran O'Reilly
     */
    public class EnumerationAsk : IBayesInference
    {
        public EnumerationAsk()
        { }

        class ProbabilityTableIteratorImpl : ProbabilityTable.ProbabilityTableIterator
        {
            private IBayesianNetwork bn;
            int cnt = 0;
            private ObservedEvidence e;
            private EnumerationAsk enumerationAsk;
            private ProbabilityTable q;
            private IRandomVariable[] x;

            public ProbabilityTableIteratorImpl(IBayesianNetwork bn, ProbabilityTable q, ObservedEvidence e, IRandomVariable[] x, EnumerationAsk enumerationAsk)
            {
                this.bn = bn;
                this.q = q;
                this.e = e;
                this.x = x;
                this.enumerationAsk = enumerationAsk;
            }

            /**
			 * <pre>
			 * Q(x<sub>i</sub>) <- ENUMERATE-ALL(bn.VARS, e<sub>x<sub>i</sub></sub>)
			 *   where e<sub>x<sub>i</sub></sub> is e extended with X = x<sub>i</sub>
			 * </pre>
			 */
            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                for (int i = 0; i < x.Length;++i)
                {
                    e.setExtendedValue(x[i], possibleWorld.Get(x[i]));
                }
                q.setValue(cnt, enumerationAsk.enumerateAll(bn.GetVariablesInTopologicalOrder(), e));
                cnt++;
            }
        }

        // function ENUMERATION-ASK(X, e, bn) returns a distribution over X
        /**
         * The ENUMERATION-ASK algorithm in Figure 14.9 evaluates expression trees
         * (Figure 14.8) using depth-first recursion.
         * 
         * @param X
         *            the query variables.
         * @param observedEvidence
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables //
         * @return a distribution over the query variables.
         */
        public ICategoricalDistribution enumerationAsk(IRandomVariable[] X,
                AssignmentProposition[] observedEvidence,
                IBayesianNetwork bn)
        {

            // Q(X) <- a distribution over X, initially empty
            ProbabilityTable Q = new ProbabilityTable(X);
            ObservedEvidence e = new ObservedEvidence(X, observedEvidence, bn);
            // for each value x<sub>i</sub> of X do
            ProbabilityTable.ProbabilityTableIterator di = new ProbabilityTableIteratorImpl(bn, Q, e, X, this);
            Q.iterateOverTable(di);

            // return NORMALIZE(Q(X))
            return Q.normalize();
        }

        //
        // START-BayesInference
        public ICategoricalDistribution Ask(IRandomVariable[] X,
                AssignmentProposition[] observedEvidence,
                IBayesianNetwork bn)
        {
            return this.enumerationAsk(X, observedEvidence, bn);
        }

        // END-BayesInference
        //

        //
        // PROTECTED METHODS
        //
        // function ENUMERATE-ALL(vars, e) returns a real number
        protected double enumerateAll(ICollection<IRandomVariable> vars, ObservedEvidence e)
        {
            // if EMPTY?(vars) then return 1.0
            if (0 == vars.Size())
            {
                return 1;
            }
            // Y <- FIRST(vars)
            IRandomVariable Y = Util.first(vars);
            // if Y has value y in e
            if (e.containsValue(Y))
            {
                // then return P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e)
                return e.posteriorForParents(Y) * enumerateAll(Util.rest(vars), e);
            }
            /**
             * <pre>
             *  else return &sum;<sub>y</sub> P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e<sub>y</sub>)
             *       where e<sub>y</sub> is e extended with Y = y
             * </pre>
             */
            double sum = 0;
            foreach (object y in ((IFiniteDomain)Y.getDomain()).GetPossibleValues())
            {
                e.setExtendedValue(Y, y);
                sum += e.posteriorForParents(Y) * enumerateAll(Util.rest(vars), e);
            }

            return sum;
        }

        protected class ObservedEvidence
        {
            private IBayesianNetwork bn = null;
            private object[] extendedValues = null;
            private int hiddenStart = 0;
            private int extendedIdx = 0;
            private IRandomVariable[] var = null;
            private IMap<IRandomVariable, int> varIdxs = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, int>();

            public ObservedEvidence(IRandomVariable[] queryVariables,
                    AssignmentProposition[] e, IBayesianNetwork bn)
            {
                this.bn = bn;

                int maxSize = bn.GetVariablesInTopologicalOrder().Size();
                extendedValues = new object[maxSize];
                var = new IRandomVariable[maxSize];
                // query variables go first
                int idx = 0;
                for (int i = 0; i < queryVariables.Length;++i)
                {
                    var[idx] = queryVariables[i];
                    varIdxs.Put(var[idx], idx);
                    idx++;
                }
                // initial evidence variables go next
                for (int i = 0; i < e.Length;++i)
                {
                    var[idx] = e[i].getTermVariable();
                    varIdxs.Put(var[idx], idx);
                    extendedValues[idx] = e[i].getValue();
                    idx++;
                }
                extendedIdx = idx - 1;
                hiddenStart = idx;
                // the remaining slots are left open for the hidden variables
                foreach (IRandomVariable rv in bn.GetVariablesInTopologicalOrder())
                {
                    if (!varIdxs.ContainsKey(rv))
                    {
                        var[idx] = rv;
                        varIdxs.Put(var[idx], idx);
                        idx++;
                    }
                }
            }

            public void setExtendedValue(IRandomVariable rv, object value)
            {
                int idx = varIdxs.Get(rv);
                extendedValues[idx] = value;
                if (idx >= hiddenStart)
                {
                    extendedIdx = idx;
                }
                else
                {
                    extendedIdx = hiddenStart - 1;
                }
            }

            public bool containsValue(IRandomVariable rv)
            {
                return varIdxs.Get(rv) <= extendedIdx;
            }

            public double posteriorForParents(IRandomVariable rv)
            {
                INode n = bn.GetNode(rv);
                if (!(n is IFiniteNode))
                {
                    throw new IllegalArgumentException("Enumeration-Ask only works with finite Nodes.");
                }
                IFiniteNode fn = (IFiniteNode)n;
                object[] vals = new object[1 + fn.GetParents().Size()];
                int idx = 0;
                foreach (INode pn in n.GetParents())
                {
                    vals[idx] = extendedValues[varIdxs.Get(pn.GetRandomVariable())];
                    idx++;
                }
                vals[idx] = extendedValues[varIdxs.Get(rv)];

                return fn.GetCPT().GetValue(vals);
            }
        }

        //
        // PRIVATE METHODS
        //
    }
}
