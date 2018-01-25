using aima.net.collections.api;
using aima.net.search.framework.problem.api;

namespace aima.net.search.framework.problem
{
    /**
     * Configurable problem which uses objects to explicitly represent the required functions.
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     */
    public class GeneralProblem<S, A> : IProblem<S, A>
    {
        private S initialState;
        private IActionsFunction<S, A> actionsFn;
        private IResultFunction<S, A> resultFn;
        private GoalTest<S> goalTest;
        private IStepCostFunction<S, A> stepCostFn;

        /**
         * Constructs a problem with the specified components, which includes a step
         * cost function.
         *
         * @param initialState
         *            the initial state of the agent.
         * @param actionsFn
         *            a description of the possible actions available to the agent.
         * @param resultFn
         *            a description of what each action does; the formal name for
         *            this is the transition model, specified by a function
         *            RESULT(s, a) that returns the state that results from doing
         *            action a in state s.
         * @param goalTest
         *            test determines whether a given state is a goal state.
         * @param stepCostFn
         *            a path cost function that assigns a numeric cost to each path.
         *            The problem-solving-agent chooses a cost function that
         *            reflects its own performance measure.
         */
        public GeneralProblem(S initialState,
                              IActionsFunction<S, A> actionsFn,
                              IResultFunction<S, A> resultFn,
                              GoalTest<S> goalTest,
                              IStepCostFunction<S, A> stepCostFn)
        {
            this.initialState = initialState;
            this.actionsFn = actionsFn;
            this.resultFn = resultFn;
            this.goalTest = goalTest;
            this.stepCostFn = stepCostFn;
        }

        /**
         * Constructs a problem with the specified components, and a default step
         * cost function (i.e. 1 per step).
         *
         * @param initialState
         *            the initial state that the agent starts in.
         * @param actionsFn
         *            a description of the possible actions available to the agent.
         * @param resultFn
         *            a description of what each action does; the formal name for
         *            this is the transition model, specified by a function
         *            RESULT(s, a) that returns the state that results from doing
         *            action a in state s.
         * @param goalTest
         *            test determines whether a given state is a goal state.
         */
        public GeneralProblem(S initialState,
                              IActionsFunction<S, A> actionsFn,
                              IResultFunction<S, A> resultFn,
                              GoalTest<S> goalTest)
            : this(initialState, actionsFn, resultFn, goalTest, new DefaultStepCostFunction<S, A>())
        { }

        public S getInitialState()
        {
            return initialState;
        }

        public ICollection<A> getActions(S state)
        {
            return actionsFn.apply(state);
        }

        public S getResult(S state, A action)
        {
            return resultFn.apply(state, action);
        }

        public bool testGoal(S state)
        {
            return goalTest(state);
        }

        public double getStepCosts(S state, A action, S statePrimed)
        {
            return stepCostFn.applyAsDouble(state, action, statePrimed);
        }
    }
}
