using aima.net.agent.api;
using aima.net.exceptions;
using aima.net.environment.eightpuzzle;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.uninformed;

namespace aima.net.demo.search.eightpuzzle
{
    public class EightPuzzleDLSDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleDLSDemo();
        }

        static void eightPuzzleDLSDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo recursive DLS (9) -->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                ISearchForActions<EightPuzzleBoard, IAction> search = new DepthLimitedSearch<EightPuzzleBoard, IAction>(9);
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
