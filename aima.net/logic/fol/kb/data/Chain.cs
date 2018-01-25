using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.logic.fol.inference.proof;

namespace aima.net.logic.fol.kb.data
{
    /**
     * 
     * A Chain is a sequence of literals (while a clause is a set) - order is
     * important for a chain.
     * 
     * @see <a
     *      href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf"
     *      >Chain</a>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class Chain
    {
        private static ICollection<Literal> _emptyLiteralsList = CollectionFactory.CreateReadOnlyQueue<Literal>(CollectionFactory.CreateQueue<Literal>());
        //
        private ICollection<Literal> literals = CollectionFactory.CreateQueue<Literal>();
        private ProofStep proofStep = null;

        public Chain()
        {
            // i.e. the empty chain
        }

        public Chain(ICollection<Literal> literals)
        {
            this.literals.AddAll(literals);
        }

        public Chain(ISet<Literal> literals)
        {
            this.literals.AddAll(literals);
        }

        public ProofStep getProofStep()
        {
            if (null == proofStep)
            {
                // Assume was a premise
                proofStep = new ProofStepPremise(this);
            }
            return proofStep;
        }

        public void setProofStep(ProofStep proofStep)
        {
            this.proofStep = proofStep;
        }

        public bool isEmpty()
        {
            return literals.Size() == 0;
        }

        public void addLiteral(Literal literal)
        {
            literals.Add(literal);
        }

        public Literal getHead()
        {
            if (0 == literals.Size())
            {
                return null;
            }
            return literals.Get(0);
        }

        public ICollection<Literal> getTail()
        {
            if (0 == literals.Size())
            {
                return _emptyLiteralsList;
            }
            return CollectionFactory.CreateReadOnlyQueue<Literal>(literals.subList(1, literals.Size()));
        }

        public int getNumberLiterals()
        {
            return literals.Size();
        }

        public ICollection<Literal> getLiterals()
        {
            return CollectionFactory.CreateReadOnlyQueue<Literal>(literals);
        }

        /**
         * A contrapositive of a chain is a permutation in which a different literal
         * is placed at the front. The contrapositives of a chain are logically
         * equivalent to the original chain.
         * 
         * @return a list of contrapositives for this chain.
         */
        public ICollection<Chain> getContrapositives()
        {
            ICollection<Chain> contrapositives = CollectionFactory.CreateQueue<Chain>();
            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();

            for (int i = 1; i < literals.Size();++i)
            {
                lits.Clear();
                lits.Add(literals.Get(i));
                lits.AddAll(literals.subList(0, i));
                lits.AddAll(literals.subList(i + 1, literals.Size()));
                Chain cont = new Chain(lits);
                cont.setProofStep(new ProofStepChainContrapositive(cont, this));
                contrapositives.Add(cont);
            }

            return contrapositives;
        }
         
        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            sb.Append("<");

            for (int i = 0; i < literals.Size();++i)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(literals.Get(i).ToString());
            }

            sb.Append(">");

            return sb.ToString();
        }
    }
}
