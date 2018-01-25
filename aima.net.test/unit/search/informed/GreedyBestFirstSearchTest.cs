using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent.api;
using aima.net.environment.eightpuzzle;
using aima.net.environment.map;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.search.framework.qsearch;
using aima.net.search.informed;

namespace aima.net.test.unit.search.informed
{
    [TestClass]
    public class GreedyBestFirstSearchTest
    {

        [TestMethod]
        public void testGreedyBestFirstSearch()
        {
            // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
            // {2,0,5,6,4,8,3,7,1});
            // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
            // {0,8,7,6,5,4,3,2,1});
            EightPuzzleBoard board = new EightPuzzleBoard(new int[] { 7, 1, 8, 0, 4, 6, 2, 3, 5 });

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(board);
            ISearchForActions<EightPuzzleBoard, IAction> search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>
                    (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);

            Assert.AreEqual(49, agent.getActions().Size()); // GraphSearchReducedFrontier: "49"
            Assert.AreEqual("332", // GraphSearchReducedFrontier: "197"
                    agent.getInstrumentation().getProperty("nodesExpanded"));
            Assert.AreEqual("241", // GraphSearchReducedFrontier: "140"
                    agent.getInstrumentation().getProperty("queueSize"));
            Assert.AreEqual("242", // GraphSearchReducedFrontier: "141"
                    agent.getInstrumentation().getProperty("maxQueueSize"));

        }

        [TestMethod]
        public void testGreedyBestFirstSearchReducedFrontier()
        {
            // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
            // {2,0,5,6,4,8,3,7,1});
            // EightPuzzleBoard extreme = new EightPuzzleBoard(new int[]
            // {0,8,7,6,5,4,3,2,1});
            EightPuzzleBoard board = new EightPuzzleBoard(new int[] { 7, 1, 8, 0, 4, 6, 2, 3, 5 });

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(board);
            QueueBasedSearch<EightPuzzleBoard, IAction> search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>
                    (new GraphSearchReducedFrontier<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createManhattanHeuristicFunction());

            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            Assert.AreEqual(49, agent.getActions().Size());
            Assert.AreEqual("197", agent.getInstrumentation().getProperty("nodesExpanded"));
            Assert.AreEqual("140", agent.getInstrumentation().getProperty("queueSize"));
            Assert.AreEqual("141", agent.getInstrumentation().getProperty("maxQueueSize"));

        }

        [TestMethod]
        public void testAIMA3eFigure3_23()
        {
            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
            IProblem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>(SimplifiedRoadMapOfPartOfRomania.ARAD,
                    MapFunctions.createActionsFunction(romaniaMap), MapFunctions.createResultFunction(),
                     SimplifiedRoadMapOfPartOfRomania.BUCHAREST.Equals,
                    MapFunctions.createDistanceStepCostFunction(romaniaMap));

            ISearchForActions<string, MoveToAction> search = new GreedyBestFirstSearch<string, MoveToAction>(new TreeSearch<string, MoveToAction>(),
                    MapFunctions.createSLDHeuristicFunction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST, romaniaMap));
            SearchAgent<string, MoveToAction> agent = new SearchAgent<string, MoveToAction>(problem, search);
            Assert.AreEqual(
                    "[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==Fagaras], Action[name==moveTo, location==Bucharest]]",
                    agent.getActions().ToString());
            Assert.AreEqual(3, agent.getActions().Size());
            Assert.AreEqual("3", agent.getInstrumentation().getProperty("nodesExpanded"));
            Assert.AreEqual("6", agent.getInstrumentation().getProperty("queueSize"));
            Assert.AreEqual("7", agent.getInstrumentation().getProperty("maxQueueSize"));
        }

        [TestMethod]
        public void testAIMA3eFigure3_23_using_GraphSearch()
        {
            Map romaniaMap = new SimplifiedRoadMapOfPartOfRomania();
            IProblem<string, MoveToAction> problem = new GeneralProblem<string, MoveToAction>(SimplifiedRoadMapOfPartOfRomania.ARAD,
                    MapFunctions.createActionsFunction(romaniaMap), MapFunctions.createResultFunction(),
                   SimplifiedRoadMapOfPartOfRomania.BUCHAREST.Equals,
                    MapFunctions.createDistanceStepCostFunction(romaniaMap));

            ISearchForActions<string, MoveToAction> search = new GreedyBestFirstSearch<string, MoveToAction>(new GraphSearch<string, MoveToAction>(),
                    MapFunctions.createSLDHeuristicFunction(SimplifiedRoadMapOfPartOfRomania.BUCHAREST, romaniaMap));
            SearchAgent<string, MoveToAction> agent = new SearchAgent<string, MoveToAction>(problem, search);
            Assert.AreEqual(
                        "[Action[name==moveTo, location==Sibiu], Action[name==moveTo, location==Fagaras], Action[name==moveTo, location==Bucharest]]",
                        agent.getActions().ToString());
            Assert.AreEqual(3, agent.getActions().Size());
            Assert.AreEqual("3", agent.getInstrumentation().getProperty("nodesExpanded"));
            Assert.AreEqual("4", agent.getInstrumentation().getProperty("queueSize"));
            Assert.AreEqual("5", agent.getInstrumentation().getProperty("maxQueueSize"));
        }
    }

}
