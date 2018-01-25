using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.eightpuzzle;

namespace aima.net.test.unit.environment.eightpuzzle
{
    [TestClass]
    public class EightPuzzleFunctionsTest
    {
        EightPuzzleBoard board;

        [TestInitialize]
        public void setUp()
        {
            board = new EightPuzzleBoard(new int[] { 1, 2, 5, 3, 4, 0, 6, 7, 8 });
        }

        [TestMethod]
        public void testGenerateCorrect3Successors()
        {
            ICollection<IAction> actions = CollectionFactory.CreateQueue<IAction>(EightPuzzleFunctions.getActions(board));
            Assert.AreEqual(3, actions.Size());

            // test first successor
            EightPuzzleBoard expectedFirst = new EightPuzzleBoard(new int[] { 1, 2,
                0, 3, 4, 5, 6, 7, 8 });
            EightPuzzleBoard actualFirst = (EightPuzzleBoard)EightPuzzleFunctions.getResult(board, actions.Get(0));
            Assert.AreEqual(expectedFirst, actualFirst);
            Assert.AreEqual(EightPuzzleBoard.UP, actions.Get(0));

            // test second successor
            EightPuzzleBoard expectedSecond = new EightPuzzleBoard(new int[] { 1,
                2, 5, 3, 4, 8, 6, 7, 0 });
            EightPuzzleBoard actualSecond = (EightPuzzleBoard)EightPuzzleFunctions.getResult(board, actions.Get(1));
            Assert.AreEqual(expectedSecond, actualSecond);
            Assert.AreEqual(EightPuzzleBoard.DOWN, actions.Get(1));

            // test third successor
            EightPuzzleBoard expectedThird = new EightPuzzleBoard(new int[] { 1, 2,
                5, 3, 0, 4, 6, 7, 8 });
            EightPuzzleBoard actualThird = (EightPuzzleBoard)EightPuzzleFunctions.getResult(board, actions.Get(2));
            Assert.AreEqual(expectedThird, actualThird);
            Assert.AreEqual(EightPuzzleBoard.LEFT, actions.Get(2));
        }

        [TestMethod]
        public void testGenerateCorrectWhenGapMovedRightward()
        {
            board.moveGapLeft();// gives { 1, 2, 5, 3, 0, 4, 6, 7, 8 }
            Assert.AreEqual(new EightPuzzleBoard(new int[] { 1, 2, 5, 3, 0, 4,
                6, 7, 8 }), board);

            ICollection<IAction> actions = CollectionFactory.CreateQueue<IAction>(EightPuzzleFunctions.getActions(board));
            Assert.AreEqual(4, actions.Size());

            EightPuzzleBoard expectedFourth = new EightPuzzleBoard(new int[] { 1,
                2, 5, 3, 4, 0, 6, 7, 8 });
            EightPuzzleBoard actualFourth = (EightPuzzleBoard)EightPuzzleFunctions.getResult(board, actions.Get(3));
            Assert.AreEqual(expectedFourth, actualFourth);
            Assert.AreEqual(EightPuzzleBoard.RIGHT, actions.Get(3));
        }
    }

}
