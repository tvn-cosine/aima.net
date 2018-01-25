using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol
{
    public class VariableCollector : FOLVisitor
    {
        public VariableCollector()
        { }

        // Note: The set guarantees the order in which they were
        // found.
        public ISet<Variable> collectAllVariables(Sentence sentence)
        {
            ISet<Variable> variables = CollectionFactory.CreateSet<Variable>();

            sentence.accept(this, variables);

            return variables;
        }

        public ISet<Variable> collectAllVariables(Term term)
        {
            ISet<Variable> variables = CollectionFactory.CreateSet<Variable>();

            term.accept(this, variables);

            return variables;
        }

        public ISet<Variable> collectAllVariables(Clause clause)
        {
            ISet<Variable> variables = CollectionFactory.CreateSet<Variable>();

            foreach (Literal l in clause.getLiterals())
            {
                l.getAtomicSentence().accept(this, variables);
            }

            return variables;
        }

        public ISet<Variable> collectAllVariables(Chain chain)
        {
            ISet<Variable> variables = CollectionFactory.CreateSet<Variable>();

            foreach (Literal l in chain.getLiterals())
            {
                l.getAtomicSentence().accept(this, variables);
            }

            return variables;
        }

        public object visitVariable(Variable var, object arg)
        {
            ISet<Variable> variables = (ISet<Variable>)arg;
            variables.Add(var);
            return var;
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            // Ensure I collect quantified variables too
            ISet<Variable> variables = (ISet<Variable>)arg;
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
