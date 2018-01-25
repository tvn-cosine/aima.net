using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.logic.propositional.visitors
{
    /**
     * Utility class for collecting propositional symbols from sentences. Will
     * exclude the always false and true symbols.
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class SymbolCollector : BasicGatherer<PropositionSymbol>
    { 
        /**
         * Collect a set of propositional symbols from a list of given sentences.
         * 
         * @param sentences
         *            a list of sentences from which to collect symbols.
         * @return a set of all the proposition symbols that are not always true or
         *         false contained within the input sentences.
         */
        public static ISet<PropositionSymbol> getSymbolsFrom(params Sentence[] sentences)
        {
            ISet<PropositionSymbol> result = CollectionFactory.CreateSet<PropositionSymbol>();

            SymbolCollector symbolCollector = new SymbolCollector();
            foreach (Sentence s in sentences)
            {
                result = s.accept(symbolCollector, result);
            }

            return result;
        }


        public override ISet<PropositionSymbol> visitPropositionSymbol(PropositionSymbol s, ISet<PropositionSymbol> arg)
        {
            // Do not add the always true or false symbols
            if (!s.isAlwaysTrue() && !s.isAlwaysFalse())
            {
                arg.Add(s);
            }
            return arg;
        }
    }
}
