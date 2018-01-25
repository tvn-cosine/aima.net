using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net;
using aima.net.environment.map;
using aima.net.util;

namespace aima.net.test.unit.search.online
{
//    [TestClass] public class LRTAStarAgentTest
//    {
//        private ExtendableMap aMap;
//        private IStringBuilder envChanges;
//        private ToDoubleFunction<string> h;

//        [TestInitialize]
//       public void setUp()
//        {
//            aMap = new ExtendableMap();
//            aMap.addBidirectionalLink("A", "B", 4.0);
//            aMap.addBidirectionalLink("B", "C", 4.0);
//            aMap.addBidirectionalLink("C", "D", 4.0);
//            aMap.addBidirectionalLink("D", "E", 4.0);
//            aMap.addBidirectionalLink("E", "F", 4.0);
//            h = (state)=> 1.0;

//            envChanges = TextFactory.CreateStringBuilder();
//        }

//        [TestMethod]
//       public void testAlreadyAtGoal()
//        {
//            MapEnvironment me = new MapEnvironment(aMap);
//            OnlineSearchProblem<string, MoveToAction> problem = new GeneralProblem<>(null,
//                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("A"),
//                    MapFunctions.createDistanceStepCostFunction(aMap));
//            LRTAStarAgent<string, MoveToAction> agent = new LRTAStarAgent<>
//                    (problem, MapFunctions.createPerceptToStateFunction(), h);

//            me.addAgent(agent, "A");
//            me.addEnvironmentView(new TestEnvironmentView());
//            me.stepUntilDone();

//            Assert.AreEqual("Action[name==NoOp]->", envChanges.ToString());
//        }

//        [TestMethod]
//       public void testNormalSearch()
//        {
//            MapEnvironment me = new MapEnvironment(aMap);
//            OnlineSearchProblem<string, MoveToAction> problem = new GeneralProblem<>(null,
//                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("F"),
//                    MapFunctions.createDistanceStepCostFunction(aMap));
//            LRTAStarAgent<string, MoveToAction> agent = new LRTAStarAgent<>
//                    (problem, MapFunctions.createPerceptToStateFunction(), h);

//            me.addAgent(agent, "A");
//            me.addEnvironmentView(new TestEnvironmentView());
//            me.stepUntilDone();

//            Assert.AreEqual(
//                    "Action[name==moveTo, location==B]->Action[name==moveTo, location==A]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==F]->Action[name==NoOp]->",
//                    envChanges.ToString());
//        }

//        [TestMethod]
//       public void testNoPath()
//        {
//            MapEnvironment me = new MapEnvironment(aMap);
//            OnlineSearchProblem<string, MoveToAction> problem = new GeneralProblem<>(null,
//                    MapFunctions.createActionsFunction(aMap), null, GoalTest.isEqual("G"),
//                    MapFunctions.createDistanceStepCostFunction(aMap));
//            LRTAStarAgent<string, MoveToAction> agent = new LRTAStarAgent<>
//                    (problem, MapFunctions.createPerceptToStateFunction(), h);

//            me.addAgent(agent, "A");
//            me.addEnvironmentView(new TestEnvironmentView());
//            // Note: Will search forever if no path is possible,
//            // Therefore restrict the number of steps to something
//            // reasonablbe, against which to test.
//            me.step(14);

//            Assert.AreEqual(
//                    "Action[name==moveTo, location==B]->Action[name==moveTo, location==A]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==B]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==C]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==D]->Action[name==moveTo, location==E]->Action[name==moveTo, location==F]->Action[name==moveTo, location==E]->",
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
