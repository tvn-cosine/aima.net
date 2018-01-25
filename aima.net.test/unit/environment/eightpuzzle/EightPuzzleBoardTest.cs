using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.environment.eightpuzzle;

namespace aima.net.test.unit.environment.eightpuzzle
{
    [TestClass]
    public class EightPuzzleBoardTest
    {
        EightPuzzleBoard board;

        [TestInitialize]
        public void setUp()
        {
            board = new EightPuzzleBoard();
        }

        [TestMethod]
        public void testGetBoard()
        {
            int[] expected = new int[] { 5, 4, 0, 6, 1, 8, 7, 3, 2 };
            int[] boardRepr = board.getState();
            Assert.AreEqual(expected[0], boardRepr[0]);
            Assert.AreEqual(expected[1], boardRepr[1]);
            Assert.AreEqual(expected[2], boardRepr[2]);
            Assert.AreEqual(expected[3], boardRepr[3]);
            Assert.AreEqual(expected[4], boardRepr[4]);
            Assert.AreEqual(expected[5], boardRepr[5]);
            Assert.AreEqual(expected[6], boardRepr[6]);
            Assert.AreEqual(expected[7], boardRepr[7]);
            Assert.AreEqual(expected[8], boardRepr[8]);
        }

        [TestMethod]
        public void testGetLocation()
        {
            Assert.AreEqual(new XYLocation(0, 2), board.getLocationOf(0));
            Assert.AreEqual(new XYLocation(1, 1), board.getLocationOf(1));
            Assert.AreEqual(new XYLocation(2, 2), board.getLocationOf(2));
            Assert.AreEqual(new XYLocation(2, 1), board.getLocationOf(3));
            Assert.AreEqual(new XYLocation(0, 1), board.getLocationOf(4));
            Assert.AreEqual(new XYLocation(0, 0), board.getLocationOf(5));
            Assert.AreEqual(new XYLocation(1, 0), board.getLocationOf(6));
            Assert.AreEqual(new XYLocation(2, 0), board.getLocationOf(7));
            Assert.AreEqual(new XYLocation(1, 2), board.getLocationOf(8));
        }

        [TestMethod]
        public void testGetValueAt()
        {
            Assert.AreEqual(5, board.getValueAt(new XYLocation(0, 0)));
            Assert.AreEqual(4, board.getValueAt(new XYLocation(0, 1)));
            Assert.AreEqual(0, board.getValueAt(new XYLocation(0, 2)));
            Assert.AreEqual(6, board.getValueAt(new XYLocation(1, 0)));
            Assert.AreEqual(1, board.getValueAt(new XYLocation(1, 1)));
            Assert.AreEqual(8, board.getValueAt(new XYLocation(1, 2)));
            Assert.AreEqual(7, board.getValueAt(new XYLocation(2, 0)));
            Assert.AreEqual(3, board.getValueAt(new XYLocation(2, 1)));
            Assert.AreEqual(2, board.getValueAt(new XYLocation(2, 2)));
        }

        [TestMethod]
        public void testGetPositions()
        {
            ICollection<XYLocation> expected = CollectionFactory.CreateQueue<XYLocation>();
            expected.Add(new XYLocation(0, 2));
            expected.Add(new XYLocation(1, 1));
            expected.Add(new XYLocation(2, 2));
            expected.Add(new XYLocation(2, 1));
            expected.Add(new XYLocation(0, 1));
            expected.Add(new XYLocation(0, 0));
            expected.Add(new XYLocation(1, 0));
            expected.Add(new XYLocation(2, 0));
            expected.Add(new XYLocation(1, 2));

            ICollection<XYLocation> actual = board.getPositions();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void testSetBoard()
        {
            ICollection<XYLocation> passedIn = CollectionFactory.CreateQueue<XYLocation>();
            passedIn.Add(new XYLocation(1, 1));
            passedIn.Add(new XYLocation(0, 2));
            passedIn.Add(new XYLocation(2, 2));
            passedIn.Add(new XYLocation(2, 1));
            passedIn.Add(new XYLocation(0, 1));
            passedIn.Add(new XYLocation(0, 0));
            passedIn.Add(new XYLocation(1, 0));
            passedIn.Add(new XYLocation(2, 0));
            passedIn.Add(new XYLocation(1, 2));
            board.setBoard(passedIn);
            Assert.AreEqual(new XYLocation(1, 1), board.getLocationOf(0));
            Assert.AreEqual(new XYLocation(0, 2), board.getLocationOf(1));
        }
    }

}
