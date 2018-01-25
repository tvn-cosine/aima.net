using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability.example;
using aima.net.probability.temporal.generic;

namespace aima.net.test.unit.probability.temporal.generic
{
    [TestClass]
    public class ForwardBackwardTest : CommonForwardBackwardTest
    {

        //
        private ForwardBackward uw = null;

        [TestInitialize]
        public void setUp()
        {
            uw = new ForwardBackward(
                    GenericTemporalModelFactory.getUmbrellaWorldTransitionModel(),
                    GenericTemporalModelFactory.getUmbrellaWorld_Xt_to_Xtm1_Map(),
                    GenericTemporalModelFactory.getUmbrellaWorldSensorModel());
        }

        [TestMethod]
        public void testForwardStep_UmbrellaWorld()
        {
            base.testForwardStep_UmbrellaWorld(uw);
        }

        [TestMethod]
        public void testBackwardStep_UmbrellaWorld()
        {
            base.testBackwardStep_UmbrellaWorld(uw);
        }

        [TestMethod]
        public void testForwardBackward_UmbrellaWorld()
        {
            base.testForwardBackward_UmbrellaWorld(uw);
        }
    }

}
