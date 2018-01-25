using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.exceptions;
using aima.net.util.math;

namespace aima.net.test.unit.util.math
{
    [TestClass]
    public class MixedRadixNumberTest
    { 
        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidRadices()
        {
            new MixedRadixNumber(100, new int[] { 1, 0, -1 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidMaxValue()
        {
            new MixedRadixNumber(100, new int[] { 3, 3, 3 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidInitialValuesValue1()
        {
            new MixedRadixNumber(new int[] { 0, 0, 4 }, new int[] { 3, 3, 3 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidInitialValuesValue2()
        {
            new MixedRadixNumber(new int[] { 1, 2, -1 }, new int[] { 3, 3, 3 });
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testInvalidInitialValuesValue3()
        {
            new MixedRadixNumber(new int[] { 1, 2, 3, 1 }, new int[] { 3, 3, 3 });
        }

        [TestMethod]
        public void testAllowedMaxValue()
        {
            Assert.AreEqual(15, (new MixedRadixNumber(0,
                    new int[] { 2, 2, 2, 2 }).GetMaxAllowedValue()));
            Assert.AreEqual(80, (new MixedRadixNumber(0,
                    new int[] { 3, 3, 3, 3 }).GetMaxAllowedValue()));
            Assert.AreEqual(5, (new MixedRadixNumber(0, new int[] { 3, 2 })
                    .GetMaxAllowedValue()));
            Assert.AreEqual(35, (new MixedRadixNumber(0,
                    new int[] { 3, 3, 2, 2 }).GetMaxAllowedValue()));
            Assert.AreEqual(359, (new MixedRadixNumber(0, new int[] { 3, 4, 5,
                6 }).GetMaxAllowedValue()));
            Assert.AreEqual(359, (new MixedRadixNumber(0, new int[] { 6, 5, 4,
                3 }).GetMaxAllowedValue()));
            Assert.AreEqual(359,
                    (new MixedRadixNumber(new int[] { 5, 4, 3, 2 }, new int[] { 6,
                        5, 4, 3 }).GetMaxAllowedValue()));
        }

        [TestMethod]
        public void testIncrement()
        {
            MixedRadixNumber mrn = new MixedRadixNumber(0, new int[] { 3, 2 });
            int i = 0;
            while (mrn.Increment())
            {
               ++i;
            }
            Assert.AreEqual(i, mrn.GetMaxAllowedValue());
        }

        [TestMethod]
        public void testDecrement()
        {
            MixedRadixNumber mrn = new MixedRadixNumber(5, new int[] { 3, 2 });
            int i = 0;
            while (mrn.Decrement())
            {
               ++i;
            }
            Assert.AreEqual(i, mrn.GetMaxAllowedValue());
            i = 0;
            while (mrn.Increment())
            {
               ++i;
            }
            while (mrn.Decrement())
            {
                i--;
            }
            Assert.AreEqual(i, mrn.IntValue());
        }

        [TestMethod]
        public void testCurrentNumberalValue()
        {
            MixedRadixNumber mrn;
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(35, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(25, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(17, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(8, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(359, new int[] { 3, 4, 5, 6 });
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(3, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(4, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(5, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(359, new int[] { 6, 5, 4, 3 });
            Assert.AreEqual(5, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(4, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(3, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(3));
        }

        [TestMethod]
        public void testCurrentValueFor()
        {
            MixedRadixNumber mrn;
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(0, mrn.GetCurrentValueFor(new int[] { 0, 0, 0, 0 }));
            //
            mrn = new MixedRadixNumber(35, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(35,
                    mrn.GetCurrentValueFor(new int[] { 2, 2, 1, 1 }));
            //
            mrn = new MixedRadixNumber(25, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(25,
                    mrn.GetCurrentValueFor(new int[] { 1, 2, 0, 1 }));
            //
            mrn = new MixedRadixNumber(17, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(17,
                    mrn.GetCurrentValueFor(new int[] { 2, 2, 1, 0 }));
            //
            mrn = new MixedRadixNumber(8, new int[] { 3, 3, 2, 2 });
            Assert.AreEqual(8, mrn.GetCurrentValueFor(new int[] { 2, 2, 0, 0 }));
            //
            mrn = new MixedRadixNumber(359, new int[] { 3, 4, 5, 6 });
            Assert.AreEqual(359,
                    mrn.GetCurrentValueFor(new int[] { 2, 3, 4, 5 }));
            //
            mrn = new MixedRadixNumber(359, new int[] { 6, 5, 4, 3 });
            Assert.AreEqual(359,
                    mrn.GetCurrentValueFor(new int[] { 5, 4, 3, 2 }));
        }

        [TestMethod]
        public void testSetCurrentValueFor()
        {
            MixedRadixNumber mrn;
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.SetCurrentValueFor(new int[] { 0, 0, 0, 0 });
            Assert.AreEqual(0, mrn.IntValue());
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.SetCurrentValueFor(new int[] { 2, 2, 1, 1 });
            Assert.AreEqual(35, mrn.IntValue());
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.SetCurrentValueFor(new int[] { 1, 2, 0, 1 });
            Assert.AreEqual(25, mrn.IntValue());
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.SetCurrentValueFor(new int[] { 2, 2, 1, 0 });
            Assert.AreEqual(17, mrn.IntValue());
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(1, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 3, 2, 2 });
            mrn.SetCurrentValueFor(new int[] { 2, 2, 0, 0 });
            Assert.AreEqual(8, mrn.IntValue());
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(0, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 3, 4, 5, 6 });
            mrn.SetCurrentValueFor(new int[] { 2, 3, 4, 5 });
            Assert.AreEqual(359, mrn.IntValue());
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(3, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(4, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(5, mrn.GetCurrentNumeralValue(3));
            //
            mrn = new MixedRadixNumber(0, new int[] { 6, 5, 4, 3 });
            mrn.SetCurrentValueFor(new int[] { 5, 4, 3, 2 });
            Assert.AreEqual(359, mrn.IntValue());
            Assert.AreEqual(5, mrn.GetCurrentNumeralValue(0));
            Assert.AreEqual(4, mrn.GetCurrentNumeralValue(1));
            Assert.AreEqual(3, mrn.GetCurrentNumeralValue(2));
            Assert.AreEqual(2, mrn.GetCurrentNumeralValue(3));
        }
    }

}
