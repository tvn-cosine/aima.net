using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.visitors;

namespace aima.net.demo.logic
{
    public class WalkSatDemo
    {
        public static void Main(params string[] args)
        {
            System.Console.WriteLine("\nWalkSatDemo\n");
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("A & P => L");
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");

            System.Console.WriteLine("Example from  page 220 of AIMA 2nd Edition");
            System.Console.WriteLine("KnowledgeBsse consists of sentences");
            System.Console.WriteLine(kb.ToString());

            WalkSAT walkSAT = new WalkSAT();
            Model m = walkSAT.walkSAT(ConvertToConjunctionOfClauses.convert(kb.asSentence()).getClauses(), 0.5, 1000);
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
