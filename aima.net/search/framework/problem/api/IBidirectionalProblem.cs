namespace aima.net.search.framework.problem.api
{
    /**
     * An interface describing a problem that can be tackled from both directions at once (i.e InitialState<->Goal).
     *
     * @param <S> The type used to represent states
     * @param <A> The type of the actions to be used to navigate through the state space
     *
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     * 
     */
    public interface IBidirectionalProblem<S, A> : IProblem<S, A>
    {
        IProblem<S, A> getOriginalProblem();
        IProblem<S, A> getReverseProblem();
    }
}
