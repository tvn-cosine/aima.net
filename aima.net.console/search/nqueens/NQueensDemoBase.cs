using aima.net.collections.api;
using aima.net.environment.nqueens;

namespace aima.net.demo.search.nqueens
{
    public abstract class NQueensDemoBase : SearchDemoBase
    {
        protected const int boardSize = 8;

        protected static void printActions(ICollection<QueenAction> actions)
        {
            foreach (QueenAction action in actions)
            {
                System.Console.WriteLine(action);
            }
        }
    }
}
