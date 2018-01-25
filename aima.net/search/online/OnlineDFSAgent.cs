using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.datastructures; 
using aima.net.util;
using aima.net.collections;
using aima.net.search.framework.problem.api;

namespace aima.net.search.online
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.21, page
     * 150.<br>
     * <br>
     * 
     * <pre>
     * function ONLINE-DFS-AGENT(s') returns an action
     *   inputs: s', a percept that identifies the current state
     *   persistent: result, a table, indexed by state and action, initially empty
     *               untried, a table that lists, for each state, the actions not yet tried
     *               unbacktracked, a table that lists, for each state, the backtracks not yet tried
     *               s, a, the previous state and action, initially null
     *    
     *   if GOAL-TEST(s') then return stop
     *   if s' is a new state (not in untried) then untried[s'] &lt;- ACTIONS(s')
     *   if s is not null then
     *       result[s, a] &lt;- s'
     *       add s to the front of the unbacktracked[s']
     *   if untried[s'] is empty then
     *       if unbacktracked[s'] is empty then return stop
     *       else a &lt;- an action b such that result[s', b] = POP(unbacktracked[s'])
     *   else a &lt;- POP(untried[s'])
     *   s &lt;- s'
     *   return a
     * </pre>
     * 
     * Figure 4.21 An online search agent that uses depth-first exploration. The
     * agent is applicable only in state spaces in which every action can be
     * "undone" by some other action.<br>
     * 
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     * 
     */
    public class OnlineDFSAgent<S, A> : DynamicAgent
        where A : IAction
    { 
        private IOnlineSearchProblem<S, A> problem;
        private Function<IPercept, S> ptsFn;
        // persistent: result, a table, indexed by state and action, initially empty
        private TwoKeyHashMap<S, A, S> result = new TwoKeyHashMap<S, A, S>();
        // untried, a table that lists, for each state, the actions not yet tried
        private IMap<S, ICollection<A>> untried = CollectionFactory.CreateInsertionOrderedMap<S, ICollection<A>>();
        // unbacktracked, a table that lists,
        // for each state, the backtracks not yet tried
        private IMap<S, ICollection<S>> unbacktracked = CollectionFactory.CreateInsertionOrderedMap<S, ICollection<S>>();
        // s, a, the previous state and action, initially null
        private S s;
        private A a;

        /**
         * Constructs an online DFS agent with the specified search problem and
         * percept to state function.
         * 
         * @param problem
         *            an online search problem for this agent to solve
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         */
        public OnlineDFSAgent(IOnlineSearchProblem<S, A> problem, Function<IPercept, S> ptsFn)
        {
            setProblem(problem);
            setPerceptToStateFunction(ptsFn);
        }

        /**
         * Returns the search problem for this agent.
         * 
         * @return the search problem for this agent.
         */
        public IOnlineSearchProblem<S, A> getProblem()
        {
            return problem;
        }

        /**
         * Sets the search problem for this agent to solve.
         * 
         * @param problem
         *            the search problem for this agent to solve.
         */
        public void setProblem(IOnlineSearchProblem<S, A> problem)
        {
            this.problem = problem;
            init();
        }

        /**
         * Returns the percept to state function of this agent.
         * 
         * @return the percept to state function of this agent.
         */
        public Function<IPercept, S> getPerceptToStateFunction()
        {
            return ptsFn;
        }

        /**
         * Sets the percept to state functino of this agent.
         * 
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         */
        public void setPerceptToStateFunction(Function<IPercept, S> ptsFn)
        {
            this.ptsFn = ptsFn;
        }

        // function ONLINE-DFS-AGENT(s') returns an action
        // inputs: s', a percept that identifies the current state

        public override IAction Execute(IPercept psPrimed)
        {
            S sPrimed = ptsFn(psPrimed);
            // if GOAL-TEST(s') then return stop
            if (problem.testGoal(sPrimed))
            {
                a = default(A);
            }
            else
            {
                // if s' is a new state (not in untried) then untried[s'] <-
                // ACTIONS(s')
                if (!untried.ContainsKey(sPrimed))
                {
                    untried.Put(sPrimed, problem.getActions(sPrimed));
                }

                // if s is not null then do
                if (null != s)
                {
                    // Note: If I've already seen the result of this
                    // [s, a] then don't put it back on the unbacktracked
                    // list otherwise you can keep oscillating
                    // between the same states endlessly.
                    if (!(sPrimed.Equals(result.Get(s, a))))
                    {
                        // result[s, a] <- s'
                        result.Put(s, a, sPrimed);

                        // Ensure the unbacktracked always has a list for s'
                        if (!unbacktracked.ContainsKey(sPrimed))
                        {
                            unbacktracked.Put(sPrimed, CollectionFactory.CreateQueue<S>());
                        }

                        // add s to the front of the unbacktracked[s']
                        unbacktracked.Get(sPrimed).Insert(0, s);
                    }
                }
                // if untried[s'] is empty then
                if (untried.Get(sPrimed).IsEmpty())
                {
                    // if unbacktracked[s'] is empty then return stop
                    if (unbacktracked.Get(sPrimed).IsEmpty())
                    {
                        a = default(A);
                    }
                    else
                    {
                        // else a <- an action b such that result[s', b] =
                        // POP(unbacktracked[s'])
                        S popped = unbacktracked.Get(sPrimed).Pop();
                        foreach (Pair<S, A> sa in result.GetKeys())
                        {
                            if (sa.GetFirst().Equals(sPrimed) && result.Get(sa).Equals(popped))
                            {
                                a = sa.getSecond();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // else a <- POP(untried[s'])
                    a = untried.Get(sPrimed).Pop();
                }
            }

            if (a == null)
            {
                // I'm either at the Goal or can't get to it,
                // which in either case I'm finished so just die.
                SetAlive(false);
            }

            // s <- s'
            s = sPrimed;
            // return a
            return a;
        }

        //
        // PRIVATE METHODS
        //

        private void init()
        {
            SetAlive(true);
            result.Clear();
            untried.Clear();
            unbacktracked.Clear();
            s = default(S);
            a = default(A);
        }
    }

}
