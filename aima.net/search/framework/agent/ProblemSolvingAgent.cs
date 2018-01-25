using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.search.framework.problem;
using aima.net.collections;
using aima.net.search.framework.problem.api;

namespace aima.net.search.framework.agent
{
    /**
     * Modified copy of class
     * {@link SimpleProblemSolvingAgent} which can be used for
     * online search, too. Here, attribute {@link #plan} (original:
     * <code>seq</code>) is protected. Static pseudo code variable state is used in
     * a more general sense including world state as well as agent state aspects.
     * This allows the agent to change the plan, if unexpected percepts are
     * observed. In the concrete java code, state corresponds with the agent
     * instance itself (this).
     * 
     * <pre>
     * <code>
     * function PROBLEM-SOLVING-AGENT(percept) returns an action
     *   inputs: percept, a percept
     *   static: state, some description of current agent and world state
     *           
     *   state <- UPDATE-STATE(state, percept)
     *   while (state.plan is empty) do
     *     goal <- FORMULATE-GOAL(state)
     *     if (goal != null) then
     *       problem    <- FORMULATE-PROBLEM(state, goal)
     *       state.plan <- SEARCH(problem)
     *       if (state.plan is empty and !tryWithAnotherGoal()) then
     *         add NO_OP to plan         // failure
     *     else
     *       add NO_OP to plan           // success
     *   action <- FIRST(state.plan)
     *   plan <- REST(state.plan)
     *   return action
     * </code>
     * </pre> 
     */
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public abstract class ProblemSolvingAgent<S, A> : DynamicAgent
        where A : IAction
    {

        /** Plan, an action sequence, initially empty. */
        protected ICollection<A> plan = CollectionFactory.CreateQueue<A>();


        /**
         * Template method, which corresponds to pseudo code function
         * <code>PROBLEM-SOLVING-AGENT(percept)</code>.
         * 
         * @return an action
         */
        public override IAction Execute(IPercept p)
        {
            IAction action = DynamicAction.NO_OP;
            // state <- UPDATE-STATE(state, percept)
            updateState(p);
            // if plan is empty then do
            while (plan.IsEmpty())
            {
                // state.goal <- FORMULATE-GOAL(state)
                object goal = formulateGoal();
                if (null != goal)
                {
                    // problem <- FORMULATE-PROBLEM(state, goal)
                    IProblem<S, A> problem = formulateProblem(goal);
                    // state.plan <- SEARCH(problem)
                    ICollection<A> actions = search(problem);
                    if (null != actions)
                        plan.AddAll(actions);
                    else if (!tryWithAnotherGoal())
                    {
                        // unable to identify a path
                        SetAlive(false);
                        break;
                    }
                }
                else
                {
                    // no further goal to achieve
                    SetAlive(false);
                    break;
                }
            }
            if (!plan.IsEmpty())
            {
                // action <- FIRST(plan)
                // plan <- REST(plan)
                action = plan.Pop();
            }
            return action;
        }

        /**
         * Primitive operation, which decides after a search for a plan failed,
         * whether to stop the whole task with a failure, or to go on with
         * formulating another goal. This implementation always returns false. If
         * the agent defines local goals to reach an externally specified global
         * goal, it might be interesting, not to stop when the first local goal
         * turns out to be unreachable.
         */
        protected bool tryWithAnotherGoal()
        {
            return false;
        }

        //
        // ABSTRACT METHODS
        //
        /**
         * Primitive operation, responsible for updating the state of the agent with
         * respect to latest feedback from the world. In this version,
         * implementations have access to the agent's current goal and plan, so they
         * can modify them if needed. For example, if the plan didn't work because
         * the model of the world proved to be wrong, implementations could update
         * the model and also clear the plan.
         */
        protected abstract void updateState(IPercept p);

        /**
         * Primitive operation, responsible for goal generation. In this version,
         * implementations are allowed to return empty to indicate that the agent has
         * finished the job an should die. Implementations can access the current
         * goal (which is a possibly modified version of the last formulated goal).
         * This might be useful in situations in which plan execution has failed.
         */
        protected abstract object formulateGoal();

        /**
         * Primitive operation, responsible for search problem generation.
         */
        protected abstract IProblem<S, A> formulateProblem(object goal);

        /**
         * Primitive operation, responsible for the generation of an action list
         * (plan) for the given search problem.
         */
        protected abstract ICollection<A> search(IProblem<S, A> problem);
    }
}
