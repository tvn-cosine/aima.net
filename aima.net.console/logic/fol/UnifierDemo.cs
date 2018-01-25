using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.demo.logic.fol
{
    public class UnifierDemo
    {
        static void Main(params string[] args)
        {
            unifierDemo();
        }

        private static void unifierDemo()
        {
            FOLParser parser = new FOLParser(DomainFactory.knowsDomain());
            Unifier unifier = new Unifier();
            IMap<Variable, Term> theta = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

            Sentence query = parser.parse("Knows(John,x)");
            Sentence johnKnowsJane = parser.parse("Knows(y,Mother(y))");

            System.Console.WriteLine("------------");
            System.Console.WriteLine("Unifier Demo");
            System.Console.WriteLine("------------");
            IMap<Variable, Term> subst = unifier.unify(query, johnKnowsJane, theta);
            System.Console.WriteLine("Unify '" + query + "' with '" + johnKnowsJane + "' to get the substitution " + subst + ".");
            System.Console.WriteLine("");
        }
    }
}
