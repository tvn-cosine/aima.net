namespace aima.net.test.unit.search.uninformed
{
////    [TestClass] public class DepthLimitedSearchTest
////    {

////        [TestMethod]
////        public void testSuccessfulDepthLimitedSearch() 
////        {
////            Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(8),
////				NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
////		SearchForActions<NQueensBoard, QueenAction> search = new DepthLimitedSearch<>(8);
////        Optional<IQueue<QueenAction>> actions = search.findActions(problem);
////        Assert.IsTrue(actions.isPresent());

////        assertCorrectPlacement(actions.Get());
////		Assert.AreEqual("113", search.getMetrics().Get("nodesExpanded"));
////	}

////    [TestMethod]
////    public void testCutOff() 
////    {
////        Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(8),
////				NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
////		DepthLimitedSearch<NQueensBoard, QueenAction> search = new DepthLimitedSearch<>(1);
////    Optional<Node<NQueensBoard, QueenAction>> result = search.findNode(problem);
////    Assert.AreEqual(true, search.isCutoffResult(result));
////	}

////[TestMethod]
////    public void testFailure() 
////{
////    Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(3),
////				NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
////		DepthLimitedSearch<NQueensBoard, QueenAction> search = new DepthLimitedSearch<>(5);
////Optional<IQueue<QueenAction>> actions = search.findActions(problem);
////Assert.IsFalse(actions.isPresent()); // failure
////	}

////	//
////	// PRIVATE METHODS
////	//
////	private void assertCorrectPlacement(List<QueenAction> actions)
////{
////    Assert.AreEqual(8, actions.Size());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 0 , 0 ) ]", actions.Get(0).ToString());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 1 , 4 ) ]", actions.Get(1).ToString());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 2 , 7 ) ]", actions.Get(2).ToString());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 3 , 5 ) ]", actions.Get(3).ToString());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 4 , 2 ) ]", actions.Get(4).ToString());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 5 , 6 ) ]", actions.Get(5).ToString());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 6 , 1 ) ]", actions.Get(6).ToString());
////    Assert.AreEqual("Action[name==placeQueenAt, location== ( 7 , 3 ) ]", actions.Get(7).ToString());
////}
////}
}
