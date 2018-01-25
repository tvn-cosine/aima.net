using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.inference;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;
using aima.net.util;

namespace aima.net.test.unit.logic.fol.inference
{
    [TestClass]
    public class ParamodulationTest
    {

        private Paramodulation paramodulation = null;

        [TestInitialize]
        public void setUp()
        {
            paramodulation = new Paramodulation();
        }

        // Note: Based on:
        // http://logic.stanford.edu/classes/cs157/2008/lectures/lecture15.pdf
        // Slide 31.
        [TestMethod]
        public void testSimpleExample()
        {
            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addPredicate("P");
            domain.addPredicate("Q");
            domain.addPredicate("R");
            domain.addFunction("F");

            FOLParser parser = new FOLParser(domain);

            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();
            AtomicSentence a1 = (AtomicSentence)parser.parse("P(F(x,B),x)");
            AtomicSentence a2 = (AtomicSentence)parser.parse("Q(x)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));

            Clause c1 = new Clause(lits);

            lits.Clear();
            a1 = (AtomicSentence)parser.parse("F(A,y) = y");
            a2 = (AtomicSentence)parser.parse("R(y)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));

            Clause c2 = new Clause(lits);

            ISet<Clause> paras = paramodulation.apply(c1, c2);
            Assert.AreEqual(2, paras.Size());
             
            Assert.AreEqual("[P(B,A), Q(A), R(B)]", Util.first(paras).ToString());
            paras.Remove(Util.first(paras));
            Assert.AreEqual("[P(F(A,F(x,B)),x), Q(x), R(F(x,B))]", Util.first(paras).ToString());
        }

        [TestMethod]
        public void testMultipleTermEqualitiesInBothClausesExample()
        {
            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addConstant("C");
            domain.addConstant("D");
            domain.addPredicate("P");
            domain.addPredicate("Q");
            domain.addPredicate("R");
            domain.addFunction("F");

            FOLParser parser = new FOLParser(domain);

            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();
            AtomicSentence a1 = (AtomicSentence)parser.parse("F(C,x) = D");
            AtomicSentence a2 = (AtomicSentence)parser.parse("A = D");
            AtomicSentence a3 = (AtomicSentence)parser.parse("P(F(x,B),x)");
            AtomicSentence a4 = (AtomicSentence)parser.parse("Q(x)");
            AtomicSentence a5 = (AtomicSentence)parser.parse("R(C)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));
            lits.Add(new Literal(a3));
            lits.Add(new Literal(a4));
            lits.Add(new Literal(a5));

            Clause c1 = new Clause(lits);

            lits.Clear();
            a1 = (AtomicSentence)parser.parse("F(A,y) = y");
            a2 = (AtomicSentence)parser.parse("F(B,y) = C");
            a3 = (AtomicSentence)parser.parse("R(y)");
            a4 = (AtomicSentence)parser.parse("R(A)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));
            lits.Add(new Literal(a3));
            lits.Add(new Literal(a4));

            Clause c2 = new Clause(lits);

            ISet<Clause> paras = paramodulation.apply(c1, c2);
            Assert.AreEqual(5, paras.Size());

            Assert.AreEqual(
                    "[F(B,B) = C, F(C,A) = D, A = D, P(B,A), Q(A), R(A), R(B), R(C)]",
                    Util.first(paras).ToString());
            paras.Remove(Util.first(paras));
            Assert.AreEqual(
                    "[F(A,F(C,x)) = D, F(B,F(C,x)) = C, A = D, P(F(x,B),x), Q(x), R(F(C,x)), R(A), R(C)]",
                    Util.first(paras).ToString());
            paras.Remove(Util.first(paras));
            Assert.AreEqual(
                    "[F(A,B) = B, F(C,B) = D, A = D, P(C,B), Q(B), R(A), R(B), R(C)]",
                    Util.first(paras).ToString());
            paras.Remove(Util.first(paras));
            Assert.AreEqual(
                    "[F(F(B,y),x) = D, F(A,y) = y, A = D, P(F(x,B),x), Q(x), R(y), R(A), R(C)]",
                    Util.first(paras).ToString());
            paras.Remove(Util.first(paras));
            Assert.AreEqual(
                    "[F(B,y) = C, F(C,x) = D, F(D,y) = y, P(F(x,B),x), Q(x), R(y), R(A), R(C)]",
                    Util.first(paras).ToString());
        }

        [TestMethod]
        public void testBypassReflexivityAxiom()
        {
            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addConstant("C");
            domain.addPredicate("P");
            domain.addFunction("F");

            FOLParser parser = new FOLParser(domain);

            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();
            AtomicSentence a1 = (AtomicSentence)parser.parse("P(y, F(A,y))");
            lits.Add(new Literal(a1));

            Clause c1 = new Clause(lits);

            lits.Clear();
            a1 = (AtomicSentence)parser.parse("x = x");
            lits.Add(new Literal(a1));

            Clause c2 = new Clause(lits);

            ISet<Clause> paras = paramodulation.apply(c1, c2);
            Assert.AreEqual(0, paras.Size());
        }

        [TestMethod]
        public void testNegativeTermEquality()
        {
            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addConstant("C");
            domain.addPredicate("P");
            domain.addFunction("F");

            FOLParser parser = new FOLParser(domain);

            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();
            AtomicSentence a1 = (AtomicSentence)parser.parse("P(y, F(A,y))");
            lits.Add(new Literal(a1));

            Clause c1 = new Clause(lits);

            lits.Clear();
            a1 = (AtomicSentence)parser.parse("F(x,B) = x");
            lits.Add(new Literal(a1, true));

            Clause c2 = new Clause(lits);

            ISet<Clause> paras = paramodulation.apply(c1, c2);
            Assert.AreEqual(0, paras.Size());
        }
    }

}
