using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.approximate;
using aima.net.probability.example;
using aima.net.probability.proposition;
using aima.net.util;

namespace aima.net.test.unit.probability.bayes.approx
{
    [TestClass]
    public class RejectionSamplingTest
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
        public void testPriorSample_basic()
        {
            IBayesianNetwork bn = BayesNetExampleFactory.constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(ExampleRV.SPRINKLER_RV, true) };
            MockRandomizer r = new MockRandomizer(new double[] { 0.1 });
            RejectionSampling rs = new RejectionSampling(new PriorSample(r));

            double[] estimate = rs.rejectionSampling(new IRandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 100).getValues();

            assertArrayEquals(new double[] { 1.0, 0.0 }, estimate, DELTA_THRESHOLD);
        }

        [TestMethod]
        public void testRejectionSampling_AIMA3e_pg532()
        {
            // AIMA3e pg. 532

            IBayesianNetwork bn = BayesNetExampleFactory.constructCloudySprinklerRainWetGrassNetwork();
            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(ExampleRV.SPRINKLER_RV, true) };

            // 400 required as 4 variables and 100 samples planned
            double[] ma = new double[400];
            for (int i = 0; i < ma.Length; i += 4)
            {
                // Of the 100 that we generate, suppose
                // that 73 have Sprinkler = false and are rejected,
                if (i < (73 * 4))
                {
                    ma[i] = 0.5; // i.e Cloudy=true
                    ma[i + 1] = 0.2; // i.e. Sprinkler=false
                    ma[i + 2] = 0.5; // i.e. Rain=true
                    ma[i + 3] = 0.1; // i.e. WetGrass=true
                }
                else
                {
                    ma[i] = 0.5; // i.e Cloudy=true
                    ma[i + 1] = 0.09; // i.e. Sprinkler=true
                                      // while 27 have Sprinkler = true; of the 27,
                                      // 8 have Rain = true
                    if (i < ((73 + 8) * 4))
                    {
                        ma[i + 2] = 0.5; // i.e. Rain=true
                    }
                    else
                    {
                        // and 19 have Rain = false.
                        ma[i + 2] = 0.9; // i.e. Rain=false
                    }

                    ma[i + 3] = 0.1; // i.e. WetGrass=true
                }
            }
            MockRandomizer r = new MockRandomizer(ma);
            RejectionSampling rs = new RejectionSampling(new PriorSample(r));

            double[] estimate = rs.rejectionSampling(
                    new IRandomVariable[] { ExampleRV.RAIN_RV }, e, bn, 100)
                    .getValues();

            assertArrayEquals(new double[] { 0.2962962962962963, 0.7037037037037037 }, estimate, DELTA_THRESHOLD);
        }
    }
}