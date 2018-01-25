using aima.net.exceptions;
using aima.net.environment.nqueens;
using aima.net.search.framework.agent;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.local;

namespace aima.net.demo.search.nqueens
{
    public class NQueensHillClimbingSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensHillClimbingSearch();
        }

        static void nQueensHillClimbingSearch()
        {
            System.Console.WriteLine("\nNQueensDemo HillClimbing  -->");
            try
            {
                IProblem<NQueensBoard, QueenAction>
                    problem = NQueensFunctions.createCompleteStateFormulationProblem(
                        boardSize,
                        NQueensBoard.Config.QUEENS_IN_FIRST_ROW);
                HillClimbingSearch<NQueensBoard, QueenAction>
                    search = new HillClimbingSearch<NQueensBoard, QueenAction>(
                        NQueensFunctions.createAttackingPairsHeuristicFunction());
                SearchAgent<NQueensBoard, QueenAction>
                    agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

                System.Console.WriteLine();
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
