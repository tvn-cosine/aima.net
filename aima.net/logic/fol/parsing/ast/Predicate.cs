using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.logic.fol.parsing.ast
{
    public class Predicate : AtomicSentence
    {
        private string predicateName;
        private ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
        private string stringRep = null;
        private int hashCode = 0;

        public Predicate(string predicateName, ICollection<Term> terms)
        {
            this.predicateName = predicateName;
            this.terms.AddAll(terms);
        }

        public string getPredicateName()
        {
            return predicateName;
        }

        public ICollection<Term> getTerms()
        {
            return CollectionFactory.CreateReadOnlyQueue<Term>(terms);
        }

        public string getSymbolicName()
        {
            return getPredicateName();
        }

        public bool isCompound()
        {
            return true;
        }

        ICollection<FOLNode> FOLNode.getArgs()
        {
            ICollection<FOLNode> obj = CollectionFactory.CreateQueue<FOLNode>();
            foreach (var folNode in getArgs())
            {
                obj.Add(folNode);
            }
            return obj;
        }

        public ICollection<Term> getArgs()
        {
            return getTerms();
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitPredicate(this, arg);
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        Sentence Sentence.copy()
        {
            return copy();
        }

        AtomicSentence AtomicSentence.copy()
        {
            return copy();
        }

        public Predicate copy()
        {
            ICollection<Term> copyTerms = CollectionFactory.CreateQueue<Term>();
            foreach (Term t in terms)
            {
                copyTerms.Add(t.copy());
            }
            return new Predicate(predicateName, copyTerms);
        }

        public override bool Equals(object o)
        { 
            if (this == o)
            {
                return true;
            }
            if (!(o is Predicate))
            {
                return false;
            }
            Predicate p = (Predicate)o;
            return p.getPredicateName().Equals(getPredicateName())
                 && p.getTerms().SequenceEqual(getTerms());
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + predicateName.GetHashCode();
                foreach (Term t in terms)
                {
                    hashCode = 37 * hashCode + t.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (null == stringRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append(predicateName);
                sb.Append("(");

                bool first = true;
                foreach (Term t in terms)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }
                    sb.Append(t.ToString());
                }

                sb.Append(")");
                stringRep = sb.ToString();
            }

            return stringRep;
        }
    }
}
