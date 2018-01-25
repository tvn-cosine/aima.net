using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net;
using aima.net.text;
using aima.net.text.api;
using aima.net.environment.vacuum;

namespace aima.net.test.unit.environment.vacuum
{
    [TestClass]
    public class TableDrivenVacuumAgentTest
    {
        private TableDrivenVacuumAgent agent;

        private IStringBuilder envChanges;

        [TestInitialize]
        public void setUp()
        {
            agent = new TableDrivenVacuumAgent();
            envChanges = TextFactory.CreateStringBuilder();
        }

        [TestMethod]
        public void testCleanClean()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.StepUntilDone();

            Assert.AreEqual(
                    "Action[name==Right]Action[name==Left]Action[name==Right]Action[name==NoOp]",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testCleanDirty()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.StepUntilDone();

            Assert.AreEqual(
                    "Action[name==Right]Action[name==Suck]Action[name==Left]Action[name==NoOp]",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testDirtyClean()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.StepUntilDone();

            Assert.AreEqual(
                    "Action[name==Suck]Action[name==Right]Action[name==Left]Action[name==NoOp]",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testDirtyDirty()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.StepUntilDone();

            Assert.AreEqual(
                    "Action[name==Suck]Action[name==Right]Action[name==Suck]Action[name==NoOp]",
                    envChanges.ToString());
        }
    }

}
