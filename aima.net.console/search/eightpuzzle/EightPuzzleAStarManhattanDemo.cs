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
    public class EightPuzzleAStarManhattanDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleAStarManhattanDemo();
        }

        static void eightPuzzleAStarManhattanDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo AStar Search (ManhattanHeursitic)-->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                ISearchForActions<EightPuzzleBoard, IAction>
                    search = new AStarSearch<EightPuzzleBoard, IAction>(
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
