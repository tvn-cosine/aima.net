using aima.net.agent.api;
using aima.net.exceptions;
using aima.net.environment.eightpuzzle;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.framework.qsearch;
using aima.net.search.informed;

namespace aima.net.demo.search.eightpuzzle
{
    public class EightPuzzleGreedyBestFirstManhattanDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleGreedyBestFirstManhattanDemo();
        }

        static void eightPuzzleGreedyBestFirstManhattanDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (ManhattanHeursitic)-->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                ISearchForActions<EightPuzzleBoard, IAction>
                    search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>(
                        new GraphSearch<EightPuzzleBoard, IAction>(),
                        EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
