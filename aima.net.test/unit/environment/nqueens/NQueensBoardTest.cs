using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.datastructures;
using aima.net.environment.nqueens;

namespace aima.net.test.unit.environment.nqueens
{
    [TestClass]
    public class NQueensBoardTest
    {
        NQueensBoard board;

        [TestInitialize]
        public void setUp()
        {

            board = new NQueensBoard(8);
        }

        [TestMethod]
        public void testBasics()
        {
            Assert.AreEqual(0, board.getNumberOfQueensOnBoard());
            board.addQueenAt(new XYLocation(0, 0));
            Assert.AreEqual(1, board.getNumberOfQueensOnBoard());
            board.addQueenAt(new XYLocation(0, 0));
            Assert.AreEqual(1, board.getNumberOfQueensOnBoard());
            board.addQueenAt(new XYLocation(1, 1));
            Assert.AreEqual(2, board.getNumberOfQueensOnBoard());
            Assert.IsTrue(board.queenExistsAt(new XYLocation(1, 1)));
            Assert.IsTrue(board.queenExistsAt(new XYLocation(0, 0)));
            board.moveQueen(new XYLocation(1, 1), new XYLocation(3, 3));
            Assert.IsTrue(board.queenExistsAt(new XYLocation(3, 3)));
            Assert.IsTrue(!(board.queenExistsAt(new XYLocation(1, 1))));
            Assert.AreEqual(2, board.getNumberOfQueensOnBoard());
        }

