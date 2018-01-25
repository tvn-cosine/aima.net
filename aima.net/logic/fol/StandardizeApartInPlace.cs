using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol
{
    public class StandardizeApartInPlace
    {
        private static CollectAllVariables _collectAllVariables = new CollectAllVariables();

        public static int standardizeApart(Chain c, int saIdx)
        {
            ICollection<Variable> variables = CollectionFactory.CreateQueue<Variable>();
            foreach (Literal l in c.getLiterals())
            {
                collectAllVariables(l.getAtomicSentence(), variables);
            }

            return standardizeApart(variables, c, saIdx);
        }

        public static int standardizeApart(Clause c, int saIdx)
        {
            ICollection<Variable> variables = CollectionFactory.CreateQueue<Variable>();
            foreach (Literal l in c.getLiterals())
            {
                collectAllVariables(l.getAtomicSentence(), variables);
            }

            return standardizeApart(variables, c, saIdx);
        }

        private static int standardizeApart(ICollection<Variable> variables, object expr, int saIdx)
        {
            IMap<string, int> indexicals = CollectionFactory.CreateInsertionOrderedMap<string, int>();
            foreach (Variable v in variables)
            {
                if (!indexicals.ContainsKey(v.getIndexedValue()))
                {
                    indexicals.Put(v.getIndexedValue(), saIdx++);
                }
            }
            foreach (Variable v in variables)
            {
                if (!indexicals.ContainsKey(v.getIndexedValue()))
                {
                    throw new RuntimeException("ERROR: duplicate var=" + v + ", expr=" + expr);
                }
                else
                {
                    v.setIndexical(indexicals.Get(v.getIndexedValue()));
                }
            }

            return saIdx;
        }

        private static void collectAllVariables(Sentence s, ICollection<Variable> vars)
        {
            s.accept(_collectAllVariables, vars);
        }
    }

    class CollectAllVariables : FOLVisitor
    {
        public CollectAllVariables()
        { }

        public object visitVariable(Variable var, object arg)
        {
            ICollection<Variable> variables = (ICollection<Variable>)arg;
            variables.Add(var);
            return var;
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            // Ensure I collect quantified variables too
            ICollection<Variable> variables = (ICollection<Variable>)arg;
            variables.AddAll(sentence.getVariables());

            sentence.getQuantified().accept(this, arg);

            return sentence;
        }

        public object visitPredicate(Predicate predicate, object arg)
        {
            foreach (Term t in predicate.getTerms())
            {
                t.accept(this, arg);
            }
            return predicate;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            equality.getTerm1().accept(this, arg);
            equality.getTerm2().accept(this, arg);
            return equality;
        }

        public object visitConstant(Constant constant, object arg)
        {
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            foreach (Term t in function.getTerms())
            {
                t.accept(this, arg);
            }
            return function;
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            sentence.getNegated().accept(this, arg);
            return sentence;
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            sentence.getFirst().accept(this, arg);
            sentence.getSecond().accept(this, arg);
            return sentence;
        }
    }
}
