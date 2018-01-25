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
    public  class EightPuzzleIDLSDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleIDLSDemo();
        }

        static void eightPuzzleIDLSDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Iterative DLS -->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                ISearchForActions<EightPuzzleBoard, IAction> search = new IterativeDeepeningSearch<EightPuzzleBoard, IAction>();
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
