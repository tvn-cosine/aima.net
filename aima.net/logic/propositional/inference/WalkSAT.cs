using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.propositional.kb.data;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.logic.propositional.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 263.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function WALKSAT(clauses, p, max_flips) returns a satisfying model or failure
     *   inputs: clauses, a set of clauses in propositional logic
     *           p, the probability of choosing to do a "random walk" move, typically around 0.5
     *           max_flips, number of flips allowed before giving up
     *           
     *   model <- a random assignment of true/false to the symbols in clauses
     *   for i = 1 to max_flips do
     *       if model satisfies clauses then return model
     *       clause <- a randomly selected clause from clauses that is false in model
     *       with probability p flip the value in model of a randomly selected symbol from clause
     *       else flip whichever symbol in clause maximizes the number of satisfied clauses
     *   return failure
     * </code>
     * </pre>
     * 
     * Figure 7.18 The WALKSAT algorithm for checking satisfiability by randomly
     * flipping the values of variables. Many versions of the algorithm exist.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class WalkSAT
    {

        /**
         * WALKSAT(clauses, p, max_flips)<br>
         * 
         * @param clauses
         *            a set of clauses in propositional logic
         * @param p
         *            the probability of choosing to do a "random walk" move,
         *            typically around 0.5
         * @param maxFlips
         *            number of flips allowed before giving up. Note: a value < 0 is
         *            interpreted as infinity.
         * 
         * @return a satisfying model or failure (null).
         */
        public Model walkSAT(ISet<Clause> clauses, double p, int maxFlips)
        {
            assertLegalProbability(p);

            // model <- a random assignment of true/false to the symbols in clauses
            Model model = randomAssignmentToSymbolsInClauses(clauses);
            // for i = 1 to max_flips do (Note: maxFlips < 0 means infinity)
            for (int i = 0; i < maxFlips || maxFlips < 0;++i)
            {
                // if model satisfies clauses then return model
                if (model.satisfies(clauses).Value)
                {
                    return model;
                }

                // clause <- a randomly selected clause from clauses that is false
                // in model
                Clause clause = randomlySelectFalseClause(clauses, model);

                // with probability p flip the value in model of a randomly selected
                // symbol from clause
                if (random.NextDouble() < p)
                {
                    model = model.flip(randomlySelectSymbolFromClause(clause));
                }
                else
                {
                    // else flip whichever symbol in clause maximizes the number of
                    // satisfied clauses
                    model = flipSymbolInClauseMaximizesNumberSatisfiedClauses(
                            clause, clauses, model);
                }
            }

            // return failure
            return null;
        }

        private IRandom random = CommonFactory.CreateRandom();

        /**
         * Default Constructor.
         */
        public WalkSAT()
        { }

        /**
         * Constructor.
         * 
         * @param random
         *            the random generator to be used by the algorithm.
         */
        public WalkSAT(IRandom random)
        {
            this.random = random;
        }

        //
        // PROTECTED
        //
        protected void assertLegalProbability(double p)
        {
            if (p < 0 || p > 1)
            {
                throw new IllegalArgumentException("p is not a legal propbability value [0-1]: " + p);
            }
        }

        protected Model randomAssignmentToSymbolsInClauses(ISet<Clause> clauses)
        {
            // Collect the symbols in clauses
            ISet<PropositionSymbol> symbols = CollectionFactory.CreateSet<PropositionSymbol>();
            foreach (Clause c in clauses)
            {
                symbols.AddAll(c.getSymbols());
            }

            // Make initial set of assignments
            IMap<PropositionSymbol, bool?> values = CollectionFactory.CreateInsertionOrderedMap<PropositionSymbol, bool?>();
            foreach (PropositionSymbol symbol in symbols)
            {
                // a random assignment of true/false to the symbols in clauses
                values.Put(symbol, random.NextBoolean());
            }

            Model result = new Model(values);

            return result;
        }

        protected Clause randomlySelectFalseClause(ISet<Clause> clauses, Model model)
        {
            // Collect the clauses that are false in the model
            ICollection<Clause> falseClauses = CollectionFactory.CreateQueue<Clause>();
            foreach (Clause c in clauses)
            {
                if (false.Equals(model.determineValue(c)))
                {
                    falseClauses.Add(c);
                }
            }

            // a randomly selected clause from clauses that is false
            Clause result = falseClauses.Get(random.Next(falseClauses.Size()));
            return result;
        }

        protected PropositionSymbol randomlySelectSymbolFromClause(Clause clause)
        {
            // all the symbols in clause
            ISet<PropositionSymbol> symbols = clause.getSymbols();

            // a randomly selected symbol from clause
            PropositionSymbol result = (CollectionFactory.CreateQueue<PropositionSymbol>(symbols)).Get(random.Next(symbols.Size()));
            return result;
        }

        protected Model flipSymbolInClauseMaximizesNumberSatisfiedClauses(Clause clause, ISet<Clause> clauses, Model model)
        {
            Model result = model;

            // all the symbols in clause
            ISet<PropositionSymbol> symbols = clause.getSymbols();
            int maxClausesSatisfied = -1;
            foreach (PropositionSymbol symbol in symbols)
            {
                Model flippedModel = result.flip(symbol);
                int numberClausesSatisfied = 0;
                foreach (Clause c in clauses)
                {
                    if (true.Equals(flippedModel.determineValue(c)))
                    {
                        numberClausesSatisfied++;
                    }
                }
                // test if this symbol flip is the new maximum
                if (numberClausesSatisfied > maxClausesSatisfied)
                {
                    result = flippedModel;
                    maxClausesSatisfied = numberClausesSatisfied;
                    if (numberClausesSatisfied == clauses.Size())
                    {
                        // i.e. satisfies all clauses
                        break; // this is our goal.
                    }
                }
            }

            return result;
        }
    }
}
