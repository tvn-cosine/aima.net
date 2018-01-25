using aima.net.agent.api;
using aima.net.collections.api;
using aima.net.probability.mdp.api;

namespace aima.net.probability.mdp 
{ 
    /// <summary>
    /// Default implementation of the IMarkovDecisionProcess interface.
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    /// <typeparam name="A">the action type.</typeparam>
    public class MDP<S, A> : IMarkovDecisionProcess<S, A>
        where A : IAction
    { 
        private ISet<S> _states ;
        private S initialState ;
        private IActionsFunction<S, A> actionsFunction = null;
        private ITransitionProbabilityFunction<S, A> transitionProbabilityFunction = null;
        private IRewardFunction<S> rewardFunction = null;

        public MDP(ISet<S> states, S initialState,
                IActionsFunction<S, A> actionsFunction,
                ITransitionProbabilityFunction<S, A> transitionProbabilityFunction,
                IRewardFunction<S> rewardFunction)
        {
            this._states = states;
            this.initialState = initialState;
            this.actionsFunction = actionsFunction;
            this.transitionProbabilityFunction = transitionProbabilityFunction;
            this.rewardFunction = rewardFunction;
        }

        public virtual ISet<S> states()
        {
            return _states;
        }

        public virtual S getInitialState()
        {
            return initialState;
        }

        public virtual ISet<A> actions(S s)
        {
            return actionsFunction.actions(s);
        }
         
        public virtual double transitionProbability(S sDelta, S s, A a)
        {
            return transitionProbabilityFunction.probability(sDelta, s, a);
        }
         
        public virtual double reward(S s)
        {
            return rewardFunction.reward(s);
        } 
    } 
}
