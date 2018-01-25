using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net;
using aima.net.environment.map;
using aima.net.search.framework.problem;

namespace aima.net.test.unit.search.online
{
//    [TestClass] public class OnlineDFSAgentTest
//    {

//        private ExtendableMap aMap;

//        private IStringBuilder envChanges;

//        [TestInitialize]
//        public void setUp()
//        {
//            aMap = new ExtendableMap();
//            aMap.addBidirectionalLink("A", "B", 5.0);
//            aMap.addBidirectionalLink("A", "C", 6.0);
//            aMap.addBidirectionalLink("B", "D", 4.0);
//            aMap.addBidirectionalLink("B", "E", 7.0);
//            aMap.addBidirectionalLink("D", "F", 4.0);
//            aMap.addBidirectionalLink("D", "G", 8.0);

//            envChanges = TextFactory.CreateStringBuilder();
//        }

//        [TestMethod]
//        public void testAlreadyAtGoal()
//        {
//            MapEnvironment me = new MapEnvironment(aMap);
//            OnlineSearchProblem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>(null,
//                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("A"),
//                    MapFunctions.createDistanceStepCostFunction(aMap));
//            OnlineDFSAgent<string, MoveToAction> agent = new OnlineDFSAgent<>
//                    (problem, MapFunctions.createPerceptToStateFunction());

//            me.addAgent(agent, "A");
//            me.addEnvironmentView(new TestEnvironmentView());
//            me.stepUntilDone();

//            Assert.AreEqual("Action[name==NoOp]->", envChanges.ToString());
//        }

//        [TestMethod]
//        public void testNormalSearch()
//        {
//            MapEnvironment me = new MapEnvironment(aMap);
//            OnlineSearchProblem<string, MoveToAction> problem = new GeneralProblem<>(null,
//                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("G"),
//                    MapFunctions.createDistanceStepCostFunction(aMap));
//            OnlineDFSAgent<string, MoveToAction> agent = new OnlineDFSAgent<>
//                    (problem, MapFunctions.createPerceptToStateFunction());

//            me.addAgent(agent, "A");
//            me.addEnvironmentView(new TestEnvironmentView());
//            me.stepUntilDone();

//            Assert.AreEqual(
//                    "Action[name==moveTo, location==B]->Action[name==moveTo, location==A]->Action[name==moveTo, location==C]->Action[name==moveTo, location==A]->Action[name==moveTo, location==C]->Action[name==moveTo, location==A]->Action[name==moveTo, location==B]->Action[name==moveTo, location==D]->Action[name==moveTo, location==B]->Action[name==moveTo, location==E]->Action[name==moveTo, location==B]->Action[name==moveTo, location==E]->Action[name==moveTo, location==B]->Action[name==moveTo, location==D]->Action[name==moveTo, location==F]->Action[name==moveTo, location==D]->Action[name==moveTo, location==G]->Action[name==NoOp]->",
//                    envChanges.ToString());
//        }

//        [TestMethod]
//        public void testNoPath()
//        {
//            aMap = new ExtendableMap();
//            aMap.addBidirectionalLink("A", "B", 1.0);
//            MapEnvironment me = new MapEnvironment(aMap);

//            OnlineSearchProblem<string, MoveToAction> problem = new GeneralProblem<>(null,
//                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("X"),
//                    MapFunctions.createDistanceStepCostFunction(aMap));
//            OnlineDFSAgent<string, MoveToAction> agent = new OnlineDFSAgent<>
//                    (problem, MapFunctions.createPerceptToStateFunction());

//            me.addAgent(agent, "A");
//            me.addEnvironmentView(new TestEnvironmentView());

//            me.stepUntilDone();

//            Assert.AreEqual(
//                    "Action[name==moveTo, location==B]->Action[name==moveTo, location==A]->Action[name==moveTo, location==B]->Action[name==moveTo, location==A]->Action[name==NoOp]->",
//                    envChanges.ToString());
//        }

//        [TestMethod]
//        public void testAIMA3eFig4_19()
//        {
//            aMap = new ExtendableMap();
//            aMap.addBidirectionalLink("1,1", "1,2", 1.0);
//            aMap.addBidirectionalLink("1,1", "2,1", 1.0);
//            aMap.addBidirectionalLink("2,1", "3,1", 1.0);
//            aMap.addBidirectionalLink("2,1", "2,2", 1.0);
//            aMap.addBidirectionalLink("3,1", "3,2", 1.0);
//            aMap.addBidirectionalLink("2,2", "2,3", 1.0);
//            aMap.addBidirectionalLink("3,2", "3,3", 1.0);
//            aMap.addBidirectionalLink("2,3", "1,3", 1.0);

//            MapEnvironment me = new MapEnvironment(aMap);
//            OnlineSearchProblem<string, MoveToAction> problem = new GeneralProblem<>(null,
//                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("3,3"),
//                    MapFunctions.createDistanceStepCostFunction(aMap));
//            OnlineDFSAgent<string, MoveToAction> agent = new OnlineDFSAgent<>
//                    (problem, MapFunctions.createPerceptToStateFunction());

//            me.addAgent(agent, "1,1");
//            me.addEnvironmentView(new TestEnvironmentView());
//            me.stepUntilDone();

//            Assert.AreEqual(
//                    "Action[name==moveTo, location==1,2]->Action[name==moveTo, location==1,1]->Action[name==moveTo, location==2,1]->Action[name==moveTo, location==1,1]->Action[name==moveTo, location==2,1]->Action[name==moveTo, location==2,2]->Action[name==moveTo, location==2,1]->Action[name==moveTo, location==3,1]->Action[name==moveTo, location==2,1]->Action[name==moveTo, location==3,1]->Action[name==moveTo, location==3,2]->Action[name==moveTo, location==3,1]->Action[name==moveTo, location==3,2]->Action[name==moveTo, location==3,3]->Action[name==NoOp]->",
//                    envChanges.ToString());
//        }

//        private class TestEnvironmentView :  EnvironmentView
//        {

//        public void notify(string msg)
//        {
//            envChanges.append(msg).append("->");
//        }

//        public void agentAdded(Agent agent, Environment source)
//        {
//            // Nothing.
//        }

//        public void agentActed(Agent agent, Percept percept, Action action, Environment source)
//        {
//            envChanges.append(action).append("->");
//        }
//    }
//}

}
