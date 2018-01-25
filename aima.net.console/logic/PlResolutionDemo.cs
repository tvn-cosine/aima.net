using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.parsing;

namespace aima.net.demo.logic
{
    public class PlResolutionDemo
    {
        static PLResolution plr = new PLResolution();

        public static void Main(params string[] args)
        {
            KnowledgeBase kb = new KnowledgeBase();
            string fact = "(B11 => ~P11) & B11)";
            kb.tell(fact);
            System.Console.WriteLine("\nPlResolutionDemo\n");
            System.Console.WriteLine("adding " + fact + "to knowldegebase");
            displayResolutionResults(kb, "~B11");
        }

        static void displayResolutionResults(KnowledgeBase kb, string query)
        {
            PLParser parser = new PLParser();
            System.Console.WriteLine("Running plResolution of query " 
                + query 
                + " on knowledgeBase  gives " 
                + plr.plResolution(kb, parser.parse(query)));
        }
    }

}