        [TestMethod]
        public void testCornerQueenAttack1()
        { 
            board.addQueenAt(new XYLocation(0, 0));
            Assert.AreEqual(false,
                    board.isSquareUnderAttack(new XYLocation(0, 0)));
            // queen on square not included
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(1, 0)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(7, 0)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(0, 7)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(1, 1)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(2, 2)));
            Assert.AreEqual(false,
                    board.isSquareUnderAttack(new XYLocation(2, 1)));
            Assert.AreEqual(false,
                    board.isSquareUnderAttack(new XYLocation(1, 2)));
        }

        [TestMethod]
        public void testCornerQueenAttack2()
        {

            board.addQueenAt(new XYLocation(7, 7));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(0, 0)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(7, 0)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(0, 7)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(7, 0)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(6, 6)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(5, 5)));
            Assert.AreEqual(false,
                    board.isSquareUnderAttack(new XYLocation(6, 5)));
            Assert.AreEqual(false,
                    board.isSquareUnderAttack(new XYLocation(5, 6)));
        }

        [TestMethod]
        public void testEdgeQueenAttack()
        {

            board.addQueenAt(new XYLocation(0, 3));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(0, 0)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(0, 7)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(7, 3)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(3, 0)));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(4, 7)));
        }

        [TestMethod]
        public void testAttack2()
        {

            board.addQueenAt(new XYLocation(7, 0));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(6, 1)));
        }

        [TestMethod]
        public void testAttack3()
        {

            board.addQueenAt(new XYLocation(0, 0));
            Assert.AreEqual(true,
                    board.isSquareUnderAttack(new XYLocation(0, 1)));
        }

        [TestMethod]
        public void testAttack4()
        {

            board.addQueenAt(new XYLocation(0, 2));
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(1, 1)));
        }

        [TestMethod]
        public void testMidBoardDiagonalAttack()
        {

            board.addQueenAt(new XYLocation(3, 3));
            // forwardDiagonal from the queen
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(4, 2)));
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(4, 4)));
            // backwardDiagonal from the queen
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(2, 2)));
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(2, 4)));
        }

        [TestMethod]
        public void testCornerDiagonalAttack()
        {

            board.addQueenAt(new XYLocation(0, 0));
            // forwardDiagonal from the queen
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(1, 1)));
            board.clear();

            board.addQueenAt(new XYLocation(7, 7));
            // backwardDiagonal from the queen
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(6, 6)));

            // Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(2, 2)));
            // Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(2, 4)));
        }

        [TestMethod]
        public void testAttack6()
        {

            board.addQueenAt(new XYLocation(1, 6));
            Assert.IsTrue(board.isSquareUnderAttack(new XYLocation(0, 7)));
        }

        [TestMethod]
        public void testRemoveQueen()
        {

            board.addQueenAt(new XYLocation(0, 0));
            Assert.AreEqual(1, board.getNumberOfQueensOnBoard());
            board.removeQueenFrom(new XYLocation(0, 0));
            Assert.AreEqual(0, board.getNumberOfQueensOnBoard());
        }

        [TestMethod]
        public void testMoveQueen()
        {

            XYLocation from = new XYLocation(0, 0);
            XYLocation to = new XYLocation(1, 1);

            board.addQueenAt(from);
            Assert.AreEqual(1, board.getNumberOfQueensOnBoard());
            Assert.IsTrue(board.queenExistsAt(from));
            Assert.IsFalse(board.queenExistsAt(to));

            board.moveQueen(from, to);
            Assert.AreEqual(1, board.getNumberOfQueensOnBoard());
            Assert.IsFalse(board.queenExistsAt(from));
            Assert.IsTrue(board.queenExistsAt(to));
        }

        [TestMethod]
        public void testMoveNonExistentQueen()
        {

            XYLocation from = new XYLocation(0, 0);
            XYLocation to = new XYLocation(1, 1);
            board.moveQueen(from, to);

            Assert.AreEqual(0, board.getNumberOfQueensOnBoard());
        }

        [TestMethod]
        public void testRemoveNonExistentQueen()
        {
            board.removeQueenFrom(new XYLocation(0, 0));
            Assert.AreEqual(0, board.getNumberOfQueensOnBoard());
        }

        [TestMethod]
        public void testEquality()
        {

            board.addQueenAt(new XYLocation(0, 0));
            NQueensBoard board2 = new NQueensBoard(8);
            board2.addQueenAt(new XYLocation(0, 0));
            Assert.AreEqual(board, board2);
            NQueensBoard board3 = new NQueensBoard(8);
            board3.addQueenAt(new XYLocation(0, 1));
            Assert.IsFalse(board.Equals(board3));
        }

        [TestMethod]
        public void testPrint()
        {

            NQueensBoard board2 = new NQueensBoard(2);
            board2.addQueenAt(new XYLocation(0, 0));
            string expected = " Q  - \n -  - \n";
            Assert.AreEqual(expected, board2.getBoardPic());
        }

        [TestMethod]
        public void testDontPlaceTwoQueensOnOneSquare()
        {

            board.addQueenAt(new XYLocation(0, 0));
            board.addQueenAt(new XYLocation(0, 0));
            Assert.AreEqual(1, board.getNumberOfQueensOnBoard());
        }

        [TestMethod]
        public void testSimpleHorizontalAttack()
        {
            XYLocation loc = new XYLocation(0, 0);
            board.addQueenAt(loc);
            Assert.AreEqual(0, board.getNumberOfAttacksOn(loc));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(new XYLocation(1, 0)));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc.Right()));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(new XYLocation(7, 0)));
        }

        [TestMethod]
        public void testSimpleVerticalAttack()
        {
            XYLocation loc = new XYLocation(0, 0);
            board.addQueenAt(loc);
            Assert.AreEqual(0, board.getNumberOfAttacksOn(loc));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc.Down()));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(new XYLocation(0, 7)));
        }

        [TestMethod]
        public void testSimpleDiagonalAttack()
        {
            XYLocation loc = new XYLocation(3, 3);
            board.addQueenAt(loc);
            Assert.AreEqual(0, board.getNumberOfAttacksOn(loc));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc.Down().Right()));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc.Down().Left()));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc.Up().Left()));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc.Up().Right()));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(new XYLocation(7, 7)));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(new XYLocation(0, 0)));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(new XYLocation(6, 0)));
            Assert.AreEqual(1, board.getNumberOfAttacksOn(new XYLocation(0, 6)));
        }

        [TestMethod]
        public void testMultipleQueens()
        {
            XYLocation loc1 = new XYLocation(3, 3);
            board.addQueenAt(loc1);
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc1.Right()));

            board.addQueenAt(loc1.Right().Right());
            Assert.AreEqual(1, board.getNumberOfAttacksOn(loc1));
            Assert.AreEqual(2, board.getNumberOfAttacksOn(loc1.Right()));

            board.addQueenAt(loc1.Right().Down());
            Assert.AreEqual(2, board.getNumberOfAttacksOn(loc1));
            Assert.AreEqual(3, board.getNumberOfAttacksOn(loc1.Right()));
            Assert.AreEqual(2, board.getNumberOfAttacksOn(loc1.Right().Right()));
        }

        [TestMethod]
        public void testBoardDisplay()
        {
            board.addQueenAt(new XYLocation(0, 5));
            board.addQueenAt(new XYLocation(1, 6));
            board.addQueenAt(new XYLocation(2, 1));
            board.addQueenAt(new XYLocation(3, 3));
            board.addQueenAt(new XYLocation(4, 6));
            board.addQueenAt(new XYLocation(5, 4));
            board.addQueenAt(new XYLocation(6, 7));
            board.addQueenAt(new XYLocation(7, 7));
            Assert.AreEqual(" -  -  -  -  -  -  -  - \n"
                    + " -  -  Q  -  -  -  -  - \n" + " -  -  -  -  -  -  -  - \n"
                    + " -  -  -  Q  -  -  -  - \n" + " -  -  -  -  -  Q  -  - \n"
                    + " Q  -  -  -  -  -  -  - \n" + " -  Q  -  -  Q  -  -  - \n"
                    + " -  -  -  -  -  -  Q  Q \n", board.getBoardPic());

            Assert.AreEqual("--------\n" + "--Q-----\n" + "--------\n"
                    + "---Q----\n" + "-----Q--\n" + "Q-------\n" + "-Q--Q---\n"
                    + "------QQ\n", board.ToString());
        }
    }

}
