using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.util;

namespace aima.net.test.unit.util
{
    /**
     * @author Ravi Mohan
     * 
     */
    [TestClass]
    public class SetOpsTest
    {
        ISet<int> s1, s2;

        [TestInitialize]
        public void setUp()
        {
            s1 = CollectionFactory.CreateSet<int>();
            s1.Add(1);
            s1.Add(2);
            s1.Add(3);
            s1.Add(4);

            s2 = CollectionFactory.CreateSet<int>();
            s2.Add(4);
            s2.Add(5);
            s2.Add(6);
        }

        [TestMethod]
        public void testUnion()
        {
            ISet<int> union = SetOps.union(s1, s2);
            Assert.AreEqual(6, union.Size());
            Assert.AreEqual(4, s1.Size());
            Assert.AreEqual(3, s2.Size());

            s1.Remove(1);
            Assert.AreEqual(6, union.Size());
            Assert.AreEqual(3, s1.Size());
            Assert.AreEqual(3, s2.Size());
        }

        [TestMethod]
        public void testIntersection()
        {
            ISet<int> intersection = SetOps.intersection(s1, s2);
            Assert.AreEqual(1, intersection.Size());
            Assert.AreEqual(4, s1.Size());
            Assert.AreEqual(3, s2.Size());

            s1.Remove(1);
            Assert.AreEqual(1, intersection.Size());
            Assert.AreEqual(3, s1.Size());
            Assert.AreEqual(3, s2.Size());
        }

        [TestMethod]
        public void testDifference()
        {
            ISet<int> difference = SetOps.difference(s1, s2);
            Assert.AreEqual(3, difference.Size());
            Assert.IsTrue(difference.Contains(1));
            Assert.IsTrue(difference.Contains(2));
            Assert.IsTrue(difference.Contains(3));
        }

        [TestMethod]
        public void testDifference2()
        {
            ISet<int> one = CollectionFactory.CreateSet<int>();
            ISet<int> two = CollectionFactory.CreateSet<int>();
            one.Add(1);
            two.Add(1);
            ISet<int> difference = SetOps.difference(one, two);
            Assert.IsTrue(difference.IsEmpty());
        }
    }
}
