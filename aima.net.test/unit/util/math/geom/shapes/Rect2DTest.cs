using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.util;
using aima.net.util.math.geom.shapes;

namespace aima.net.test.unit.util.math.geom.shapes
{
    /**
     * Test case for the {@code aima.core.util.math.geom} package.
     * Tests valid implementation of the {@link IGeometric2D} interface by {@link Rect2D}.
     * 
     * @author Arno v. Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */

    [TestClass]
    public class Rect2DTest
    {

        private Rect2D testRect;
        private Point2D zeroPoint;

        [TestInitialize]
        public void setUp()
        {
            testRect = new Rect2D(3.0d, 4.0d, 5.0d, 8.0d);
            zeroPoint = new Point2D(0.0d, 0.0d);
        }

        [TestMethod]
        public void testRandomPoint()
        {
            for (int i = 0; i < 1000;++i)
            {
                Assert.IsTrue(testRect.isInsideBorder(testRect.randomPoint()));
            }
        }

        [TestMethod]
        public void testIsInside()
        {
            Assert.IsFalse(testRect.isInside(zeroPoint));
            Assert.IsFalse(testRect.isInside(new Point2D(3.0d, 6.0d)));
            Assert.IsTrue(testRect.isInside(new Point2D(4.0d, 6.0d)));
        }

        [TestMethod]
        public void testIsInsideBorder()
        {
            Assert.IsFalse(testRect.isInsideBorder(zeroPoint));
            Assert.IsTrue(testRect.isInsideBorder(new Point2D(3.0d, 6.0d)));
            Assert.IsTrue(testRect.isInsideBorder(new Point2D(4.0d, 6.0d)));
        }

        [TestMethod]
        public void testRayCast()
        {
            // Static RayCast tests
            Assert.AreEqual(double.PositiveInfinity, testRect.rayCast(new Ray2D(1.0d, 1.0d, -7.0d, -8.0d)), 0.000005d);
            Assert.AreEqual(System.Math.Sqrt(2), testRect.rayCast(new Ray2D(2.0d, 3.0d, 3.0d, 4.0d)), 0.000005d);

            // Serial RayCast tests
            Rect2D randomRect;
            Line2D currentSide;
            Point2D randomPointOnSide;
            Point2D randomPoint;
            int i = 100;
            do
            {
                randomRect = new Rect2D(Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d));
                int j = 50;
                do
                {
                    currentSide = new Line2D(randomRect.getUpperLeft(), randomRect.getUpperRight());
                    randomPointOnSide = currentSide.randomPoint();
                    randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(randomPointOnSide.getY(), 1000.0d));
                    Assert.AreEqual(randomPoint.distance(randomPointOnSide), randomRect.rayCast(new Ray2D(randomPoint, randomPoint.vec(randomPointOnSide))), 0.000005d);
                    currentSide = new Line2D(randomRect.getLowerLeft(), randomRect.getLowerRight());
                    randomPointOnSide = currentSide.randomPoint();
                    randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, randomPointOnSide.getY()));
                    Assert.AreEqual(randomPoint.distance(randomPointOnSide), randomRect.rayCast(new Ray2D(randomPoint, randomPoint.vec(randomPointOnSide))), 0.000005d);
                    currentSide = new Line2D(randomRect.getLowerLeft(), randomRect.getUpperLeft());
                    randomPointOnSide = currentSide.randomPoint();
                    randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, randomPointOnSide.getX()), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d));
                    Assert.AreEqual(randomPoint.distance(randomPointOnSide), randomRect.rayCast(new Ray2D(randomPoint, randomPoint.vec(randomPointOnSide))), 0.000005d);
                    currentSide = new Line2D(randomRect.getLowerRight(), randomRect.getUpperRight());
                    randomPointOnSide = currentSide.randomPoint();
                    randomPoint = new Point2D(Util.generateRandomDoubleBetween(randomPointOnSide.getX(), 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d));
                    Assert.AreEqual(randomPoint.distance(randomPointOnSide), randomRect.rayCast(new Ray2D(randomPoint, randomPoint.vec(randomPointOnSide))), 0.000005d);
                    j -= 1;
                } while (j > 0);
                i -= 1;
            } while (i > 0);

        }

        [TestMethod]
        public void testGetBounds()
        {
            Assert.AreNotEqual(1.0d, testRect.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreNotEqual(6.0d, testRect.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreNotEqual(6.0d, testRect.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreNotEqual(1.0d, testRect.getBounds().getLowerRight().getY(), 0.000005d);
            Assert.AreEqual(3.0d, testRect.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreEqual(8.0d, testRect.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreEqual(5.0d, testRect.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreEqual(4.0d, testRect.getBounds().getLowerRight().getY(), 0.000005d);
        }

        [TestMethod]
        public void testTransform()
        {
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.UNITY_MATRIX)).getUpperLeft().getX(), testRect.getUpperLeft().getX(), 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.UNITY_MATRIX)).getUpperLeft().getY(), testRect.getUpperLeft().getY(), 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.UNITY_MATRIX)).getLowerRight().getX(), testRect.getLowerRight().getX(), 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.UNITY_MATRIX)).getLowerRight().getY(), testRect.getLowerRight().getY(), 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.translate(3.0d, 5.0d))).getUpperLeft().getX(), 6.0d, 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.translate(3.0d, 5.0d))).getUpperLeft().getY(), 13.0d, 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.translate(3.0d, 5.0d))).getLowerRight().getX(), 8.0d, 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.translate(3.0d, 5.0d))).getLowerRight().getY(), 9.0d, 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.scale(2.0d, 4.0d))).getUpperLeft().getX(), 6.0d, 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.scale(2.0d, 4.0d))).getUpperLeft().getY(), 32.0d, 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.scale(2.0d, 4.0d))).getLowerRight().getX(), 10.0d, 0.000005d);
            Assert.AreEqual(((Rect2D)testRect.transform(TransformMatrix2D.scale(2.0d, 4.0d))).getLowerRight().getY(), 16.0d, 0.000005d);
        }

    }

}
