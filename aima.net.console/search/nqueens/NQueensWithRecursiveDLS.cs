using aima.net.exceptions;
using aima.net.environment.nqueens;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.uninformed;

namespace aima.net.demo.search.nqueens
{
    public class NQueensWithRecursiveDLS : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensWithRecursiveDLS();
        }

        static void nQueensWithRecursiveDLS()
        {
            System.Console.WriteLine("\nNQueensDemo recursive DLS -->");
            try
            {
                IProblem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                ISearchForActions<NQueensBoard, QueenAction> 
                    search = new DepthLimitedSearch<NQueensBoard, QueenAction>(boardSize);
                SearchAgent<NQueensBoard, QueenAction> 
                    agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
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
