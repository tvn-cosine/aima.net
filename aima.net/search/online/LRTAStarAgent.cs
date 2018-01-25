using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.util;
using aima.net.collections;
using aima.net.search.framework.problem.api;
using aima.net.util.api;

namespace aima.net.search.online
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.24, page
     * 152.<br>
     * <br>
     * 
     * <pre>
     * function LRTA*-AGENT(s') returns an action
     *   inputs: s', a percept that identifies the current state
     *   persistent: result, a table, indexed by state and action, initially empty
     *               H, a table of cost estimates indexed by state, initially empty
     *               s, a, the previous state and action, initially null
     *           
     *   if GOAL-TEST(s') then return stop
     *   if s' is a new state (not in H) then H[s'] &lt;- h(s')
     *   if s is not null
     *     result[s, a] &lt;- s'
     *     H[s] &lt;-        min LRTA*-COST(s, b, result[s, b], H)
     *             b (element of) ACTIONS(s)
     *   a &lt;- an action b in ACTIONS(s') that minimizes LRTA*-COST(s', b, result[s', b], H)
     *   s &lt;- s'
     *   return a
     *   
     * function LRTA*-COST(s, a, s', H) returns a cost estimate
     *   if s' is undefined then return h(s)
     *   else return c(s, a, s') + H[s']
     * </pre>
     * 
     * Figure 4.24 LRTA*-AGENT selects an action according to the value of
     * neighboring states, which are updated as the agent moves about the state
     * space.<br>
     * <br>
     * <b>Note:</b> This algorithm fails to exit if the goal does not exist (e.g.
     * A<->B Goal=X), this could be an issue with the implementation. Comments
     * welcome.
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class LRTAStarAgent<S, A> : DynamicAgent
        where A : IAction
    {
        private IOnlineSearchProblem<S, A> problem;
        private Function<IPercept, S> ptsFn;
        private IToDoubleFunction<S> h;
        // persistent: result, a table, indexed by state and action, initially empty
        private TwoKeyHashMap<S, A, S> result = new TwoKeyHashMap<S, A, S>();
        // H, a table of cost estimates indexed by state, initially empty
        private IMap<S, double> H = CollectionFactory.CreateInsertionOrderedMap<S, double>();
        // s, a, the previous state and action, initially null
        private S s = default(S);
        private A a = default(A);

        /**
         * Constructs a LRTA* agent with the specified search problem, percept to
         * state function, and heuristic function.
         * 
         * @param problem
         *            an online search problem for this agent to solve.
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         * @param h
         *            heuristic function <em>h(n)</em>, which estimates the cost of
         *            the cheapest path from the state at node <em>n</em> to a goal
         *            state.
         */
        public LRTAStarAgent(IOnlineSearchProblem<S, A> problem, Function<IPercept, S> ptsFn, IToDoubleFunction<S> h)
        {
            setProblem(problem);
            setPerceptToStateFunction(ptsFn);
            setHeuristicFunction(h);
        }

        /**
         * Returns the search problem of this agent.
         * 
         * @return the search problem of this agent.
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
         * Sets the percept to state function of this agent.
         * 
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         */
        public void setPerceptToStateFunction(Function<IPercept, S> ptsFn)
        {
            this.ptsFn = ptsFn;
        }

        /**
         * Returns the heuristic function of this agent.
         */
        public IToDoubleFunction<S> getHeuristicFunction()
        {
            return h;
        }

        /**
         * Sets the heuristic function of this agent.
         * 
         * @param h
         *            heuristic function <em>h(n)</em>, which estimates the cost of
         *            the cheapest path from the state at node <em>n</em> to a goal
         *            state.
         */
        public void setHeuristicFunction(IToDoubleFunction<S> h)
        {
            this.h = h;
        }

        // function LRTA*-AGENT(s') returns an action
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
                // if s' is a new state (not in H) then H[s'] <- h(s')
                if (!H.ContainsKey(sPrimed))
                {
                    H.Put(sPrimed, getHeuristicFunction().applyAsDouble(sPrimed));
                }
                // if s is not null
                double min = double.MaxValue;
                if (null != s)
                {
                    // result[s, a] <- s'
                    result.Put(s, a, sPrimed);

                    // H[s] <- min LRTA*-COST(s, b, result[s, b], H)
                    // b (element of) ACTIONS(s)
                    min = double.MaxValue;
                    foreach (A b in problem.getActions(s))
                    {
                        double cost = lrtaCost(s, b, result.Get(s, b));
                        if (cost < min)
                        {
                            min = cost;
                        }
                    }
                    H.Put(s, min);
                }
                // a <- an action b in ACTIONS(s') that minimizes LRTA*-COST(s', b,
                // result[s', b], H)
                // Just in case no actions
                a = default(A);
                foreach (A b in problem.getActions(sPrimed))
                {
                    double cost = lrtaCost(sPrimed, b, result.Get(sPrimed, b));
                    if (cost < min)
                    {
                        min = cost;
                        a = b;
                    }
                }
            }

            // s <- s'
            s = sPrimed;

            if (a == null)
            {
                // I'm either at the Goal or can't get to it,
                // which in either case I'm finished so just die.
                SetAlive(false);
            }
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
            H.Clear();
            s = default(S);
            a = default(A);
        }

        // function LRTA*-COST(s, a, s', H) returns a cost estimate
        private double lrtaCost(S s, A action, S sDelta)
        {
            // if s' is undefined then return h(s)
            if (null == sDelta)
            {
                return getHeuristicFunction().applyAsDouble(s);
            }
            // else return c(s, a, s') + H[s']
            return problem.getStepCosts(s, action, sDelta) + H.Get(sDelta);
        }
    }

}
