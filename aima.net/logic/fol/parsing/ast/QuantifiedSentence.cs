using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.logic.fol.parsing.ast
{
    public class QuantifiedSentence : Sentence
    {
        private string quantifier;
        private ICollection<Variable> variables = CollectionFactory.CreateQueue<Variable>();
        private Sentence quantified;
        private ICollection<FOLNode> args = CollectionFactory.CreateQueue<FOLNode>();
        private string stringRep = null;
        private int hashCode = 0;

        public QuantifiedSentence(string quantifier, ICollection<Variable> variables,                Sentence quantified)
        {
            this.quantifier = quantifier;
            this.variables.AddAll(variables);
            this.quantified = quantified;
            foreach (Variable var in variables)
                this.args.Add(var);

            this.args.Add(quantified);
        }

        public string getQuantifier()
        {
            return quantifier;
        }

        public ICollection<Variable> getVariables()
        {
            return CollectionFactory.CreateReadOnlyQueue<Variable>(variables);
        }

        public Sentence getQuantified()
        {
            return quantified;
        }

        public string getSymbolicName()
        {
            return getQuantifier();
        }

        public bool isCompound()
        {
            return true;
        }

        public ICollection<FOLNode> getArgs()
        {
            return CollectionFactory.CreateReadOnlyQueue<FOLNode>(args);
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitQuantifiedSentence(this, arg);
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        Sentence Sentence.copy()
        {
            return copy();
        }

        public QuantifiedSentence copy()
        {
            ICollection<Variable> copyVars = CollectionFactory.CreateQueue<Variable>();
            foreach (Variable v in variables)
            {
                copyVars.Add(v.copy());
            }
            return new QuantifiedSentence(quantifier, copyVars, quantified.copy());
        }

        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if ((o == null) || (this.GetType() != o.GetType()))
            {
                return false;
            }
            QuantifiedSentence cs = (QuantifiedSentence)o;
            return cs.quantifier.Equals(quantifier)
                    && cs.variables.Equals(variables)
                    && cs.quantified.Equals(quantified);
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + quantifier.GetHashCode();
                foreach (Variable v in variables)
                {
                    hashCode = 37 * hashCode + v.GetHashCode();
                }
                hashCode = hashCode * 37 + quantified.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (null == stringRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append(quantifier);
                sb.Append(" ");
                foreach (Variable v in variables)
                {
                    sb.Append(v.ToString());
                    sb.Append(" ");
                }
                sb.Append(quantified.ToString());
                stringRep = sb.ToString();
            }
            return stringRep;
        }
    }
}
