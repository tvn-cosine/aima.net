using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference
{
    /**
     * Abstract base class for Demodulation and Paramodulation algorithms.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public abstract class AbstractModulation
    { 
        protected VariableCollector variableCollector = new VariableCollector();
        protected Unifier unifier = new Unifier();
        protected SubstVisitor substVisitor = new SubstVisitor();
         
        protected abstract bool isValidMatch(Term toMatch,
                ISet<Variable> toMatchVariables, Term possibleMatch,
                IMap<Variable, Term> substitution);

        protected IdentifyCandidateMatchingTerm getMatchingSubstitution(Term toMatch, AtomicSentence expression)
        {

            IdentifyCandidateMatchingTerm icm = new IdentifyCandidateMatchingTerm(toMatch, expression, this);

            if (icm.isMatch())
            {
                return icm;
            }

            // indicates no match
            return null;
        }

        protected class IdentifyCandidateMatchingTerm : FOLVisitor
        {
            private Term toMatch = null;
            private ISet<Variable> toMatchVariables = null;
            private Term matchingTerm = null;
            private IMap<Variable, Term> substitution = null;
            private AbstractModulation abstractModulation;

            public IdentifyCandidateMatchingTerm(Term toMatch,
                    AtomicSentence expression,
                    AbstractModulation abstractModulation)
            {
                this.abstractModulation = abstractModulation;
                this.toMatch = toMatch;
                this.toMatchVariables = abstractModulation.variableCollector.collectAllVariables(toMatch);

                expression.accept(this, null);
            }

            public bool isMatch()
            {
                return null != matchingTerm;
            }

            public Term getMatchingTerm()
            {
                return matchingTerm;
            }

            public IMap<Variable, Term> getMatchingSubstitution()
            {
                return substitution;
            }

            //
            // START-FOLVisitor
            public object visitPredicate(Predicate p, object arg)
            {
                foreach (Term t in p.getArgs())
                {
                    // Finish processing if have found a match
                    if (null != matchingTerm)
                    {
                        break;
                    }
                    t.accept(this, null);
                }
                return p;
            }

            public object visitTermEquality(TermEquality equality, object arg)
            {
                foreach (Term t in equality.getArgs())
                {
                    // Finish processing if have found a match
                    if (null != matchingTerm)
                    {
                        break;
                    }
                    t.accept(this, null);
                }
                return equality;
            }

            public object visitVariable(Variable variable, object arg)
            {

                if (null != (substitution = abstractModulation.unifier.unify(toMatch, variable)))
                {
                    if (abstractModulation.isValidMatch(toMatch, toMatchVariables, variable,
                            substitution))
                    {
                        matchingTerm = variable;
                    }
                }

                return variable;
            }

            public object visitConstant(Constant constant, object arg)
            {
                if (null != (substitution = abstractModulation.unifier.unify(toMatch, constant)))
                {
                    if (abstractModulation.isValidMatch(toMatch, toMatchVariables, constant, substitution))
                    {
                        matchingTerm = constant;
                    }
                }

                return constant;
            }

            public object visitFunction(Function function, object arg)
            {
                if (null != (substitution = abstractModulation.unifier.unify(toMatch, function)))
                {
                    if (abstractModulation.isValidMatch(toMatch, toMatchVariables, function, substitution))
                    {
                        matchingTerm = function;
                    }
                }

                if (null == matchingTerm)
                {
                    // Try the Function's arguments
                    foreach (Term t in function.getArgs())
                    {
                        // Finish processing if have found a match
                        if (null != matchingTerm)
                        {
                            break;
                        }
                        t.accept(this, null);
                    }
                }

                return function;
            }

            public object visitNotSentence(NotSentence sentence, object arg)
            {
                throw new IllegalStateException("visitNotSentence() should not be called.");
            }

            public object visitConnectedSentence(ConnectedSentence sentence, object arg)
            {
                throw new IllegalStateException("visitConnectedSentence() should not be called.");
            }

            public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
            {
                throw new IllegalStateException("visitQuantifiedSentence() should not be called.");
            }
        }

        protected class ReplaceMatchingTerm : FOLVisitor
        {
            private Term toReplace = null;
            private Term replaceWith = null;
            private bool replaced = false;

            public ReplaceMatchingTerm()
            { }

            public AtomicSentence replace(AtomicSentence expression, Term toReplace, Term replaceWith)
            {
                this.toReplace = toReplace;
                this.replaceWith = replaceWith;

                return (AtomicSentence)expression.accept(this, null);
            }

            public object visitPredicate(Predicate p, object arg)
            {
                ICollection<Term> newTerms = CollectionFactory.CreateQueue<Term>();
                foreach (Term t in p.getTerms())
                {
                    Term subsTerm = (Term)t.accept(this, arg);
                    newTerms.Add(subsTerm);
                }
                return new Predicate(p.getPredicateName(), newTerms);
            }

            public object visitTermEquality(TermEquality equality, object arg)
            {
                Term newTerm1 = (Term)equality.getTerm1().accept(this, arg);
                Term newTerm2 = (Term)equality.getTerm2().accept(this, arg);
                return new TermEquality(newTerm1, newTerm2);
            }

            public object visitVariable(Variable variable, object arg)
            {
                if (!replaced)
                {
                    if (toReplace.Equals(variable))
                    {
                        replaced = true;
                        return replaceWith;
                    }
                }
                return variable;
            }

            public object visitConstant(Constant constant, object arg)
            {
                if (!replaced)
                {
                    if (toReplace.Equals(constant))
                    {
                        replaced = true;
                        return replaceWith;
                    }
                }
                return constant;
            }

            public object visitFunction(Function function, object arg)
            {
                if (!replaced)
                {
                    if (toReplace.Equals(function))
                    {
                        replaced = true;
                        return replaceWith;
                    }
                }

                ICollection<Term> newTerms = CollectionFactory.CreateQueue<Term>();
                foreach (Term t in function.getTerms())
                {
                    Term subsTerm = (Term)t.accept(this, arg);
                    newTerms.Add(subsTerm);
                }
                return new Function(function.getFunctionName(), newTerms);
            }

            public object visitNotSentence(NotSentence sentence, object arg)
            {
                throw new IllegalStateException("visitNotSentence() should not be called.");
            }

            public object visitConnectedSentence(ConnectedSentence sentence, object arg)
            {
                throw new IllegalStateException("visitConnectedSentence() should not be called.");
            }

            public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
            {
                throw new IllegalStateException("visitQuantifiedSentence() should not be called.");
            }
        }
    }
}
