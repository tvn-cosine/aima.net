using aima.net.collections.api;

namespace aima.net.logic.fol.parsing.ast
{
    public class Variable : Term
    {

        private string value;
        private int hashCode = 0;
        private int indexical = -1;

        public Variable(string s)
        {
            value = s.Trim();
        }

        public Variable(string s, int idx)
        {
            value = s.Trim();
            indexical = idx;
        }

        public string getValue()
        {
            return value;
        }
         
        public string getSymbolicName()
        {
            return getValue();
        }

        public bool isCompound()
        {
            return false;
        }

        ICollection<FOLNode> FOLNode.getArgs()
        {
            // Is not Compound, therefore should return null for its arguments
            return null;
        }

        public ICollection<Term> getArgs()
        {
            // Is not Compound, therefore should return null for its arguments
            return null;
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitVariable(this, arg);
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        Term Term.copy()
        {
            return copy();
        }

        public Variable copy()
        {
            return new Variable(value, indexical);
        }
         
        public int getIndexical()
        {
            return indexical;
        }

        public void setIndexical(int idx)
        {
            indexical = idx;
            hashCode = 0;
        }

        public string getIndexedValue()
        {
            return value + indexical;
        }
         
        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if (!(o is Variable))
            {
                return false;
            }

            Variable v = (Variable)o;
            return v.getValue().Equals(getValue())
                && v.getIndexical() == getIndexical();
        }
         
        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode += indexical;
                hashCode = 37 * hashCode + value.GetHashCode();
            }

            return hashCode;
        }
         
        public override string ToString()
        {
            return value;
        }
    }
}
