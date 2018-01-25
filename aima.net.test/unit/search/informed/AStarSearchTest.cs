using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent.api;
using aima.net.collections.api;
using aima.net.environment.eightpuzzle;
using aima.net.environment.map;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.framework.qsearch;
using aima.net.search.informed;
using aima.net.util;

namespace aima.net.test.unit.search.informed
{
    [TestClass]
    public class AStarSearchTest
    {

        [TestMethod]
        public void testAStarSearch()
        {
            // added to narrow down bug report filed by L.N.Sudarshan of
            // Thoughtworks and Xin Lu of UCI

            // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
            // {2,0,5,6,4,8,3,7,1});
            // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
            // {0,8,7,6,5,4,3,2,1});
            EightPuzzleBoard board = new EightPuzzleBoard(new int[]
            { 7, 1, 8, 0, 4, 6, 2, 3, 5 });
            //EightPuzzleBoard board = new EightPuzzleBoard(new int[]
            //{ 1, 0, 2, 3, 4, 5, 6, 7, 8 });

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(board);
            ISearchForActions<EightPuzzleBoard, IAction> search 
                = new AStarSearch<EightPuzzleBoard, IAction>(
                    new GraphSearch<EightPuzzleBoard, IAction>(),
                    EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent 
                = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            Assert.AreEqual(23, agent.getActions().Size());
            Assert.AreEqual("1133", // "926" GraphSearchReduced Frontier
                    agent.getInstrumentation().getProperty("nodesExpanded"));
            Assert.AreEqual("676", // "534" GraphSearchReduced Frontier
                    agent.getInstrumentation().getProperty("queueSize"));
            Assert.AreEqual("677", // "535" GraphSearchReduced Frontier
                    agent.getInstrumentation().getProperty("maxQueueSize"));

        }

        [TestMethod]
        public void testAIMA3eFigure3_15()
        {
            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
            IProblem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>(
                    SimplifiedRoadMapOfPartOfRomania.SIBIU,
                    MapFunctions.createActionsFunction(romaniaMap),
                    MapFunctions.createResultFunction(),
                     SimplifiedRoadMapOfPartOfRomania.BUCHAREST.Equals,
                    MapFunctions.createDistanceStepCostFunction(romaniaMap));

            ISearchForActions<string, MoveToAction> search 
                = new AStarSearch<string, MoveToAction>(
                    new GraphSearch<string, MoveToAction>(),
                    MapFunctions.createSLDHeuristicFunction(
                        SimplifiedRoadMapOfPartOfRomania.BUCHAREST, 
                        romaniaMap));
            SearchAgent<string, MoveToAction> agent = new SearchAgent<string, MoveToAction>(problem, search);

            ICollection<MoveToAction> actions = agent.getActions();

            Assert.AreEqual(
                    "[Action[name==moveTo, location==RimnicuVilcea], Action[name==moveTo, location==Pitesti], Action[name==moveTo, location==Bucharest]]",
                    actions.ToString());
            Assert.AreEqual("278",
                    search.getMetrics().get(QueueSearch<string, MoveToAction>.METRIC_PATH_COST));
        }

        [TestMethod]
        public void testAIMA3eFigure3_24()
        {
            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
            IProblem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>(
                            SimplifiedRoadMapOfPartOfRomania.ARAD,
                            MapFunctions.createActionsFunction(romaniaMap),
                            MapFunctions.createResultFunction(),
                            SimplifiedRoadMapOfPartOfRomania.BUCHAREST.Equals,
                            MapFunctions.createDistanceStepCostFunction(romaniaMap));

            ISearchForActions<string, MoveToAction> search = new AStarSearch<string, MoveToAction>(new TreeSearch<string, MoveToAction>(),
                    MapFunctions.createSLDHeuristicFunction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST, romaniaMap));
            SearchAgent<string, MoveToAction> agent = new SearchAgent<string, MoveToAction>(problem, search);
            Assert.AreEqual(
                        "[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==RimnicuVilcea], Action[name==moveTo, location==Pitesti], Action[name==moveTo, location==Bucharest]]",
                        agent.getActions().ToString());
            Assert.AreEqual(4, agent.getActions().Size());
            Assert.AreEqual("5",
                    agent.getInstrumentation().getProperty("nodesExpanded"));
            Assert.AreEqual("10",
                    agent.getInstrumentation().getProperty("queueSize"));
            Assert.AreEqual("11",
                    agent.getInstrumentation().getProperty("maxQueueSize"));
        }

        [TestMethod]
        public void testAIMA3eFigure3_24_using_GraphSearch()
        {
            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
            IProblem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>(
                    SimplifiedRoadMapOfPartOfRomania.ARAD,
                    MapFunctions.createActionsFunction(romaniaMap),
                    MapFunctions.createResultFunction(),
                     SimplifiedRoadMapOfPartOfRomania.BUCHAREST.Equals,
                    MapFunctions.createDistanceStepCostFunction(romaniaMap));

            ISearchForActions<string, MoveToAction> search = new AStarSearch<string, MoveToAction>(new GraphSearch<string, MoveToAction>(),
                    MapFunctions.createSLDHeuristicFunction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST, romaniaMap));
            SearchAgent<string, MoveToAction> agent = new SearchAgent<string, MoveToAction>(problem, search);
            Assert.AreEqual(
                            "[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==RimnicuVilcea], Action[name==moveTo, location==Pitesti], Action[name==moveTo, location==Bucharest]]",
                            agent.getActions().ToString());
            Assert.AreEqual(4, agent.getActions().Size());
            Assert.AreEqual("5",
                    agent.getInstrumentation().getProperty("nodesExpanded"));
            Assert.AreEqual("6",
                    agent.getInstrumentation().getProperty("queueSize"));
            Assert.AreEqual("7",
                    agent.getInstrumentation().getProperty("maxQueueSize"));
        }

        //[TestMethod]
        //public void testCheckFrontierPathCost()
        //{
        //    ExtendableMap map = new ExtendableMap();
        //    map.addBidirectionalLink("start", "b", 2.5);
        //    map.addBidirectionalLink("start", "c", 1.0);
        //    map.addBidirectionalLink("b", "d", 2.0);
        //    map.addBidirectionalLink("c", "d", 4.0);
        //    map.addBidirectionalLink("c", "e", 1.0);
        //    map.addBidirectionalLink("d", "goal", 1.0);
        //    map.addBidirectionalLink("e", "goal", 5.0);
        //    Problem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>("start",
        //            MapFunctions.createActionsFunction(map),
        //            MapFunctions.createResultFunction(), GoalTest.isEqual("goal"),
        //            MapFunctions.createDistanceStepCostFunction(map));

        //    ToDoubleFunction<Node<string, MoveToAction>> h = node=> 0.0; // Don't have one for this test

        //    SearchForActions<string, MoveToAction> search = new AStarSearch<>(new GraphSearch<>(), h);
        //    SearchAgent<string, MoveToAction> agent = new SearchAgent<>(problem, search);

        //    IQueue<Action> actions = agent.getActions();

        //    Assert.AreEqual(
        //                    "[Action[name==moveTo, location==b], Action[name==moveTo, location==d], Action[name==moveTo, location==goal]]",
        //                    actions.ToString());
        //    Assert.AreEqual("5.5",
        //            search.getMetrics().Get(QueueSearch.METRIC_PATH_COST));
        //}
    }

}
