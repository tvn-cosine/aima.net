using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.domain;
using aima.net.probability.util;

namespace aima.net.test.unit.probability.util
{
    [TestClass]
    public class ProbUtilTest
    {
        [TestMethod]
        public void test_indexOf()
        {
            RandVar X = new RandVar("X", new BooleanDomain());
            RandVar Y = new RandVar("Y", new ArbitraryTokenDomain("A", "B", "C"));
            RandVar Z = new RandVar("Z", new BooleanDomain());

            // An ordered X,Y,Z enumeration of values should look like:
            // 00: true, A, true
            // 01: true, A, false
            // 02: true, B, true
            // 03: true, B, false
            // 04: true, C, true
            // 05: true, C, false
            // 06: false, A, true
            // 07: false, A, false
            // 08: false, B, true
            // 09: false, B, false
            // 10: false, C, true
            // 11: false, C, false
            IRandomVariable[] vars = new IRandomVariable[] { X, Y, Z };
            IMap<IRandomVariable, object> even = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();


            even.Put(X, true);

            even.Put(Y, "A");

            even.Put(Z, true);
            Assert.AreEqual(0, ProbUtil.indexOf(vars, even));

            even.Put(Z, false);
            Assert.AreEqual(1, ProbUtil.indexOf(vars, even));

            even.Put(Y, "B");

            even.Put(Z, true);
            Assert.AreEqual(2, ProbUtil.indexOf(vars, even));

            even.Put(Z, false);
            Assert.AreEqual(3, ProbUtil.indexOf(vars, even));

            even.Put(Y, "C");

            even.Put(Z, true);
            Assert.AreEqual(4, ProbUtil.indexOf(vars, even));

            even.Put(Z, false);
            Assert.AreEqual(5, ProbUtil.indexOf(vars, even));
            //
            even.Put(X, false);

            even.Put(Y, "A");

            even.Put(Z, true);
            Assert.AreEqual(6, ProbUtil.indexOf(vars, even));

            even.Put(Z, false);
            Assert.AreEqual(7, ProbUtil.indexOf(vars, even));

            even.Put(Y, "B");

            even.Put(Z, true);
            Assert.AreEqual(8, ProbUtil.indexOf(vars, even));

            even.Put(Z, false);
            Assert.AreEqual(9, ProbUtil.indexOf(vars, even));

            even.Put(Y, "C");

            even.Put(Z, true);
            Assert.AreEqual(10, ProbUtil.indexOf(vars, even));

            even.Put(Z, false);
            Assert.AreEqual(11, ProbUtil.indexOf(vars, even));
        }

        [TestMethod]
        public void test_indexesOfValue()
        {
            RandVar X = new RandVar("X", new BooleanDomain());
            RandVar Y = new RandVar("Y", new ArbitraryTokenDomain("A", "B", "C"));
            RandVar Z = new RandVar("Z", new BooleanDomain());

            // An ordered X,Y,Z enumeration of values should look like:
            // 00: true, A, true
            // 01: true, A, false
            // 02: true, B, true
            // 03: true, B, false
            // 04: true, C, true
            // 05: true, C, false
            // 06: false, A, true
            // 07: false, A, false
            // 08: false, B, true
            // 09: false, B, false
            // 10: false, C, true
            // 11: false, C, false
            IRandomVariable[] vars = new IRandomVariable[] { X, Y, Z };
            IMap<IRandomVariable, object> even = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();


            even.Put(X, true);
            CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5 },
                    ProbUtil.indexesOfValue(vars, 0, even));

            even.Put(X, false);
            CollectionAssert.AreEqual(new int[] { 6, 7, 8, 9, 10, 11 },
                    ProbUtil.indexesOfValue(vars, 0, even));


            even.Put(Y, "A");
            CollectionAssert.AreEqual(new int[] { 0, 1, 6, 7 },
                    ProbUtil.indexesOfValue(vars, 1, even));

            even.Put(Y, "B");
            CollectionAssert.AreEqual(new int[] { 2, 3, 8, 9 },
                    ProbUtil.indexesOfValue(vars, 1, even));

            even.Put(Y, "C");
            CollectionAssert.AreEqual(new int[] { 4, 5, 10, 11 },
                    ProbUtil.indexesOfValue(vars, 1, even));


            even.Put(Z, true);
            CollectionAssert.AreEqual(new int[] { 0, 2, 4, 6, 8, 10 },
                    ProbUtil.indexesOfValue(vars, 2, even));

            even.Put(Z, false);
            CollectionAssert.AreEqual(new int[] { 1, 3, 5, 7, 9, 11 },
                    ProbUtil.indexesOfValue(vars, 2, even));
        }

        [TestMethod]
        public void test_randomVariableName()
        {
            string[] names = new[] { "B\ta\t\nf", "B___\n?", null, "a ", " b", "_A1",   "Aa \tb c d e", "12asb", "33", "-A\t\b", "-_-" };
            foreach (string name in names)
            {
                try
                {
                    ProbUtil.checkValidRandomVariableName(name);
                    Assert.Fail("Invalid name string not caught!");
                }
                catch (Exception)
                { }
            }
            ProbUtil.checkValidRandomVariableName("A");
            ProbUtil.checkValidRandomVariableName("A1");
            ProbUtil.checkValidRandomVariableName("A1_2");
            ProbUtil.checkValidRandomVariableName("A_a");
        }
    } 
}
