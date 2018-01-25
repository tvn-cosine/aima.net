using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.common;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.test.unit.logic.fol.parsing
{
    [TestClass]
    public class FOLParserTest
    {
        FOLLexer lexer;

        FOLParser parser;

        [TestInitialize]
        public void setUp()
        {
            FOLDomain domain = DomainFactory.crusadesDomain();

            lexer = new FOLLexer(domain);
            parser = new FOLParser(lexer);
        }

        [TestMethod]
        public void testParseSimpleVariable()
        {
            parser.setUpToParse("x");
            Term v = parser.parseVariable();
            Assert.AreEqual(v, new Variable("x"));
        }

        [TestMethod]
        public void testParseIndexedVariable()
        {
            parser.setUpToParse("x1");
            Term v = parser.parseVariable();
            Assert.AreEqual(v, new Variable("x1"));
        }


        [TestMethod]
        [ExpectedException(typeof(LexerException))]
        public void testNotAllowedParseLeadingIndexedVariable()
        {
            parser.setUpToParse("1x");
            parser.parseVariable();
        }

        [TestMethod]
        public void testParseSimpleConstant()
        {
            parser.setUpToParse("John");
            Term c = parser.parseConstant();
            Assert.AreEqual(c, new Constant("John"));
        }

        [TestMethod]
        public void testParseFunction()
        {
            parser.setUpToParse("BrotherOf(John)");
            Term f = parser.parseFunction();
            Assert.AreEqual(f, getBrotherOfFunction(new Constant("John")));
        }

        [TestMethod]
        public void testParseMultiArityFunction()
        {
            parser.setUpToParse("LegsOf(John,Saladin,Richard)");
            Term f = parser.parseFunction();
            Assert.AreEqual(f, getLegsOfFunction());
            Assert.AreEqual(3, ((Function)f).getTerms().Size());
        }

        [TestMethod]
        public void testPredicate()
        {
            // parser.setUpToParse("King(John)");
            Predicate p = (Predicate)parser.parse("King(John)");
            Assert.AreEqual(p, getKingPredicate(new Constant("John")));
        }

        [TestMethod]
        public void testTermEquality()
        {
            try
            {
                TermEquality te = (TermEquality)parser
                        .parse("BrotherOf(John) = EnemyOf(Saladin)");
                Assert.AreEqual(te, new TermEquality(
                        getBrotherOfFunction(new Constant("John")),
                        getEnemyOfFunction()));
            }
            catch (RuntimeException )
            {
                Assert.Fail("RuntimeException thrown");
            }
        }

        [TestMethod]
        public void testTermEquality2()
        {
            try
            {
                TermEquality te = (TermEquality)parser
                        .parse("BrotherOf(John) = x)");
                Assert.AreEqual(te, new TermEquality(
                        getBrotherOfFunction(new Constant("John")), new Variable(
                                "x")));
            }
            catch (RuntimeException )
            {
                Assert.Fail("RuntimeException thrown");
            }
        }

        [TestMethod]
        public void testNotSentence()
        {
            NotSentence ns = (NotSentence)parser
                    .parse("NOT BrotherOf(John) = EnemyOf(Saladin)");
            Assert.AreEqual(ns.getNegated(), new TermEquality(
                    getBrotherOfFunction(new Constant("John")),
                    getEnemyOfFunction()));
        }

        [TestMethod]
        public void testSimpleParanthizedSentence()
        {
            Sentence ps = parser.parse("(NOT King(John))");
            Assert.AreEqual(ps, new NotSentence(getKingPredicate(new Constant(
                    "John"))));
        }

        [TestMethod]
        public void testExtraParanthizedSentence()
        {
            Sentence ps = parser.parse("(((NOT King(John))))");
            Assert.AreEqual(ps, new NotSentence(getKingPredicate(new Constant(
                    "John"))));
        }

        [TestMethod]
        public void testParseComplexParanthizedSentence()
        {
            Sentence ps = parser.parse("(NOT BrotherOf(John) = EnemyOf(Saladin))");
            Assert.AreEqual(ps, new NotSentence(new TermEquality(
                    getBrotherOfFunction(new Constant("John")),
                    getEnemyOfFunction())));
        }

        [TestMethod]
        public void testParseSimpleConnectedSentence()
        {
            Sentence ps = parser.parse("(King(John) AND NOT King(Richard))");

            Assert.AreEqual(ps, new ConnectedSentence("AND",
                    getKingPredicate(new Constant("John")), new NotSentence(
                            getKingPredicate(new Constant("Richard")))));

            ps = parser.parse("(King(John) AND King(Saladin))");
            Assert.AreEqual(ps, new ConnectedSentence("AND",
                    getKingPredicate(new Constant("John")),
                    getKingPredicate(new Constant("Saladin"))));
        }

        [TestMethod]
        public void testComplexConnectedSentence1()
        {
            Sentence ps = parser
                    .parse("((King(John) AND NOT King(Richard)) OR King(Saladin))");

            Assert.AreEqual(ps, 
                new ConnectedSentence("OR",
                    new ConnectedSentence("AND", 
                        getKingPredicate(new Constant("John")), 
                        new NotSentence(getKingPredicate(new Constant("Richard")))),
                    getKingPredicate(new Constant("Saladin"))));
        }

        [TestMethod]
        public void testQuantifiedSentenceWithSingleVariable()
        {
            Sentence qs = parser.parse("FORALL x  King(x)");
            ICollection<Variable> vars = CollectionFactory.CreateQueue<Variable>();
            vars.Add(new Variable("x"));
            Assert.AreEqual(qs, new QuantifiedSentence("FORALL", vars,
                    getKingPredicate(new Variable("x"))));
        }

        [TestMethod]
        public void testQuantifiedSentenceWithTwoVariables()
        {
            Sentence qs = parser
                    .parse("EXISTS x,y  (King(x) AND BrotherOf(x) = y)");
            ICollection<Variable> vars = CollectionFactory.CreateQueue<Variable>();
            vars.Add(new Variable("x"));
            vars.Add(new Variable("y"));
            ConnectedSentence cse = new ConnectedSentence("AND",
                    getKingPredicate(new Variable("x")), new TermEquality(
                            getBrotherOfFunction(new Variable("x")), new Variable(
                                    "y")));
            Assert.AreEqual(qs, new QuantifiedSentence("EXISTS", vars, cse));
        }

        [TestMethod]
        public void testQuantifiedSentenceWithPathologicalParanthising()
        {
            Sentence qs = parser
                    .parse("(( (EXISTS x,y  (King(x) AND (BrotherOf(x) = y)) ) ))");
            ICollection<Variable> vars = CollectionFactory.CreateQueue<Variable>();
            vars.Add(new Variable("x"));
            vars.Add(new Variable("y"));
            ConnectedSentence cse = new ConnectedSentence("AND",
                    getKingPredicate(new Variable("x")), new TermEquality(
                            getBrotherOfFunction(new Variable("x")), new Variable(
                                    "y")));
            Assert.AreEqual(qs, new QuantifiedSentence("EXISTS", vars, cse));
        }

        [TestMethod]
        public void testParseMultiArityFunctionEquality()
        {
            parser.setUpToParse("LegsOf(John,Saladin,Richard)");
            Term f = parser.parseFunction();

            parser.setUpToParse("LegsOf(John,Saladin,Richard)");
            Term f2 = parser.parseFunction();
            Assert.AreEqual(f, f2);
            Assert.AreEqual(3, ((Function)f).getTerms().Size());
        }

        [TestMethod]
        public void testConnectedImplication()
        {
            parser = new FOLParser(DomainFactory.weaponsDomain());
            parser.parse("((Missile(m) AND Owns(Nono,m)) => Sells(West , m ,Nono))");
        }

        //
        // PRIVATE METHODS
        //
        private Function getBrotherOfFunction(Term t)
        {
            ICollection<Term> l = CollectionFactory.CreateQueue<Term>();
            l.Add(t);
            return new Function("BrotherOf", l);
        }

        private Function getEnemyOfFunction()
        {
            ICollection<Term> l = CollectionFactory.CreateQueue<Term>();
            l.Add(new Constant("Saladin"));
            return new Function("EnemyOf", l);
        }

        private Function getLegsOfFunction()
        {
            ICollection<Term> l = CollectionFactory.CreateQueue<Term>();
            l.Add(new Constant("John"));
            l.Add(new Constant("Saladin"));
            l.Add(new Constant("Richard"));
            return new Function("LegsOf", l);
        }

        private Predicate getKingPredicate(Term t)
        {
            ICollection<Term> l = CollectionFactory.CreateQueue<Term>();
            l.Add(t);
            return new Predicate("King", l);
        }
    }
}
