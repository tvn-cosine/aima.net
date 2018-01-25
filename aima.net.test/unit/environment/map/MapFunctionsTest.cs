using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.map;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.test.unit.environment.map
{
    [TestClass]
    public class MapFunctionsTest
    {
        private IActionsFunction<string, MoveToAction> actionsFn;
        private IResultFunction<string, MoveToAction> resultFn;
        private IStepCostFunction<string, MoveToAction> stepCostFn;

        [TestInitialize]
        public void setUp()
        {
            ExtendableMap aMap = new ExtendableMap();
            aMap.addBidirectionalLink("A", "B", 5.0);
            aMap.addBidirectionalLink("A", "C", 6.0);
            aMap.addBidirectionalLink("B", "C", 4.0);
            aMap.addBidirectionalLink("C", "D", 7.0);
            aMap.addUnidirectionalLink("B", "E", 14.0);

            actionsFn = MapFunctions.createActionsFunction(aMap);
            resultFn = MapFunctions.createResultFunction();
            stepCostFn = MapFunctions.createDistanceStepCostFunction(aMap);
        }

        [TestMethod]
        public void testSuccessors()
        {
            ICollection<string> locations = CollectionFactory.CreateQueue<string>();

            // A
            locations.Clear();
            locations.Add("B");
            locations.Add("C");
            foreach (MoveToAction a in actionsFn.apply("A"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn.apply("A", a)));
            }

            // B
            locations.Clear();
            locations.Add("A");
            locations.Add("C");
            locations.Add("E");
            foreach (MoveToAction a in actionsFn.apply("B"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn.apply("B", a)));
            }

            // C
            locations.Clear();
            locations.Add("A");
            locations.Add("B");
            locations.Add("D");
            foreach (MoveToAction a in actionsFn.apply("C"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn.apply("C", a)));
            }

            // D
            locations.Clear();
            locations.Add("C");
            foreach (MoveToAction a in actionsFn.apply("D"))
            {
                Assert.IsTrue(locations.Contains(a.getToLocation()));
                Assert.IsTrue(locations.Contains(resultFn.apply("D", a)));
            }
            // E
            locations.Clear();
            Assert.IsTrue(0 == actionsFn.apply("E").Size());
        }

        [TestMethod]
        public void testCosts()
        {
            Assert.AreEqual(5.0, stepCostFn.applyAsDouble("A", new MoveToAction("B"), "B"), 0.001);
            Assert.AreEqual(6.0, stepCostFn.applyAsDouble("A", new MoveToAction("C"), "C"), 0.001);
            Assert.AreEqual(4.0, stepCostFn.applyAsDouble("B", new MoveToAction("C"), "C"), 0.001);
            Assert.AreEqual(7.0, stepCostFn.applyAsDouble("C", new MoveToAction("D"), "D"), 0.001);
            Assert.AreEqual(14.0, stepCostFn.applyAsDouble("B", new MoveToAction("E"), "E"), 0.001);
            //
            Assert.AreEqual(5.0, stepCostFn.applyAsDouble("B", new MoveToAction("A"), "A"), 0.001);
            Assert.AreEqual(6.0, stepCostFn.applyAsDouble("C", new MoveToAction("A"), "A"), 0.001);
            Assert.AreEqual(4.0, stepCostFn.applyAsDouble("C", new MoveToAction("B"), "B"), 0.001);
            Assert.AreEqual(7.0, stepCostFn.applyAsDouble("D", new MoveToAction("C"), "C"), 0.001);
            //
            Assert.AreEqual(1.0, stepCostFn.applyAsDouble("X", new MoveToAction("Z"), "Z"), 0.001);
            Assert.AreEqual(1.0, stepCostFn.applyAsDouble("A", new MoveToAction("Z"), "Z"), 0.001);
            Assert.AreEqual(1.0, stepCostFn.applyAsDouble("A", new MoveToAction("D"), "D"), 0.001);
            Assert.AreEqual(1.0, stepCostFn.applyAsDouble("A", new MoveToAction("B"), "E"), 0.001);
        }
    } 
}
