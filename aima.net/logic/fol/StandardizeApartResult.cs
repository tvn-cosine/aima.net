using aima.net.collections.api;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol
{
    public class StandardizeApartResult
    {
        private Sentence originalSentence = null;
        private Sentence standardized = null;
        private IMap<Variable, Term> forwardSubstitution = null;
        private IMap<Variable, Term> reverseSubstitution = null;

        public StandardizeApartResult(Sentence originalSentence,
                Sentence standardized, IMap<Variable, Term> forwardSubstitution,
                IMap<Variable, Term> reverseSubstitution)
        {
            this.originalSentence = originalSentence;
            this.standardized = standardized;
            this.forwardSubstitution = forwardSubstitution;
            this.reverseSubstitution = reverseSubstitution;
        }

        public Sentence getOriginalSentence()
        {
            return originalSentence;
        }

        public Sentence getStandardized()
        {
            return standardized;
        }

        public IMap<Variable, Term> getForwardSubstitution()
        {
            return forwardSubstitution;
        }

        public IMap<Variable, Term> getReverseSubstitution()
        {
            return reverseSubstitution;
        }
    }
}
