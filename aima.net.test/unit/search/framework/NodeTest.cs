using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections.api;
using aima.net.search.framework;

namespace aima.net.test.unit.search.framework
{
    [TestClass]
    public class NodeTest
    {

        [TestMethod]
        public void testRootNode()
        {
            Node<string, string> node1 = new Node<string, string>("state1");
            Assert.IsTrue(node1.isRootNode());
            Node<string, string> node2 = new Node<string, string>("state2", node1, null, 1.0);
            Assert.IsTrue(node1.isRootNode());
            Assert.IsFalse(node2.isRootNode());
            Assert.AreEqual(node1, node2.getParent());
        }

        [TestMethod]
        public void testGetPathFromRoot()
        {
            Node<string, string> node1 = new Node<string, string>("state1");
            Node<string, string> node2 = new Node<string, string>("state2", node1, null, 1.0);
            Node<string, string> node3 = new Node<string, string>("state3", node2, null, 2.0);
            ICollection<Node<string, string>> path = SearchUtils.getPathFromRoot(node3);
            Assert.AreEqual(node1, path.Get(0));
            Assert.AreEqual(node2, path.Get(1));
            Assert.AreEqual(node3, path.Get(2));
        }
    }

}
