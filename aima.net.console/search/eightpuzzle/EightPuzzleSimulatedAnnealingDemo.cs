using aima.net.agent.api;
using aima.net.exceptions;
using aima.net.environment.eightpuzzle;
using aima.net.search.framework.agent;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.local;

namespace aima.net.demo.search.eightpuzzle
{
    public class EightPuzzleSimulatedAnnealingDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleSimulatedAnnealingDemo();
        }

        static void eightPuzzleSimulatedAnnealingDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Simulated Annealing  Search -->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                SimulatedAnnealingSearch<EightPuzzleBoard, IAction>
                    search = new SimulatedAnnealingSearch<EightPuzzleBoard, IAction>(
                        EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                System.Console.WriteLine("Search Outcome=" + search.getOutcome());
                System.Console.WriteLine("Final State=\n" + search.getLastSearchState());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
