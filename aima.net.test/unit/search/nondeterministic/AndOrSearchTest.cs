﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.environment.vacuum;
using aima.net.search.nondeterministic;

namespace aima.net.test.unit.search.nondeterministic
{
    /**
     * Tests the AND-OR search algorithm using the erratic vacuum world of page 133,
     * AIMA3e. In essence, a two-square office is cleaned by a vacuum that randomly
     * (1) cleans the square, (2) cleans both squares, or (3) dirties the square it
     * meant to clean.
     *
     * @author Andrew Brown
     */
    //[TestClass] public class AndOrSearchTest
    //{

    //    private NondeterministicVacuumAgent agent;
    //    private NondeterministicVacuumEnvironment world;
    //    private NondeterministicProblem< problem;

    //    /**
    //     * Create the vacuum world with the classes defined in this file.
    //     */
    //    [TestInitialize]
    //    public void setUp()
    //    {

    //        this.agent = new NondeterministicVacuumAgent(
    //                new FullyObservableVacuumEnvironmentPerceptToStateFunction());
    //        // create state: both rooms are dirty and the vacuum is in room A
    //        VacuumEnvironmentState state = new VacuumEnvironmentState();
    //        state.setLocationState(LOCATION_A, LocationState.Dirty);
    //        state.setLocationState(LOCATION_B, LocationState.Dirty);
    //        state.setAgentLocation(this.agent, LOCATION_A);
    //        // create problem
    //        this.problem = new NondeterministicProblem<>(
    //                state, VacuumWorldFunctions.getActions,
    //                VacuumWorldFunctions.createResultsFunction(agent),
    //                VacuumWorldFunctions::testGoal,
    //                (s, a, sPrimed) => 1.0);
    //        // set the problem and agent
    //        this.agent.setProblem(this.problem);

    //        // create world
    //        this.world = new NondeterministicVacuumEnvironment(LocationState.Dirty, LocationState.Dirty);
    //        this.world.addAgent(this.agent, LOCATION_A);
    //    }

    //    /**
    //     * Test whether two identically-initialized states will equals() each other.
    //     */
    //    [TestMethod]
    //    public void testStateEquality()
    //    {
    //        // create state 1
    //        VacuumEnvironmentState s1 = new VacuumEnvironmentState();
    //        s1.setLocationState(LOCATION_A, LocationState.Dirty);
    //        s1.setLocationState(LOCATION_B, LocationState.Dirty);
    //        s1.setAgentLocation(this.agent, LOCATION_A);
    //        // create state 2
    //        VacuumEnvironmentState s2 = new VacuumEnvironmentState();
    //        s2.setLocationState(LOCATION_A, LocationState.Dirty);
    //        s2.setLocationState(LOCATION_B, LocationState.Dirty);
    //        s2.setAgentLocation(this.agent, LOCATION_A);
    //        // test
    //        bool actual = s1.Equals(s2);
    //        Assert.AreEqual(true, actual);
    //    }

    //    /**
    //     * Test whether a Path contains() a state; uses state enumeration from page
    //     * 134, AIMA3e.
    //     */
    //    [TestMethod]
    //    public void testPathContains()
    //    {
    //        // create state 1
    //        VacuumEnvironmentState s1 = new VacuumEnvironmentState();
    //        s1.setLocationState(LOCATION_A, LocationState.Dirty);
    //        s1.setLocationState(LOCATION_B, LocationState.Dirty);
    //        s1.setAgentLocation(this.agent, LOCATION_A);
    //        // create state 2
    //        VacuumEnvironmentState s2 = new VacuumEnvironmentState();
    //        s2.setLocationState(LOCATION_A, LocationState.Dirty);
    //        s2.setLocationState(LOCATION_B, LocationState.Dirty);
    //        s2.setAgentLocation(this.agent, LOCATION_B);
    //        // create state 3
    //        VacuumEnvironmentState s3 = new VacuumEnvironmentState();
    //        s3.setLocationState(LOCATION_A, LocationState.Dirty);
    //        s3.setLocationState(LOCATION_B, LocationState.Clean);
    //        s3.setAgentLocation(this.agent, LOCATION_A);
    //        // create state 4
    //        VacuumEnvironmentState s4 = new VacuumEnvironmentState();
    //        s4.setLocationState(LOCATION_A, LocationState.Dirty);
    //        s4.setLocationState(LOCATION_B, LocationState.Clean);
    //        s4.setAgentLocation(this.agent, LOCATION_B);
    //        // create test state 1
    //        VacuumEnvironmentState test1 = new VacuumEnvironmentState();
    //        test1.setLocationState(LOCATION_A, LocationState.Dirty);
    //        test1.setLocationState(LOCATION_B, LocationState.Clean);
    //        test1.setAgentLocation(this.agent, LOCATION_A);
    //        // create test state 2
    //        VacuumEnvironmentState test2 = new VacuumEnvironmentState();
    //        test2.setLocationState(LOCATION_A, LocationState.Clean);
    //        test2.setLocationState(LOCATION_B, LocationState.Clean);
    //        test2.setAgentLocation(this.agent, LOCATION_B);
    //        // add to path
    //        Path path = new Path();
    //        path = path.append(s1, s2, s3, s4);
    //        // test
    //        Assert.AreEqual(true, path.Contains(test1));
    //        Assert.AreEqual(false, path.Contains(test2));
    //    }

    //    /**
    //     * Use AND-OR search to create a contingency plan; execute the plan and
    //     * verify that it successfully cleans the NondeterministicVacuumWorld.
    //     */
    //    [TestMethod]
    //    public void testSearchExecutesSuccessfully()
    //    {
    //        // execute plan
    //        this.world.stepUntilDone();
    //        // test
    //        VacuumEnvironmentState endState = (VacuumEnvironmentState)this.world.getCurrentState();
    //        LocationState a = endState.getLocationState(LOCATION_A);
    //        LocationState b = endState.getLocationState(LOCATION_B);
    //        Assert.AreEqual(LocationState.Clean, a);
    //        Assert.AreEqual(LocationState.Clean, b);
    //    }
    //}
}
