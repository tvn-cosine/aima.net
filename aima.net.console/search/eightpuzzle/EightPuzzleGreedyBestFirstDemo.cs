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
    class EightPuzzleGreedyBestFirstDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleGreedyBestFirstDemo();
        }

        static void eightPuzzleGreedyBestFirstDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (MisplacedTileHeursitic)-->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                ISearchForActions<EightPuzzleBoard, IAction>
                    search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>(
                        new GraphSearch<EightPuzzleBoard, IAction>(), 
                        EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
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
