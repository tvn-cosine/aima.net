using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent;
using aima.net.agent.api;

namespace aima.net.test.unit.agent
{
    [TestClass]
    public class DynamicActionTest
    {
        [TestMethod]
        public void TestInitialisation()
        {
            DynamicAction action = new DynamicAction("test");

            Assert.AreEqual("test", action.GetName());
            Assert.AreEqual(DynamicAction.TYPE, action.DescribeType());
            Assert.IsFalse(action.IsNoOp());
            Assert.IsInstanceOfType(action, typeof(IAction));
        }

        [TestMethod]
        public void TestNoOp()
        {
            Assert.IsNotNull(DynamicAction.NO_OP);
            Assert.AreEqual("NoOp", DynamicAction.NO_OP.GetName());
            Assert.AreEqual(DynamicAction.TYPE, DynamicAction.NO_OP.DescribeType());
            Assert.IsTrue(DynamicAction.NO_OP.IsNoOp());
        }
    }
}
