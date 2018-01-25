using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.util.math.geom.shapes;

namespace aima.net.test.unit.util.math.geom.shapes
{
    /**
     * Test case for the {@code aima.core.util.math.geom} package.
     * Tests valid implementation of the {@link IGeometric2D} interface by {@link Polyline2D}.
     * 
     * @author Arno v. Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */

    [TestClass]
    public class Polyline2DTest
    {

        private Point2D[] testVertices = { new Point2D(2.0d, 2.0d), new Point2D(5.0d, 7.0d), new Point2D(6.0d, 4.0d), new Point2D(6.0d, -3.0d) };
        private Polyline2D testPolylineOpen;
        private Polyline2D testPolylineClosed;
        private Point2D zeroPoint;

        [TestInitialize]
        public void setUp()
        {
            testPolylineOpen = new Polyline2D(testVertices, false);
            testPolylineClosed = new Polyline2D(testVertices, true);
            zeroPoint = new Point2D(0.0d, 0.0d);
        }

        [TestMethod]
        public void testRandomPoint()
        {
            Point2D randomPoint = testPolylineOpen.randomPoint();
            for (int i = 0; i < 1000;++i)
            {
                randomPoint = testPolylineOpen.randomPoint();
                Assert.IsTrue(testPolylineOpen.isInsideBorder(randomPoint));
            }
            for (int i = 0; i < 1000;++i)
            {
                randomPoint = testPolylineClosed.randomPoint();
                Assert.IsTrue(testPolylineClosed.isInsideBorder(testPolylineClosed.randomPoint()));
            }


        }

        [TestMethod]
        public void testIsInside()
        {
            Assert.IsFalse(testPolylineOpen.isInside(new Point2D(3.0d, 3.0d)));
            Assert.IsFalse(testPolylineOpen.isInside(new Point2D(6.0d, 2.0d)));
            Assert.IsFalse(testPolylineClosed.isInside(zeroPoint));
            Assert.IsFalse(testPolylineClosed.isInside(new Point2D(6.0d, 2.0d)));
            Assert.IsTrue(testPolylineClosed.isInside(new Point2D(3.0d, 3.0d)));
        }

        [TestMethod]
        public void testIsInsideBorder()
        {
            Assert.IsFalse(testPolylineOpen.isInsideBorder(new Point2D(3.0d, 3.0d)));
            Assert.IsTrue(testPolylineOpen.isInsideBorder(new Point2D(6.0d, 2.0d)));
            Assert.IsFalse(testPolylineClosed.isInsideBorder(zeroPoint));
            Assert.IsTrue(testPolylineClosed.isInsideBorder(new Point2D(6.0d, 2.0d)));
            Assert.IsTrue(testPolylineClosed.isInsideBorder(new Point2D(3.0d, 3.0d)));
        }

        [TestMethod]
        public void testRayCast()
        {

            // Static RayCast tests
            Assert.AreEqual(double.PositiveInfinity, testPolylineOpen.rayCast(new Ray2D(1.0d, 1.0d, -7.0d, -8.0d)), 0.000005d);
            Assert.AreEqual(System.Math.Sqrt(2), testPolylineOpen.rayCast(new Ray2D(1.0d, 1.0d, 4.0d, 4.0d)), 0.000005d);
            Assert.AreEqual(double.PositiveInfinity, testPolylineClosed.rayCast(new Ray2D(1.0d, 1.0d, -7.0d, -8.0d)), 0.000005d);
            Assert.AreEqual(System.Math.Sqrt(2), testPolylineClosed.rayCast(new Ray2D(1.0d, 1.0d, 4.0d, 4.0d)), 0.000005d);

            // Serial RayCast tests	
            /*Point2D randomPoint;
            Point2D randomPointOnEdge;
            Line2D currentEdge;
            int counter = 500;
            do {
                for (int i = 1; i < testVertices.Length;++i){
                    currentEdge = new Line2D(testVertices[i], testVertices[i-1]);
                    randomPointOnEdge = currentEdge.randomPoint();
                    randomPoint = new Point2D(Util.generateRandomDoubleBetween(-1000.0d, 1000.0d), Util.generateRandomDoubleBetween(-1000.0d, 1000.0d));
                    AreEqual("Serial rayCast test for Polyline2D (open).", randomPoint.distance(randomPointOnEdge), testPolylineOpen.rayCast(new Ray2D(randomPoint,randomPoint.vec(randomPointOnEdge))), 0.000005d);
                }
                counter -= 1;	
            } while (counter > 0);*/
        }

        [TestMethod]
        public void testGetBounds()
        {
            Assert.AreNotEqual(1.0d, testPolylineOpen.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreNotEqual(8.0d, testPolylineOpen.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreNotEqual(8.0d, testPolylineOpen.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreNotEqual(1.0d, testPolylineOpen.getBounds().getLowerRight().getY(), 0.000005d);
            Assert.AreEqual(2.0d, testPolylineOpen.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreEqual(7.0d, testPolylineOpen.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreEqual(6.0d, testPolylineOpen.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreEqual(-3.0d, testPolylineOpen.getBounds().getLowerRight().getY(), 0.000005d);
            Assert.AreNotEqual(1.0d, testPolylineClosed.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreNotEqual(8.0d, testPolylineClosed.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreNotEqual(8.0d, testPolylineClosed.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreNotEqual(1.0d, testPolylineClosed.getBounds().getLowerRight().getY(), 0.000005d);
            Assert.AreEqual(2.0d, testPolylineClosed.getBounds().getUpperLeft().getX(), 0.000005d);
            Assert.AreEqual(7.0d, testPolylineClosed.getBounds().getUpperLeft().getY(), 0.000005d);
            Assert.AreEqual(6.0d, testPolylineClosed.getBounds().getLowerRight().getX(), 0.000005d);
            Assert.AreEqual(-3.0d, testPolylineClosed.getBounds().getLowerRight().getY(), 0.000005d);
        }

        [TestMethod]
        public void testTransform()
        {
            for (int i = 0; i < testPolylineOpen.getVertexes().Length;++i)
            {
                Assert.AreEqual(  testPolylineOpen.transform(TransformMatrix2D.UNITY_MATRIX).getVertexes()[i].getX(), testPolylineOpen.getVertexes()[i].getX(), 0.000005d);
                Assert.AreEqual(  testPolylineOpen.transform(TransformMatrix2D.UNITY_MATRIX).getVertexes()[i].getY(), testPolylineOpen.getVertexes()[i].getY(), 0.000005d);
            }
            for (int i = 0; i < testPolylineClosed.getVertexes().Length;++i)
            {
                Assert.AreEqual(  testPolylineClosed.transform(TransformMatrix2D.UNITY_MATRIX).getVertexes()[i].getX(), testPolylineClosed.getVertexes()[i].getX(), 0.000005d);
                Assert.AreEqual(  testPolylineClosed.transform(TransformMatrix2D.UNITY_MATRIX).getVertexes()[i].getY(), testPolylineClosed.getVertexes()[i].getY(), 0.000005d);
            }
        }

    }

}
