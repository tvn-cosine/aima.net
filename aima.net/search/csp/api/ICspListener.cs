namespace aima.net.search.csp.api
{
    /// <summary>
    /// Interface which allows interested clients to register at a CSP solver and follow its progress step by step.
    /// </summary>
    /// <typeparam name="VAR"></typeparam>
    /// <typeparam name="VAL"></typeparam>
    public interface ICspListener<VAR, VAL>
        where VAR : Variable
    {  
        /// <summary>
        /// Informs about changed assignments and inference steps.
        /// </summary>
        /// <param name="csp">a CSP, possibly changed by an inference step.</param>
        /// <param name="assignment">a new assignment or null if the last processing step was an inference step.</param>
        /// <param name="variable">a variable, whose domain or assignment value has been changed (may be null).</param>
        void stateChanged(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR variable); 
    }
}
