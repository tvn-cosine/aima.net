using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.util;
using aima.net.util.math.geom.shapes;

namespace aima.net.test.unit.util.math.geom.shapes
{
    /**
     * Test case for the {@code aima.core.util.math.geom} package.
     * Tests valid implementation of the {@link IGeometric2D} interface by {@link Line2D}.
     * 
     * @author Arno v. Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    [TestClass]
    public class Line2DTest
    {
        private Line2D testLine;
        private Line2D testLine2;
        private Line2D testLine3;
        private Line2D testLine4;
        private Point2D zeroPoint;

        [TestInitialize]
        public void setUp()
        {
            testLine = new Line2D(2.0d, 3.0d, 4.0d, 5.0d);
            testLine2 = new Line2D(6.0d, 4.0d, 6.0d, -3.0d);
            testLine3 = new Line2D(6.0d, -3.0d, 2.0d, 2.0d);
            testLine4 = new Line2D(3.0d, 4.0d, 6.0d, 4.0d);
            zeroPoint = new Point2D(0.0d, 0.0d);
        }

        [TestMethod]
        public void testRandomPoint()
        {
            Assert.IsTrue(  testLine4.isInsideBorder(testLine4.randomPoint()));

            for (int i = 0; i < 1000;++i)
            {
                Assert.IsTrue( testLine.isInsideBorder(testLine.randomPoint()));
            }
        }

        [TestMethod]
        public void testIsInside()
        {
            Assert.IsFalse( testLine.isInside(zeroPoint));
            Assert.IsFalse(  testLine.isInside(new Point2D(3.0d, 4.0d)));
        }

        [TestMethod]
        public void testIsInsideBorder()
        {
            Assert.IsFalse( testLine.isInsideBorder(zeroPoint));
            Assert.IsTrue( testLine.isInsideBorder(new Point2D(3.0d, 4.0d)));
            Assert.IsTrue( testLine2.isInsideBorder(new Point2D(6.0d, 2.0d)));
        }

        [TestMethod]
        public void testRayCast()
        {

            // Static rayCast tests
            Assert.AreEqual(  double.PositiveInfinity, testLine.rayCast(new Ray2D(1.0d, 1.0d, -7.0d, -8.0d)), 0.000005d);
            Assert.AreEqual(  System.Math.Sqrt(2), testLine.rayCast(new Ray2D(2.0d, 5.0d, 4.0d, 3.0d)), 0.000005d);
            Assert.AreEqual(  6, testLine2.rayCast(new Ray2D(zeroPoint, Vector2D.X_VECTOR)), 0.000005d);
            Assert.AreEqual(  3.0, testLine2.rayCast(new Ray2D(new Point2D(3.0d, 3.0d), Vector2D.X_VECTOR)), 0.000005d);
            Assert.AreEqual(  double.PositiveInfinity, testLine2.rayCast(new Ray2D(new Point2D(6.0d, 2.0d), Vector2D.X_VECTOR)), 0.000005d);
            Assert.AreEqual(  3.6, testLine3.rayCast(new Ray2D(zeroPoint, Vector2D.X_VECTOR)), 0.000005d);

            // Serial RayCast tests
            Point2D randomPoint;
            Line2D randomLine;
            Point2D randomPointOnLine;
            int counter = 5000;

            // generate a random point and another random point on a random Line and compare the distance between the two with a rayCast from the former towards the latter.
            do
            {
                randomLine = new Line2D(Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d));
                randomPointOnLine = randomLine.randomPoint();
                randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d));
                // System.Console.Writef("Line2D rayCast test no. %d: Line (%.2f,%.2f,%.2f,%.2f), point (%.2f,%.2f), point on line (%.2f,%.2f), distance: %.2f.\n", counter, randomLine.getStart().getX(), randomLine.getStart().getY(), randomLine.getEnd().getX(), randomLine.getEnd().getY(), randomPoint.getX(), randomPoint.getY(), randomPointOnLine.getX(), randomPointOnLine.getY(), randomPoint.distance(randomPointOnLine));
                Assert.AreEqual(  randomPoint.distance(randomPointOnLine), randomLine.rayCast(new Ray2D(randomPoint, randomPoint.vec(randomPointOnLine))), 0.000005d);
                counter -= 1;
            } while (counter > 0);
        }

        [TestMethod]
        public void testGetBounds()
        {
            Assert.AreNotEqual(  1.0d, testLine.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreNotEqual(  6.0d, testLine.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreNotEqual(  6.0d, testLine.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreNotEqual(  1.0d, testLine.getBounds().getLowerRight().getY(), 0.000005d);
            Assert.AreEqual(  2.0d, testLine.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreEqual(  5.0d, testLine.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreEqual(  4.0d, testLine.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreEqual(  3.0d, testLine.getBounds().getLowerRight().getY(), 0.000005d);
        }

        [TestMethod]
        public void testTransform()
        {
            Assert.AreEqual(  testLine.transform(TransformMatrix2D.UNITY_MATRIX).getStart().getX(), testLine.getStart().getX(), 0.000005d);
            Assert.AreEqual(  testLine.transform(TransformMatrix2D.UNITY_MATRIX).getStart().getY(), testLine.getStart().getY(), 0.000005d);
            Assert.AreEqual(  testLine.transform(TransformMatrix2D.UNITY_MATRIX).getEnd().getX(), testLine.getEnd().getX(), 0.000005d);
            Assert.AreEqual(  testLine.transform(TransformMatrix2D.UNITY_MATRIX).getEnd().getY(), testLine.getEnd().getY(), 0.000005d);
        }
    }

}
