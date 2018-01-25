using aima.net.search.csp;
using aima.net.search.csp.examples;

namespace aima.net.demo.search.mapcoloring
{
    public class FlexibleBacktrackingSolver
    {
        static void Main(params string[] args)
        {
            CSP<Variable, string> csp = new MapCSP();
            CspListenerStepCounter<Variable, string> stepCounter = new CspListenerStepCounter<Variable, string>();
            CspSolver<Variable, string> solver;

            solver = new FlexibleBacktrackingSolver<Variable, string>();
            solver.addCspListener(stepCounter);
            stepCounter.reset();
            System.Console.WriteLine("Map Coloring (Backtracking)");
            System.Console.WriteLine(solver.solve(csp));
            System.Console.WriteLine(stepCounter.getResults() + "\n");
        }
    }
}
