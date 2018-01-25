using aima.net.search.csp.api;
using aima.net.search.csp.inference.api;

namespace aima.net.search.csp.inference
{
    /// <summary>
    /// Implements forward checking. Constraints which are not binary are ignored here.
    /// </summary>
    /// <typeparam name="VAR"></typeparam>
    /// <typeparam name="VAL"></typeparam>
    public class ForwardCheckingStrategy<VAR, VAL> : IInferenceStrategy<VAR, VAL>
        where VAR : Variable
    {
        /// <summary>
        /// The CSP is not changed at the beginning.
        /// </summary>
        /// <param name="csp"></param>
        /// <returns></returns>
        public IInferenceLog<VAR, VAL> apply(CSP<VAR, VAL> csp)
        {
            return new InferenceEmptyLog<VAR, VAL>();
        }

        /**
         * Removes all values from the domains of the neighbor variables of <code>var</code> in the
         * constraint graph which are not consistent with the new value for <code>var</code>.
         * It is called after <code>assignment</code> has (recursively) been extended with a value
         * assignment for <code>var</code>.
         */

        public IInferenceLog<VAR, VAL> apply(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var)
        {
            DomainLog<VAR, VAL> log = new DomainLog<VAR, VAL>();
            foreach (IConstraint<VAR, VAL> constraint in csp.getConstraints(var))
            {
                VAR neighbor = csp.getNeighbor(var, constraint);
                if (neighbor != null && !assignment.contains(neighbor))
                {
                    if (revise(neighbor, constraint, assignment, csp, log))
                    {
                        if (csp.getDomain(neighbor).isEmpty())
                        {
                            log.setEmptyDomainFound(true);
                            return log;
                        }
                    }
                }
            }
            return log;
        }

        /**
         * Removes all values from the domain of <code>var</code> which are not consistent with
         * <code>constraint</code> and <code>assignment</code>. Modifies the domain log accordingly so
         * that all changes can be undone later on.
         */
        private bool revise(VAR var, IConstraint<VAR, VAL> constraint, Assignment<VAR, VAL> assignment,
                               CSP<VAR, VAL> csp, DomainLog<VAR, VAL> log)
        {
            bool revised = false;
            foreach (VAL value in csp.getDomain(var))
            {
                assignment.add(var, value);
                if (!constraint.isSatisfiedWith(assignment))
                {
                    log.storeDomainFor(var, csp.getDomain(var));
                    csp.removeValueFromDomain(var, value);
                    revised = true;
                }
                assignment.remove(var);
            }
            return revised;
        }
    } 
}
