using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing;
using aima.net.logic.propositional.visitors;

namespace aima.net.test.experiment.logic.propositional.algorithms
{
    [TestClass]
    public class WalkSATExperiment
    {
        private PLParser parser = new PLParser();

        // NOT REALLY A JUNIT TESTCASE BUT written as one to allow easy execution
        [TestMethod]
        public void testWalkSat()
        {
            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(parser.parse("A & B"))
                    .getClauses(), 0.5, 1000);
            if (m == null)
            {
                System.Console.WriteLine("failure");
            }
            else
            {
                m.print();
            }
        }

        [TestMethod]
        public void testWalkSat2()
        {
            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(parser.parse("A & ~B"))
                    .getClauses(), 0.5, 1000);
            if (m == null)
            {
                System.Console.WriteLine("failure");
            }
            else
            {
                m.print();
            }
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
            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(kb.asSentence())
                    .getClauses(), 0.5, 1000);
            if (m == null)
            {
                System.Console.WriteLine("failure");
            }
            else
            {
                m.print();
            }
        }
    }

}
