using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net;
using aima.net.agent.api;
using aima.net.environment.map;
using aima.net.search.informed;

namespace aima.net.test.unit.search.informed
{
    //[TestClass]
    //public class RecursiveBestFirstSearchTest
    //{

    //    private static IStringBuilder envChanges = TextFactory.CreateStringBuilder();

    //    private Map aMap  ;

    //      private RecursiveBestFirstSearch<string, MoveToAction> recursiveBestFirstSearch;
    //     private RecursiveBestFirstSearch<string, MoveToAction> recursiveBestFirstSearchAvoidingLoops;

    //    //[TestInitialize]
    //    //public void setUp()
    //    //{
    //    //    envChanges = TextFactory.CreateStringBuilder();
    //    //    aMap = new SimplifiedRoadMapOfPartOfRomania();

    //    //    ToDoubleFunction<Node<string, MoveToAction>> heuristicFunction = (node) =>
    //    //    {
    //    //        Point2D pt1 = aMap.getPosition((String)node.getState());
    //    //        Point2D pt2 = aMap.getPosition(SimplifiedRoadMapOfPartOfRomania.BUCHAREST);
    //    //        return pt1.distance(pt2);
    //    //    };

    //    //    recursiveBestFirstSearch = new RecursiveBestFirstSearch<>(new AStarSearch.EvalFunction<>(heuristicFunction));
    //    //    recursiveBestFirstSearchAvoidingLoops = new RecursiveBestFirstSearch<>(
    //    //            new AStarSearch.EvalFunction<>(heuristicFunction), true);
    //    //}

    //    [TestMethod]
    //    public void testStartingAtGoal()
    //    {
    //        MapEnvironment me = new MapEnvironment(aMap);
    //        SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, recursiveBestFirstSearch,
    //                new string[] { SimplifiedRoadMapOfPartOfRomania.BUCHAREST });

    //        me.addAgent(ma, SimplifiedRoadMapOfPartOfRomania.BUCHAREST);
    //        me.addEnvironmentView(new TestEnvironmentView());
    //        me.stepUntilDone();

    //        Assert.AreEqual(
    //                "CurrentLocation=In(Bucharest), Goal=In(Bucharest):Action[name==NoOp]:METRIC[pathCost]=0.0:METRIC[maxRecursiveDepth]=0:METRIC[nodesExpanded]=0:Action[name==NoOp]:",
    //                envChanges.ToString());
    //    }

    //    [TestMethod]
    //    public void testAIMA3eFigure3_27()
    //    {
    //        MapEnvironment me = new MapEnvironment(aMap);
    //        SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, recursiveBestFirstSearch,
    //                new string[] { SimplifiedRoadMapOfPartOfRomania.BUCHAREST });

    //        me.addAgent(ma, SimplifiedRoadMapOfPartOfRomania.ARAD);
    //        me.addEnvironmentView(new TestEnvironmentView());
    //        me.stepUntilDone();

    //        Assert.AreEqual(
    //                "CurrentLocation=In(Arad), Goal=In(Bucharest):Action[name==moveTo, location==Sibiu]:Action[name==moveTo, location==RimnicuVilcea]:Action[name==moveTo, location==Pitesti]:Action[name==moveTo, location==Bucharest]:METRIC[pathCost]=418.0:METRIC[maxRecursiveDepth]=4:METRIC[nodesExpanded]=6:Action[name==NoOp]:",
    //                envChanges.ToString());
    //    }

    //    [TestMethod]
    //    public void testAIMA3eAradNeamtA()
    //    {
    //        MapEnvironment me = new MapEnvironment(aMap);
    //        SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, recursiveBestFirstSearch,
    //                new string[] { SimplifiedRoadMapOfPartOfRomania.NEAMT });

    //        me.addAgent(ma, SimplifiedRoadMapOfPartOfRomania.ARAD);
    //        me.addEnvironmentView(new TestEnvironmentView());
    //        me.stepUntilDone();

    //        Assert.AreEqual(
    //                "CurrentLocation=In(Arad), Goal=In(Neamt):Action[name==moveTo, location==Sibiu]:Action[name==moveTo, location==RimnicuVilcea]:Action[name==moveTo, location==Pitesti]:Action[name==moveTo, location==Bucharest]:Action[name==moveTo, location==Urziceni]:Action[name==moveTo, location==Vaslui]:Action[name==moveTo, location==Iasi]:Action[name==moveTo, location==Neamt]:METRIC[pathCost]=824.0:METRIC[maxRecursiveDepth]=12:METRIC[nodesExpanded]=340208:Action[name==NoOp]:",
    //                envChanges.ToString());
    //    }

    //    [TestMethod]
    //    public void testAIMA3eAradNeamtB()
    //    {
    //        MapEnvironment me = new MapEnvironment(aMap);
    //        SimpleMapAgent ma = new SimpleMapAgent(me.getMap(), me, recursiveBestFirstSearchAvoidingLoops,
    //                new string[] { SimplifiedRoadMapOfPartOfRomania.NEAMT });

    //        me.addAgent(ma, SimplifiedRoadMapOfPartOfRomania.ARAD);
    //        me.addEnvironmentView(new TestEnvironmentView());
    //        me.stepUntilDone();

    //        // loops avoided, now much less number of expanded nodes ...
    //        Assert.AreEqual(
    //                "CurrentLocation=In(Arad), Goal=In(Neamt):Action[name==moveTo, location==Sibiu]:Action[name==moveTo, location==RimnicuVilcea]:Action[name==moveTo, location==Pitesti]:Action[name==moveTo, location==Bucharest]:Action[name==moveTo, location==Urziceni]:Action[name==moveTo, location==Vaslui]:Action[name==moveTo, location==Iasi]:Action[name==moveTo, location==Neamt]:METRIC[pathCost]=824.0:METRIC[maxRecursiveDepth]=9:METRIC[nodesExpanded]=1367:Action[name==NoOp]:",
    //                envChanges.ToString());
    //    }

    //    private class TestEnvironmentView : EnvironmentView
    //    {

    //        public void notify(string msg)
    //        {
    //            envChanges.Append(msg).Append(":");
    //        }

    //        public void agentAdded(Agent agent, Environment source)
    //        {
    //            // Nothing.
    //        }

    //        public void agentActed(Agent agent, Percept percept, Action action, Environment source)
    //        {
    //            envChanges.Append(action).Append(":");
    //        }
    //    }
    //}

}
