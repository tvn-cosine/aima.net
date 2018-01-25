using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability.example;
using aima.net.probability.hmm.exact;
using aima.net.test.unit.probability.temporal;

namespace aima.net.test.unit.probability.hmm.exact
{
    [TestClass]
    public class HMMForwardBackwardConstantSpaceTest : CommonForwardBackwardTest
    {

        //
        private HMMForwardBackwardConstantSpace uw = null;

        [TestInitialize]
        public void setUp()
        {
            uw = new HMMForwardBackwardConstantSpace(
                    HMMExampleFactory.getUmbrellaWorldModel());
        }

        [TestMethod]
        public void testForwardBackward_UmbrellaWorld()
        {
            base.testForwardBackward_UmbrellaWorld(uw);
        }
    }

}
