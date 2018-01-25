using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing.ast;
using aima.net.logic.propositional.visitors;
using aima.net.util;

namespace aima.net.logic.propositional.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 258.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function PL-FC-ENTAILS?(KB, q) returns true or false
     *   inputs: KB, the knowledge base, a set of propositional definite clauses
     *           q, the query, a proposition symbol
     *   count &larr; a table, where count[c] is the number of symbols in c's premise
     *   inferred &larr; a table, where inferred[s] is initially false for all symbols
     *   agenda &larr; a queue of symbols, initially symbols known to be true in KB
     *   
     *   while agenda is not empty do
     *     p &larr; Pop(agenda)
     *     if p = q then return true
     *     if inferred[p] = false then
     *        inferred[p] &larr; true
     *        for each clause c in KB where p is in c.PREMISE do
     *            decrement count[c]
     *            if count[c] = 0 then add c.CONCLUSION to agenda
     *   return false
     * </code>
     * </pre>
     * 
     * Figure 7.15 the forward-chaining algorithm for propositional logic. The
     * <i>agenda</i> keeps track of symbols known to be true but not yet
     * "processed". The <i>count</i> table keeps track of how many premises of each
     * implication are as yet unknown. Whenever a new symbol p from the agenda is
     * processed, the count is reduced by one for each implication in whose premise
     * p appears (easily identified in constant time with appropriate indexing.) If
     * a count reaches zero, all the premises of the implication are known, so its
     * conclusion can be added to the agenda. Finally, we need to keep track of
     * which symbols have been processed; a symbol that is already in the set of
     * inferred symbols need not be added to the agenda again. This avoids redundant
     * work and prevents loops caused by implications such as P &rArr; Q and Q
     * &rArr; P.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class PLFCEntails
    {

        /**
         * PL-FC-ENTAILS?(KB, q)<br>
         * The forward-chaining algorithm for propositional logic.
         * 
         * @param kb
         *            the knowledge base, a set of propositional definite clauses.
         * @param q
         *            q, the query, a proposition symbol
         * @return true if KB |= q, false otherwise.
         * @throws IllegalArgumentException
         *             if KB contains any non-definite clauses.
         */
        public bool plfcEntails(KnowledgeBase kb, PropositionSymbol q)
        {
            // count <- a table, where count[c] is the number of symbols in c's
            // premise
            IMap<Clause, int> count = initializeCount(kb);
            // inferred <- a table, where inferred[s] is initially false for all
            // symbols
            IMap<PropositionSymbol, bool?> inferred = initializeInferred(kb);
            // agenda <- a queue of symbols, initially symbols known to be true in
            // KB
            ICollection<PropositionSymbol> agenda = initializeAgenda(count);
            // Note: an index for p to the clauses where p appears in the premise
            IMap<PropositionSymbol, ISet<Clause>> pToClausesWithPInPremise = initializeIndex(count, inferred);

            // while agenda is not empty do
            while (!agenda.IsEmpty())
            {
                // p <- Pop(agenda)
                PropositionSymbol p = agenda.Pop();
                // if p = q then return true
                if (p.Equals(q))
                {
                    return true;
                }
                // if inferred[p] = false then
                if (inferred.Get(p).Equals(false))
                {
                    // inferred[p] <- true
                    inferred.Put(p, true);
                    // for each clause c in KB where p is in c.PREMISE do
                    foreach (Clause c in pToClausesWithPInPremise.Get(p))
                    {
                        // decrement count[c]
                        decrement(count, c);
                        // if count[c] = 0 then add c.CONCLUSION to agenda
                        if (count.Get(c) == 0)
                        {
                            agenda.Add(conclusion(c));
                        }
                    }
                }
            }

            // return false
            return false;
        }

        protected IMap<Clause, int> initializeCount(KnowledgeBase kb)
        {
            // count <- a table, where count[c] is the number of symbols in c's
            // premise
            IMap<Clause, int> count = CollectionFactory.CreateInsertionOrderedMap<Clause, int>();

            ISet<Clause> clauses = ConvertToConjunctionOfClauses.convert(kb.asSentence()).getClauses();
            foreach (Clause c in clauses)
            {
                if (!c.isDefiniteClause())
                {
                    throw new IllegalArgumentException("Knowledge Base contains non-definite clauses:" + c);
                }
                // Note: # of negative literals is equivalent to the number of symbols in c's premise
                count.Put(c, c.getNumberNegativeLiterals());
            }

            return count;
        }

        protected IMap<PropositionSymbol, bool?> initializeInferred(KnowledgeBase kb)
        {
            // inferred <- a table, where inferred[s] is initially false for all
            // symbols
            IMap<PropositionSymbol, bool?> inferred = CollectionFactory.CreateInsertionOrderedMap<PropositionSymbol, bool?>();
            foreach (PropositionSymbol p in SymbolCollector.getSymbolsFrom(kb.asSentence()))
            {
                inferred.Put(p, false);
            }
            return inferred;
        }

        // Note: at the point of calling this routine, count will contain all the
        // clauses in KB.
        protected ICollection<PropositionSymbol> initializeAgenda(IMap<Clause, int> count)
        {
            // agenda <- a queue of symbols, initially symbols known to be true in KB
            ICollection<PropositionSymbol> agenda = CollectionFactory.CreateQueue<PropositionSymbol>();
            foreach (Clause c in count.GetKeys())
            {
                // No premise just a conclusion, then we know its true
                if (c.getNumberNegativeLiterals() == 0)
                {
                    agenda.Add(conclusion(c));
                }
            }
            return agenda;
        }

        // Note: at the point of calling this routine, count will contain all the
        // clauses in KB while inferred will contain all the proposition symbols.
        protected IMap<PropositionSymbol, ISet<Clause>> initializeIndex(IMap<Clause, int> count, IMap<PropositionSymbol, bool?> inferred)
        {
            IMap<PropositionSymbol, ISet<Clause>> pToClausesWithPInPremise = CollectionFactory.CreateInsertionOrderedMap<PropositionSymbol, ISet<Clause>>();
            foreach (PropositionSymbol p in inferred.GetKeys())
            {
                ISet<Clause> clausesWithPInPremise = CollectionFactory.CreateSet<Clause>();
                foreach (Clause c in count.GetKeys())
                {
                    // Note: The negative symbols comprise the premise
                    if (c.getNegativeSymbols().Contains(p))
                    {
                        clausesWithPInPremise.Add(c);
                    }
                }
                pToClausesWithPInPremise.Put(p, clausesWithPInPremise);
            }
            return pToClausesWithPInPremise;
        }

        protected void decrement(IMap<Clause, int> count, Clause c)
        {
            int currentCount = count.Get(c);
            // Note: a definite clause can just be a fact (i.e. 1 positive literal)
            // However, we only decrement those where the symbol is in the premise
            // so we don't need to worry about going < 0.
            count.Put(c, currentCount - 1);
        }

        protected PropositionSymbol conclusion(Clause c)
        {
            // Note: the conclusion is from the single positive
            // literal in the definite clause (which we are
            // restricted to).
            return Util.first(c.getPositiveSymbols());
        }
    }
}
