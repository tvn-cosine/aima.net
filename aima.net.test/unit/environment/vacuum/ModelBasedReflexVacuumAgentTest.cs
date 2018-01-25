using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net;
using aima.net.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.environment.vacuum;

namespace aima.net.test.unit.environment.vacuum
{
    [TestClass]
    public class ModelBasedReflexVacuumAgentTest
    {
        private ModelBasedReflexVacuumAgent<object> agent;

        private IStringBuilder envChanges;

        [TestInitialize]
        public void setUp()
        {
            agent = new ModelBasedReflexVacuumAgent<object>();
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

            Assert.AreEqual("Action[name==Right]Action[name==NoOp]",
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
                    "Action[name==Right]Action[name==Suck]Action[name==NoOp]",
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
                    "Action[name==Suck]Action[name==Right]Action[name==NoOp]",
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

        [TestMethod]
        public void testAgentActionNumber1()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // cleans location A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // moves to lcation B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // cleans location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(19, tve.GetPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber2()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // cleans location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // moves to lcation A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // cleans location A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(19, tve.GetPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber3()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // moves to location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(-1, tve.GetPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber4()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // moves to location A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(-1, tve.GetPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber5()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // moves to B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // cleans location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.GetPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber6()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // cleans B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // moves to A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.GetPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber7()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // cleans A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // moves to B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.GetPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber8()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.GetAgents().Size());
            tve.Step(); // moves to A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // cleans A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.Step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.GetPerformanceMeasure(agent), 0.001);
        }
    }
}
