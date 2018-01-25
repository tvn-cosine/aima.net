using aima.net.collections.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.nondeterministic.api;

namespace aima.net.search.nondeterministic
{
    /**
     * Non-deterministic problems may have multiple results for a given state and
     * action; this class handles these results by mimicking Problem and replacing
     * ResultFunction (one result) with ResultsFunction (a set of results).
     * 
     * @author Andrew Brown
     * @author Ruediger Lunde
     */
    public class NondeterministicProblem<S, A>
    {
        protected S initialState;
        protected IActionsFunction<S, A> actionsFn;
        protected GoalTest<S> goalTest;
        protected IStepCostFunction<S, A> stepCostFn;
        protected IResultsFunction<S, A> resultsFn;

        /**
         * Constructor
         */
        public NondeterministicProblem(S initialState,
                IActionsFunction<S, A> actionsFn, IResultsFunction<S, A> resultsFn,
                GoalTest<S> goalTest)
            : this(initialState, actionsFn, resultsFn, goalTest, new DefaultStepCostFunction<S, A>())
        { }

        /**
         * Constructor
         */
        public NondeterministicProblem(S initialState,
                IActionsFunction<S, A> actionsFn, IResultsFunction<S, A> resultsFn,
                GoalTest<S> goalTest, IStepCostFunction<S, A> stepCostFn)
        {
            this.initialState = initialState;
            this.actionsFn = actionsFn;
            this.resultsFn = resultsFn;
            this.goalTest = goalTest;
            this.stepCostFn = stepCostFn;
        }

        /**
         * Returns the initial state of the agent.
         * 
         * @return the initial state of the agent.
         */
        public S getInitialState()
        {
            return initialState;
        }

        /**
         * Returns <code>true</code> if the given state is a goal state.
         * 
         * @return <code>true</code> if the given state is a goal state.
         */
        public bool testGoal(S state)
        {
            return goalTest(state);
        }

        /**
         * Returns the description of the possible actions available to the agent.
         */
        public ICollection<A> getActions(S state)
        {
            return actionsFn.apply(state);
        }

        /**
         * Return the description of what each action does.
         * 
         * @return the description of what each action does.
         */
        public ICollection<S> getResults(S state, A action)
        {
            return this.resultsFn.results(state, action);
        }

        /**
         * Returns the <b>step cost</b> of taking action <code>action</code> in state <code>state</code> to reach state
         * <code>stateDelta</code> denoted by c(s, a, s').
         */
        double getStepCosts(S state, A action, S stateDelta)
        {
            return stepCostFn.applyAsDouble(state, action, stateDelta);
        }
    }
}
