using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.util;
using aima.net.util.math.geom.shapes;

namespace aima.net.test.unit.util.math.geom.shapes
{
    /**
     * Test case for the {@code aima.core.util.math.geom} package.
     * Tests valid implementation of the {@link IGeometric2D} interface by {@link Circle2D}.
     * 
     * @author Arno v. Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    [TestClass]
    public class Circle2DTest
    {

        private Circle2D testCircle;
        private Point2D center;
        private Point2D zeroPoint;

        [TestInitialize]
        public void setUp()
        {
            center = new Point2D(12.0d, 14.0d);
            testCircle = new Circle2D(center, 10.0d);
            zeroPoint = new Point2D(0.0d, 0.0d);
        }

        [TestMethod]
        public void testRandomPoint()
        {
            for (int i = 0; i < 1000;++i)
            {
                Assert.IsTrue(testCircle.isInsideBorder(testCircle.randomPoint()));
            }
        }

        [TestMethod]
        public void testIsInside()
        {
            Assert.IsFalse(testCircle.isInside(zeroPoint));
            Assert.IsFalse(testCircle.isInside(new Point2D(12.0d, 4.0d)));
            Assert.IsTrue(testCircle.isInside(new Point2D(10.0d, 8.0d)));
        }

        [TestMethod]
        public void testIsInsideBorder()
        {
            Assert.IsFalse(testCircle.isInsideBorder(zeroPoint));
            Assert.IsTrue(testCircle.isInsideBorder(new Point2D(12.0d, 4.0d)));
            Assert.IsTrue(testCircle.isInsideBorder(new Point2D(10.0d, 8.0d)));
        }

        [TestMethod]
        public void testRayCast()
        {

            // static tests
            Assert.AreEqual(double.PositiveInfinity, testCircle.rayCast(new Ray2D(1.0d, 1.0d, 0.0d, 2.0d)), 0.000005d);
            Assert.AreEqual(System.Math.Sqrt(2), testCircle.rayCast(new Ray2D(11.0d, 3.0d, 12.0d, 4.0d)), 0.000005d);

            // serial tests
            Circle2D randomCircle;
            Point2D randomPointOnCircle;
            Point2D randomPoint;
            double currentRadius;
            double xvalue;
            double yvalue;
            int sector;
            int counter = 1000;

            do
            {
                randomCircle = new Circle2D(new Point2D(Util.generateRandomDoubleBetween(-500.0d, 500.0d), Util.generateRandomDoubleBetween(-500.0d, 500.0d)), Util.generateRandomDoubleBetween(0.0d, 200.0d));
                currentRadius = randomCircle.getRadius();
                xvalue = Util.generateRandomDoubleBetween(0.0d, currentRadius);
                yvalue = System.Math.Sqrt(currentRadius * currentRadius - xvalue * xvalue);
                sector = Util.randomNumberBetween(1, 4);
                switch (sector)
                {
                    case 2:
                        {
                            yvalue = -yvalue;
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(randomCircle.getCenter().getX() + xvalue, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, randomCircle.getCenter().getY() + yvalue));
                            break;
                        }
                    case 3:
                        {
                            xvalue = -xvalue;
                            yvalue = -yvalue;
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, randomCircle.getCenter().getX() + xvalue), Util.generateRandomDoubleBetween(-1000.0d, randomCircle.getCenter().getY() + yvalue));
                            break;
                        }
                    case 4:
                        {
                            xvalue = -xvalue;
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, randomCircle.getCenter().getX() + xvalue), Util.generateRandomDoubleBetween(randomCircle.getCenter().getY() + yvalue, 1000.0d));
                            break;
                        }
                    default:
                        {
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(randomCircle.getCenter().getX() + xvalue, 1000.0d), Util.generateRandomDoubleBetween(randomCircle.getCenter().getY() + yvalue, 1000.0d));
                            break;
                        }
                }
                randomPointOnCircle = new Point2D(randomCircle.getCenter().getX() + xvalue, randomCircle.getCenter().getY() + yvalue);
                // System.Console.Writef("Circle at (%.2f,%.2f), Radius %.2f. Point on Circle: (%.2f,%.2f). Outside Point: (%.2f,%.2f). Distance: %.2f.\n", randomCircle.getCenter().getX(), randomCircle.getCenter().getY(), randomCircle.getRadius(), randomPointOnCircle.getX(), randomPointOnCircle.getY(), randomPoint.getX(), randomPoint.getY(), randomPoint.distance(randomPointOnCircle));
                Assert.AreEqual(  randomPoint.distance(randomPointOnCircle), randomCircle.rayCast(new Ray2D(randomPoint, randomPoint.vec(randomPointOnCircle))), 0.000005d);
                counter -= 1;
            } while (counter > 0);
        }

        [TestMethod]
        public void testGetBounds()
        {
            Assert.AreNotEqual(1.0d, testCircle.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreNotEqual(6.0d, testCircle.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreNotEqual(6.0d, testCircle.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreNotEqual(1.0d, testCircle.getBounds().getLowerRight().getY(), 0.000005d);
            Assert.AreEqual(2.0d, testCircle.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreEqual(24.0d, testCircle.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreEqual(22.0d, testCircle.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreEqual(4.0d, testCircle.getBounds().getLowerRight().getY(), 0.000005d);
        }

        [TestMethod]
        public void testTransform()
        {
            Assert.AreEqual(((Circle2D)testCircle.transform(TransformMatrix2D.UNITY_MATRIX)).getCenter().getX(), testCircle.getCenter().getX(), 0.000005d);
            Assert.AreEqual(((Circle2D)testCircle.transform(TransformMatrix2D.UNITY_MATRIX)).getCenter().getY(), testCircle.getCenter().getY(), 0.000005d);
            Assert.AreEqual(((Circle2D)testCircle.transform(TransformMatrix2D.UNITY_MATRIX)).getRadius(), testCircle.getRadius(), 0.000005d);
            Assert.AreEqual(((Circle2D)testCircle.transform(TransformMatrix2D.translate(4.0d, 5.0d))).getCenter().getX(), 16.0d, 0.000005d);
            Assert.AreEqual(((Circle2D)testCircle.transform(TransformMatrix2D.translate(4.0d, 5.0d))).getCenter().getY(), 19.0d, 0.000005d);
            Assert.AreEqual(((Ellipse2D)testCircle.transform(TransformMatrix2D.scale(2.0d, 0D))).getHorizontalLength(), 20.0d, 0.000005d);
            Assert.AreEqual(((Ellipse2D)testCircle.transform(TransformMatrix2D.scale(0, 0.5d))).getVerticalLength(), 5.0d, 0.000005d);
        }

    }

}
