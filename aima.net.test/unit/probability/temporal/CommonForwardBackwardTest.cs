using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.example;
using aima.net.probability.proposition;
using aima.net.probability.temporal;
using aima.net.probability.temporal.api;
using aima.net.probability.util;

namespace aima.net.test.unit.probability.temporal
{
    public abstract class CommonForwardBackwardTest
    {
        public static readonly double DELTA_THRESHOLD = 1e-3;

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
        
        protected void testForwardStep_UmbrellaWorld(IForwardStepInference uw)
        {
            // AIMA3e pg. 572
            // Day 0, no observations only the security guards prior beliefs
            // P(R<sub>0</sub>) = <0.5, 0.5>
            ICategoricalDistribution prior = new ProbabilityTable(new double[] { 0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1, the umbrella appears, so U<sub>1</sub> = true.
            // &asymp; <0.818, 0.182>
            ICollection<AssignmentProposition> e1 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            ICategoricalDistribution f1 = uw.forward(prior, e1);
            assertArrayEquals(new double[] { 0.818, 0.182 }, f1.getValues(), DELTA_THRESHOLD);

            // Day 2, the umbrella appears, so U<sub>2</sub> = true.
            // &asymp; <0.883, 0.117>
            ICollection<AssignmentProposition> e2 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            ICategoricalDistribution f2 = uw.forward(f1, e2);
            assertArrayEquals(new double[] { 0.883, 0.117 }, f2.getValues(),
                    DELTA_THRESHOLD);
        }

        protected void testBackwardStep_UmbrellaWorld(IBackwardStepInference uw)
        {
            // AIMA3e pg. 575
            ICategoricalDistribution b_kp2t = new ProbabilityTable(new double[] { 1.0, 1.0 }, ExampleRV.RAIN_t_RV);
            ICollection<AssignmentProposition> e2 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            ICategoricalDistribution b1 = uw.backward(b_kp2t, e2);
            assertArrayEquals(new double[] { 0.69, 0.41 }, b1.getValues(), DELTA_THRESHOLD);
        }

        protected void testForwardBackward_UmbrellaWorld(IForwardBackwardInference uw)
        {
            // AIMA3e pg. 572
            // Day 0, no observations only the security guards prior beliefs
            // P(R<sub>0</sub>) = <0.5, 0.5>
            ICategoricalDistribution prior = new ProbabilityTable(new double[] { 0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1
            ICollection<ICollection<AssignmentProposition>> evidence = CollectionFactory.CreateQueue<ICollection<AssignmentProposition>>();
            ICollection<AssignmentProposition> e1 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e1);

            ICollection<ICategoricalDistribution> smoothed = uw.forwardBackward(evidence, prior);

            Assert.AreEqual(1, smoothed.Size());
            assertArrayEquals(new double[] { 0.818, 0.182 }, smoothed.Get(0).getValues(), DELTA_THRESHOLD);

            // Day 2
            ICollection<AssignmentProposition> e2 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e2);

            smoothed = uw.forwardBackward(evidence, prior);

            Assert.AreEqual(2, smoothed.Size());
            assertArrayEquals(new double[] { 0.883, 0.117 }, smoothed.Get(0).getValues(), DELTA_THRESHOLD);
            assertArrayEquals(new double[] { 0.883, 0.117 }, smoothed.Get(1).getValues(), DELTA_THRESHOLD);

            // Day 3
            ICollection<AssignmentProposition> e3 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false));
            evidence.Add(e3);

            smoothed = uw.forwardBackward(evidence, prior);

            Assert.AreEqual(3, smoothed.Size());
            assertArrayEquals(new double[] { 0.861, 0.138 }, smoothed.Get(0).getValues(), DELTA_THRESHOLD);
            assertArrayEquals(new double[] { 0.799, 0.201 }, smoothed.Get(1).getValues(), DELTA_THRESHOLD);
            assertArrayEquals(new double[] { 0.190, 0.810 }, smoothed.Get(2).getValues(), DELTA_THRESHOLD);
        }
    }

}
