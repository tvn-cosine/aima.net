using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.bayes;
using aima.net.probability.domain;
using aima.net.probability.util;

namespace aima.net.test.unit.probability.bayes.impl
{
    [TestClass]
    public class CPTTest
    {
        public static readonly double DELTA_THRESHOLD = ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;

        private static void assertArrayEquals(double[] arr1, double[] arr2, double delta)
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
        public void test_getConditioningCase()
        {
            RandVar aRV = new RandVar("A", new BooleanDomain());
            RandVar bRV = new RandVar("B", new BooleanDomain());
            RandVar cRV = new RandVar("C", new BooleanDomain());

            CPT cpt = new CPT(cRV, new double[] {
				// A = true, B = true, C = true
				0.1,
				// A = true, B = true, C = false
				0.9,
				// A = true, B = false, C = true
				0.2,
				// A = true, B = false, C = false
				0.8,
				// A = false, B = true, C = true
				0.3,
				// A = false, B = true, C = false
				0.7,
				// A = false, B = false, C = true
				0.4,
				// A = false, B = false, C = false
				0.6 }, aRV, bRV);

            assertArrayEquals(new double[] { 0.1, 0.9 }, cpt
                     .GetConditioningCase(true, true).getValues(), DELTA_THRESHOLD);

            assertArrayEquals(new double[] { 0.2, 0.8 }, cpt
                    .GetConditioningCase(true, false).getValues(), DELTA_THRESHOLD);

            assertArrayEquals(new double[] { 0.3, 0.7 }, cpt
                    .GetConditioningCase(false, true).getValues(), DELTA_THRESHOLD);

            assertArrayEquals(new double[] { 0.4, 0.6 }, cpt
                   .GetConditioningCase(false, false).getValues(), DELTA_THRESHOLD);

        }
    }

}
