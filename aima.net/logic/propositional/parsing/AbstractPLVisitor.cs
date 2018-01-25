using aima.net.logic.propositional.parsing.ast;

namespace aima.net.logic.propositional.parsing
{
    /**
     * Abstract implementation of the PLVisitor interface that provides default
     * behavior for each of the methods.
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * 
     * @param <A>
     *            the argument type to be passed to the visitor methods.
     */
    public abstract class AbstractPLVisitor<A> : PLVisitor<A, Sentence>
    { 
        public virtual Sentence visitPropositionSymbol(PropositionSymbol s, A arg)
        {
            // default behavior is to treat propositional symbols as atomic
            // and leave unchanged.
            return s;
        }


        public virtual Sentence visitUnarySentence(ComplexSentence s, A arg)
        {
            // a new Complex Sentence with the same connective but possibly
            // with its simpler sentence replaced by the visitor.
            return new ComplexSentence(s.getConnective(), s.getSimplerSentence(0)
                    .accept(this, arg));
        }


        public virtual Sentence visitBinarySentence(ComplexSentence s, A arg)
        {
            // a new Complex Sentence with the same connective but possibly
            // with its simpler sentences replaced by the visitor.
            return new ComplexSentence(s.getConnective(), s.getSimplerSentence(0)
                    .accept(this, arg), s.getSimplerSentence(1).accept(this, arg));
        }
    }
}
