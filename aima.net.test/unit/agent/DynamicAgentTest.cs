using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent;
using aima.net.agent.api;

namespace aima.net.test.unit.agent
{
    [TestClass]
    public class DynamicAgentTest
    {
        [TestMethod]
        public void TestNullAgentProgram()
        {
            DynamicAgent agent = new DynamicAgent();

            Assert.AreEqual(DynamicAction.NO_OP, agent.Execute(null));
            Assert.IsTrue(agent.IsAlive());
            agent.SetAlive(false);
            Assert.IsFalse(agent.IsAlive());
            Assert.IsInstanceOfType(agent, typeof(IAgent));
        }
    }
}
