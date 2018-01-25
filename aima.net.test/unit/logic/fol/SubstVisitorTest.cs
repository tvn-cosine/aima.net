using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.test.unit.logic.fol
{
    [TestClass] public class SubstVisitorTest
    {

        private FOLParser parser;
        private SubstVisitor sv;

        [TestInitialize]
        public void setUp()
        {
            parser = new FOLParser(DomainFactory.crusadesDomain());
            sv = new SubstVisitor();
        }

        [TestMethod]
        public void testSubstSingleVariableSucceedsWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x)");
            Sentence expectedAfterSubst = parser.parse(" King(John) ");
            Sentence expectedAfterSubstCopy = expectedAfterSubst.copy();

            Assert.AreEqual(expectedAfterSubst, expectedAfterSubstCopy);
            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x)"));
        }

        [TestMethod]
        public void testSubstSingleVariableFailsWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x)");
            Sentence expectedAfterSubst = parser.parse(" King(x) ");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("y"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x)"));
        }

        [TestMethod]
        public void testMultipleVariableSubstitutionWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x,y)");
            Sentence expectedAfterSubst = parser.parse(" King(John ,England) ");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));
            p.Put(new Variable("y"), new Constant("England"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x,y)"));
        }

        [TestMethod]
        public void testMultipleVariablePartiallySucceedsWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x,y)");
            Sentence expectedAfterSubst = parser.parse(" King(John ,y) ");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));
            p.Put(new Variable("z"), new Constant("England"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x,y)"));
        }

        [TestMethod]
        public void testSubstSingleVariableSucceedsWithTermEquality()
        {
            Sentence beforeSubst = parser.parse("BrotherOf(x) = EnemyOf(y)");
            Sentence expectedAfterSubst = parser
                    .parse("BrotherOf(John) = EnemyOf(Saladin)");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));
            p.Put(new Variable("y"), new Constant("Saladin"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst,
                    parser.parse("BrotherOf(x) = EnemyOf(y)"));
        }

        [TestMethod]
        public void testSubstSingleVariableSucceedsWithTermEquality2()
        {
            Sentence beforeSubst = parser.parse("BrotherOf(John) = x)");
            Sentence expectedAfterSubst = parser.parse("BrotherOf(John) = Richard");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("Richard"));
            p.Put(new Variable("y"), new Constant("Saladin"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("BrotherOf(John) = x)"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithUniversalQuantifierAndSngleVariable()
        {
            Sentence beforeSubst = parser.parse("FORALL x King(x))");
            Sentence expectedAfterSubst = parser.parse("King(John)");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("FORALL x King(x))"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithUniversalQuantifierAndZeroVariablesMatched()
        {
            Sentence beforeSubst = parser.parse("FORALL x King(x))");
            Sentence expectedAfterSubst = parser.parse("FORALL x King(x)");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("y"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("FORALL x King(x))"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithUniversalQuantifierAndOneOfTwoVariablesMatched()
        {
            Sentence beforeSubst = parser.parse("FORALL x,y King(x,y))");
            Sentence expectedAfterSubst = parser.parse("FORALL x King(x,John)");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("y"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("FORALL x,y King(x,y))"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithExistentialQuantifierAndSngleVariable()
        {
            Sentence beforeSubst = parser.parse("EXISTS x King(x))");
            Sentence expectedAfterSubst = parser.parse("King(John)");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);

            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("EXISTS x King(x)"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithNOTSentenceAndSngleVariable()
        {
            Sentence beforeSubst = parser.parse("NOT King(x))");
            Sentence expectedAfterSubst = parser.parse("NOT King(John)");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("NOT King(x))"), beforeSubst);
        }

        [TestMethod]
        public void testConnectiveANDSentenceAndSngleVariable()
        {
            Sentence beforeSubst = parser
                    .parse("EXISTS x ( King(x) AND BrotherOf(x) = EnemyOf(y) )");
            Sentence expectedAfterSubst = parser
                    .parse("( King(John) AND BrotherOf(John) = EnemyOf(Saladin) )");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));
            p.Put(new Variable("y"), new Constant("Saladin"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser
                    .parse("EXISTS x ( King(x) AND BrotherOf(x) = EnemyOf(y) )"),
                    beforeSubst);
        }

        [TestMethod]
        public void testParanthisedSingleVariable()
        {
            Sentence beforeSubst = parser.parse("((( King(x))))");
            Sentence expectedAfterSubst = parser.parse("King(John) ");

            IMap<Variable, Term> p = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            p.Put(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("((( King(x))))"), beforeSubst);
        }
    }
}
