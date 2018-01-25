using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.environment.nqueens;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.test.unit.environment.nqueens
{
    [TestClass]
    public class NQueensFunctionsTest
    {
        private IActionsFunction<NQueensBoard, QueenAction> actionsFn;
        private IResultFunction<NQueensBoard, QueenAction> resultFn;
        private GoalTest<NQueensBoard> goalTest;

        private NQueensBoard oneBoard;
        private NQueensBoard eightBoard;
        private NQueensBoard board;
         
        [TestInitialize]
        public void setUp()
        {
            oneBoard = new NQueensBoard(1);
            eightBoard = new NQueensBoard(8);
            board = new NQueensBoard(8);

            actionsFn = NQueensFunctions.getIFActionsFunction();
            resultFn = NQueensFunctions.getResultFunction();
            goalTest = NQueensFunctions.testGoal;
        }

        [TestMethod]
        public void testSimpleBoardSuccessorGenerator()
        {
            ICollection<QueenAction> actions = CollectionFactory.CreateQueue<QueenAction>(actionsFn.apply(oneBoard));
            Assert.AreEqual(1, actions.Size());
            NQueensBoard next = resultFn.apply(oneBoard, actions.Get(0));
            Assert.AreEqual(1, next.getNumberOfQueensOnBoard());
        }

        [TestMethod]
        public void testComplexBoardSuccessorGenerator()
        {
            ICollection<QueenAction> actions = CollectionFactory.CreateQueue<QueenAction>(actionsFn.apply(eightBoard));
            Assert.AreEqual(8, actions.Size());
            NQueensBoard next = resultFn.apply(eightBoard, actions.Get(0));
            Assert.AreEqual(1, next.getNumberOfQueensOnBoard());

            actions = CollectionFactory.CreateQueue<QueenAction>(actionsFn.apply(next));
            Assert.AreEqual(6, actions.Size());
        }


        [TestMethod]
        public void testEmptyBoard()
        {
            Assert.IsFalse(goalTest(board));
        }

        [TestMethod]
        public void testSingleSquareBoard()
        {
            board = new NQueensBoard(1);
            Assert.IsFalse(goalTest(board));
            board.addQueenAt(new XYLocation(0, 0));
            Assert.IsTrue(goalTest(board));
        }

        [TestMethod]
        public void testInCorrectPlacement()
        {
            Assert.IsFalse(goalTest(board));
            // This is the configuration of Figure 3.5 (b) in AIMA 2nd Edition
            board.addQueenAt(new XYLocation(0, 0));
            board.addQueenAt(new XYLocation(1, 2));
            board.addQueenAt(new XYLocation(2, 4));
            board.addQueenAt(new XYLocation(3, 6));
            board.addQueenAt(new XYLocation(4, 1));
            board.addQueenAt(new XYLocation(5, 3));
            board.addQueenAt(new XYLocation(6, 5));
            board.addQueenAt(new XYLocation(7, 7));
            Assert.IsFalse(goalTest(board));
        }

        [TestMethod]
        public void testCorrectPlacement()
        {
            Assert.IsFalse(goalTest(board));
            // This is the configuration of Figure 5.9 (c) in AIMA 2nd Edition
            board.addQueenAt(new XYLocation(0, 1));
            board.addQueenAt(new XYLocation(1, 4));
            board.addQueenAt(new XYLocation(2, 6));
            board.addQueenAt(new XYLocation(3, 3));
            board.addQueenAt(new XYLocation(4, 0));
            board.addQueenAt(new XYLocation(5, 7));
            board.addQueenAt(new XYLocation(6, 5));
            board.addQueenAt(new XYLocation(7, 2));

            Assert.IsTrue(goalTest(board));
        }
    }
}
