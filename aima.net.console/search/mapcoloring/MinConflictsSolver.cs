using aima.net.search.csp;
using aima.net.search.csp.examples;

namespace aima.net.demo.search.mapcoloring
{
    public class MinConflictsSolver
    {
        static void Main(params string[] args)
        {
            CSP<Variable, string> csp = new MapCSP();
            CspListenerStepCounter<Variable, string> stepCounter = new CspListenerStepCounter<Variable, string>();
            CspSolver<Variable, string> solver;

            solver = new MinConflictsSolver<Variable, string>(1000);
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            System.Console.WriteLine("Map Coloring (Minimum Conflicts)");
            System.Console.WriteLine(solver.solve(csp));
            System.Console.WriteLine(stepCounter.getResults() + "\n");
        } 
    }
}
