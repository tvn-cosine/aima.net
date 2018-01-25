using aima.net.exceptions;
using aima.net.environment.nqueens;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.framework.qsearch;
using aima.net.search.uninformed;

namespace aima.net.demo.search.nqueens
{
    public class NQueensWithBreadthFirstSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensWithBreadthFirstSearch();
        }

        static void nQueensWithBreadthFirstSearch()
        {
            try
            {
                System.Console.WriteLine("\nNQueensDemo BFS -->");
                IProblem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                ISearchForActions<NQueensBoard, QueenAction> 
                    search = new BreadthFirstSearch<NQueensBoard, QueenAction>(new TreeSearch<NQueensBoard, QueenAction>());
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
