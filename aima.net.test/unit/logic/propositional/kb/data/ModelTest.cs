using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.test.unit.logic.propositional.kb.data
{
    [TestClass] public class ModelTest
    {
        private Model m;

        private PLParser parser;

        Sentence trueSentence, falseSentence, andSentence, orSentence,
                impliedSentence, biConditionalSentence;

        [TestInitialize]
        public void setUp()
        {
            parser = new PLParser();
            trueSentence = (Sentence)parser.parse("true");
            falseSentence = (Sentence)parser.parse("false");
            andSentence = (Sentence)parser.parse("(P  &  Q)");
            orSentence = (Sentence)parser.parse("(P  |  Q)");
            impliedSentence = (Sentence)parser.parse("(P  =>  Q)");
            biConditionalSentence = (Sentence)parser.parse("(P  <=>  Q)");
            m = new Model();
        }

        [TestMethod]
        public void testEmptyModel()
        {
            Assert.AreEqual(null, m.getValue(new PropositionSymbol("P")));
            Assert.AreEqual(true, m.isUnknown(new PropositionSymbol("P")));
        }

        [TestMethod]
        public void testExtendModel()
        {
            string p = "P";
            m = m.union(new PropositionSymbol(p), true);
            Assert.AreEqual(true, m.getValue(new PropositionSymbol("P")));
        }

        [TestMethod]
        public void testTrueFalseEvaluation()
        {
            Assert.AreEqual(true, m.isTrue(trueSentence));
            Assert.AreEqual(false, m.isFalse(trueSentence));
            Assert.AreEqual(false, m.isTrue(falseSentence));
            Assert.AreEqual(true, m.isFalse(falseSentence));
        }

        [TestMethod]
        public void testSentenceStatusWhenPTrueAndQTrue()
        {
            string p = "P";
            string q = "Q";
            m = m.union(new PropositionSymbol(p), true);
            m = m.union(new PropositionSymbol(q), true);
            Assert.AreEqual(true, m.isTrue(andSentence));
            Assert.AreEqual(true, m.isTrue(orSentence));
            Assert.AreEqual(true, m.isTrue(impliedSentence));
            Assert.AreEqual(true, m.isTrue(biConditionalSentence));
        }

        [TestMethod]
        public void testSentenceStatusWhenPFalseAndQFalse()
        {
            string p = "P";
            string q = "Q";
            m = m.union(new PropositionSymbol(p), false);
            m = m.union(new PropositionSymbol(q), false);
            Assert.AreEqual(true, m.isFalse(andSentence));
            Assert.AreEqual(true, m.isFalse(orSentence));
            Assert.AreEqual(true, m.isTrue(impliedSentence));
            Assert.AreEqual(true, m.isTrue(biConditionalSentence));
        }

        [TestMethod]
        public void testSentenceStatusWhenPTrueAndQFalse()
        {
            string p = "P";
            string q = "Q";
            m = m.union(new PropositionSymbol(p), true);
            m = m.union(new PropositionSymbol(q), false);
            Assert.AreEqual(true, m.isFalse(andSentence));
            Assert.AreEqual(true, m.isTrue(orSentence));
            Assert.AreEqual(true, m.isFalse(impliedSentence));
            Assert.AreEqual(true, m.isFalse(biConditionalSentence));
        }

        [TestMethod]
        public void testSentenceStatusWhenPFalseAndQTrue()
        {
            string p = "P";
            string q = "Q";
            m = m.union(new PropositionSymbol(p), false);
            m = m.union(new PropositionSymbol(q), true);
            Assert.AreEqual(true, m.isFalse(andSentence));
            Assert.AreEqual(true, m.isTrue(orSentence));
            Assert.AreEqual(true, m.isTrue(impliedSentence));
            Assert.AreEqual(true, m.isFalse(biConditionalSentence));
        }

        [TestMethod]
        public void testComplexSentence()
        {
            string p = "P";
            string q = "Q";
            m = m.union(new PropositionSymbol(p), true);
            m = m.union(new PropositionSymbol(q), false);
            Sentence sent = (Sentence)parser.parse("((P | Q) &  (P => Q))");
            Assert.IsFalse(m.isTrue(sent));
            Assert.IsTrue(m.isFalse(sent));
            Sentence sent2 = (Sentence)parser.parse("((P | Q) & (Q))");
            Assert.IsFalse(m.isTrue(sent2));
            Assert.IsTrue(m.isFalse(sent2));
        }
    }
}
