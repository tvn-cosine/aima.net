using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.api;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.bayes;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.approximate;
using aima.net.probability.example;
using aima.net.probability.proposition;
using aima.net.util;

namespace aima.net.test.unit.probability.bayes.approx
{
    [TestClass]
    public class LikelihoodWeightingTest
    {
        public static readonly double DELTA_THRESHOLD = ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;
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

        [TestMethod]
        public void testLikelihoodWeighting_basic()
        {
            IBayesianNetwork bn = BayesNetExampleFactory.constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(ExampleRV.SPRINKLER_RV, true) };
            MockRandomizer r = new MockRandomizer(new double[] { 0.5, 0.5, 0.5, 0.5 });

            LikelihoodWeighting lw = new LikelihoodWeighting(r);

            double[] estimate = lw.likelihoodWeighting(
                    new IRandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1000)
                    .getValues();

            assertArrayEquals(new double[] { 1.0, 0.0 }, estimate, DELTA_THRESHOLD);
        }

        [TestMethod]
        public void testLikelihoodWeighting_AIMA3e_pg533()
        {
            // AIMA3e pg. 533
            // <b>P</b>(Rain | Cloudy = true, WetGrass = true)
            IBayesianNetwork bn = BayesNetExampleFactory.constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] {
                new AssignmentProposition(ExampleRV.CLOUDY_RV, true),
                new AssignmentProposition(ExampleRV.WET_GRASS_RV, true) };
            // sample P(Sprinkler | Cloudy = true) = <0.1, 0.9>; suppose
            // Sprinkler=false
            // sample P(Rain | Cloudy = true) = <0.8, 0.2>; suppose Rain=true
            MockRandomizer r = new MockRandomizer(new double[] { 0.5, 0.5 });

            LikelihoodWeighting lw = new LikelihoodWeighting(r);
            double[] estimate = lw.likelihoodWeighting(
                    new IRandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 1)
                    .getValues();

            // Here the event [true,false,true,true] should have weight 0.45,
            // and this is tallied under Rain = true, which when normalized
            // should be <1.0, 0.0>;
            assertArrayEquals(new double[] { 1.0, 0.0 }, estimate, DELTA_THRESHOLD);
        }
    }

}
