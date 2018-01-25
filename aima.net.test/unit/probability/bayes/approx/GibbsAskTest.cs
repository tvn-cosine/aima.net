using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.bayes;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.approximate;
using aima.net.probability.domain;
using aima.net.probability.example;
using aima.net.probability.proposition;
using aima.net.probability.util;
using aima.net.util;

namespace aima.net.test.unit.probability.bayes.approx
{
    [TestClass]
    public class GibbsAskTest
    {
        public const double DELTA_THRESHOLD = 0.1;

        protected static void assertArrayEquals(double[] arr1, double[] arr2, double delta)
        {
            if (arr1.Length != arr2.Length)
            {
                Assert.Fail("Two arrays not same length");
            }

            for (int i = 0; i < arr1.Length; ++i)
            {
                Assert.AreEqual(arr1[i], arr2[i], delta);
            }
        }

        /** Mock randomizer - A very skewed distribution results from the choice of 
         * MockRandomizer that always favours one type of sample over others
         */
        [TestMethod]
        public void testGibbsAsk_mock()
        {
            IBayesianNetwork bn = BayesNetExampleFactory.constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(ExampleRV.SPRINKLER_RV, true) };
            MockRandomizer r = new MockRandomizer(new double[] { 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.6, 0.5, 0.5, 0.6, 0.5, 0.5 });

            GibbsAsk ga = new GibbsAsk(r);

            double[] estimate = ga.gibbsAsk(new IRandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000).getValues();

            assertArrayEquals(new double[] { 0, 1 }, estimate, DELTA_THRESHOLD);
        }

        /** Same test as above but with JavaRandomizer
         * <p>
         * Expected result : <br/>
         * P(Rain = true | Sprinkler = true) = 0.3 <br/>
         * P(Rain = false | Sprinkler = true) = 0.7 <br/>
         */
        [TestMethod]
        public void testGibbsAsk_basic()
        {
            IBayesianNetwork bn = BayesNetExampleFactory.constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(ExampleRV.SPRINKLER_RV, true) };

            GibbsAsk ga = new GibbsAsk();

            double[] estimate = ga.gibbsAsk(new IRandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000).getValues();

            assertArrayEquals(new double[] { 0.3, 0.7 }, estimate, DELTA_THRESHOLD);
        }

        [TestMethod]
        public void testGibbsAsk_compare()
        {
            // create two nodes: parent and child with an arc from parent to child 
            IRandomVariable rvParent = new RandVar("Parent", new BooleanDomain());
            IRandomVariable rvChild = new RandVar("Child", new BooleanDomain());
            FullCPTNode nodeParent = new FullCPTNode(rvParent, new double[] { 0.7, 0.3 });
            new FullCPTNode(rvChild, new double[] { 0.8, 0.2, 0.2, 0.8 }, nodeParent);

            // create net
            BayesNet net = new BayesNet(nodeParent);

            // query parent probability
            IRandomVariable[] rvX = new IRandomVariable[] { rvParent };

            // ...given child evidence (true)
            AssignmentProposition[] propE = new AssignmentProposition[] { new AssignmentProposition(rvChild, true) };

            // sample with LikelihoodWeighting
            ICategoricalDistribution samplesLW = new LikelihoodWeighting().Ask(rvX, propE, net, 1000);
            Assert.AreEqual(0.9, samplesLW.getValue(true), DELTA_THRESHOLD);

            // sample with RejectionSampling
            ICategoricalDistribution samplesRS = new RejectionSampling().Ask(rvX, propE, net, 1000);
            Assert.AreEqual(0.9, samplesRS.getValue(true), DELTA_THRESHOLD);

            // sample with GibbsAsk
            ICategoricalDistribution samplesGibbs = new GibbsAsk().Ask(rvX, propE, net, 1000);
            Assert.AreEqual(0.9, samplesGibbs.getValue(true), DELTA_THRESHOLD);
        }
    }

}
