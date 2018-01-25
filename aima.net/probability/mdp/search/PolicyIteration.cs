using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability.mdp.api; 
using aima.net.util;

namespace aima.net.probability.mdp.search
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 657.<br>
     * <br>
     * 
     * <pre>
     * function POLICY-ITERATION(mdp) returns a policy
     *   inputs: mdp, an MDP with states S, actions A(s), transition model P(s' | s, a)
     *   local variables: U, a vector of utilities for states in S, initially zero
     *                    &pi;, a policy vector indexed by state, initially random
     *                    
     *   repeat
     *      U <- POLICY-EVALUATION(&pi;, U, mdp)
     *      unchanged? <- true
     *      for each state s in S do
     *          if max<sub>a &isin; A(s)</sub> &Sigma;<sub>s'</sub>P(s'|s,a)U[s'] > &Sigma;<sub>s'</sub>P(s'|s,&pi;[s])U[s'] then do
     *             &pi;[s] <- argmax<sub>a &isin; A(s)</sub> &Sigma;<sub>s'</sub>P(s'|s,a)U[s']
     *             unchanged? <- false
     *   until unchanged?
     *   return &pi;
     * </pre>
     * 
     * Figure 17.7 The policy iteration algorithm for calculating an optimal policy.
     * 
     * @param <S>
     *            the state type.
     * @param <A>
     *            the action type.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * 
     */
    public class PolicyIteration<S, A>        where A : IAction
    {
        private IPolicyEvaluation<S, A> policyEvaluation = null;

        /**
         * Constructor.
         * 
         * @param policyEvaluation
         *            the policy evaluation function to use.
         */
        public PolicyIteration(IPolicyEvaluation<S, A> policyEvaluation)
        {
            this.policyEvaluation = policyEvaluation;
        }

        // function POLICY-ITERATION(mdp) returns a policy
        /**
         * The policy iteration algorithm for calculating an optimal policy.
         * 
         * @param mdp
         *            an MDP with states S, actions A(s), transition model P(s'|s,a)
         * @return an optimal policy
         */
        public IPolicy<S, A> policyIteration(IMarkovDecisionProcess<S, A> mdp)
        {
            // local variables: U, a vector of utilities for states in S, initially
            // zero
            IMap<S, double> U = Util.create(mdp.states(), 0D);
            // &pi;, a policy vector indexed by state, initially random
            IMap<S, A> pi = initialPolicyVector(mdp);
            bool unchanged;
            // repeat
            do
            {
                // U <- POLICY-EVALUATION(&pi;, U, mdp)
                U = policyEvaluation.evaluate(pi, U, mdp);
                // unchanged? <- true
                unchanged = true;
                // for each state s in S do
                foreach (S s in mdp.states())
                {
                    // calculate:
                    // max<sub>a &isin; A(s)</sub>
                    // &Sigma;<sub>s'</sub>P(s'|s,a)U[s']
                    double aMax = double.NegativeInfinity, piVal = 0;
                    A aArgmax = pi.Get(s);
                    foreach (A a in mdp.actions(s))
                    {
                        double aSum = 0;
                        foreach (S sDelta in mdp.states())
                        {
                            aSum += mdp.transitionProbability(sDelta, s, a) * U.Get(sDelta);
                        }
                        if (aSum > aMax)
                        {
                            aMax = aSum;
                            aArgmax = a;
                        }
                        // track:
                        // &Sigma;<sub>s'</sub>P(s'|s,&pi;[s])U[s']
                        if (a.Equals(pi.Get(s)))
                        {
                            piVal = aSum;
                        }
                    }
                    // if max<sub>a &isin; A(s)</sub>
                    // &Sigma;<sub>s'</sub>P(s'|s,a)U[s']
                    // > &Sigma;<sub>s'</sub>P(s'|s,&pi;[s])U[s'] then do
                    if (aMax > piVal)
                    {
                        // &pi;[s] <- argmax<sub>a &isin;A(s)</sub>
                        // &Sigma;<sub>s'</sub>P(s'|s,a)U[s']
                        pi.Put(s, aArgmax);
                        // unchanged? <- false
                        unchanged = false;
                    }
                }
                // until unchanged?
            } while (!unchanged);

            // return &pi;
            return new LookupPolicy<S, A>(pi);
        }

        /**
         * Create a policy vector indexed by state, initially random.
         * 
         * @param mdp
         *            an MDP with states S, actions A(s), transition model P(s'|s,a)
         * @return a policy vector indexed by state, initially random.
         */
        public static IMap<S, A> initialPolicyVector(IMarkovDecisionProcess<S, A> mdp)
        {
            IMap<S, A> pi = CollectionFactory.CreateInsertionOrderedMap<S, A>();
            ICollection<A> actions = CollectionFactory.CreateQueue<A>();
            foreach (S s in mdp.states())
            {
                actions.Clear();
                actions.AddAll(mdp.actions(s));
                // Handle terminal states (i.e. no actions).
                if (actions.Size() > 0)
                {
                    pi.Put(s, Util.selectRandomlyFromList(actions));
                }
            }
            return pi;
        }
    } 
}
