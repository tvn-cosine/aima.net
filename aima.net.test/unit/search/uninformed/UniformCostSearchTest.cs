namespace aima.net.test.unit.search.uninformed
{
////    [TestClass] public class UniformCostSearchTest
////    {

////        [TestMethod]
////        public void testUniformCostSuccesfulSearch() 
////        {
////            Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(8),
////				NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
////		SearchForActions<NQueensBoard, QueenAction> search = new UniformCostSearch<>();
////        SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<>(problem, search);

////     IQueue<Action> actions = agent.getActions();

////        Assert.AreEqual(8, actions.Size());

////		Assert.AreEqual("1965", agent.getInstrumentation().getProperty("nodesExpanded"));

////		Assert.AreEqual("8.0", agent.getInstrumentation().getProperty("pathCost"));
////	}

////    [TestMethod]
////    public void testUniformCostUnSuccesfulSearch() 
////    {
////        Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(3),
////				NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
////		SearchForActions<NQueensBoard, QueenAction> search = new UniformCostSearch<>();
////    SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<>(problem, search);

//// IQueue<Action> actions = agent.getActions();

////    Assert.AreEqual(0, actions.Size());

////		Assert.AreEqual("6", agent.getInstrumentation().getProperty("nodesExpanded"));

////		// Will be 0 as did not reach goal state.
////		Assert.AreEqual("0", agent.getInstrumentation().getProperty("pathCost"));
////	}

////[TestMethod]
////    public void testAIMA3eFigure3_15() 
////{
////    Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
////Problem<string, MoveToAction> problem = new GeneralProblem<>(SimplifiedRoadMapOfPartOfRomania.SIBIU,
////        MapFunctions.createActionsFunction(romaniaMap), MapFunctions.createResultFunction(),
////        GoalTest.isEqual(SimplifiedRoadMapOfPartOfRomania.BUCHAREST),
////        MapFunctions.createDistanceStepCostFunction(romaniaMap));

////SearchForActions<string, MoveToAction> search = new UniformCostSearch<>();
////SearchAgent<string, MoveToAction> agent = new SearchAgent<>(problem, search);

////List<Action> actions = agent.getActions();

////Assert.AreEqual(
////				"[Action[name==moveTo, location==RimnicuVilcea], Action[name==moveTo, location==Pitesti], Action[name==moveTo, location==Bucharest]]",
////				actions.ToString());
////		Assert.AreEqual("278.0", search.getMetrics().Get(QueueSearch.METRIC_PATH_COST));
////	}

////	[TestMethod]
////    public void testCheckFrontierPathCost() 
////{
////    ExtendableMap map = new ExtendableMap();
////map.addBidirectionalLink("start", "b", 2.5);
////		map.addBidirectionalLink("start", "c", 1.0);
////		map.addBidirectionalLink("b", "d", 2.0);
////		map.addBidirectionalLink("c", "d", 4.0);
////		map.addBidirectionalLink("c", "e", 1.0);
////		map.addBidirectionalLink("d", "goal", 1.0);
////		map.addBidirectionalLink("e", "goal", 5.0);
////		Problem<string, MoveToAction> problem = new GeneralProblem<>("start", MapFunctions.createActionsFunction(map),
////                MapFunctions.createResultFunction(), GoalTest.isEqual("goal"),
////                MapFunctions.createDistanceStepCostFunction(map));

////SearchForActions<string, MoveToAction> search = new UniformCostSearch<>();
////SearchAgent<string, MoveToAction> agent = new SearchAgent<>(problem, search);

////List<Action> actions = agent.getActions();

////Assert.AreEqual(
////				"[Action[name==moveTo, location==b], Action[name==moveTo, location==d], Action[name==moveTo, location==goal]]",
////				actions.ToString());
////		Assert.AreEqual("5.5", search.getMetrics().Get(QueueSearch.METRIC_PATH_COST));
////	}
////}

}
