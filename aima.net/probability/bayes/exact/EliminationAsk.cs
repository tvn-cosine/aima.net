using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.proposition;
using aima.net.probability.util;

namespace aima.net.probability.bayes.exact
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 14.11, page
     * 528.<br>
     * <br>
     * 
     * <pre>
     * function ELIMINATION-ASK(X, e, bn) returns a distribution over X
     *   inputs: X, the query variable
     *           e, observed values for variables E
     *           bn, a Bayesian network specifying joint distribution P(X<sub>1</sub>, ..., X<sub>n</sub>)
     *   
     *   factors <- []
     *   for each var in ORDER(bn.VARS) do
     *       factors <- [MAKE-FACTOR(var, e) | factors]
     *       if var is hidden variable the factors <- SUM-OUT(var, factors)
     *   return NORMALIZE(POINTWISE-PRODUCT(factors))
     * </pre>
     * 
     * Figure 14.11 The variable elimination algorithm for inference in Bayesian
     * networks. <br>
     * <br>
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables. <br>
     * 
     * @author Ciaran O'Reilly
     */
    public class EliminationAsk : IBayesInference
    {
        //
        private static readonly ProbabilityTable _identity = new ProbabilityTable(new double[] { 1.0 });

        public EliminationAsk()
        {

        }

        // function ELIMINATION-ASK(X, e, bn) returns a distribution over X
        /**
         * The ELIMINATION-ASK algorithm in Figure 14.11.
         * 
         * @param X
         *            the query variables.
         * @param e
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables //
         * @return a distribution over the query variables.
         */
        public ICategoricalDistribution eliminationAsk(IRandomVariable[] X, AssignmentProposition[] e, IBayesianNetwork bn)
        {

            ISet<IRandomVariable> hidden = CollectionFactory.CreateSet<IRandomVariable>();
            ICollection<IRandomVariable> VARS = CollectionFactory.CreateQueue<IRandomVariable>();
            calculateVariables(X, e, bn, hidden, VARS);

            // factors <- []
            ICollection<IFactor> factors = CollectionFactory.CreateQueue<IFactor>();
            // for each var in ORDER(bn.VARS) do
            foreach (IRandomVariable var in order(bn, VARS))
            {
                // factors <- [MAKE-FACTOR(var, e) | factors]
                factors.Insert(0, makeFactor(var, e, bn));
                // if var is hidden variable then factors <- SUM-OUT(var, factors)
                if (hidden.Contains(var))
                {
                    factors = sumOut(var, factors, bn);
                }
            }
            // return NORMALIZE(POINTWISE-PRODUCT(factors))
            IFactor product = pointwiseProduct(factors);
            // Note: Want to ensure the order of the product matches the
            // query variables
            return ((ProbabilityTable)product.pointwiseProductPOS(_identity, X)).normalize();
        }

        //
        // START-BayesInference
        public ICategoricalDistribution Ask(IRandomVariable[] X,
                 AssignmentProposition[] observedEvidence,
                 IBayesianNetwork bn)
        {
            return this.eliminationAsk(X, observedEvidence, bn);
        }

        // END-BayesInference
        //

        //
        // PROTECTED METHODS
        //
        /**
         * <b>Note:</b>Override this method for a more efficient implementation as
         * outlined in AIMA3e pgs. 527-28. Calculate the hidden variables from the
         * Bayesian Network. The default implementation does not perform any of
         * these.<br>
         * <br>
         * Two calcuations to be performed here in order to optimize iteration over
         * the Bayesian Network:<br>
         * 1. Calculate the hidden variables to be enumerated over. An optimization
         * (AIMA3e pg. 528) is to remove 'every variable that is not an ancestor of
         * a query variable or evidence variable as it is irrelevant to the query'
         * (i.e. sums to 1). 2. The subset of variables from the Bayesian Network to
         * be retained after irrelevant hidden variables have been removed.
         * 
         * @param X
         *            the query variables.
         * @param e
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables //
         * @param hidden
         *            to be populated with the relevant hidden variables Y.
         * @param bnVARS
         *            to be populated with the subset of the random variables
         *            comprising the Bayesian Network with any irrelevant hidden
         *            variables removed.
         */
        protected void calculateVariables(IRandomVariable[] X,
                  AssignmentProposition[] e, IBayesianNetwork bn,
                ISet<IRandomVariable> hidden, ICollection<IRandomVariable> bnVARS)
        {

            bnVARS.AddAll(bn.GetVariablesInTopologicalOrder());
            hidden.AddAll(bnVARS);

            foreach (IRandomVariable x in X)
            {
                hidden.Remove(x);
            }
            foreach (AssignmentProposition ap in e)
            {
                hidden.RemoveAll(ap.getScope());
            }

            return;
        }

        /**
         * <b>Note:</b>Override this method for a more efficient implementation as
         * outlined in AIMA3e pgs. 527-28. The default implementation does not
         * perform any of these.<br>
         * 
         * @param bn
         *            the Bayesian Network over which the query is being made. Note,
         *            is necessary to provide this in order to be able to determine
         *            the dependencies between variables.
         * @param vars
         *            a subset of the RandomVariables making up the Bayesian
         *            Network, with any irrelevant hidden variables alreay removed.
         * @return a possibly opimal ordering for the random variables to be
         *         iterated over by the algorithm. For example, one fairly effective
         *         ordering is a greedy one: eliminate whichever variable minimizes
         *         the size of the next factor to be constructed.
         */
        protected ICollection<IRandomVariable> order(IBayesianNetwork bn,
                ICollection<IRandomVariable> vars)
        {
            // Note: Trivial Approach:
            // For simplicity just return in the reverse order received,
            // i.e. received will be the default topological order for
            // the Bayesian Network and we want to ensure the network
            // is iterated from bottom up to ensure when hidden variables
            // are come across all the factors dependent on them have
            // been seen so far.
            ICollection<IRandomVariable> order = CollectionFactory.CreateQueue<IRandomVariable>(vars);
            order.Reverse();

            return order;
        }

        //
        // PRIVATE METHODS
        //
        private IFactor makeFactor(IRandomVariable var, AssignmentProposition[] e, IBayesianNetwork bn)
        {
            INode n = bn.GetNode(var);
            if (!(n is IFiniteNode))
            {
                throw new IllegalArgumentException("Elimination-Ask only works with finite Nodes.");
            }
            IFiniteNode fn = (IFiniteNode)n;
            ICollection<AssignmentProposition> evidence = CollectionFactory.CreateQueue<AssignmentProposition>();
            foreach (AssignmentProposition ap in e)
            {
                if (fn.GetCPT().Contains(ap.getTermVariable()))
                {
                    evidence.Add(ap);
                }
            }

            return fn.GetCPT().GetFactorFor(evidence.ToArray());
        }

        private ICollection<IFactor> sumOut(IRandomVariable var, ICollection<IFactor> factors, IBayesianNetwork bn)
        {
            ICollection<IFactor> summedOutFactors = CollectionFactory.CreateQueue<IFactor>();
            ICollection<IFactor> toMultiply = CollectionFactory.CreateQueue<IFactor>();
            foreach (IFactor f in factors)
            {
                if (f.contains(var))
                {
                    toMultiply.Add(f);
                }
                else
                {
                    // This factor does not contain the variable
                    // so no need to sum out - see AIMA3e pg. 527.
                    summedOutFactors.Add(f);
                }
            }

            summedOutFactors.Add(pointwiseProduct(toMultiply).sumOut(var));

            return summedOutFactors;
        }

        private IFactor pointwiseProduct(ICollection<IFactor> factors)
        {

            IFactor product = factors.Get(0);
            for (int i = 1; i < factors.Size(); ++i)
            {
                product = product.pointwiseProduct(factors.Get(i));
            }

            return product;
        }
    }
}
