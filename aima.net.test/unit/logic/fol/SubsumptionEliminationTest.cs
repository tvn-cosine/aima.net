using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.test.unit.logic.fol
{
    [TestClass] public class SubsumptionEliminationTest
    { 
        [TestMethod]
        public void testFindSubsumedClauses()
        {
            // Taken from AIMA2e pg 679
            FOLDomain domain = new FOLDomain();
            domain.addPredicate("patrons");
            domain.addPredicate("hungry");
            domain.addPredicate("type");
            domain.addPredicate("fri_sat");
            domain.addPredicate("will_wait");
            domain.addConstant("Some");
            domain.addConstant("Full");
            domain.addConstant("French");
            domain.addConstant("Thai");
            domain.addConstant("Burger");
            FOLParser parser = new FOLParser(domain);

            string c1 = "patrons(v,Some)";
            string c2 = "patrons(v,Full) AND (hungry(v) AND type(v,French))";
            string c3 = "patrons(v,Full) AND (hungry(v) AND (type(v,Thai) AND fri_sat(v)))";
            string c4 = "patrons(v,Full) AND (hungry(v) AND type(v,Burger))";
            string sh = "FORALL v (will_wait(v) <=> (" + c1 + " OR (" + c2
                    + " OR (" + c3 + " OR (" + c4 + ")))))";

            Sentence s = parser.parse(sh);

            CNFConverter cnfConv = new CNFConverter(parser);

            CNF cnf = cnfConv.convertToCNF(s);

            // Contains 9 duplicates
            Assert.AreEqual(40, cnf.getNumberOfClauses());

            ISet<Clause> clauses = CollectionFactory.CreateSet<Clause>(cnf.getConjunctionOfClauses());

            // duplicates removed
            Assert.AreEqual(31, clauses.Size());

            clauses.RemoveAll(SubsumptionElimination.findSubsumedClauses(clauses));

            // subsumed clauses removed
            Assert.AreEqual(8, clauses.Size());

            // Ensure only the 8 correct/expected clauses remain
            Clause cl1 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(will_wait(v)) OR (patrons(v,Full) OR patrons(v,Some)))"))
                    .getConjunctionOfClauses().Get(0);
            Clause cl2 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(will_wait(v)) OR (hungry(v) OR patrons(v,Some)))"))
                    .getConjunctionOfClauses().Get(0);
            Clause cl3 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(will_wait(v)) OR (patrons(v,Some) OR (type(v,Burger) OR (type(v,French) OR type(v,Thai)))))"))
                    .getConjunctionOfClauses().Get(0);
            Clause cl4 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(will_wait(v)) OR (fri_sat(v) OR (patrons(v,Some) OR (type(v,Burger) OR type(v,French)))))"))
                    .getConjunctionOfClauses().Get(0);
            Clause cl5 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(patrons(v,Some)) OR will_wait(v))"))
                    .getConjunctionOfClauses().Get(0);
            Clause cl6 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(hungry(v)) OR (NOT(patrons(v,Full)) OR (NOT(type(v,French)) OR will_wait(v))))"))
                    .getConjunctionOfClauses().Get(0);
            Clause cl7 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(fri_sat(v)) OR (NOT(hungry(v)) OR (NOT(patrons(v,Full)) OR (NOT(type(v,Thai)) OR will_wait(v)))))"))
                    .getConjunctionOfClauses().Get(0);
            Clause cl8 = cnfConv
                    .convertToCNF(
                            parser.parse("(NOT(hungry(v)) OR (NOT(patrons(v,Full)) OR (NOT(type(v,Burger)) OR will_wait(v))))"))
                    .getConjunctionOfClauses().Get(0);

            Assert.IsTrue(clauses.Contains(cl1));
            Assert.IsTrue(clauses.Contains(cl2));
            Assert.IsTrue(clauses.Contains(cl3));
            Assert.IsTrue(clauses.Contains(cl4));
            Assert.IsTrue(clauses.Contains(cl5));
            Assert.IsTrue(clauses.Contains(cl6));
            Assert.IsTrue(clauses.Contains(cl7));
            Assert.IsTrue(clauses.Contains(cl8));
        }
    }

}
