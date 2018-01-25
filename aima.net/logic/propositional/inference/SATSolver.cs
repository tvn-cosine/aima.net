using aima.net.collections.api;
using aima.net.logic.propositional.kb.data;

namespace aima.net.logic.propositional.inference
{
    /**
     * Basic interface to a SAT Solver.
     * 
     * @author Ciaran O'Reilly
     *
     */
    public interface SATSolver
    {
        /**
         * Solve a given problem in CNF format.
         * 
         * @param cnf
         *        a CNF representation of the problem to be solved.
         * @return a satisfiable model or null if it cannot be satisfied.
         */
        Model solve(ISet<Clause> cnf);
    }
}
