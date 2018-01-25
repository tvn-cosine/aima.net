using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.api;
using aima.net.collections.api;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.approximate;
using aima.net.probability.example;
using aima.net.util;

namespace aima.net.test.unit.probability.bayes.approx
{
    [TestClass]
    public class PriorSampleTest
    {
        [TestMethod]
        public void testPriorSample_basic()
        {
            // AIMA3e pg. 530
            IBayesianNetwork bn = BayesNetExampleFactory
                    .constructCloudySprinklerRainWetGrassNetwork();
            IRandom r = new MockRandomizer(
                    new double[] { 0.5, 0.5, 0.5, 0.5 });

            PriorSample ps = new PriorSample(r);
            IMap<IRandomVariable, object> even = ps.priorSample(bn);

            Assert.AreEqual(4, even.GetKeys().Size());
            Assert.AreEqual(true, even.Get(ExampleRV.CLOUDY_RV));
            Assert.AreEqual(false, even.Get(ExampleRV.SPRINKLER_RV));
            Assert.AreEqual(true, even.Get(ExampleRV.RAIN_RV));
            Assert.AreEqual(true, even.Get(ExampleRV.WET_GRASS_RV));
        }
    }

}
