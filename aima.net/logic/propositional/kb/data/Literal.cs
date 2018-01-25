using aima.net;
using aima.net.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.logic.propositional.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244.<br>
     * <br>
     * A literal is either an atomic sentence (a positive literal) or a negated
     * atomic sentence (a negative literal). In propositional logic the atomic
     * sentences consist of a single proposition symbol. In addition, a literal as
     * implemented is immutable.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class Literal
    {
        private PropositionSymbol atom = null;
        private bool positive = true; // Assume positive by default.
                                      //
        private string cachedStringRep = null;
        private int cachedHashCode = -1;

        /**
         * Constructor for a positive literal.
         * 
         * @param atom
         *            the atomic sentence comprising the literal.
         */
        public Literal(PropositionSymbol atom)
            : this(atom, true)
        { }

        /**
         * Constructor.
         * 
         * @param atom
         *            the atomic sentence comprising the literal.
         * @param positive
         *            true if to be a positive literal, false to be a negative
         *            literal.
         */
        public Literal(PropositionSymbol atom, bool positive)
        {
            this.atom = atom;
            this.positive = positive;
        }

        /**
         * 
         * @return true if a positive literal, false otherwise.
         */
        public bool isPositiveLiteral()
        {
            return positive;
        }

        /**
         * 
         * @return true if a negative literal, false otherwise.
         */
        public bool isNegativeLiteral()
        {
            return !positive;
        }

        /**
         * 
         * @return the atomic sentence comprising the literal.
         */
        public PropositionSymbol getAtomicSentence()
        {
            return atom;
        }

        /**
         * 
         * @return true if the literal is representative of an always true
         *         proposition (i.e. True or ~False), false otherwise.
         */
        public bool isAlwaysTrue()
        {
            // True | ~False
            if (isPositiveLiteral())
            {
                return getAtomicSentence().isAlwaysTrue();
            }
            else
            {
                return getAtomicSentence().isAlwaysFalse();
            }
        }

        /**
         * 
         * @return true if the literal is representative of an always false
         *         proposition (i.e. False or ~True), false othwerwise.
         */
        public bool isAlwaysFalse()
        {
            // False | ~True
            if (isPositiveLiteral())
            {
                return getAtomicSentence().isAlwaysFalse();
            }
            else
            {
                return getAtomicSentence().isAlwaysTrue();
            }
        }


        public override string ToString()
        {
            if (null == cachedStringRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                if (isNegativeLiteral())
                {
                    sb.Append(Connective.NOT.ToString());
                }
                sb.Append(getAtomicSentence().ToString());
                cachedStringRep = sb.ToString();
            }

            return cachedStringRep;
        }


        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is Literal))
            {
                return false;
            }
            Literal l = (Literal)o;
            return l.isPositiveLiteral() == isPositiveLiteral()
                    && l.getAtomicSentence().Equals(getAtomicSentence());
        }


        public override int GetHashCode()
        {
            if (cachedHashCode == -1)
            {
                cachedHashCode = 17;
                cachedHashCode = (cachedHashCode * 37)
                        + (isPositiveLiteral() ? "+".GetHashCode() : "-".GetHashCode());
                cachedHashCode = (cachedHashCode * 37) + atom.GetHashCode();
            }
            return cachedHashCode;
        }
    }
}
