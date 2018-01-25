using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent;
using aima.net.environment.map;
using aima.net.search.uninformed;

namespace aima.net.test.unit.environment.map
{
    [TestClass]
    public class MapEnvironmentTest
    {
        MapEnvironment me;

        SimpleMapAgent ma;

        [TestInitialize]
        public void setUp()
        {
            ExtendableMap aMap = new ExtendableMap();
            aMap.addBidirectionalLink("A", "B", 5.0);
            aMap.addBidirectionalLink("A", "C", 6.0);
            aMap.addBidirectionalLink("B", "C", 4.0);
            aMap.addBidirectionalLink("C", "D", 7.0);
            aMap.addUnidirectionalLink("B", "E", 14.0);

            me = new MapEnvironment(aMap);
            ma = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<string, MoveToAction>(),
                    new string[] { "A" });
        }

        [TestMethod]
        public void testAddAgent()
        {
            me.addAgent(ma, "E");
            Assert.AreEqual(me.getAgentLocation(ma), "E");
        }

        [TestMethod]
        public void testExecuteAction()
        {
            me.addAgent(ma, "D");
            me.executeAction(ma, new MoveToAction("C"));
            Assert.AreEqual(me.getAgentLocation(ma), "C");
        }

        [TestMethod]
        public void testPerceptSeenBy()
        {
            me.addAgent(ma, "D");
            DynamicPercept p = (DynamicPercept)me.getPerceptSeenBy(ma);
            Assert.AreEqual(p.GetAttribute(DynAttributeNames.PERCEPT_IN), "D");
        }

        [TestMethod]
        public void testTwoAgentsSupported()
        {
            SimpleMapAgent ma1 = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<string, MoveToAction>(),
                    new string[] { "A" });
            SimpleMapAgent ma2 = new SimpleMapAgent(me.getMap(), me, new UniformCostSearch<string, MoveToAction>(),
                    new string[] { "A" });

            me.addAgent(ma1, "A");
            me.addAgent(ma2, "A");
            me.executeAction(ma1, new MoveToAction("B"));
            me.executeAction(ma2, new MoveToAction("C"));

            Assert.AreEqual(me.getAgentLocation(ma1), "B");
            Assert.AreEqual(me.getAgentLocation(ma2), "C");
        }
    }

}
