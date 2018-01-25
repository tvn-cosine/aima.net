using aima.net.logic.fol.domain;
using aima.net.logic.fol.inference;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.demo.logic.fol
{
    public class FOL_Demodulation : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_Demodulation();
        }

        static void fOL_Demodulation()
        {
            System.Console.WriteLine("-----------------");
            System.Console.WriteLine("Demodulation Demo");
            System.Console.WriteLine("-----------------");
            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addConstant("C");
            domain.addConstant("D");
            domain.addConstant("E");
            domain.addPredicate("P");
            domain.addFunction("F");
            domain.addFunction("G");
            domain.addFunction("H");
            domain.addFunction("J");

            FOLParser parser = new FOLParser(domain);

            Predicate expression = (Predicate)parser.parse("P(A,F(B,G(A,H(B)),C),D)");
            TermEquality assertion = (TermEquality)parser.parse("B = E");

            Demodulation demodulation = new Demodulation();
            Predicate altExpression = (Predicate)demodulation.apply(assertion, expression);

            System.Console.WriteLine("Demodulate '" + expression + "' with '" + assertion + "' to give");
            System.Console.WriteLine(altExpression.ToString());
            System.Console.WriteLine("and again to give");
            System.Console.WriteLine(demodulation.apply(assertion, altExpression).ToString());
            System.Console.WriteLine("");
        }
    }
}
