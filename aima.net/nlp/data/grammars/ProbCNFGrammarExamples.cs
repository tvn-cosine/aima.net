using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.data.lexicons;
using aima.net.nlp.parsing;
using aima.net.nlp.parsing.grammars;

namespace aima.net.nlp.data.grammars
{
    /**
     * A store of example Probabilistic Chomsky-Normal-Form grammars for testing and 
     * demonstrating CYK.
     * @author Jonathon
     *
     */
    public class ProbCNFGrammarExamples
    {

        /** 
         * An elementary Chomsky-Normal-Form grammar for simple testing and 
         * demonstrating. This type of grammar is seen more in Computing Theory classes,
         * and does not mock a subset of English phrase-structure.
         * @return
         */
        public static ProbCNFGrammar buildExampleGrammarOne()
        {
            ProbCNFGrammar g = new ProbCNFGrammar();
            ICollection<Rule> rules = CollectionFactory.CreateQueue<Rule>();
            // Start Rules
            rules.Add(new Rule("S", "Y,Z", (float)0.10));
            rules.Add(new Rule("B", "B,D", (float)0.10));
            rules.Add(new Rule("B", "G,D", (float)0.10));
            rules.Add(new Rule("C", "E,C", (float)0.10));
            rules.Add(new Rule("C", "E,H", (float)0.10));
            rules.Add(new Rule("E", "M,N", (float)0.10));
            rules.Add(new Rule("D", "M,N", (float)0.10));
            rules.Add(new Rule("Y", "E,C", (float)0.10));
            rules.Add(new Rule("Z", "E,C", (float)0.10));

            // Terminal Rules
            rules.Add(new Rule("M", "m", (float)1.0));
            rules.Add(new Rule("N", "n", (float)1.0));
            rules.Add(new Rule("B", "a", (float)0.25));
            rules.Add(new Rule("B", "b", (float)0.25));
            rules.Add(new Rule("B", "c", (float)0.25));
            rules.Add(new Rule("B", "d", (float)0.25));
            rules.Add(new Rule("G", "a", (float)0.50));
            rules.Add(new Rule("G", "d", (float)0.50));
            rules.Add(new Rule("C", "x", (float)0.20));
            rules.Add(new Rule("C", "y", (float)0.20));
            rules.Add(new Rule("C", "z", (float)0.60));
            rules.Add(new Rule("H", "u", (float)0.50));
            rules.Add(new Rule("H", "z", (float)0.50));

            // Add all these rules into the grammar
            if (!g.addRules(rules))
            {
                return null;
            }
            return g;
        }

        /**
         * A more restrictive phrase-structure grammar, used in testing and demonstrating 
         * the CYK Algorithm. 
         * Note: It is complemented by the "trivial lexicon" in LexiconExamples.java
         * @return
         */
        public static ProbCNFGrammar buildTrivialGrammar()
        {
            ProbCNFGrammar g = new ProbCNFGrammar();
            ICollection<Rule> rules = CollectionFactory.CreateQueue<Rule>();
            rules.Add(new Rule("S", "NP,VP", (float)1.0));
            rules.Add(new Rule("NP", "ARTICLE,NOUN", (float)0.50));
            rules.Add(new Rule("NP", "PRONOUN,ADVERB", (float)0.5));
            rules.Add(new Rule("VP", "VERB,NP", (float)1.0));
            // add terminal rules
            Lexicon trivLex = LexiconExamples.buildTrivialLexicon();
            ICollection<Rule> terminalRules = CollectionFactory.CreateQueue<Rule>(trivLex.getAllTerminalRules());
            rules.AddAll(terminalRules);
            // Add all these rules into the grammar
            if (!g.addRules(rules))
            {
                return null;
            }
            return g;
        }
    }
}
