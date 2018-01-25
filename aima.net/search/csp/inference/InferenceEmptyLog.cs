using aima.net.search.csp.inference.api;

namespace aima.net.search.csp.inference
{
    /// <summary>
    /// Returns an empty inference log.
    /// </summary>
    /// <typeparam name="VAR"></typeparam>
    /// <typeparam name="VAL"></typeparam>
    public class InferenceEmptyLog<VAR, VAL> : IInferenceLog<VAR, VAL>
        where VAR : Variable
    {
        public bool isEmpty() { return true; }
        public bool inconsistencyFound() { return false; }
        public void undo(CSP<VAR, VAL> csp) { }
    } 
}