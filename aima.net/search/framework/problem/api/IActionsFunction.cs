using aima.net.collections.api;

namespace aima.net.search.framework.problem.api
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 67.<br>
     * <br>
     * Given a particular state s, ACTIONS(s) returns the set of actions that can be
     * executed in s. We say that each of these actions is <b>applicable</b> in s.
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ruediger Lunde
     */
    public interface IActionsFunction<S, A>
    {
        ICollection<A> apply(S state);
    }
}