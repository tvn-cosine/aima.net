using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.proposition.api;

namespace aima.net.probability.proposition
{
    public abstract class AbstractTermProposition : AbstractProposition, ITermProposition
    {
        private IRandomVariable termVariable = null;

        public AbstractTermProposition(IRandomVariable var)
        {
            if (null == var)
            {
                throw new IllegalArgumentException("The Random Variable for the Term must be specified.");
            }
            this.termVariable = var;
            addScope(this.termVariable);
        }

        public IRandomVariable getTermVariable()
        {
            return termVariable;
        }
    }
}
