namespace aima.net.search.csp.inference.api
{
    /**
     * Defines a common interface for backtracking inference strategies.
     *
     * @author Ruediger Lunde
     */
    public interface IInferenceStrategy<VAR, VAL>
        where VAR : Variable
    { 
        /**
         * Inference method which is called before backtracking is started.
         */
        IInferenceLog<VAR, VAL> apply(CSP<VAR, VAL> csp);

        /**
         * Inference method which is called after the assignment has (recursively) been extended by a value assignment
         * for <code>var</code>.
         */
        IInferenceLog<VAR, VAL> apply(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var);
    }

}
