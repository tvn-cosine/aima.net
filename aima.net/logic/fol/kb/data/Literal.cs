using aima.net;
using aima.net.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244.<br>
     * <br>
     * A literal is either an atomic sentence (a positive literal) or a negated
     * atomic sentence (a negative literal).
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class Literal
    {
        private AtomicSentence atom = null;
        private bool negativeLiteral = false;
        private string strRep = null;
        private int hashCode = 0;

        public Literal(AtomicSentence atom)
        {
            this.atom = atom;
        }

        public Literal(AtomicSentence atom, bool negated)
        {
            this.atom = atom;
            this.negativeLiteral = negated;
        }

        public virtual Literal newInstance(AtomicSentence atom)
        {
            return new Literal(atom, negativeLiteral);
        }

        public virtual bool isPositiveLiteral()
        {
            return !negativeLiteral;
        }

        public virtual bool isNegativeLiteral()
        {
            return negativeLiteral;
        }

        public virtual AtomicSentence getAtomicSentence()
        {
            return atom;
        }


        public override string ToString()
        {
            if (null == strRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                if (isNegativeLiteral())
                {
                    sb.Append("~");
                }
                sb.Append(getAtomicSentence().ToString());
                strRep = sb.ToString();
            }

            return strRep;
        }
         
        public override bool Equals(object o)
        { 
            if (this == o)
            {
                return true;
            }
            if (o.GetType() != GetType())
            {
                // This prevents ReducedLiterals
                // being treated as equivalent to
                // normal Literals.
                return false;
            }
            if (!(o is Literal))
            {
                return false;
            }
            Literal l = (Literal)o;
            return l.isPositiveLiteral() == isPositiveLiteral()
                    && l.getAtomicSentence().getSymbolicName()
                            .Equals(atom.getSymbolicName())
                    && l.getAtomicSentence().getArgs().SequenceEqual(atom.getArgs());
        }


        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + (GetType().Name.GetHashCode())
                        + (isPositiveLiteral() ? "+".GetHashCode() : "-".GetHashCode())
                        + atom.getSymbolicName().GetHashCode();
                foreach (Term t in atom.getArgs())
                {
                    hashCode = 37 * hashCode + t.GetHashCode();
                }
            }
            return hashCode;
        }
    }
}
