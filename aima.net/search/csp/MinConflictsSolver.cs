﻿using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.csp.api;
using aima.net.util;

namespace aima.net.search.csp
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Figure 6.8, Page 221.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function MIN-CONFLICTS(csp, max-steps) returns a solution or failure
     *    inputs: csp, a constraint satisfaction problem
     *            max-steps, the number of steps allowed before giving up
     *    current = an initial complete assignment for csp
     *    for i = 1 to max steps do
     *       if current is a solution for csp then return current
     *       var = a randomly chosen conflicted variable from csp.VARIABLES
     *       value = the value v for var that minimizes CONFLICTS(var, v, current, csp)
     *       set var = value in current
     *    return failure
     * </code>
     * </pre>
     * 
     * Figure 6.8 The MIN-CONFLICTS algorithm for solving CSPs by local search. The
     * initial state may be chosen randomly or by a greedy assignment process that
     * chooses a minimal-conflict value for each variable in turn. The CONFLICTS
     * function counts the number of constraints violated by a particular value,
     * given the rest of the current assignment.
     *
     * @param <VAR> Type which is used to represent variables
     * @param <VAL> Type which is used to represent the values in the domains
     *
     * @author Ruediger Lunde
     * @author Mike Stampone
     */
    public class MinConflictsSolver<VAR, VAL> : CspSolver<VAR, VAL>
        where VAR : Variable
    {
        private int maxSteps;

        private bool currIsCancelled;

        public void SetCurrIsCancelled(bool value)
        {
            currIsCancelled = value;
        }

        public bool GetCurrIsCancelled()
        {
            return currIsCancelled;
        }

        /**
         * Constructs a min-conflicts strategy with a given number of steps allowed
         * before giving up.
         * 
         * @param maxSteps
         *            the number of steps allowed before giving up
         */
        public MinConflictsSolver(int maxSteps)
        {
            this.maxSteps = maxSteps;
        }

        public override Assignment<VAR, VAL> solve(CSP<VAR, VAL> csp)
        {
            Assignment<VAR, VAL> current = generateRandomAssignment(csp);
            fireStateChanged(csp, current, null);
            for (int i = 0; i < maxSteps && !currIsCancelled;++i)
            {
                if (current.isSolution(csp))
                {
                    return current;
                }
                else
                {
                    ISet<VAR> vars = getConflictedVariables(current, csp);
                    VAR var = Util.selectRandomlyFromSet(vars);
                    VAL value = getMinConflictValueFor(var, current, csp);
                    current.add(var, value);
                    fireStateChanged(csp, current, var);
                }
            }
            return null;
        }

        private Assignment<VAR, VAL> generateRandomAssignment(CSP<VAR, VAL> csp)
        {
            Assignment<VAR, VAL> result = new Assignment<VAR, VAL>();
            foreach (VAR var in csp.getVariables())
            {
                VAL randomValue = Util.selectRandomlyFromList(csp.getDomain(var).asList());
                result.add(var, randomValue);
            }
            return result;
        }

        private ISet<VAR> getConflictedVariables(Assignment<VAR, VAL> assignment, CSP<VAR, VAL> csp)
        {
            ISet<VAR> result = CollectionFactory.CreateSet<VAR>();
            foreach (IConstraint<VAR, VAL> constraint in csp.getConstraints())
            {
                if (!constraint.isSatisfiedWith(assignment))
                {
                    foreach (VAR var in constraint.getScope())
                    {
                        if (!result.Contains(var))
                        {
                            result.Add(var);
                        }
                    }
                }
            } 
            return result;
        }

        private VAL getMinConflictValueFor(VAR var, Assignment<VAR, VAL> assignment, CSP<VAR, VAL> csp)
        {
            ICollection<IConstraint<VAR, VAL>> constraints = csp.getConstraints(var);
            Assignment<VAR, VAL> testAssignment = assignment.Clone();
            int minConflict = int.MaxValue;
            ICollection<VAL> resultCandidates = CollectionFactory.CreateQueue<VAL>();
            foreach (VAL value in csp.getDomain(var))
            {
                testAssignment.add(var, value);
                int currConflict = countConflicts(testAssignment, constraints);
                if (currConflict <= minConflict)
                {
                    if (currConflict < minConflict)
                    {
                        resultCandidates.Clear();
                        minConflict = currConflict;
                    }
                    resultCandidates.Add(value);
                }
            }
            return (!resultCandidates.IsEmpty()) ? Util.selectRandomlyFromList<VAL>(resultCandidates) : default(VAL);
        }

        private int countConflicts(Assignment<VAR, VAL> assignment,
                ICollection<IConstraint<VAR, VAL>> constraints)
        {
            int result = 0;
            foreach (IConstraint<VAR, VAL> constraint in constraints)
                if (!constraint.isSatisfiedWith(assignment))
                    result++;
            return result;
        }
    }

}
