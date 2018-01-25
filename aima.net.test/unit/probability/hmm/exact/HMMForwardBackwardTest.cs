using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability.example;
using aima.net.probability.hmm.exact;
using aima.net.test.unit.probability.temporal;

namespace aima.net.test.unit.probability.hmm.exact
{
    [TestClass]
    public class HMMForwardBackwardTest : CommonForwardBackwardTest
    {

        //
        private HMMForwardBackward uw = null;

        [TestInitialize]
        public void setUp()
        {
            uw = new HMMForwardBackward(HMMExampleFactory.getUmbrellaWorldModel());
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
