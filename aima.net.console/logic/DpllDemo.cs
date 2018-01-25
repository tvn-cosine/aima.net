using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.parsing;

namespace aima.net.demo.logic
{
    public class DpllDemo
    {
        private static DPLLSatisfiable dpll = new DPLLSatisfiable();
         
        public static void Main(params string[] args)
        { 
            displayDPLLSatisfiableStatus("A & B");
            displayDPLLSatisfiableStatus("A & ~A");
            displayDPLLSatisfiableStatus("(A | ~A) & (A | B)");
        }

        public static void displayDPLLSatisfiableStatus(string  query)
        {
            PLParser parser = new PLParser();
            if (dpll.dpllSatisfiable(parser.parse(query)))
            {
                System.Console.WriteLine(query + " is  (DPLL) satisfiable");
            }
            else
            {
                System.Console.WriteLine(query + " is NOT (DPLL)  satisfiable");
            }
        }
    }

}
