﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.util;

namespace aima.net.test.unit.util
{
    [TestClass]
    public class UtilTest
    {
        private ICollection<double> values;

        [TestInitialize]
        public void setUp()
        {
            values = CollectionFactory.CreateQueue<double>();
            values.Add(1.0);
            values.Add(2.0);
            values.Add(3.0);
            values.Add(4.0);
            values.Add(5.0);
        }

        [TestMethod]
        public void testModeFunction()
        {
            ICollection<int> l = CollectionFactory.CreateQueue<int>();
            l.Add(1);
            l.Add(2);
            l.Add(2);
            l.Add(3);
            int i = (Util.mode(l));
            Assert.AreEqual(2, i);

            ICollection<int> l2 = CollectionFactory.CreateQueue<int>();
            l2.Add(1);
            i = (Util.mode(l2));
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void testMeanCalculation()
        {
            Assert.AreEqual(3.0, Util.calculateMean(values), 0.001);
        }

        [TestMethod]
        public void testStDevCalculation()
        {
            Assert.AreEqual(1.5811, Util.calculateStDev(values, 3.0), 0.0001);
        }

        [TestMethod]
        public void testNormalization()
        {
            ICollection<double> nrm = Util.normalizeFromMeanAndStdev(values, 3.0, 1.5811);
            Assert.AreEqual(-1.264, nrm.Get(0), 0.001);
            Assert.AreEqual(-0.632, nrm.Get(1), 0.001);
            Assert.AreEqual(0.0, nrm.Get(2), 0.001);
            Assert.AreEqual(0.632, nrm.Get(3), 0.001);
            Assert.AreEqual(1.264, nrm.Get(4), 0.001);

        }

        [TestMethod]
        public void testRandomNumberGenrationWhenStartAndEndNumbersAreSame()
        {
            int i = Util.randomNumberBetween(0, 0);
            int j = Util.randomNumberBetween(23, 23);
            Assert.AreEqual(0, i);
            Assert.AreEqual(23, j);
        }
    }

}
