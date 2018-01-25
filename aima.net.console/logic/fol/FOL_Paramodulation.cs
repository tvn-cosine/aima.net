using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.inference;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.demo.logic.fol
{
    public class FOL_Paramodulation : FolDemoBase
    {
        static void Main(params string[] args)
        {
            fOL_Paramodulation();
        }

        static void fOL_Paramodulation()
        {
            System.Console.WriteLine("-------------------");
            System.Console.WriteLine("Paramodulation Demo");
            System.Console.WriteLine("-------------------");

            FOLDomain domain = new FOLDomain();
            domain.addConstant("A");
            domain.addConstant("B");
            domain.addPredicate("P");
            domain.addPredicate("Q");
            domain.addPredicate("R");
            domain.addFunction("F");

            FOLParser parser = new FOLParser(domain);

            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();
            AtomicSentence a1 = (AtomicSentence)parser.parse("P(F(x,B),x)");
            AtomicSentence a2 = (AtomicSentence)parser.parse("Q(x)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));

            Clause c1 = new Clause(lits);

            lits.Clear();
            a1 = (AtomicSentence)parser.parse("F(A,y) = y");
            a2 = (AtomicSentence)parser.parse("R(y)");
            lits.Add(new Literal(a1));
            lits.Add(new Literal(a2));

            Clause c2 = new Clause(lits);

            Paramodulation paramodulation = new Paramodulation();
            ISet<Clause> paras = paramodulation.apply(c1, c2);

            System.Console.WriteLine("Paramodulate '" + c1 + "' with '" + c2 + "' to give");
            System.Console.WriteLine(paras.ToString());
            System.Console.WriteLine("");
        }
    }
}
