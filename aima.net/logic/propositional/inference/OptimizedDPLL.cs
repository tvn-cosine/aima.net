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
    public class OptimizedDPLL : DPLL
    {
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
            // if some clause in clauses is false in model then return false
            // NOTE: for optimization reasons we only want to determine the
            // values of clauses once on each call to dpll
            bool allTrue = true;
            ISet<Clause> unknownClauses = CollectionFactory.CreateSet<Clause>();
            foreach (Clause c in clauses)
            {
                bool? value = model.determineValue(c);
                if (!true.Equals(value))
                {
                    allTrue = false;
                    if (false.Equals(value))
                    {
                        return false;
                    }
                    unknownClauses.Add(c);
                }
            }
            if (allTrue)
            {
                return true;
            }
            else if (currIsCancelled)
                return false;

            // NOTE: Performance Optimization -
            // Going forward, algorithm can ignore clauses that are already 
            // known to be true (reduces overhead on recursive calls and simplifies
            // findPureSymbols() and findUnitClauses() logic as they can
            // always assume unknown).
            clauses = unknownClauses;

            // P, value <- FIND-PURE-SYMBOL(symbols, clauses, model)
            Pair<PropositionSymbol, bool?> pAndValue = findPureSymbol(symbols, clauses, model);
            // if P is non-null then
            if (pAndValue != null)
            {
                // return DPLL(clauses, symbols - P, model U {P = value})
                return callDPLL(clauses, minus(symbols, pAndValue.GetFirst()), model, pAndValue.GetFirst(), pAndValue.getSecond());
            }

            // P, value <- FIND-UNIT-CLAUSE(clauses, model)
            pAndValue = findUnitClause(clauses, model);
            // if P is non-null then
            if (pAndValue != null)
            {
                // return DPLL(clauses, symbols - P, model U {P = value})
                return callDPLL(clauses, minus(symbols, pAndValue.GetFirst()), model, pAndValue.GetFirst(), pAndValue.getSecond());
            }

            // P <- FIRST(symbols); rest <- REST(symbols)
            PropositionSymbol p = Util.first(symbols);
            ICollection<PropositionSymbol> rest = Util.rest(symbols);
            // return DPLL(clauses, rest, model U {P = true}) or
            // ...... DPLL(clauses, rest, model U {P = false})
            return callDPLL(clauses, rest, model, p, true)
                    || callDPLL(clauses, rest, model, p, false);
        }

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
        // END-DPLL
        //

        //
        // PROTECTED
        //

        // Note: Override this method if you wish to change the initial variable
        // ordering when dpllSatisfiable is called.
        protected ICollection<PropositionSymbol> getPropositionSymbolsInSentence(Sentence s)
        {
            return CollectionFactory.CreateQueue<PropositionSymbol>(SymbolCollector.getSymbolsFrom(s));
        }

        protected bool callDPLL(ISet<Clause> clauses, ICollection<PropositionSymbol> symbols,
                Model model, PropositionSymbol p, bool? value)
        {
            // We update the model in place with the assignment p=value,
            bool result = dpll(clauses, symbols, model.unionInPlace(p, value));
            // as backtracking can occur during the recursive calls we
            // need to remove the assigned value before we pop back out from this
            // call.
            model.remove(p);
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
                // NOTE: no longer need to do this here as we remove, true clauses
                // up front in the dpll call (as an optimization)

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
                // NOTE: no longer need to perform this check
                // as only clauses with unknown values will
                // be passed to this routine from dpll as it
                // removes known ones up front.
                Literal unassigned = null;
                // Default definition of a unit clause is a clause
                // with just one literal
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

            return result;
        }

        // symbols - P
        protected ICollection<PropositionSymbol> minus(ICollection<PropositionSymbol> symbols, PropositionSymbol p)
        {
            ICollection<PropositionSymbol> result = CollectionFactory.CreateQueue<PropositionSymbol>();
            foreach (PropositionSymbol s in symbols)
            {
                // symbols - P
                if (!p.Equals(s))
                    result.Add(s);
            }
            return result;
        }
    }
}
