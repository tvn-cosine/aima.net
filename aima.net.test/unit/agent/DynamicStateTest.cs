using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent;
using aima.net.agent.api;

namespace aima.net.test.unit.agent
{
    [TestClass]
    public class DynamicStateTest
    {
        [TestMethod]
        public void TestInitialisation()
        {
            DynamicState state = new DynamicState();

            Assert.IsInstanceOfType(state, typeof(IState));
            Assert.AreEqual(DynamicState.TYPE, state.DescribeType());
        }
    }
}
