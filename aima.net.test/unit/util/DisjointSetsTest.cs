using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.util;

namespace aima.net.test.unit.util
{
    [TestClass]
    public class DisjointSetsTest
    {
        protected static void assertArrayEquals<T>(ISet<T> arr1, ISet<T> arr2)
        {
            if (arr1.Size() != arr2.Size())
            {
                Assert.Fail("Two arrays not same length");
            }

            for (int i = 0; i < arr1.Size(); ++i)
            {
                Assert.AreEqual(arr1.Get(i), arr2.Get(i));
            }
        }

        [TestMethod]
        public void testConstructors()
        {
            DisjointSets<string> disjSets = new DisjointSets<string>();
            Assert.AreEqual(0, disjSets.numberDisjointSets());

            disjSets = new DisjointSets<string>("a", "a", "b");
            Assert.AreEqual(2, disjSets.numberDisjointSets());

            disjSets = new DisjointSets<string>(CollectionFactory.CreateQueue<string>(new[] { "a", "a", "b" }));
            Assert.AreEqual(2, disjSets.numberDisjointSets());
        }

        [TestMethod]
        public void testMakeSet()
        {
            DisjointSets<string> disjSets = new DisjointSets<string>();

            disjSets.makeSet("a");
            Assert.AreEqual(1, disjSets.numberDisjointSets());

            disjSets.makeSet("a");
            Assert.AreEqual(1, disjSets.numberDisjointSets());

            disjSets.makeSet("b");
            Assert.AreEqual(2, disjSets.numberDisjointSets());
        }

        [TestMethod]
        public void testUnion()
        {
            DisjointSets<string> disjSets = new DisjointSets<string>("a", "b", "c", "d");
            Assert.AreEqual(4, disjSets.numberDisjointSets());

            disjSets.union("a", "b");
            Assert.AreEqual(3, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("a"), disjSets.find("b"));

            disjSets.union("c", "d");
            Assert.AreEqual(2, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("c"), disjSets.find("d"));

            disjSets.union("b", "c");
            Assert.AreEqual(1, disjSets.numberDisjointSets());
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testUnionIllegalArgumentException1()
        {
            DisjointSets<string> disjSets = new DisjointSets<string>(
                    "a");
            disjSets.union("b", "a");
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testUnionIllegalArgumentException2()
        {
            DisjointSets<string> disjSets = new DisjointSets<string>("a");
            disjSets.union("a", "b");
        }

        /**
         * Note: This is based on the example given in Figure 21.1 of 'Introduction
         * to Algorithm 2nd Edition' (by Cormen, Leriserson, Rivest, and Stein)
         */
        [TestMethod]
        public void testWorkedExample()
        {
            // Should be the following when finished:
            // {a, b, c, d}, {e, f, g}, {h, i}, and {j}

            // 1. initial sets
            DisjointSets<string> disjSets = new DisjointSets<string>("a", "b", "c", "d", "e", "f", "g", "h", "i", "j");

            Assert.AreEqual(10, disjSets.numberDisjointSets());
            Assert.AreEqual(1, disjSets.find("a").Size());
            Assert.AreEqual(1, disjSets.find("b").Size());
            Assert.AreEqual(1, disjSets.find("c").Size());
            Assert.AreEqual(1, disjSets.find("d").Size());
            Assert.AreEqual(1, disjSets.find("e").Size());
            Assert.AreEqual(1, disjSets.find("f").Size());
            Assert.AreEqual(1, disjSets.find("g").Size());
            Assert.AreEqual(1, disjSets.find("h").Size());
            Assert.AreEqual(1, disjSets.find("i").Size());
            Assert.AreEqual(1, disjSets.find("j").Size());

            // 2. (b, d)
            disjSets.union("b", "d");
            Assert.AreEqual(9, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("b"), disjSets.find("d"));

            // 3. (e, g)
            disjSets.union("e", "g");
            Assert.AreEqual(8, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("e"), disjSets.find("g"));

            // 4. (a, c)
            disjSets.union("a", "c");
            Assert.AreEqual(7, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("a"), disjSets.find("c"));

            // 5. (h, i)
            disjSets.union("h", "i");
            Assert.AreEqual(6, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("h"), disjSets.find("i"));

            // 6. (a, b)
            disjSets.union("a", "b");
            Assert.AreEqual(5, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("a"), disjSets.find("b"));
            assertArrayEquals(disjSets.find("b"), disjSets.find("c"));
            assertArrayEquals(disjSets.find("c"), disjSets.find("d"));

            // 7. (e, f)
            disjSets.union("e", "f");
            Assert.AreEqual(4, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("e"), disjSets.find("f"));
            assertArrayEquals(disjSets.find("f"), disjSets.find("g"));

            // 8. (b, c)
            disjSets.union("b", "c");
            Assert.AreEqual(4, disjSets.numberDisjointSets());
            assertArrayEquals(disjSets.find("a"), disjSets.find("b"));
            assertArrayEquals(disjSets.find("b"), disjSets.find("c"));
            assertArrayEquals(disjSets.find("c"), disjSets.find("d"));
        }
    }

}
