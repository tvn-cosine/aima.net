using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.environment.nqueens;
using aima.net.search.framework.problem;

namespace aima.net.test.unit.search.uninformed
{
//    [TestClass] public class DepthFirstSearchTest
//    {

//        [TestMethod]
//        public void testDepthFirstSuccesfulSearch() 
//        {
//            Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(8),
//				NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
//		SearchForActions<NQueensBoard, QueenAction> search = new DepthFirstSearch<>(new GraphSearch<>());
//        Optional<IQueue<QueenAction>> actions = search.findActions(problem);
//        Assert.IsTrue(actions.isPresent());

//        assertCorrectPlacement(actions.Get());
//		Assert.AreEqual("113", search.getMetrics().Get("nodesExpanded"));
//	}

//    [TestMethod]
//    public void testDepthFirstUnSuccessfulSearch() 
//    {
//        Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(3),
//				NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
//		SearchForActions<NQueensBoard, QueenAction> search = new DepthFirstSearch<>(new GraphSearch<>());
//    SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<>(problem, search);
// IQueue<Action> actions = agent.getActions();
//    Assert.AreEqual(0, actions.Size());
//		Assert.AreEqual("6", agent.getInstrumentation().getProperty("nodesExpanded"));
//	}

////
//// PRIVATE METHODS
////
//private void assertCorrectPlacement(List<QueenAction> actions)
//{
//    Assert.AreEqual(8, actions.Size());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 0 , 7 ) ]", actions.Get(0).ToString());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 1 , 3 ) ]", actions.Get(1).ToString());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 2 , 0 ) ]", actions.Get(2).ToString());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 3 , 2 ) ]", actions.Get(3).ToString());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 4 , 5 ) ]", actions.Get(4).ToString());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 5 , 1 ) ]", actions.Get(5).ToString());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 6 , 6 ) ]", actions.Get(6).ToString());
//    Assert.AreEqual("Action[name==placeQueenAt, location== ( 7 , 4 ) ]", actions.Get(7).ToString());
//}
//}

}
