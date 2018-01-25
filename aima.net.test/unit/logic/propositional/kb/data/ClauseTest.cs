using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing.ast;
using aima.net.util;

namespace aima.net.test.unit.logic.propositional.kb.data
{
    [TestClass]
    public class ClauseTest
    {
        private readonly Literal LITERAL_P = new Literal(new PropositionSymbol("P"));
        private readonly Literal LITERAL_NOT_P = new Literal(new PropositionSymbol("P"), false);
        private readonly Literal LITERAL_Q = new Literal(new PropositionSymbol("Q"));
        private readonly Literal LITERAL_NOT_Q = new Literal(new PropositionSymbol("Q"), false);
        private readonly Literal LITERAL_R = new Literal(new PropositionSymbol("R"));

        private void testClauseLiterals<T>(ISet<T> s1, ISet<T> s2)
        {
            Assert.AreEqual(s1.Size(), s2.Size());
            foreach (T c in s1)
            {
                Assert.IsTrue(s2.Contains(c));
            }
        }

        [TestMethod]
        public void testAlwaysFalseLiteralsExcludedOnConstruction()
        {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(1, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE));
            Assert.AreEqual(1, clause.getNumberLiterals());
             
            testClauseLiterals(Util.createSet(LITERAL_P), clause.getLiterals());


            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE, false));
            Assert.AreEqual(1, clause.getNumberLiterals());

            testClauseLiterals(Util.createSet(LITERAL_P), clause.getLiterals()); 

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE, false));
            Assert.AreEqual(2, clause.getNumberLiterals());

            testClauseLiterals(Util.createSet(LITERAL_P, new Literal(PropositionSymbol.FALSE, false)), 
                clause.getLiterals()); 

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE));
            Assert.AreEqual(2, clause.getNumberLiterals());

            testClauseLiterals(Util.createSet(LITERAL_P, new Literal(PropositionSymbol.TRUE)),
                clause.getLiterals()); 
        }

        [TestMethod]
        public void testIsFalse()
        {
            Clause clause = new Clause();
            Assert.IsTrue(clause.isFalse());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.isFalse());
        }

        [TestMethod]
        public void testIsEmpty()
        {
            Clause clause = new Clause();
            Assert.IsTrue(clause.isEmpty());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.isEmpty());
        }

        [TestMethod]
        public void testIsUnitClause()
        {
            Clause clause = new Clause();
            Assert.IsFalse(clause.isUnitClause());

            clause = new Clause(LITERAL_P);
            Assert.IsTrue(clause.isUnitClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.isUnitClause());
        }

        [TestMethod]
        public void testIsDefiniteClause()
        {
            Clause clause = new Clause();
            Assert.IsFalse(clause.isDefiniteClause());

            clause = new Clause(LITERAL_P);
            Assert.IsTrue(clause.isDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.isDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.isDefiniteClause());
        }

        [TestMethod]
        public void testIsImplicationDefiniteClause()
        {
            Clause clause = new Clause();
            Assert.IsFalse(clause.isImplicationDefiniteClause());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.isImplicationDefiniteClause());

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsFalse(clause.isImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.isImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.isImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.isImplicationDefiniteClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q, LITERAL_NOT_Q);
            Assert.IsFalse(clause.isImplicationDefiniteClause());
        }

        [TestMethod]
        public void testIsHornClause()
        {
            Clause clause = new Clause();
            Assert.IsFalse(clause.isHornClause());

            clause = new Clause(LITERAL_P);
            Assert.IsTrue(clause.isHornClause());

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsTrue(clause.isHornClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.isHornClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.isHornClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.isHornClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q, LITERAL_NOT_Q);
            Assert.IsFalse(clause.isHornClause());
        }

        [TestMethod]
        public void testIsGoalClause()
        {
            Clause clause = new Clause();
            Assert.IsFalse(clause.isGoalClause());

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.isGoalClause());

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsTrue(clause.isGoalClause());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsFalse(clause.isGoalClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q);
            Assert.IsFalse(clause.isGoalClause());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsFalse(clause.isGoalClause());

            clause = new Clause(LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.IsTrue(clause.isGoalClause());
        }

        [TestMethod]
        public void testIsTautology()
        {
            Clause clause = new Clause();
            Assert.IsFalse(clause.isTautology().Value);

            clause = new Clause(LITERAL_P);
            Assert.IsFalse(clause.isTautology().Value);

            clause = new Clause(LITERAL_NOT_P);
            Assert.IsFalse(clause.isTautology().Value);

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE), LITERAL_R);
            Assert.IsTrue(clause.isTautology().Value);

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE, false), LITERAL_R);
            Assert.IsTrue(clause.isTautology().Value);

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE, false), LITERAL_R);
            Assert.IsFalse(clause.isTautology().Value);

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE), LITERAL_R);
            Assert.IsFalse(clause.isTautology().Value);

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R, LITERAL_NOT_Q);
            Assert.IsTrue(clause.isTautology().Value);

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.IsFalse(clause.isTautology().Value);
        }

        [TestMethod]
        public void testGetNumberLiterals()
        {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(1, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE));
            Assert.AreEqual(1, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE, false));
            Assert.AreEqual(1, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.FALSE, false));
            Assert.AreEqual(2, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, new Literal(PropositionSymbol.TRUE));
            Assert.AreEqual(2, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, LITERAL_P);
            Assert.AreEqual(1, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, LITERAL_Q);
            Assert.AreEqual(2, clause.getNumberLiterals());

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.AreEqual(3, clause.getNumberLiterals());
        }

        [TestMethod]
        public void testGetNumberPositiveLiterals()
        {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.getNumberPositiveLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(1, clause.getNumberPositiveLiterals());

            clause = new Clause(LITERAL_NOT_P);
            Assert.AreEqual(0, clause.getNumberPositiveLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q);
            Assert.AreEqual(2, clause.getNumberPositiveLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual(2, clause.getNumberPositiveLiterals());
        }

        [TestMethod]
        public void testGetNumberNegativeLiterals()
        {
            Clause clause = new Clause();
            Assert.AreEqual(0, clause.getNumberNegativeLiterals());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual(0, clause.getNumberNegativeLiterals());

            clause = new Clause(LITERAL_NOT_P);
            Assert.AreEqual(1, clause.getNumberNegativeLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_Q);
            Assert.AreEqual(1, clause.getNumberNegativeLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual(1, clause.getNumberNegativeLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_P, LITERAL_NOT_Q);
            Assert.AreEqual(2, clause.getNumberNegativeLiterals());
        }

        [TestMethod]
        public void testGetLiterals()
        {
            Clause clause = new Clause();
            testClauseLiterals(CollectionFactory.CreateSet<Literal>(), clause.getLiterals());

            clause = new Clause(LITERAL_P);
            testClauseLiterals(Util.createSet(LITERAL_P), clause.getLiterals());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            testClauseLiterals(Util.createSet(LITERAL_P, LITERAL_NOT_Q, LITERAL_R), clause.getLiterals());
        }

        [TestMethod]
        public void testGetPositiveSymbols()
        {
            Clause clause = new Clause();
            testClauseLiterals(CollectionFactory.CreateSet<PropositionSymbol>(), clause.getPositiveSymbols());

            clause = new Clause(LITERAL_P);
            testClauseLiterals(Util.createSet(new PropositionSymbol("P")), clause.getPositiveSymbols());

            clause = new Clause(LITERAL_P, LITERAL_NOT_Q, LITERAL_R);
            testClauseLiterals(Util.createSet(new PropositionSymbol("P"), new PropositionSymbol("R")), clause.getPositiveSymbols());
        }

        [TestMethod]
        public void testGetNegativeSymbols()
        {
            Clause clause = new Clause();
            testClauseLiterals(CollectionFactory.CreateSet<PropositionSymbol>(), clause.getNegativeSymbols());

            clause = new Clause(LITERAL_NOT_P);
            testClauseLiterals(Util.createSet(new PropositionSymbol("P")), clause.getNegativeSymbols());

            clause = new Clause(LITERAL_NOT_P, LITERAL_NOT_Q, LITERAL_R);
            testClauseLiterals(Util.createSet(new PropositionSymbol("P"), new PropositionSymbol("Q")), clause.getNegativeSymbols());
        }

        [TestMethod]
        public void testToString()
        {
            Clause clause = new Clause();
            Assert.AreEqual("{}", clause.ToString());

            clause = new Clause(LITERAL_P);
            Assert.AreEqual("{P}", clause.ToString());

            clause = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.AreEqual("{P, Q, R}", clause.ToString());

            clause = new Clause(LITERAL_NOT_P, LITERAL_NOT_Q, LITERAL_R);
            Assert.AreEqual("{~P, ~Q, R}", clause.ToString());
        }

        [TestMethod]
        public void testEquals()
        {
            Clause clause1 = new Clause();
            Clause clause2 = new Clause();
            Assert.IsTrue(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_P);
            Assert.IsTrue(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsTrue(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_R, LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.IsTrue(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_Q);
            Assert.IsFalse(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_R);
            Assert.IsFalse(clause1.Equals(clause2));

            clause1 = new Clause(LITERAL_P);
            Assert.IsFalse(clause1.Equals(LITERAL_P));
        }

        [TestMethod]
        public void testHashCode()
        {
            Clause clause1 = new Clause();
            Clause clause2 = new Clause();
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_P);
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q);
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_R, LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_Q, LITERAL_R);
            Assert.IsTrue(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_P);
            clause2 = new Clause(LITERAL_Q);
            Assert.IsFalse(clause1.GetHashCode() == clause2.GetHashCode());

            clause1 = new Clause(LITERAL_P, LITERAL_Q);
            clause2 = new Clause(LITERAL_P, LITERAL_R);
            Assert.IsFalse(clause1.GetHashCode() == clause2.GetHashCode());
        }


        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void testLiteralsImmutable()
        {
            Clause clause = new Clause(LITERAL_P);
            clause.getLiterals().Add(LITERAL_Q);
        }


        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void testPostivieSymbolsImmutable()
        {
            Clause clause = new Clause(LITERAL_P);
            clause.getPositiveSymbols().Add(new PropositionSymbol("Q"));
        }


        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void testNegativeSymbolsImmutable()
        {
            Clause clause = new Clause(LITERAL_P);
            clause.getNegativeSymbols().Add(new PropositionSymbol("Q"));
        }
    } 
}
