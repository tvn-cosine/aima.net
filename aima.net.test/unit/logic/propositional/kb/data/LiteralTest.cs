using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.test.unit.logic.propositional.kb.data
{
    [TestClass] public class LiteralTest
    {
        private readonly PropositionSymbol SYMBOL_P = new PropositionSymbol("P");
        private readonly PropositionSymbol SYMBOL_Q = new PropositionSymbol("Q");

        [TestMethod]
        public void testIsPositiveLiteral()
        {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsTrue(literal.isPositiveLiteral());

            literal = new Literal(SYMBOL_P, true);
            Assert.IsTrue(literal.isPositiveLiteral());

            literal = new Literal(SYMBOL_P, false);
            Assert.IsFalse(literal.isPositiveLiteral());
        }

        [TestMethod]
        public void testIsNegativeLiteral()
        {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsFalse(literal.isNegativeLiteral());

            literal = new Literal(SYMBOL_P, true);
            Assert.IsFalse(literal.isNegativeLiteral());

            literal = new Literal(SYMBOL_P, false);
            Assert.IsTrue(literal.isNegativeLiteral());
        }

        [TestMethod]
        public void testGetAtomicSentence()
        {
            Literal literal = new Literal(SYMBOL_P);
            Assert.AreEqual(literal.getAtomicSentence(), SYMBOL_P);
        }

        [TestMethod]
        public void testIsAlwaysTrue()
        {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsFalse(literal.isAlwaysTrue());

            literal = new Literal(PropositionSymbol.TRUE);
            Assert.IsTrue(literal.isAlwaysTrue());

            literal = new Literal(PropositionSymbol.TRUE, false);
            Assert.IsFalse(literal.isAlwaysTrue());

            literal = new Literal(PropositionSymbol.FALSE);
            Assert.IsFalse(literal.isAlwaysTrue());

            literal = new Literal(PropositionSymbol.FALSE, false);
            Assert.IsTrue(literal.isAlwaysTrue());
        }

        [TestMethod]
        public void testIsAlwaysFalse()
        {
            Literal literal = new Literal(SYMBOL_P);
            Assert.IsFalse(literal.isAlwaysFalse());

            literal = new Literal(PropositionSymbol.TRUE);
            Assert.IsFalse(literal.isAlwaysFalse());

            literal = new Literal(PropositionSymbol.TRUE, false);
            Assert.IsTrue(literal.isAlwaysFalse());

            literal = new Literal(PropositionSymbol.FALSE);
            Assert.IsTrue(literal.isAlwaysFalse());

            literal = new Literal(PropositionSymbol.FALSE, false);
            Assert.IsFalse(literal.isAlwaysFalse());
        }

        [TestMethod]
        public void testToString()
        {
            Literal literal = new Literal(SYMBOL_P);
            Assert.AreEqual("P", literal.ToString());

            literal = new Literal(SYMBOL_P, false);
            Assert.AreEqual("~P", literal.ToString());
        }

        [TestMethod]
        public void testEquals()
        {
            Literal literal1 = new Literal(SYMBOL_P);
            Literal literal2 = new Literal(SYMBOL_P);
            Assert.IsTrue(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P, false);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsTrue(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsFalse(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P);
            literal2 = new Literal(SYMBOL_Q);
            Assert.IsFalse(literal1.Equals(literal2));

            literal1 = new Literal(SYMBOL_P);
            Assert.IsFalse(literal1.Equals(SYMBOL_P));
        }

        [TestMethod]
        public void testHashCode()
        {
            Literal literal1 = new Literal(SYMBOL_P);
            Literal literal2 = new Literal(SYMBOL_P);
            Assert.IsTrue(literal1.GetHashCode() == literal2.GetHashCode());

            literal1 = new Literal(SYMBOL_P, false);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsTrue(literal1.GetHashCode() == literal2.GetHashCode());

            literal1 = new Literal(SYMBOL_P);
            literal2 = new Literal(SYMBOL_P, false);
            Assert.IsFalse(literal1.GetHashCode() == literal2.GetHashCode());
        }
    }

}
