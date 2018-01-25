using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.search.framework;

namespace aima.net.test.unit.search.framework
{
    [TestClass] public class MetricsTest
    {

        private Metrics metrics;

        [TestInitialize]
        public void before()
        {
            metrics = new Metrics();
        }

        [TestMethod]
        public void testGetInt()
        {
            int x = 893597823;
            metrics.set("abcd", x);
            Assert.AreEqual(x, metrics.getInt("abcd"));
            Assert.AreNotEqual(1234, metrics.getInt("abcd"));
        }

        [TestMethod]
        public void testGetDouble()
        {
            double x = 1231397235234.48 ;
            metrics.set("abcd", x);
            Assert.AreEqual(x, metrics.getDouble("abcd"), 0);
            Assert.AreNotEqual(1234.56789, metrics.getDouble("abcd"), 0);
        }

        [TestMethod]
        public void testGetLong()
        {
            long x = 893597823;
            metrics.set("abcd", x);
            Assert.AreEqual(x, metrics.getLong("abcd"));
            Assert.AreNotEqual(841356458, metrics.getLong("abcd"));
        }

        [TestMethod]
        public void testGet()
        {
            int x = 123;
            metrics.set("abcd", x);
            Assert.AreEqual("123", metrics.get("abcd"));
            Assert.AreNotEqual("1234", metrics.get("abcd"));
        }

    }

}
