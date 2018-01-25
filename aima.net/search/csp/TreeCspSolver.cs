using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.search.csp.api;
using aima.net.util;

namespace aima.net.search.csp
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Figure 6.11, Page
     * 224.<br>
     * <br>
     * <p>
     * <pre>
     * <code>
     * function TREE-CSP-SOLVER(csp) returns a solution, or failure
     * 		inputs: csp, a CSP with components X, D, C
     * 		n &larr; number of variables in X
     * 		assignment &larr; an empty assignment
     * 		root  &larr; any variable in X
     * 		X &larr; TOPOLOGICALSORT(X, root )
     * 		for j = n down to 2 do
     * 			MAKE-ARC-CONSISTENT(PARENT(Xj),Xj )
     * 			if it cannot be made consistent then return failure
     * 		for i = 1 to n do
     * 			assignment[Xi] &larr; any consistent value from Di
     * 			if there is no consistent value then return failure (*)
     * 		return assignment
     * </code>
     *
     * <pre>
     *
     * Figure 6.11 The TREE-CSP-SOLVER algorithm for solving tree-structured CSPs.
     * If the CSP has a solution, we will find it in linear time; if not, we will
     * detect a contradiction. Comment to (*) (RL): If no empty domain was found in the
     * previous loop, this will only happen if n == 1.
     *
     * @author Ruediger Lunde
     * @author Anurag Rai
     */
    public class TreeCspSolver<VAR, VAL> : CspSolver<VAR, VAL>
        where VAR : Variable
    {
        private bool _useRandom;

        public TreeCspSolver<VAR, VAL> useRandom(bool b)
        {
            _useRandom = b;
            return this;
        }


        public override Assignment<VAR, VAL> solve(CSP<VAR, VAL> csp)
        {
            Assignment<VAR, VAL> assignment = new Assignment<VAR, VAL>();
            // Select a root from the List of Variables
            VAR root = _useRandom ? Util.selectRandomlyFromList(csp.getVariables()) : csp.getVariables().Get(0);
            // Sort the variables in topological order
            ICollection<VAR> orderedVars = CollectionFactory.CreateQueue<VAR>();
            IMap<VAR, IConstraint<VAR, VAL>> parentConstraints = CollectionFactory.CreateInsertionOrderedMap<VAR, IConstraint<VAR, VAL>>();
            topologicalSort(csp, root, orderedVars, parentConstraints);
            if (csp.getDomain(root).isEmpty())
                return null; // CSP has no solution! (needed if orderedVars.size() == 1)

            // Establish arc consistency from top to bottom (starting at the bottom).
            csp = csp.copyDomains(); // do not change the original CSP!
            for (int i = orderedVars.Size() - 1; i > 0; i--)
            {
                VAR var = orderedVars.Get(i);
                IConstraint<VAR, VAL> constraint = parentConstraints.Get(var);
                VAR parent = csp.getNeighbor(var, constraint);
                if (makeArcConsistent(parent, var, constraint, csp))
                {
                    fireStateChanged(csp, null, parent);
                    if (csp.getDomain(parent).isEmpty())
                        return null; // CSP has no solution!
                }
            }

            // Assign values to variables from top to bottom.
            for (int i = 0; i < orderedVars.Size();++i)
            {
                VAR var = orderedVars.Get(i);
                foreach (VAL value in csp.getDomain(var))
                {
                    assignment.add(var, value);
                    if (assignment.isConsistent(csp.getConstraints(var)))
                    {
                        fireStateChanged(csp, assignment, var);
                        break;
                    }
                }
            }
            return assignment;
        }

        /**
         * Computes an explicit representation of the tree structure and a total order which is consistent with the
         * parent-child relations. If the provided CSP has not the required properties (CSP contains only binary
         * constraints, constraint graph is tree-structured and connected), an exception is thrown.
         *
         * @param csp               A CSP
         * @param root              A root variable
         * @param orderedVars       The computed total order (initially empty)
         * @param parentConstraints The tree structure, maps a variable to the constraint representing the arc to the parent
         *                          variable (initially empty)
         */
        private void topologicalSort(CSP<VAR, VAL> csp, VAR root, ICollection<VAR> orderedVars,
                                     IMap<VAR, IConstraint<VAR, VAL>> parentConstraints)
        {
            orderedVars.Add(root);
            parentConstraints.Put(root, null);
            int currParentIdx = -1;
            while (currParentIdx < orderedVars.Size() - 1)
            {
                currParentIdx++;
                VAR currParent = orderedVars.Get(currParentIdx);
                int arcsPointingUpwards = 0;
                foreach (IConstraint<VAR, VAL> constraint in csp.getConstraints(currParent))
                {
                    VAR neighbor = csp.getNeighbor(currParent, constraint);
                    if (neighbor == null)
                        throw new IllegalArgumentException("Constraint " + constraint + " is not binary.");
                    if (parentConstraints.ContainsKey(neighbor))
                    { // faster than orderedVars.contains(neighbor)!
                        arcsPointingUpwards++;
                        if (arcsPointingUpwards > 1)
                            throw new IllegalArgumentException("CSP is not tree-structured.");
                    }
                    else
                    {
                        orderedVars.Add(neighbor);
                        parentConstraints.Put(neighbor, constraint);
                    }
                }
            }
            if (orderedVars.Size() < csp.getVariables().Size())
                throw new IllegalArgumentException("Constraint graph is not connected.");
        }

        /**
         * Establishes arc-consistency for (xi, xj).
         * @return value true if the domain of xi was reduced.
         */
        private bool makeArcConsistent(VAR xi, VAR xj, IConstraint<VAR, VAL> constraint, CSP<VAR, VAL> csp)
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
                csp.setDomain(xi, new Domain<VAL>(newValues));
                return true;
            }
            return false;
        }
    } 
}
