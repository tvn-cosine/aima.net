using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.environment.cellworld;

namespace aima.net.test.unit.environment.cellworld
{
    [TestClass]
    public class CellWorldTest
    { 
        private CellWorld<double> cw;

        [TestInitialize]
        public void setUp()
        {
            cw = CellWorldFactory.CreateCellWorldForFig17_1();
        }

        [TestMethod]
        public void testNumberOfCells()
        {
            Assert.AreEqual(11, cw.GetCells().Size());
        }

        [TestMethod]
        public void testMoveUpIntoAdjacentCellChangesPositionCorrectly()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(1, 1), CellWorldAction.Up);
            Assert.AreEqual(1, sDelta.getX());
            Assert.AreEqual(2, sDelta.getY());
        }

        [TestMethod]
        public void testMoveUpIntoWallLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(1, 3), CellWorldAction.Up);
            Assert.AreEqual(1, sDelta.getX());
            Assert.AreEqual(3, sDelta.getY());
        }

        [TestMethod]
        public void testMoveUpIntoRemovedCellLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(2, 1),
                    CellWorldAction.Up);
            Assert.AreEqual(2, sDelta.getX());
            Assert.AreEqual(1, sDelta.getY());
        }

        [TestMethod]
        public void testMoveDownIntoAdjacentCellChangesPositionCorrectly()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(1, 2),
                    CellWorldAction.Down);
            Assert.AreEqual(1, sDelta.getX());
            Assert.AreEqual(1, sDelta.getY());
        }

        [TestMethod]
        public void testMoveDownIntoWallLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(1, 1),
                    CellWorldAction.Down);
            Assert.AreEqual(1, sDelta.getX());
            Assert.AreEqual(1, sDelta.getY());
        }

        [TestMethod]
        public void testMoveDownIntoRemovedCellLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(2, 3),
                    CellWorldAction.Down);
            Assert.AreEqual(2, sDelta.getX());
            Assert.AreEqual(3, sDelta.getY());
        }

        [TestMethod]
        public void testMoveLeftIntoAdjacentCellChangesPositionCorrectly()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(2, 1),
                    CellWorldAction.Left);
            Assert.AreEqual(1, sDelta.getX());
            Assert.AreEqual(1, sDelta.getY());
        }

        [TestMethod]
        public void testMoveLeftIntoWallLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(1, 1),
                    CellWorldAction.Left);
            Assert.AreEqual(1, sDelta.getX());
            Assert.AreEqual(1, sDelta.getY());
        }

        [TestMethod]
        public void testMoveLeftIntoRemovedCellLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(3, 2),
                    CellWorldAction.Left);
            Assert.AreEqual(3, sDelta.getX());
            Assert.AreEqual(2, sDelta.getY());
        }

        [TestMethod]
        public void testMoveRightIntoAdjacentCellChangesPositionCorrectly()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(1, 1),
                    CellWorldAction.Right);
            Assert.AreEqual(2, sDelta.getX());
            Assert.AreEqual(1, sDelta.getY());
        }

        [TestMethod]
        public void testMoveRightIntoWallLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(4, 1),
                    CellWorldAction.Right);
            Assert.AreEqual(4, sDelta.getX());
            Assert.AreEqual(1, sDelta.getY());
        }

        [TestMethod]
        public void testMoveRightIntoRemovedCellLeavesPositionUnchanged()
        {
            Cell<double> sDelta = cw.Result(cw.GetCellAt(1, 2),
                    CellWorldAction.Right);
            Assert.AreEqual(1, sDelta.getX());
            Assert.AreEqual(2, sDelta.getY());
        }
    }

}
