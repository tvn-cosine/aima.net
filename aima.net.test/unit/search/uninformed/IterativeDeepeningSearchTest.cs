namespace aima.net.test.unit.search.uninformed
{
    //[TestClass] public class IterativeDeepeningSearchTest
    //{

    //    [TestMethod]
    //    public void testIterativeDeepeningSearch()
    //    {
    //        try
    //        {
    //            Problem<NQueensBoard, QueenAction> problem = new GeneralProblem<>(new NQueensBoard(8),
    //                    NQueensFunctions::getIFActions, NQueensFunctions::getResult, NQueensFunctions::testGoal);
    //            SearchForActions<NQueensBoard, QueenAction> search = new IterativeDeepeningSearch<>();
    //            Optional<IQueue<QueenAction>> actions = search.findActions(problem);
    //            Assert.IsTrue(actions.isPresent());
    //            assertCorrectPlacement(actions.Get());
    //            Assert.AreEqual("3656", search.getMetrics().Get("nodesExpanded"));

    //        }
    //        catch (Exception e)
    //        {
    //            e.printStackTrace();
    //            Assert.Fail("Exception should not occur");
    //        }
    //    }

    //    private void assertCorrectPlacement(List<QueenAction> actions)
    //    {
    //        Assert.AreEqual(8, actions.Size());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 0 , 0 ) ]", actions.Get(0).ToString());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 1 , 4 ) ]", actions.Get(1).ToString());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 2 , 7 ) ]", actions.Get(2).ToString());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 3 , 5 ) ]", actions.Get(3).ToString());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 4 , 2 ) ]", actions.Get(4).ToString());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 5 , 6 ) ]", actions.Get(5).ToString());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 6 , 1 ) ]", actions.Get(6).ToString());
    //        Assert.AreEqual("Action[name==placeQueenAt, location== ( 7 , 3 ) ]", actions.Get(7).ToString());
    //    }
    //}
}
