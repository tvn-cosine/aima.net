using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.logic.fol.parsing.ast
{
    public class NotSentence : Sentence
    {
        private Sentence negated;
        private ICollection<Sentence> args = CollectionFactory.CreateQueue<Sentence>();
        private string stringRep = null;
        private int hashCode = 0;

        public NotSentence(Sentence negated)
        {
            this.negated = negated;
            args.Add(negated);
        }

        public Sentence getNegated()
        {
            return negated;
        }

        public string getSymbolicName()
        {
            return Connectors.NOT;
        }

        public bool isCompound()
        {
            return true;
        }

        ICollection<FOLNode> FOLNode.getArgs()
        {
            ICollection<FOLNode> obj = CollectionFactory.CreateQueue<FOLNode>();
            foreach (Sentence sentence in args)
            {
                obj.Add(sentence);
            }

            return CollectionFactory.CreateReadOnlyQueue<FOLNode>(obj);
        }

        public ICollection<Sentence> getArgs()
        {
            return CollectionFactory.CreateReadOnlyQueue<Sentence>(args);
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitNotSentence(this, arg);
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        Sentence Sentence.copy()
        {
            return copy();
        }

        public NotSentence copy()
        {
            return new NotSentence(negated.copy());
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
            NotSentence ns = (NotSentence)o;
            return (ns.negated.Equals(negated));
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + negated.GetHashCode();
            }
            return hashCode;
        }


        public override string ToString()
        {
            if (null == stringRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append("NOT(");
                sb.Append(negated.ToString());
                sb.Append(")");
                stringRep = sb.ToString();
            }
            return stringRep;
        }
    }
}
