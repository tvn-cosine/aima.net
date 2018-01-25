using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing.ast;
using aima.net.logic.propositional.visitors;
using aima.net.util;

namespace aima.net.logic.propositional.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 261.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function DPLL-SATISFIABLE?(s) returns true or false
     *   inputs: s, a sentence in propositional logic.
     *   
     *   clauses &larr; the set of clauses in the CNF representation of s
     *   symbols &larr; a list of the proposition symbols in s
     *   return DPLL(clauses, symbols, {})
     * 
     * --------------------------------------------------------------------------------
     * 
     * function DPLL(clauses, symbols, model) returns true or false
     *   
     *   if every clause in clauses is true in model then return true
     *   if some clause in clauses is false in model then return false
     *   P, value &larr; FIND-PURE-SYMBOL(symbols, clauses, model)
     *   if P is non-null then return DPLL(clauses, symbols - P, model &cup; {P = value})
     *   P, value &larr; FIND-UNIT-CLAUSE(clauses, model)
     *   if P is non-null then return DPLL(clauses, symbols - P, model &cup; {P = value})
     *   P &larr; FIRST(symbols); rest &larr; REST(symbols)
     *   return DPLL(clauses, rest, model &cup; {P = true}) or
     *          DPLL(clauses, rest, model &cup; {P = false})
     * </code>
     * </pre>
     * 
     * Figure 7.17 The DPLL algorithm for checking satisfiability of a sentence in
     * propositional logic. The ideas behind FIND-PURE-SYMBOL and FIND-UNIT-CLAUSE
     * are described in the test; each returns a symbol (or null) and the truth
     * value to assign to that symbol. Like TT-ENTAILS?, DPLL operates over partial
     * models.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class DPLLSatisfiable : DPLL
    {
        /**
         * DPLL-SATISFIABLE?(s)<br>
         * Checks the satisfiability of a sentence in propositional logic.
         * 
         * @param s
         *            a sentence in propositional logic.
         * @return true if the sentence is satisfiable, false otherwise.
         */

        public bool dpllSatisfiable(Sentence s)
        {
            // clauses <- the set of clauses in the CNF representation of s
            ISet<Clause> clauses = ConvertToConjunctionOfClauses.convert(s).getClauses();
            // symbols <- a list of the proposition symbols in s
            ICollection<PropositionSymbol> symbols = getPropositionSymbolsInSentence(s);

            // return DPLL(clauses, symbols, {})
            return dpll(clauses, symbols, new Model());
        }

        /**
         * DPLL(clauses, symbols, model)<br>
         * 
         * @param clauses
         *            the set of clauses.
         * @param symbols
         *            a list of unassigned symbols.
         * @param model
         *            contains the values for assigned symbols.
         * @return true if the model is satisfiable under current assignments, false
         *         otherwise.
         */

        private bool currIsCancelled = false;

        public void SetCurrIsCancelled(bool value)
        {
            currIsCancelled = value;
        }

        public bool GetCurrIsCancelled()
        {
            return currIsCancelled;
        }

        public bool dpll(ISet<Clause> clauses, ICollection<PropositionSymbol> symbols, Model model)
        {
            // if every clause in clauses is true in model then return true
            if (everyClauseTrue(clauses, model).Value)
                return true;

            // if some clause in clauses is false in model then return false
            if (someClauseFalse(clauses, model) || currIsCancelled)
                return false;

            // P, value <- FIND-PURE-SYMBOL(symbols, clauses, model)
            Pair<PropositionSymbol, bool?> pAndValue = findPureSymbol(symbols,
                    clauses, model);
            // if P is non-null then
            if (pAndValue != null)
            {
                // return DPLL(clauses, symbols - P, model U {P = value})
                return dpll(clauses, minus(symbols, pAndValue.GetFirst()),
                        model.union(pAndValue.GetFirst(), pAndValue.getSecond()));
            }

            // P, value <- FIND-UNIT-CLAUSE(clauses, model)
            pAndValue = findUnitClause(clauses, model);
            // if P is non-null then
            if (pAndValue != null)
            {
                // return DPLL(clauses, symbols - P, model U {P = value})
                return dpll(clauses, minus(symbols, pAndValue.GetFirst()),
                        model.union(pAndValue.GetFirst(), pAndValue.getSecond()));
            }

            // P <- FIRST(symbols); rest <- REST(symbols)
            PropositionSymbol p = Util.first(symbols);
            ICollection<PropositionSymbol> rest = Util.rest(symbols);
            // return DPLL(clauses, rest, model U {P = true}) or
            // ...... DPLL(clauses, rest, model U {P = false})
            return dpll(clauses, rest, model.union(p, true))
                    || dpll(clauses, rest, model.union(p, false));
        }

        //
        // SUPPORTING CODE
        //

        /**
         * Determine if KB |= &alpha;, i.e. alpha is entailed by KB.
         * 
         * @param kb
         *            a Knowledge Base in propositional logic.
         * @param alpha
         *            a propositional sentence.
         * @return true, if &alpha; is entailed by KB, false otherwise.
         */

        public bool isEntailed(KnowledgeBase kb, Sentence alpha)
        {
            // AIMA3e p.g. 260: kb |= alpha, can be done by testing
            // unsatisfiability of kb & ~alpha.
            ISet<Clause> kbAndNotAlpha = CollectionFactory.CreateSet<Clause>();
            Sentence notQuery = new ComplexSentence(Connective.NOT, alpha);
            ISet<PropositionSymbol> symbols = CollectionFactory.CreateSet<PropositionSymbol>();
            ICollection<PropositionSymbol> querySymbols = CollectionFactory.CreateQueue<PropositionSymbol>(SymbolCollector.getSymbolsFrom(notQuery));

            kbAndNotAlpha.AddAll(kb.asCNF());
            kbAndNotAlpha.AddAll(ConvertToConjunctionOfClauses.convert(notQuery).getClauses());
            symbols.AddAll(querySymbols);
            symbols.AddAll(kb.getSymbols());

            return !dpll(kbAndNotAlpha, CollectionFactory.CreateQueue<PropositionSymbol>(symbols), new Model());
        }

        //
        // PROTECTED:
        //

        // Note: Override this method if you wish to change the initial variable
        // ordering when dpllSatisfiable is called.
        protected ICollection<PropositionSymbol> getPropositionSymbolsInSentence(Sentence s)
        {
            ICollection<PropositionSymbol> result = CollectionFactory.CreateQueue<PropositionSymbol>(
                    SymbolCollector.getSymbolsFrom(s));

            return result;
        }

        /**
         * AIMA3e p.g. 260:<br>
         * <quote><i>Pure symbol heuristic:</i> A <b>pure symbol</b> is a symbol
         * that always appears with the same "sign" in all clauses. For example, in
         * the three clauses (A | ~B), (~B | ~C), and (C | A), the symbol A is pure
         * because only the positive literal appears, B is pure because only the
         * negative literal appears, and C is impure. It is easy to see that if a
         * sentence has a model, then it has a model with the pure symbols assigned
         * so as to make their literals true, because doing so can never make a
         * clause false. Note that, in determining the purity of a symbol, the
         * algorithm can ignore clauses that are already known to be true in the
         * model constructed so far. For example, if the model contains B=false,
         * then the clause (~B | ~C) is already true, and in the remaining clauses C
         * appears only as a positive literal; therefore C becomes pure.</quote>
         * 
         * @param symbols
         *            a list of currently unassigned symbols in the model (to be
         *            checked if pure or not).
         * @param clauses
         * @param model
         * @return a proposition symbol and value pair identifying a pure symbol and
         *         a value to be assigned to it, otherwise null if no pure symbol
         *         can be identified.
         */
        protected Pair<PropositionSymbol, bool?> findPureSymbol(ICollection<PropositionSymbol> symbols, ISet<Clause> clauses, Model model)
        {
            Pair<PropositionSymbol, bool?> result = null;

            ISet<PropositionSymbol> symbolsToKeep = CollectionFactory.CreateSet<PropositionSymbol>(symbols);
            // Collect up possible positive and negative candidate sets of pure
            // symbols
            ISet<PropositionSymbol> candidatePurePositiveSymbols = CollectionFactory.CreateSet<PropositionSymbol>();
            ISet<PropositionSymbol> candidatePureNegativeSymbols = CollectionFactory.CreateSet<PropositionSymbol>();
            foreach (Clause c in clauses)
            {
                // Algorithm can ignore clauses that are already known to be true
                if (true.Equals(model.determineValue(c)))
                {
                    continue;
                }
                // Collect possible candidates, removing all candidates that are
                // not part of the input list of symbols to be considered.
                foreach (PropositionSymbol p in c.getPositiveSymbols())
                {
                    if (symbolsToKeep.Contains(p))
                    {
                        candidatePurePositiveSymbols.Add(p);
                    }
                }
                foreach (PropositionSymbol n in c.getNegativeSymbols())
                {
                    if (symbolsToKeep.Contains(n))
                    {
                        candidatePureNegativeSymbols.Add(n);
                    }
                }
            }

            // Determine the overlap/intersection between the positive and negative
            // candidates
            foreach (PropositionSymbol s in symbolsToKeep)
            {
                // Remove the non-pure symbols
                if (candidatePurePositiveSymbols.Contains(s) && candidatePureNegativeSymbols.Contains(s))
                {
                    candidatePurePositiveSymbols.Remove(s);
                    candidatePureNegativeSymbols.Remove(s);
                }
            }

            // We have an implicit preference for positive pure symbols
            if (candidatePurePositiveSymbols.Size() > 0)
            {
                result = new Pair<PropositionSymbol, bool?>(Util.first(candidatePurePositiveSymbols), true);
            } // We have a negative pure symbol
            else if (candidatePureNegativeSymbols.Size() > 0)
            {
                result = new Pair<PropositionSymbol, bool?>(Util.first(candidatePureNegativeSymbols), false);
            }

            return result;
        }

        /**
         * AIMA3e p.g. 260:<br>
         * <quote><i>Unit clause heuristic:</i> A <b>unit clause</b> was defined
         * earlier as a clause with just one literal. In the context of DPLL, it
         * also means clauses in which all literals but one are already assigned
         * false by the model. For example, if the model contains B = true, then (~B
         * | ~C) simplifies to ~C, which is a unit clause. Obviously, for this
         * clause to be true, C must be set to false. The unit clause heuristic
         * assigns all such symbols before branching on the remainder. One important
         * consequence of the heuristic is that any attempt to prove (by refutation)
         * a literal that is already in the knowledge base will succeed immediately.
         * Notice also that assigning one unit clause can create another unit clause
         * - for example, when C is set to false, (C | A) becomes a unit clause,
         * causing true to be assigned to A. This "cascade" of forced assignments is
         * called <b>unit propagation</b>. It resembles the process of forward
         * chaining with definite clauses, and indeed, if the CNF expression
         * contains only definite clauses then DPLL essentially replicates forward
         * chaining.</quote>
         * 
         * @param clauses
         * @param model
         * @return a proposition symbol and value pair identifying a unit clause and
         *         a value to be assigned to it, otherwise null if no unit clause
         *         can be identified.
         */
        protected Pair<PropositionSymbol, bool?> findUnitClause(
                ISet<Clause> clauses, Model model)
        {
            Pair<PropositionSymbol, bool?> result = null;

            foreach (Clause c in clauses)
            {
                // if clauses value is currently unknown
                // (i.e. means known literals are false)
                if (model.determineValue(c) == null)
                {
                    Literal unassigned = null;
                    // Default definition of a unit clause is a clause with just one literal
                    if (c.isUnitClause())
                    {
                        unassigned = Util.first(c.getLiterals());
                    }
                    else
                    {
                        // Also, a unit clause in the context of DPLL, also means a
                        // clauseF in which all literals but one are already
                        // assigned false by the model.
                        // Note: at this point we already know the clause is not
                        // true, so just need to determine if the clause has a
                        // single unassigned literal
                        foreach (Literal l in c.getLiterals())
                        {
                            bool? value = model.getValue(l.getAtomicSentence());
                            if (value == null)
                            {
                                // The first unassigned literal encountered.
                                if (unassigned == null)
                                {
                                    unassigned = l;
                                }
                                else
                                {
                                    // This means we have more than 1 unassigned
                                    // literal so lets skip
                                    unassigned = null;
                                    break;
                                }
                            }
                        }
                    }

                    // if a value assigned it means we have a single
                    // unassigned literal and all the assigned literals
                    // are not true under the current model as we were
                    // unable to determine a value.
                    if (unassigned != null)
                    {
                        result = new Pair<PropositionSymbol, bool?>(
                                unassigned.getAtomicSentence(),
                                unassigned.isPositiveLiteral());
                        break;
                    }
                }
            }

            return result;
        }

        protected bool? everyClauseTrue(ISet<Clause> clauses, Model model)
        {
            return model.satisfies(clauses);
        }

        protected bool someClauseFalse(ISet<Clause> clauses, Model model)
        {
            foreach (Clause c in clauses)
            {
                // Only 1 needs to be false
                if (false.Equals(model.determineValue(c)))
                {
                    return true;
                }
            }
            return false;
        }

        // symbols - P
        protected ICollection<PropositionSymbol> minus(ICollection<PropositionSymbol> symbols, PropositionSymbol p)
        {
            ICollection<PropositionSymbol> result = CollectionFactory.CreateQueue<PropositionSymbol>();
            foreach (PropositionSymbol s in symbols)
            {
                // symbols - P
                if (!p.Equals(s))
                {
                    result.Add(s);
                }
            }
            return result;
        }
    }
}
