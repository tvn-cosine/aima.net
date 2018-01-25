using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.search.csp;
using aima.net.search.csp.examples;

namespace aima.net.test.unit.search.csp
{
    [TestClass]
    public class MapCSPTest
    {
        private CSP<Variable, string> csp;

        [TestInitialize]
        public void setUp()
        {
            csp = new MapCSP();
        }

        [TestMethod]
        public void testBackTrackingSearch()
        {
            Assignment<Variable, string> results = new FlexibleBacktrackingSolver<Variable, string>().solve(csp);
            Assert.IsTrue(results != null);
            Assert.AreEqual(MapCSP.GREEN, results.getValue(MapCSP.WA));
            Assert.AreEqual(MapCSP.RED, results.getValue(MapCSP.NT));
            Assert.AreEqual(MapCSP.BLUE, results.getValue(MapCSP.SA));
            Assert.AreEqual(MapCSP.GREEN, results.getValue(MapCSP.Q));
            Assert.AreEqual(MapCSP.RED, results.getValue(MapCSP.NSW));
            Assert.AreEqual(MapCSP.GREEN, results.getValue(MapCSP.V));
            Assert.AreEqual(MapCSP.RED, results.getValue(MapCSP.T));
        }

        [TestMethod]
        public void testMCSearch()
        {
            new MinConflictsSolver<Variable, string>(100).solve(csp);
        }
    } 
}
