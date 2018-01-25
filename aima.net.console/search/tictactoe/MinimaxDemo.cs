using aima.net.datastructures;
using aima.net.environment.tictactoe;
using aima.net.search.adversarial;
using aima.net.search.adversarial.api;

namespace aima.net.demo.search.tictactoe
{
    public class MinimaxDemo
    {
        public static void Main(params string[] args)
        {
            System.Console.WriteLine("TIC-TAC-TOE DEMO");
            System.Console.WriteLine("");
            startMinimaxDemo();
        }

        static void startMinimaxDemo()
        {
            System.Console.WriteLine("MINI MAX DEMO\n");
            TicTacToeGame game = new TicTacToeGame();
            TicTacToeState currState = game.GetInitialState();
            IAdversarialSearch<TicTacToeState, XYLocation>
                search = MinimaxSearch<TicTacToeState, XYLocation, string>.createFor(game);
            while (!(game.IsTerminal(currState)))
            {
                System.Console.WriteLine(game.GetPlayer(currState) + "  playing ... ");
                XYLocation action = search.makeDecision(currState);
                currState = game.GetResult(currState, action);
                System.Console.WriteLine(currState);
            }
            System.Console.WriteLine("MINI MAX DEMO done");
        }
    }
}
