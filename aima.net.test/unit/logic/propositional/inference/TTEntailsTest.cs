using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.test.unit.logic.propositional.inference
{
    [TestClass]
    public class TTEntailsTest
    {
        TTEntails tte;

        KnowledgeBase kb;

        [TestInitialize]
        public void setUp()
        {
            tte = new TTEntails();
            kb = new KnowledgeBase();
        }

        [TestMethod]
        public void testSimpleSentence1()
        {
            kb.tell("A & B");
            Assert.AreEqual(true, kb.askWithTTEntails("A"));
        }

        [TestMethod]
        public void testSimpleSentence2()
        {
            kb.tell("A | B");
            Assert.AreEqual(false, kb.askWithTTEntails("A"));
        }

        [TestMethod]
        public void testSimpleSentence3()
        {
            kb.tell("(A => B) & A");
            Assert.AreEqual(true, kb.askWithTTEntails("B"));
        }

        [TestMethod]
        public void testSimpleSentence4()
        {
            kb.tell("(A => B) & B");
            Assert.AreEqual(false, kb.askWithTTEntails("A"));
        }

        [TestMethod]
        public void testSimpleSentence5()
        {
            kb.tell("A");
            Assert.AreEqual(false, kb.askWithTTEntails("~A"));
        }

        [TestMethod]
        public void testSUnkownSymbol()
        {
            kb.tell("(A => B) & B");
            Assert.AreEqual(false, kb.askWithTTEntails("X"));
        }

        [TestMethod]
        public void testSimpleSentence6()
        {
            kb.tell("~A");
            Assert.AreEqual(false, kb.askWithTTEntails("A"));
        }

        [TestMethod]
        public void testNewAIMAExample()
        {
            kb.tell("~P11");
            kb.tell("B11 <=> P12 | P21");
            kb.tell("B21 <=> P11 | P22 | P31");
            kb.tell("~B11");
            kb.tell("B21");

            Assert.AreEqual(true, kb.askWithTTEntails("~P12"));
            Assert.AreEqual(false, kb.askWithTTEntails("P22"));
        }

        [TestMethod]
        public void testTTEntailsSucceedsWithChadCarffsBugReport()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("B12 <=> P11 | P13 | P22 | P02");
            kb.tell("B21 <=> P20 | P22 | P31 | P11");
            kb.tell("B01 <=> P00 | P02 | P11");
            kb.tell("B10 <=> P11 | P20 | P00");
            kb.tell("~B21");
            kb.tell("~B12");
            kb.tell("B10");
            kb.tell("B01");

            Assert.IsTrue(kb.askWithTTEntails("P00"));
            Assert.IsFalse(kb.askWithTTEntails("~P00"));
        }

        [TestMethod]
        public void testDoesNotKnow()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("A");
            Assert.IsFalse(kb.askWithTTEntails("B"));
            Assert.IsFalse(kb.askWithTTEntails("~B"));
        }

        public void testTTEntailsSucceedsWithCStackOverFlowBugReport()
        {
            KnowledgeBase kb = new KnowledgeBase();

            Assert.IsTrue(kb.askWithTTEntails("((A | (~ A)) & (A | B))"));
        }

        [TestMethod]
        public void testModelEvaluation()
        {
            kb.tell("~P11");
            kb.tell("B11 <=> P12 | P21");
            kb.tell("B21 <=> P11 | P22 | P31");
            kb.tell("~B11");
            kb.tell("B21");

            Model model = new Model();
            model = model.union(new PropositionSymbol("B11"), false);
            model = model.union(new PropositionSymbol("B21"), true);
            model = model.union(new PropositionSymbol("P11"), false);
            model = model.union(new PropositionSymbol("P12"), false);
            model = model.union(new PropositionSymbol("P21"), false);
            model = model.union(new PropositionSymbol("P22"), false);
            model = model.union(new PropositionSymbol("P31"), true);

            Sentence kbs = kb.asSentence();
            Assert.AreEqual(true, model.isTrue(kbs));
        }
    }
}
