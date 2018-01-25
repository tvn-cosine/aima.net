using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.util.math.geom.shapes;

namespace aima.net.test.unit.util.math.geom.shapes
{
    /**
     * Test case for the {@code aima.core.util.math.geom} package.
     * Tests valid implementation of {@link Vector2D}.
     * 
     * @author Arno v. Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */

    [TestClass]
    public class Vector2DTest
    {
        private Vector2D testVector;

        [TestInitialize]
        public void setUp()
        {
            testVector = new Vector2D(3.0d, 4.0d);
        }

        [TestMethod]
        public void testAdd()
        {
            Assert.AreEqual(testVector.add(new Vector2D(8.0d, 12.0d)).getX(), 11.0d, 0.000005d);
            Assert.AreEqual(testVector.add(new Vector2D(8.0d, 12.0d)).getY(), 16.0d, 0.000005d);

        }

        [TestMethod]
        public void testSub()
        {
            Assert.AreEqual(testVector.sub(new Vector2D(8.0d, 12.0d)).getX(), -5.0d, 0.000005d);
            Assert.AreEqual(testVector.sub(new Vector2D(8.0d, 12.0d)).getY(), -8.0d, 0.000005d);
        }

        [TestMethod]
        public void testInvert()
        {
            Assert.AreEqual(testVector.invert().getX(), -3.0d, 0.000005d);
            Assert.AreEqual(testVector.invert().getY(), -4.0d, 0.000005d);
        }

        [TestMethod]
        public void testIsParallel()
        {
            Assert.IsFalse(testVector.isAbsoluteParallel(new Vector2D(4.0d, 7.0d)));
            Assert.IsTrue(testVector.isAbsoluteParallel(new Vector2D(9.0d, 12.0d)));
            Assert.IsTrue(testVector.isAbsoluteParallel(Vector2D.ZERO_VECTOR));

        }

        [TestMethod]
        public void testIsAbsoluteParallel()
        {
            Assert.IsFalse(testVector.isParallel(new Vector2D(6.5d, 5.8d)));
            Assert.IsFalse(testVector.isParallel(Vector2D.ZERO_VECTOR));
            Assert.IsTrue(testVector.isParallel(new Vector2D(9, 12)));
            Assert.IsTrue(testVector.isParallel(new Vector2D(-9, -12)));
        }

        [TestMethod]
        public void testAngleTo()
        {
            Assert.AreEqual(testVector.angleTo(new Vector2D(-4.0d, 3.0d)), 1.0d / 2.0d * System.Math.PI, 0.000005d);
            Assert.AreEqual(testVector.angleTo(new Vector2D(-3.0d, -4.0d)), System.Math.PI, 0.000005d);
            Assert.AreEqual(testVector.angleTo(new Vector2D(4.0d, -3.0d)), 3.0d / 2.0d * System.Math.PI, 0.000005d);
            Assert.AreEqual(testVector.angleTo(new Vector2D(6.0d, 8.0d)), 0.0d, 0.000005d);
        }

        [TestMethod]
        public void testLength()
        {
            Assert.AreEqual(testVector.length(), 5.0d, 0.000005d);
        }

        [TestMethod]
        public void testEqualsVector2D()
        {
            Assert.IsFalse(testVector.Equals(new Vector2D(6.0d, 5.0d)));
            Assert.IsTrue(testVector.Equals(new Vector2D(3.0d, 4.0d)));
        }

        [TestMethod]
        public void testEqualsObject()
        {
            Assert.IsFalse(testVector.Equals(new Point2D(3.0d, 4.0d)));
            Assert.IsTrue(testVector.Equals(new Vector2D(3.0d, 4.0d)));
        }

    }

}
