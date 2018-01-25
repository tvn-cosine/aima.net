using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections.api;
using aima.net.environment.cellworld;
using aima.net.probability.example;
using aima.net.probability.mdp;
using aima.net.probability.mdp.api;
using aima.net.probability.mdp.search;

namespace aima.net.test.unit.probability.mdp
{
    [TestClass]
    public class ValueIterationTest
    {
        public static readonly double DELTA_THRESHOLD = 1e-3;

        private CellWorld<double> cw = null;
        private IMarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = null;
        private ValueIteration<Cell<double>, CellWorldAction> vi = null;

        [TestInitialize]
        public void setUp()
        {
            cw = CellWorldFactory.CreateCellWorldForFig17_1();
            mdp = MDPFactory.createMDPForFigure17_3(cw);
            vi = new ValueIteration<Cell<double>, CellWorldAction>(1.0);
        }

        [TestMethod]
        public void testValueIterationForFig17_3()
        {
            IMap<Cell<double>, double> U = vi.valueIteration(mdp, 0.0001);

            Assert.AreEqual(0.705, U.Get(cw.GetCellAt(1, 1)), DELTA_THRESHOLD);
            Assert.AreEqual(0.762, U.Get(cw.GetCellAt(1, 2)), DELTA_THRESHOLD);
            Assert.AreEqual(0.812, U.Get(cw.GetCellAt(1, 3)), DELTA_THRESHOLD);
            Assert.AreEqual(0.655, U.Get(cw.GetCellAt(2, 1)), DELTA_THRESHOLD);
            Assert.AreEqual(0.868, U.Get(cw.GetCellAt(2, 3)), DELTA_THRESHOLD);
            Assert.AreEqual(0.611, U.Get(cw.GetCellAt(3, 1)), DELTA_THRESHOLD);
            Assert.AreEqual(0.660, U.Get(cw.GetCellAt(3, 2)), DELTA_THRESHOLD);
            Assert.AreEqual(0.918, U.Get(cw.GetCellAt(3, 3)), DELTA_THRESHOLD);
            Assert.AreEqual(0.388, U.Get(cw.GetCellAt(4, 1)), DELTA_THRESHOLD);
            Assert.AreEqual(-1.0, U.Get(cw.GetCellAt(4, 2)), DELTA_THRESHOLD);
            Assert.AreEqual(1.0, U.Get(cw.GetCellAt(4, 3)), DELTA_THRESHOLD);
        }
    }

}
