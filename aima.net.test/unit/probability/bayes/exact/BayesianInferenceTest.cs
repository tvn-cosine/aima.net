using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.bayes;
using aima.net.probability.bayes.api;
using aima.net.probability.example;
using aima.net.probability.proposition;

namespace aima.net.test.unit.probability.bayes.exact
{
    public abstract class BayesianInferenceTest
    { 
        protected IBayesInference bayesInference = null;

        public abstract void setUp();

        [TestMethod]
        public void testInferenceOnToothacheCavityCatchNetwork()
        {
            IBayesianNetwork bn = BayesNetExampleFactory
                    .constructToothacheCavityCatchNetwork();

            ICategoricalDistribution d = bayesInference.Ask(
                    new IRandomVariable[] { ExampleRV.CAVITY_RV },
                    new AssignmentProposition[] { }, bn);

            // System.Console.WriteLine("P(Cavity)=" + d);
            Assert.AreEqual(2, d.getValues().Length);
            Assert.AreEqual(0.2, d.getValues()[0],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
            Assert.AreEqual(0.8, d.getValues()[1],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);

            // AIMA3e pg. 493
            // P(Cavity | toothache) = <0.6, 0.4>
            d = bayesInference.Ask(new IRandomVariable[] { ExampleRV.CAVITY_RV },
                    new AssignmentProposition[] { new AssignmentProposition(
                        ExampleRV.TOOTHACHE_RV, true) }, bn);

            // System.Console.WriteLine("P(Cavity | toothache)=" + d);
            Assert.AreEqual(2, d.getValues().Length);
            Assert.AreEqual(0.6, d.getValues()[0],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
            Assert.AreEqual(0.4, d.getValues()[1],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);

            // AIMA3e pg. 497
            // P(Cavity | toothache AND catch) = <0.871, 0.129>
            d = bayesInference
                    .Ask(new IRandomVariable[] { ExampleRV.CAVITY_RV },
                            new AssignmentProposition[] {
                                new AssignmentProposition(
                                        ExampleRV.TOOTHACHE_RV, true),
                                new AssignmentProposition(ExampleRV.CATCH_RV,
                                        true) }, bn);

            // System.Console.WriteLine("P(Cavity | toothache, catch)=" + d);
            Assert.AreEqual(2, d.getValues().Length);
            Assert.AreEqual(0.8709677419354839, d.getValues()[0],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
            Assert.AreEqual(0.12903225806451615, d.getValues()[1],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
        }

        [TestMethod]
        public void testInferenceOnBurglaryAlarmNetwork()
        {
            IBayesianNetwork bn = BayesNetExampleFactory
                    .constructBurglaryAlarmNetwork();

            // AIMA3e. pg. 514
            ICategoricalDistribution d = bayesInference
                    .Ask(new IRandomVariable[] { ExampleRV.ALARM_RV },
                            new AssignmentProposition[] {
                                new AssignmentProposition(
                                        ExampleRV.BURGLARY_RV, false),
                                new AssignmentProposition(
                                        ExampleRV.EARTHQUAKE_RV, false),
                                new AssignmentProposition(
                                        ExampleRV.JOHN_CALLS_RV, true),
                                new AssignmentProposition(
                                        ExampleRV.MARY_CALLS_RV, true) }, bn);

            // System.Console.WriteLine("P(Alarm | ~b, ~e, j, m)=" + d);
            Assert.AreEqual(2, d.getValues().Length);
            Assert.AreEqual(0.5577689243027888, d.getValues()[0],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
            Assert.AreEqual(0.44223107569721115, d.getValues()[1],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);

            // AIMA3e pg. 523
            // P(Burglary | JohnCalls = true, MaryCalls = true) = <0.284, 0.716>
            d = bayesInference
                    .Ask(new IRandomVariable[] { ExampleRV.BURGLARY_RV },
                            new AssignmentProposition[] {
                                new AssignmentProposition(
                                        ExampleRV.JOHN_CALLS_RV, true),
                                new AssignmentProposition(
                                        ExampleRV.MARY_CALLS_RV, true) }, bn);

            // System.Console.WriteLine("P(Burglary | j, m)=" + d);
            Assert.AreEqual(2, d.getValues().Length);
            Assert.AreEqual(0.2841718353643929, d.getValues()[0],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
            Assert.AreEqual(0.7158281646356071, d.getValues()[1],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);

            // AIMA3e pg. 528
            // P(JohnCalls | Burglary = true)
            d = bayesInference.Ask(
                    new IRandomVariable[] { ExampleRV.JOHN_CALLS_RV },
                    new AssignmentProposition[] { new AssignmentProposition(
                        ExampleRV.BURGLARY_RV, true) }, bn);
            // System.Console.WriteLine("P(JohnCalls | b)=" + d);
            Assert.AreEqual(2, d.getValues().Length);
            Assert.AreEqual(0.8490169999999999, d.getValues()[0],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
            Assert.AreEqual(0.15098299999999998, d.getValues()[1],
                    ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD);
        }
    }

}
