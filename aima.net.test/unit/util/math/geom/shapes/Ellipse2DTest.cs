using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.util;
using aima.net.util.math.geom.shapes;

namespace aima.net.test.unit.util.math.geom.shapes
{
    /**
     * Test case for the {@code aima.core.util.math.geom} package.
     * Tests valid implementation of the {@link IGeometric2D} interface by {@link Ellipse2D}.
     * 
     * @author Arno v. Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    [TestClass]
    public class Ellipse2DTest
    {
        private Ellipse2D testEllipse;
        private Point2D center;
        private Point2D zeroPoint;

        [TestInitialize]
        public void setUp()
        {
            center = new Point2D(12.0d, 14.0d);
            testEllipse = new Ellipse2D(center, 10.0d, 5.0d);
            zeroPoint = new Point2D(0.0d, 0.0d);
        }

        [TestMethod]
        public void testRandomPoint()
        {
            for (int i = 0; i < 1000;++i)
            {
                Assert.IsTrue(testEllipse.isInsideBorder(testEllipse.randomPoint()));
            }
        }

        [TestMethod]
        public void testIsInside()
        {
            Assert.IsFalse(testEllipse.isInside(zeroPoint));
            Assert.IsFalse(testEllipse.isInside(new Point2D(12.0d, 9.0d)));
            Assert.IsTrue(testEllipse.isInside(new Point2D(10.0d, 12.0d)));
        }

        [TestMethod]
        public void testIsInsideBorder()
        {
            Assert.IsFalse(testEllipse.isInsideBorder(zeroPoint));
            Assert.IsTrue(testEllipse.isInsideBorder(new Point2D(12.0d, 9.0d)));
            Assert.IsTrue(testEllipse.isInsideBorder(new Point2D(10.0d, 12.0d)));
        }

        [TestMethod]
        public void testRayCast()
        {
            // static tests
            Assert.AreEqual(double.PositiveInfinity, testEllipse.rayCast(new Ray2D(0.0d, 0.0d, 0.0d, 2.0d)), 0.000005d);
            Assert.AreEqual(2.0d, testEllipse.rayCast(new Ray2D(0.0d, 14.0d, 12.0d, 14.0d)), 0.000005d);

            // serial tests
            Ellipse2D randomEllipse;
            Point2D randomPointOnEllipse;
            Point2D randomPoint;
            double currentXRadius;
            double currentYRadius;
            double xvalue;
            double yvalue;
            double randomAngle;
            int sector;
            int counter = 1000;

            do
            {
                randomEllipse = new Ellipse2D(new Point2D(Util.generateRandomDoubleBetween(-500.0d, 500.0d), Util.generateRandomDoubleBetween(-500.0d, 500.0d)), Util.generateRandomDoubleBetween(0.0d, 200.0d), Util.generateRandomDoubleBetween(0.0d, 200.0d));
                currentXRadius = randomEllipse.getHorizontalLength();
                currentYRadius = randomEllipse.getVerticalLength();
                xvalue = Util.generateRandomDoubleBetween(0.0d, currentXRadius);
                yvalue = (currentYRadius * System.Math.Sqrt(currentXRadius * currentXRadius - xvalue * xvalue)) / currentXRadius;
                sector = Util.randomNumberBetween(1, 4);
                switch (sector)
                {
                    case 2:
                        {
                            yvalue = -yvalue;
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(randomEllipse.getCenter().getX() + xvalue, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, randomEllipse.getCenter().getY() + yvalue));
                            break;
                        }
                    case 3:
                        {
                            xvalue = -xvalue;
                            yvalue = -yvalue;
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, randomEllipse.getCenter().getX() + xvalue), Util.generateRandomDoubleBetween(-1000.0d, randomEllipse.getCenter().getY() + yvalue));
                            break;
                        }
                    case 4:
                        {
                            xvalue = -xvalue;
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, randomEllipse.getCenter().getX() + xvalue), Util.generateRandomDoubleBetween(randomEllipse.getCenter().getY() + yvalue, 1000.0d));
                            break;
                        }
                    default:
                        {
                            randomPoint = new Point2D(Util.generateRandomDoubleBetween(randomEllipse.getCenter().getX() + xvalue, 1000.0d), Util.generateRandomDoubleBetween(randomEllipse.getCenter().getY() + yvalue, 1000.0d));
                            break;
                        }
                }
                randomPointOnEllipse = new Point2D(randomEllipse.getCenter().getX() + xvalue, randomEllipse.getCenter().getY() + yvalue);

                randomAngle = Util.generateRandomDoubleBetween(-System.Math.PI / 2, System.Math.PI / 2);
                randomEllipse = (Ellipse2D)(randomEllipse.transform(TransformMatrix2D.rotate(randomAngle)));
                randomPoint = TransformMatrix2D.rotate(randomAngle).multiply(randomPoint);
                randomPointOnEllipse = TransformMatrix2D.rotate(randomAngle).multiply(randomPointOnEllipse);
                // System.Console.Writef("RayCast No. %d: Ellipse at (%.2f,%.2f), radii: (%.2f,%.2f). Rotation angle: %.2f, original angle: %.2f, point on ellipse: (%.2f,%.2f), outside point: (%.2f,%.2f), distance: %.2f.\n", 1000-counter, randomEllipse.getCenter().getX(), randomEllipse.getCenter().getY(), randomEllipse.getHorizontalLength(), randomEllipse.getVerticalLength(), randomEllipse.getAngle(), randomAngle, randomPointOnEllipse.getX(), randomPointOnEllipse.getY(), randomPoint.getX(), randomPoint.getY(), randomPoint.distance(randomPointOnEllipse));

                Assert.AreEqual(randomPoint.distance(randomPointOnEllipse), randomEllipse.rayCast(new Ray2D(randomPoint, randomPoint.vec(randomPointOnEllipse))), 0.000005d);
                counter -= 1;
            } while (counter > 0);

        }

        [TestMethod]
        public void testGetBounds()
        {
            Assert.AreNotEqual(1.0d, testEllipse.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreNotEqual(6.0d, testEllipse.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreNotEqual(6.0d, testEllipse.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreNotEqual(1.0d, testEllipse.getBounds().getLowerRight().getY(), 0.000005d);
            Assert.AreEqual(2.0d, testEllipse.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreEqual(19.0d, testEllipse.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreEqual(22.0d, testEllipse.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreEqual(9.0d, testEllipse.getBounds().getLowerRight().getY(), 0.000005d);
        }


        [TestMethod]
        public void testTransform()
        {
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.UNITY_MATRIX)).getCenter().getX(), testEllipse.getCenter().getX(), 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.UNITY_MATRIX)).getCenter().getY(), testEllipse.getCenter().getY(), 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.UNITY_MATRIX)).getHorizontalLength(), testEllipse.getHorizontalLength(), 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.UNITY_MATRIX)).getVerticalLength(), testEllipse.getVerticalLength(), 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.UNITY_MATRIX)).getAngle(), testEllipse.getAngle(), 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.translate(4.0d, 5.0d))).getCenter().getX(), 16.0d, 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.translate(4.0d, 5.0d))).getCenter().getY(), 19.0d, 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.scale(0.5d, 0D))).getHorizontalLength(), 5.0d, 0.000005d);
            Assert.AreEqual(((Ellipse2D)testEllipse.transform(TransformMatrix2D.scale(0, 2.0d))).getVerticalLength(), 10.0d, 0.000005d);
        }

    }

}
