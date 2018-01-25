using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.logic.fol.parsing.ast
{
    public class ConnectedSentence : Sentence
    { 
        private string connector;
        private Sentence first, second;
        private ICollection<Sentence> args = CollectionFactory.CreateQueue<Sentence>();
        private string stringRep = null;
        private int hashCode = 0;

        public ConnectedSentence(string connector, Sentence first, Sentence second)
        {
            this.connector = connector;
            this.first = first;
            this.second = second;
            args.Add(first);
            args.Add(second);
        }

        public string getConnector()
        {
            return connector;
        }

        public Sentence getFirst()
        {
            return first;
        }

        public Sentence getSecond()
        {
            return second;
        }
         
        public string getSymbolicName()
        {
            return getConnector();
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
            return v.visitConnectedSentence(this, arg);
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        Sentence Sentence.copy()
        {
            return copy();
        }

        public ConnectedSentence copy()
        {
            return new ConnectedSentence(connector, first.copy(), second.copy());
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
            ConnectedSentence cs = (ConnectedSentence)o;
            return cs.getConnector().Equals(getConnector())
                    && cs.getFirst().Equals(getFirst())
                    && cs.getSecond().Equals(getSecond());
        }
         
        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + getConnector().GetHashCode();
                hashCode = 37 * hashCode + getFirst().GetHashCode();
                hashCode = 37 * hashCode + getSecond().GetHashCode();
            }
            return hashCode;
        }
         
        public override string ToString()
        {
            if (null == stringRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append("(");
                sb.Append(first.ToString());
                sb.Append(" ");
                sb.Append(connector);
                sb.Append(" ");
                sb.Append(second.ToString());
                sb.Append(")");
                stringRep = sb.ToString();
            }
            return stringRep;
        }
    }
}
