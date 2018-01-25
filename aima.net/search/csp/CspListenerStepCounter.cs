using aima.net.search.csp.api;
using aima.net.search.framework;

namespace aima.net.search.csp
{
    /// <summary>
    /// /A simple CSP listener implementation which counts assignment 
    /// changes and changes caused by inference steps and provides some metrics.
    /// </summary>
    /// <typeparam name="VAR"></typeparam>
    /// <typeparam name="VAL"></typeparam>
    public class CspListenerStepCounter<VAR, VAL> : ICspListener<VAR, VAL>
        where VAR : Variable
    {
        private int assignmentCount = 0;
        private int inferenceCount = 0;
         
        public void stateChanged(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR variable)
        {
            if (assignment != null)
            {
                ++assignmentCount;
            }
            else
            {
                ++inferenceCount;
            }
        }

        public void reset()
        {
            assignmentCount = 0;
            inferenceCount = 0;
        }

        public Metrics getResults()
        {
            Metrics result = new Metrics();
            result.set("assignmentCount", assignmentCount);
            if (inferenceCount != 0)
            {
                result.set("inferenceCount", inferenceCount);
            }
            return result;
        }
    }
}
