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
    public class NQueensWithIterativeDeepeningSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensWithIterativeDeepeningSearch();
        }

        static void nQueensWithIterativeDeepeningSearch()
        {
            System.Console.WriteLine("\nNQueensDemo Iterative DS  -->");
            try
            {
                IProblem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                ISearchForActions<NQueensBoard, QueenAction> 
                    search = new IterativeDeepeningSearch<NQueensBoard, QueenAction>();
                SearchAgent<NQueensBoard, QueenAction> 
                    agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

                System.Console.WriteLine();
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
