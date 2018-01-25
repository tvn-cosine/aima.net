using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.datastructures;
using aima.net.environment.tictactoe;
using aima.net.search.adversarial;

namespace aima.net.test.unit.environment.tictactoe
{
    [TestClass]  
    public class TicTacToeTest
    {
        private TicTacToeGame game;
        private TicTacToeState state;
        private double epsilon = 0.0001;

        [TestInitialize]
        public void setUp()
        {
            game = new TicTacToeGame();
            state = game.GetInitialState();
        }

        [TestMethod]
        public void testCreation()
        {
            game = new TicTacToeGame();
            state = game.GetInitialState();
            Assert.AreEqual(9, game.GetActions(state).Size());
            Assert.AreEqual(TicTacToeState.X, game.GetPlayer(state));
        }

        [TestMethod]
        public void testHashCode()
        {
            TicTacToeState initialState1 = game.GetInitialState();
            TicTacToeState initialState2 = game.GetInitialState();
            Assert.AreEqual(initialState1.GetHashCode(), initialState2.GetHashCode());
            TicTacToeState state1 = game.GetResult(initialState1, new XYLocation(0, 0));
            Assert.AreNotEqual(state1.GetHashCode(), initialState2.GetHashCode());
            TicTacToeState state2 = game.GetResult(initialState2, new XYLocation(0, 0));
            Assert.AreEqual(state1.GetHashCode(), state2.GetHashCode());

        }

        [TestMethod]
        public void testOnCreationBoardIsEmpty()
        {
            Assert.AreEqual(TicTacToeState.EMPTY, state.getValue(0, 0));
            Assert.AreEqual(TicTacToeState.EMPTY, state.getValue(0, 2));
            Assert.AreEqual(TicTacToeState.EMPTY, state.getValue(2, 0));
            Assert.AreEqual(TicTacToeState.EMPTY, state.getValue(2, 2));
            Assert.AreEqual(true, state.isEmpty(0, 0));
            Assert.AreEqual(true, state.isEmpty(2, 2));
        }

        [TestMethod]
        public void testMakingOneMoveChangesState()
        {
            state = game.GetResult(state, new XYLocation(0, 0));
            Assert.AreEqual(TicTacToeState.X, state.getValue(0, 0));
            Assert.AreEqual(false, state.isEmpty(0, 0));
            Assert.AreEqual(8, game.GetActions(state).Size());
            Assert.AreEqual(TicTacToeState.O, game.GetPlayer(state));
        }

        [TestMethod]
        public void testMakingTwoMovesChangesState()
        {
            state = game.GetResult(state, new XYLocation(0, 0));
            state = game.GetResult(state, new XYLocation(0, 1));
            Assert.AreEqual(TicTacToeState.O, state.getValue(0, 1));
            Assert.AreEqual(false, state.isEmpty(0, 1));
            Assert.AreEqual(true, state.isEmpty(1, 0));
            Assert.AreEqual(7, game.GetActions(state).Size());
            Assert.AreEqual(TicTacToeState.X, game.GetPlayer(state));
        }

        [TestMethod]
        public void testVerticalLineThroughBoard()
        {
            state.mark(0, 0);
            state.mark(1, 0);
            state.mark(0, 1);
            state.mark(1, 1);
            Assert.AreEqual(false, state.lineThroughBoard());
            state.mark(new XYLocation(0, 2));
            Assert.AreEqual(true, state.lineThroughBoard());
        }

        [TestMethod]
        public void testHorizontalLineThroughBoard()
        {
            state.mark(0, 0);
            state.mark(0, 1);
            state.mark(1, 0);
            state.mark(1, 1);
            Assert.AreEqual(false, state.lineThroughBoard());
            state.mark(2, 0);
            Assert.AreEqual(true, state.lineThroughBoard());
        }

        [TestMethod]
        public void testDiagonalLineThroughBoard()
        {
            state.mark(0, 0);
            state.mark(0, 1);
            state.mark(1, 1);
            state.mark(0, 2);
            Assert.AreEqual(false, state.lineThroughBoard());
            state.mark(2, 2);
            Assert.AreEqual(true, state.lineThroughBoard());
        }

        [TestMethod]
        public void testMinmaxValueCalculation()
        {
            MinimaxSearch<TicTacToeState, XYLocation, string> search = MinimaxSearch<TicTacToeState, XYLocation, string>.createFor(game);
            Assert.IsTrue(epsilon > System.Math.Abs(search.maxValue(state,                    TicTacToeState.X) - 0.5));
            Assert.IsTrue(epsilon > System.Math.Abs(search.minValue(state,                    TicTacToeState.O) - 0.5));

            // x o x
            // o o x
            // - - -
            // next move: x
            state.mark(0, 0); // x
            state.mark(1, 0); // o
            state.mark(2, 0); // x

            state.mark(0, 1); // o
            state.mark(2, 1); // x
            state.mark(1, 1); // o

            Assert.IsTrue(epsilon > System.Math.Abs(search.maxValue(state, TicTacToeState.X) - 1));
            Assert.IsTrue(epsilon > System.Math.Abs(search.minValue(state, TicTacToeState.O)));
            XYLocation action = search.makeDecision(state);
            Assert.AreEqual(new XYLocation(2, 2), action);
        }

        [TestMethod] 
        public void testMinmaxDecision()
        {
            MinimaxSearch<TicTacToeState, XYLocation, string> search = MinimaxSearch<TicTacToeState, XYLocation, string>                    .createFor(game);
            search.makeDecision(state);
            int expandedNodes = search.getMetrics().getInt(MinimaxSearch<TicTacToeState, XYLocation, string>.METRICS_NODES_EXPANDED);
            Assert.AreEqual(549945, expandedNodes);
        }

        [TestMethod]
        public void testAlphaBetaDecision()
        {
            AlphaBetaSearch<TicTacToeState, XYLocation, string> search = AlphaBetaSearch<TicTacToeState, XYLocation, string>.createFor(game);
            search.makeDecision(state);
            int expandedNodes = search.getMetrics().getInt(MinimaxSearch<TicTacToeState, XYLocation, string>.METRICS_NODES_EXPANDED);
            Assert.AreEqual(30709, expandedNodes);
        }

        [TestMethod]
        public void testIterativeDeepeningAlphaBetaDecision()
        {
            IterativeDeepeningAlphaBetaSearch<TicTacToeState, XYLocation, string> 
                search = IterativeDeepeningAlphaBetaSearch<TicTacToeState, XYLocation, string>
                    .createFor(game, 0.0, 1.0, 100);
            search.makeDecision(state);
            int expandedNodes = search.getMetrics().getInt(MinimaxSearch<TicTacToeState, XYLocation, string>.METRICS_NODES_EXPANDED);
            Assert.AreEqual(76035, expandedNodes);
        }
    } 
}
