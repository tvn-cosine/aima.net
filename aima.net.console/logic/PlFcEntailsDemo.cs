using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.demo.logic
{
    public class PlFcEntailsDemo
    {
        private static PLFCEntails plfce = new PLFCEntails();

        public static void Main(params string[] args)
        {

            System.Console.WriteLine("\nPlFcEntailsDemo\n");
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
            System.Console.WriteLine("P => Q");
            System.Console.WriteLine("L & M => P");
            System.Console.WriteLine("B & L => M");
            System.Console.WriteLine("A & P => L");
            System.Console.WriteLine("A & B => L");
            System.Console.WriteLine("A");
            System.Console.WriteLine("B");

            displayPLFCEntailment(kb, "Q");
        }

        private static void displayPLFCEntailment(KnowledgeBase kb, string q)
        {
            System.Console.WriteLine("Running PLFCEntailment on knowledge base"
                    + " with query " + q + " gives " + plfce.plfcEntails(kb, new PropositionSymbol(q)));
        }
    }

}
