using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability.example;

namespace aima.net.test.unit.probability.full
{
    [TestClass]
    public class FullJointProbabilityModelTest : CommonFiniteProbabilityModelTests
    {

        //
        // ProbabilityModel Tests
        [TestMethod]

        public void test_RollingPairFairDiceModel()
        {
            test_RollingPairFairDiceModel(new FullJointDistributionPairFairDiceModel());
        }

        [TestMethod]
        public void test_ToothacheCavityCatchModel()
        {
            test_ToothacheCavityCatchModel(new FullJointDistributionToothacheCavityCatchModel());
        }

        [TestMethod]
        public void test_ToothacheCavityCatchWeatherModel()
        {
            test_ToothacheCavityCatchWeatherModel(new FullJointDistributionToothacheCavityCatchWeatherModel());
        }

        [TestMethod]
        public void test_MeningitisStiffNeckModel()
        {
            test_MeningitisStiffNeckModel(new FullJointDistributionMeningitisStiffNeckModel());
        }

        [TestMethod]
        public void test_BurglaryAlarmModel()
        {
            test_BurglaryAlarmModel(new FullJointDistributionBurglaryAlarmModel());
        }

        //
        // FiniteProbabilityModel Tests
        [TestMethod]
        public void test_RollingPairFairDiceModel_Distributions()
        {
            test_RollingPairFairDiceModel_Distributions(new FullJointDistributionPairFairDiceModel());
        }

        [TestMethod]
        public void test_ToothacheCavityCatchModel_Distributions()
        {
            test_ToothacheCavityCatchModel_Distributions(new FullJointDistributionToothacheCavityCatchModel());
        }

        [TestMethod]
        public void test_ToothacheCavityCatchWeatherModel_Distributions()
        {
            test_ToothacheCavityCatchWeatherModel_Distributions(new FullJointDistributionToothacheCavityCatchWeatherModel());
        }

        [TestMethod]
        public void test_MeningitisStiffNeckModel_Distributions()
        {
            test_MeningitisStiffNeckModel_Distributions(new FullJointDistributionMeningitisStiffNeckModel());
        }

        [TestMethod]
        public void test_BurglaryAlarmModel_Distributions()
        {
            test_BurglaryAlarmModel_Distributions(new FullJointDistributionBurglaryAlarmModel());
        }
    }

}
