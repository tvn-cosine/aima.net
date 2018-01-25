using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.exceptions;
using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.parsing;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.test.unit.logic.propositional.inference
{
    [TestClass]
    public class PLFCEntailsTest
    {
        private PLParser parser;
        private PLFCEntails plfce;

        [TestInitialize]
        public void setUp()
        {
            parser = new PLParser();
            plfce = new PLFCEntails();
        }

        [TestMethod]
        public void testAIMAExample()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("A & P => L");
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");
            PropositionSymbol q = (PropositionSymbol)parser.parse("Q");

            Assert.AreEqual(true, plfce.plfcEntails(kb, q));
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testKBWithNonDefiniteClauses()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("~A & P => L"); // Not a definite clause
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");
            PropositionSymbol q = (PropositionSymbol)parser.parse("Q");

            Assert.AreEqual(true, plfce.plfcEntails(kb, q));
        }
    }
}
