using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.exceptions;
using aima.net.nlp.parsing.grammars;

namespace aima.net.test.unit.nlp.parse
{
    [TestClass]
    public class RuleTest
    {
        Rule testR;

        [TestMethod]
        public void testStringSplitConstructor()
        {
            testR = new Rule("A,B", "a,bb,c", 0.50F);
            Assert.AreEqual(testR.lhs.Size(), 2);
            Assert.AreEqual(testR.rhs.Size(), 3);
            Assert.AreEqual(testR.lhs.Get(1), "B");
            Assert.AreEqual(testR.rhs.Get(2), "c");
        }

        [TestMethod]
        public void testStringSplitConstructorOnEmptyStrings()
        {
            testR = new Rule("", "", 0.50F);
            Assert.AreEqual(testR.lhs.Size(), 0);
            Assert.AreEqual(testR.rhs.Size(), 0);
        }

        [TestMethod]
        public void testStringSplitConstructorOnCommas()
        {
            testR = new Rule(",", ",", 0.50F);
            Assert.AreEqual(testR.lhs.Size(), 0);
            Assert.AreEqual(testR.rhs.Size(), 0);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void testStringSplitConstructorElementAccess()
        {
            testR = new Rule(",", "", 0.50F);
            testR.lhs.Get(0);
        }
    }
}
