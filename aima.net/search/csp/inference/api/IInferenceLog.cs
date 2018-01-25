namespace aima.net.search.csp.inference.api
{
    /// <summary> 
    /// Provides information about (1) whether changes have been performed, (2) possibly inferred empty domains , and
    /// (3) how to restore the CSP.
    /// </summary>
    /// <typeparam name="VAR"></typeparam>
    /// <typeparam name="VAL"></typeparam>
    public interface IInferenceLog<VAR, VAL>
        where VAR : Variable
    {
        bool isEmpty();
        bool inconsistencyFound();
        void undo(CSP<VAR, VAL> csp);
    }
}
