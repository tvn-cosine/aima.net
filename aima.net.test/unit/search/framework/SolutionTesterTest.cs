using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.environment.map;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.problem;

namespace aima.net.test.unit.search.framework
{
//    [TestClass] public class SolutionTesterTest
//    {

//        [TestMethod]
//        public void testMultiGoalProblem() 
//        {
//            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();

//        Problem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>
//                (SimplifiedRoadMapOfPartOfRomania.ARAD,
//                MapFunctions.createActionsFunction(romaniaMap), MapFunctions.createResultFunction(),
//                GoalTest.< string > isEqual(SimplifiedRoadMapOfPartOfRomania.BUCHAREST).or
//                        (GoalTest.isEqual(SimplifiedRoadMapOfPartOfRomania.HIRSOVA)),
//                MapFunctions.createDistanceStepCostFunction(romaniaMap)) {
//            @Override

//            public bool testSolution(Node<string, MoveToAction> node)
//        {
//            return testGoal(node.getState()) && node.getPathCost() > 550;
//            // accept paths to goal only if their costs are above 550
//        }
//    };

//    SearchForActions<string, MoveToAction> search = new UniformCostSearch<string, MoveToAction>(new GraphSearch<>());

//    SearchAgent<string, MoveToAction> agent = new SearchAgent<string, MoveToAction>(problem, search);
//    Assert.AreEqual(
//				"[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==RimnicuVilcea], Action[name==moveTo, location==Pitesti], Action[name==moveTo, location==Bucharest], Action[name==moveTo, location==Urziceni], Action[name==moveTo, location==Hirsova]]",
//				agent.getActions().ToString());
//		Assert.AreEqual(6, agent.getActions().Size());
//		Assert.AreEqual("15", agent.getInstrumentation().getProperty("nodesExpanded"));
//		Assert.AreEqual("1", agent.getInstrumentation().getProperty("queueSize"));
//		Assert.AreEqual("5", agent.getInstrumentation().getProperty("maxQueueSize"));
//	}
//}

}
