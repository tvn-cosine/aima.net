using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.search.csp.api;
using aima.net.search.csp.inference.api;

namespace aima.net.search.csp.inference
{
    /**
     * 
     * Artificial Intelligence A Modern Approach (3rd Ed.): Figure 6.3, Page 209.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function AC-3(csp) returns false if an inconsistency is found and true otherwise
     *    inputs: csp, a binary CSP with components (X, D, C)
     *    local variables: queue, a queue of arcs, initially all the arcs in csp
     *    while queue is not empty do
     *       (Xi, Xj) = REMOVE-FIRST(queue)
     *       if REVISE(csp, Xi, Xj) then
     *          if size of Di = 0 then return false
     *             for each Xk in Xi.NEIGHBORS - {Xj} do
     *                add (Xk, Xi) to queue
     *    return true
     * 
     * function REVISE(csp, Xi, Xj) returns true iff we revise the domain of Xi
     *    revised = false
     *    for each x in Di do
     *       if no value y in Dj allows (x ,y) to satisfy the constraint between Xi and Xj then
     *          delete x from Di
     *          revised = true
     *    return revised
     * </code>
     * </pre>
     * 
     * Figure 6.3 The arc-consistency algorithm AC-3. After applying AC-3, either
     * every arc is arc-consistent, or some variable has an empty domain, indicating
     * that the CSP cannot be solved. The name "AC-3" was used by the algorithm's
     * inventor (Mackworth, 1977) because it's the third version developed in the
     * paper.
     * 
     * @author Ruediger Lunde
     */
    public class AC3Strategy<VAR, VAL> : IInferenceStrategy<VAR, VAL>
        where VAR : Variable
    {

        /**
         * Makes a CSP consisting of binary constraints arc-consistent.
         * 
         * @return An object which indicates success/failure and contains data to
         *         undo the operation.
         */
        public IInferenceLog<VAR, VAL> apply(CSP<VAR, VAL> csp)
        {
            ICollection<VAR> queue = CollectionFactory.CreateFifoQueueNoDuplicates<VAR>();
            queue.AddAll(csp.getVariables());
            DomainLog<VAR, VAL> log = new DomainLog<VAR, VAL>();
            reduceDomains(queue, csp, log);
            return log.compactify();
        }

        /**
         * Reduces the domain of the specified variable to the specified value and
         * reestablishes arc-consistency. It is assumed that the provided CSP was
         * arc-consistent before the call.
         * 
         * @return An object which indicates success/failure and contains data to
         *         undo the operation.
         */
        public IInferenceLog<VAR, VAL> apply(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var)
        {
            Domain<VAL> domain = csp.getDomain(var);
            VAL value = assignment.getValue(var);
            if (!domain.contains(value))
            {
                throw new Exception("domain does not contain value");
            }

            DomainLog<VAR, VAL> log = new DomainLog<VAR, VAL>();
            if (domain.size() > 1)
            {
                ICollection<VAR> queue = CollectionFactory.CreateFifoQueue<VAR>();
                queue.Add(var);
                log.storeDomainFor(var, domain);
                csp.setDomain(var, new Domain<VAL>(value));
                reduceDomains(queue, csp, log);
            }
            return log.compactify();
        }

        /**
         * For efficiency reasons the queue manages updated variables vj whereas the original AC3
         * manages neighbor arcs (vi, vj). Constraints which are not binary are ignored.
         */
        private void reduceDomains(ICollection<VAR> queue, CSP<VAR, VAL> csp, DomainLog<VAR, VAL> log)
        {
            while (!queue.IsEmpty())
            {
                VAR var = queue.Pop();
                foreach (IConstraint<VAR, VAL> constraint in csp.getConstraints(var))
                {
                    VAR neighbor = csp.getNeighbor(var, constraint);
                    if (neighbor != null && revise(neighbor, var, constraint, csp, log))
                    {
                        if (csp.getDomain(neighbor).isEmpty())
                        {
                            log.setEmptyDomainFound(true);
                            return;
                        }
                        queue.Add(neighbor);
                    }
                }
            }
        }

        /**
         * Establishes arc-consistency for (xi, xj).
         * @return value true if the domain of xi was reduced.
         */
        private bool revise(VAR xi, VAR xj, IConstraint<VAR, VAL> constraint, CSP<VAR, VAL> csp, DomainLog<VAR, VAL> log)
        {
            Domain<VAL> currDomain = csp.getDomain(xi);
            ICollection<VAL> newValues = CollectionFactory.CreateQueue<VAL>();
            Assignment<VAR, VAL> assignment = new Assignment<VAR, VAL>();
            foreach (VAL vi in currDomain)
            {
                assignment.add(xi, vi);
                foreach (VAL vj in csp.getDomain(xj))
                {
                    assignment.add(xj, vj);
                    if (constraint.isSatisfiedWith(assignment))
                    {
                        newValues.Add(vi);
                        break;
                    }
                }
            }
            if (newValues.Size() < currDomain.size())
            {
                log.storeDomainFor(xi, csp.getDomain(xi));
                csp.setDomain(xi, new Domain<VAL>(newValues));
                return true;
            }
            return false;
        }
    }
}
