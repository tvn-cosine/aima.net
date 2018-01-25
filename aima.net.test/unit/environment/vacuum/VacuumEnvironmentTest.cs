using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.environment.vacuum;

namespace aima.net.test.unit.environment.vacuum
{
    [TestClass] public class VacuumEnvironmentTest
    {
        VacuumEnvironment tve, tve2, tve3, tve4;

        ModelBasedReflexVacuumAgent<object> a;

        [TestInitialize]
        public void setUp()
        {
            tve = new VacuumEnvironment(VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve2 = new VacuumEnvironment(VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve3 = new VacuumEnvironment(VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve4 = new VacuumEnvironment(VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            a = new ModelBasedReflexVacuumAgent<object>();
        }

        [TestMethod]
        public void testTVEConstruction()
        {
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve2.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve2.getLocationState(VacuumEnvironment.LOCATION_B));
        }

        [TestMethod]
        public void testAgentAdd()
        {
            tve.addAgent(a, VacuumEnvironment.LOCATION_A);
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(a));
            Assert.AreEqual(1, tve.GetAgents().Size());
        }
    }

}
