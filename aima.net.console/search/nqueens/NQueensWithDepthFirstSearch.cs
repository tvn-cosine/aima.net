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
    public class NQueensWithDepthFirstSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensWithDepthFirstSearch();
        }

        static void nQueensWithDepthFirstSearch()
        {
            System.Console.WriteLine("\nNQueensDemo DFS -->");
            try
            {
                IProblem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                ISearchForActions<NQueensBoard, QueenAction>
                    search = new DepthFirstSearch<NQueensBoard, QueenAction>(
                        new GraphSearch<NQueensBoard, QueenAction>());
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
