using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability.bayes;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.exact;
using aima.net.probability.bayes.model;
using aima.net.probability.example;

namespace aima.net.test.unit.probability.bayes.model
{
    [TestClass]
    public class FiniteBayesModelTest : CommonFiniteProbabilityModelTests
    {

        //
        // ProbabilityModel Tests
        [TestMethod]

        public void test_RollingPairFairDiceModel()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_RollingPairFairDiceModel(new FiniteBayesModel(
                        BayesNetExampleFactory.construct2FairDiceNetwor(), bi));
            }
        }

        [TestMethod]
        public void test_ToothacheCavityCatchModel()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_ToothacheCavityCatchModel(new FiniteBayesModel(
                        BayesNetExampleFactory
                                .constructToothacheCavityCatchNetwork(),
                        bi));
            }
        }

        [TestMethod]
        public void test_ToothacheCavityCatchWeatherModel()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_ToothacheCavityCatchWeatherModel(new FiniteBayesModel(
                        BayesNetExampleFactory
                                .constructToothacheCavityCatchWeatherNetwork(),
                        bi));
            }
        }

        [TestMethod]
        public void test_MeningitisStiffNeckModel()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_MeningitisStiffNeckModel(new FiniteBayesModel(
                        BayesNetExampleFactory.constructMeningitisStiffNeckNetwork(),
                        bi));
            }
        }

        [TestMethod]
        public void test_BurglaryAlarmModel()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_BurglaryAlarmModel(
                    new FiniteBayesModel(
                        BayesNetExampleFactory.constructBurglaryAlarmNetwork(), 
                        bi));
            }
        }

        //
        // FiniteProbabilityModel Tests
        [TestMethod]
        public void test_RollingPairFairDiceModel_Distributions()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_RollingPairFairDiceModel_Distributions(new FiniteBayesModel(
                        BayesNetExampleFactory.construct2FairDiceNetwor(), bi));
            }
        }

        [TestMethod]
        public void test_ToothacheCavityCatchModel_Distributions()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_ToothacheCavityCatchModel_Distributions(new FiniteBayesModel(
                        BayesNetExampleFactory
                                .constructToothacheCavityCatchNetwork(),
                        bi));
            }
        }

        [TestMethod]
        public void test_ToothacheCavityCatchWeatherModel_Distributions()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_ToothacheCavityCatchWeatherModel_Distributions(new FiniteBayesModel(
                        BayesNetExampleFactory
                                .constructToothacheCavityCatchWeatherNetwork(),
                        bi));
            }
        }

        [TestMethod]
        public void test_MeningitisStiffNeckModel_Distributions()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_MeningitisStiffNeckModel_Distributions(new FiniteBayesModel(
                        BayesNetExampleFactory
                                .constructMeningitisStiffNeckNetwork(),
                        bi));
            }
        }

        [TestMethod]
        public void test_BurglaryAlarmModel_Distributions()
        {
            foreach (IBayesInference bi in getBayesInferenceImplementations())
            {
                test_BurglaryAlarmModel_Distributions(new FiniteBayesModel(
                        BayesNetExampleFactory.constructBurglaryAlarmNetwork(), bi));
            }
        }

        //
        // PRIVATE METHODS
        //
        private IBayesInference[] getBayesInferenceImplementations()
        {
            return new IBayesInference[] { new EnumerationAsk(),
                new EliminationAsk() };
        }
    }

}
