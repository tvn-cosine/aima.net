using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent;
using aima.net.agent.api;

namespace aima.net.test.unit.agent
{
    [TestClass]
    public class DynamicEnvironmentStateTest
    {
        [TestMethod]
        public void TestInitialisation()
        {
            DynamicEnvironmentState state = new DynamicEnvironmentState();
            Assert.IsInstanceOfType(state, typeof(IEnvironmentState));
            Assert.AreEqual(DynamicEnvironmentState.TYPE, state.DescribeType());
        }
    }
}
