using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.propositional.parsing;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.logic.propositional.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pages 240, 245.<br>
     * <br>
     * Models are mathematical abstractions, each of which simply fixes the truth or
     * falsehood of every relevant sentence. In propositional logic, a model simply
     * fixes the <b>truth value</b> - <em>true</em> or <em>false</em> - for
     * every proposition symbol.<br>
     * <br>
     * Models as implemented here can represent partial assignments 
     * to the set of proposition symbols in a Knowledge Base (i.e. a partial model).
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class Model : PLVisitor<bool?, bool?>
    {
        private IMap<PropositionSymbol, bool?> assignments = CollectionFactory.CreateInsertionOrderedMap<PropositionSymbol, bool?>();

        /**
         * Default Constructor.
         */
        public Model()
        { }

        public Model(IMap<PropositionSymbol, bool?> values)
        {
            assignments.PutAll(values);
        }

        public bool? getValue(PropositionSymbol symbol)
        {
            return assignments.Get(symbol);
        }

        public bool isTrue(PropositionSymbol symbol)
        {
            return true.Equals(assignments.Get(symbol));
        }

        public bool isFalse(PropositionSymbol symbol)
        {
            return false.Equals(assignments.Get(symbol));
        }

        public Model union(PropositionSymbol symbol, bool? b)
        {
            Model m = new Model();
            m.assignments.PutAll(this.assignments);
            m.assignments.Put(symbol, b);
            return m;
        }

        public Model unionInPlace(PropositionSymbol symbol, bool? b)
        {
            assignments.Put(symbol, b);
            return this;
        }

        public bool remove(PropositionSymbol p)
        {
            return assignments.Remove(p);
        }

        public bool isTrue(Sentence s)
        {
            return true.Equals(s.accept(this, null));
        }

        public bool isFalse(Sentence s)
        {
            return false.Equals(s.accept(this, null));
        }

        public bool isUnknown(Sentence s)
        {
            return null == s.accept(this, null);
        }

        public Model flip(PropositionSymbol s)
        {
            if (isTrue(s))
            {
                return union(s, false);
            }
            if (isFalse(s))
            {
                return union(s, true);
            }
            return this;
        }

        public ISet<PropositionSymbol> getAssignedSymbols()
        {
            return CollectionFactory.CreateReadOnlySet<PropositionSymbol>(assignments.GetKeys());
        }

        /**
         * Determine if the model satisfies a set of clauses.
         * 
         * @param clauses
         *            a set of propositional clauses.
         * @return if the model satisfies the clauses, false otherwise.
         */
        public bool? satisfies(ISet<Clause> clauses)
        {
            foreach (Clause c in clauses)
            {
                // All must to be true
                if (!true.Equals(determineValue(c)))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * Determine based on the current assignments within the model, whether a
         * clause is known to be true, false, or unknown.
         * 
         * @param c
         *            a propositional clause.
         * @return true, if the clause is known to be true under the model's
         *         assignments. false, if the clause is known to be false under the
         *         model's assignments. null, if it is unknown whether the clause is
         *         true or false under the model's current assignments.
         */
        public bool? determineValue(Clause c)
        {
            bool? result = null; // i.e. unknown

            if (c.isTautology().Value)
            { // Test independent of the model's assignments.
                result = true;
            }
            else if (c.isFalse())
            { // Test independent of the model's
              // assignments.
                result = false;
            }
            else
            {
                bool unassignedSymbols = false;
                bool? value = null;
                foreach (PropositionSymbol positive in c.getPositiveSymbols())
                {
                    value = assignments.Get(positive);
                    if (value != null)
                    {
                        if (true.Equals(value))
                        {
                            result = true;
                            break;
                        }
                    }
                    else
                    {
                        unassignedSymbols = true;
                    }
                }
                // If truth not determined, continue checking negative symbols
                if (result == null)
                {
                    foreach (PropositionSymbol negative in c.getNegativeSymbols())
                    {
                        value = assignments.Get(negative);
                        if (value != null)
                        {
                            if (false.Equals(value))
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            unassignedSymbols = true;
                        }
                    }

                    if (result == null)
                    {
                        // If truth not determined and there are no
                        // unassigned symbols then we can determine falsehood
                        // (i.e. all of its literals are assigned false under the
                        // model)
                        if (!unassignedSymbols)
                        {
                            result = false;
                        }
                    }
                }
            }

            return result;
        }

        public void print()
        {
            foreach (var e in assignments)
            {
                System.Console.Write(e.GetKey() + " = " + e.GetValue() + " ");
            }
            System.Console.WriteLine();
        }


        public override string ToString()
        {
            return assignments.ToString();
        }

        public bool? visitPropositionSymbol(PropositionSymbol s, bool? arg)
        {
            if (s.isAlwaysTrue())
            {
                return true;
            }
            if (s.isAlwaysFalse())
            {
                return false;
            }
            return getValue(s);
        }


        public bool? visitUnarySentence(ComplexSentence fs, bool? arg)
        {
            object negatedValue = fs.getSimplerSentence(0).accept(this, null);
            if (negatedValue != null)
            {
                return !((bool)negatedValue);
            }
            else
            {
                return null;
            }
        }


        public bool? visitBinarySentence(ComplexSentence bs, bool? arg)
        {
            bool? firstValue = bs.getSimplerSentence(0).accept(this, null);
            bool? secondValue = bs.getSimplerSentence(1).accept(this, null);
            if ((firstValue == null) || (secondValue == null))
            {
                // strictly not true for or/and
                // -FIX later
                return null;
            }
            else
            {
                Connective connective = bs.getConnective();
                if (connective.Equals(Connective.AND))
                {
                    return firstValue.Value && secondValue.Value;
                }
                else if (connective.Equals(Connective.OR))
                {
                    return firstValue.Value || secondValue.Value;
                }
                else if (connective.Equals(Connective.IMPLICATION))
                {
                    return !(firstValue.Value && !secondValue.Value);
                }
                else if (connective.Equals(Connective.BICONDITIONAL))
                {
                    return firstValue.Equals(secondValue);
                }
                return null;
            }
        }
    }
}
