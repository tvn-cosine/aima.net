using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.logic.fol.parsing.ast
{
    public class TermEquality : AtomicSentence
    {
        private Term term1, term2;
        private ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
        private string stringRep = null;
        private int hashCode = 0;

        public static string getEqualitySynbol()
        {
            return "=";
        }

        public TermEquality(Term term1, Term term2)
        {
            this.term1 = term1;
            this.term2 = term2;
            terms.Add(term1);
            terms.Add(term2);
        }

        public Term getTerm1()
        {
            return term1;
        }

        public Term getTerm2()
        {
            return term2;
        }

        //
        // START-AtomicSentence
        public string getSymbolicName()
        {
            return getEqualitySynbol();
        }

        public bool isCompound()
        {
            return true;
        }

        ICollection<FOLNode> FOLNode.getArgs()
        {
            ICollection<FOLNode> obj = CollectionFactory.CreateQueue<FOLNode>();
            foreach (Term term in terms)
            {
                obj.Add(term);
            }

            return CollectionFactory.CreateReadOnlyQueue<FOLNode>(obj);
        }

        public ICollection<Term> getArgs()
        {
            return CollectionFactory.CreateReadOnlyQueue<Term>(terms);
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitTermEquality(this, arg);
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

        public TermEquality copy()
        {
            return new TermEquality(term1.copy(), term2.copy());
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
            TermEquality te = (TermEquality)o;

            return te.getTerm1().Equals(term1) && te.getTerm2().Equals(term2);
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + getTerm1().GetHashCode();
                hashCode = 37 * hashCode + getTerm2().GetHashCode();
            }
            return hashCode;
        }


        public override string ToString()
        {
            if (null == stringRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append(term1.ToString());
                sb.Append(" = ");
                sb.Append(term2.ToString());
                stringRep = sb.ToString();
            }
            return stringRep;
        }
    }
}
