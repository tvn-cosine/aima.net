using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.exceptions;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.test.unit.logic.propositional.parsing
{
    [TestClass]
    public class PropositionSymbolTest
    {

        [TestMethod]
        public void test_isAlwaysTrueSymbol()
        {
            Assert.IsTrue(PropositionSymbol.isAlwaysTrueSymbol("True"));
            Assert.IsTrue(PropositionSymbol.isAlwaysTrueSymbol("tRue"));
            Assert.IsTrue(PropositionSymbol.isAlwaysTrueSymbol("trUe"));
            Assert.IsTrue(PropositionSymbol.isAlwaysTrueSymbol("truE"));
            Assert.IsTrue(PropositionSymbol.isAlwaysTrueSymbol("TRUE"));
            Assert.IsTrue(PropositionSymbol.isAlwaysTrueSymbol("true"));
            //
            Assert.IsFalse(PropositionSymbol.isAlwaysTrueSymbol("Tru3"));
            Assert.IsFalse(PropositionSymbol.isAlwaysTrueSymbol("True "));
            Assert.IsFalse(PropositionSymbol.isAlwaysTrueSymbol(" True"));
        }

        [TestMethod]
        public void test_isAlwaysFalseSymbol()
        {
            Assert.IsTrue(PropositionSymbol.isAlwaysFalseSymbol("False"));
            Assert.IsTrue(PropositionSymbol.isAlwaysFalseSymbol("fAlse"));
            Assert.IsTrue(PropositionSymbol.isAlwaysFalseSymbol("faLse"));
            Assert.IsTrue(PropositionSymbol.isAlwaysFalseSymbol("falSe"));
            Assert.IsTrue(PropositionSymbol.isAlwaysFalseSymbol("falsE"));
            Assert.IsTrue(PropositionSymbol.isAlwaysFalseSymbol("FALSE"));
            Assert.IsTrue(PropositionSymbol.isAlwaysFalseSymbol("false"));
            //
            Assert.IsFalse(PropositionSymbol.isAlwaysFalseSymbol("Fals3"));
            Assert.IsFalse(PropositionSymbol.isAlwaysFalseSymbol("False "));
            Assert.IsFalse(PropositionSymbol.isAlwaysFalseSymbol(" False"));
        }

        [TestMethod]
        public void test_isPropositionSymbol()
        {
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("True"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("False"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("A"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("A1"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("A_1"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("a"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("a1"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("A_1"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("_"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("_1"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("_1_2"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("$"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("$1"));
            Assert.IsTrue(PropositionSymbol.isPropositionSymbol("$1_1"));

            // Commas not allowed (only legal java identifier characters).
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A1,2"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol(" A"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A "));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A B"));
        }

        [TestMethod]
        public void test_isPropositionSymbolDoesNotContainConnectiveChars()
        {
            // '~', '&', '|', '=', '<', '>'
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("~"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("&"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("|"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("="));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("<"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol(">"));

            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A~"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A&"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A|"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A="));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A<"));
            Assert.IsFalse(PropositionSymbol.isPropositionSymbol("A>"));
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void test_IllegalArgumentOnConstruction()
        {
            new PropositionSymbol("A_1,2");
        }
    }

}
