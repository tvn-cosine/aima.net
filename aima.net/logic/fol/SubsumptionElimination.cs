using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol
{
    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): page 356.<br>
    * <br>
    * The subsumption method eliminates all sentences that are subsumed by (that
    * is, more specific than) an existing sentence in the KB. For example, P(x) is
    * in the KB, then there is no sense in adding P(A) and even less sense in
    * adding P(A) V Q(B). Subsumption helps keep the KB small and thus helps keep
    * the search space small.<br>
    * <br>
    * <b>Note:</b> <a
    * href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture12.pdf"
    * >From slide 17.</a> <br>
    * <br>
    * Relational Subsumption<br>
    * <br>
    * A relational clause &Phi; subsumes &Psi; if and only if there is a
    * substitution &delta; that, when applied to &Phi;, produces a clause &Phi;'
    * that is a subset of &Psi;.
    * 
    * @author Ciaran O'Reilly
    * @author Mike Stampone
    */
    public class SubsumptionElimination
    {
        /**
         * Returns the clauses that are subsumed by (that is, more specific than) an
         * existing clause in the specified set of clauses.
         * 
         * @param clauses
         *            a set of clauses in first order logic
         * 
         * @return the clauses that are subsumed by (that is, more specific than) an
         *         existing clause in the specified set of clauses.
         */
        public static ISet<Clause> findSubsumedClauses(ISet<Clause> clauses)
        {
            ISet<Clause> subsumed = CollectionFactory.CreateSet<Clause>();

            // Group the clauses by their # of literals.
            // Keep track of the min and max # of literals.
            int min = int.MaxValue;
            int max = 0;
            IMap<int, ISet<Clause>> clausesGroupedBySize = CollectionFactory.CreateInsertionOrderedMap<int, ISet<Clause>>();
            foreach (Clause c in clauses)
            {
                int size = c.getNumberLiterals();
                if (size < min)
                {
                    min = size;
                }
                if (size > max)
                {
                    max = size;
                }
                ISet<Clause> cforsize = clausesGroupedBySize.Get(size);
                if (null == cforsize)
                {
                    cforsize = CollectionFactory.CreateSet<Clause>();
                    clausesGroupedBySize.Put(size, cforsize);
                }
                cforsize.Add(c);
            }
            // Check if each smaller clause
            // subsumes any of the larger clauses.
            for (int i = min; i < max;++i)
            {
                ISet<Clause> scs = clausesGroupedBySize.Get(i);
                // Ensure there are clauses with this # of literals
                if (null != scs)
                {
                    for (int j = i + 1; j <= max; j++)
                    {
                        ISet<Clause> lcs = clausesGroupedBySize.Get(j);
                        // Ensure there are clauses with this # of literals
                        if (null != lcs)
                        {
                            foreach (Clause sc in scs)
                            {
                                // Don't bother checking clauses
                                // that are already subsumed.
                                if (!subsumed.Contains(sc))
                                {
                                    foreach (Clause lc in lcs)
                                    {
                                        if (!subsumed.Contains(lc))
                                        {
                                            if (sc.subsumes(lc))
                                            {
                                                subsumed.Add(lc);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return subsumed;
        }
    }
}
