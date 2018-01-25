using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.data.lexicons;
using aima.net.nlp.parsing;
using aima.net.nlp.parsing.grammars;

namespace aima.net.nlp.data.grammars
{
    public class ProbContextFreeExamples
    { 
        public static ProbContextFreeGrammar buildWumpusGrammar()
        {
            ProbContextFreeGrammar g = new ProbContextFreeGrammar();
            ICollection<Rule> rules = CollectionFactory.CreateQueue<Rule>();
            // Start Rules
            rules.Add(new Rule("S", "NP,VP", (float)0.90));
            rules.Add(new Rule("S", "CONJ,S", (float)0.10));
            // Noun Phrase Rules
            rules.Add(new Rule("NP", "PRONOUN", (float)0.30));
            rules.Add(new Rule("NP", "NAME", (float)0.10));
            rules.Add(new Rule("NP", "NOUN", (float)0.10));
            rules.Add(new Rule("NP", "ARTICLE,NOUN", (float)0.25));
            rules.Add(new Rule("NP", "AP,NOUN", (float)0.05));
            rules.Add(new Rule("NP", "DIGIT,DIGIT", (float)0.05));
            rules.Add(new Rule("NP", "NP,PP", (float)0.10));
            rules.Add(new Rule("NP", "NP,RELCLAUSE", (float)0.05));
            // add verb phrase rules
            rules.Add(new Rule("VP", "VERB", (float)0.40));
            rules.Add(new Rule("VP", "VP,NP", (float)0.35));
            rules.Add(new Rule("VP", "VP,ADJS", (float)0.05));
            rules.Add(new Rule("VP", "VP,PP", (float)0.10));
            rules.Add(new Rule("VP", "VP,ADVERB", (float)0.10));
            // add adjective rules
            rules.Add(new Rule("AJD", "AJDS", (float)0.80));
            rules.Add(new Rule("AJD", "AJD,AJDS", (float)0.20));
            // add Article Phrase
            // This deviates from the text because the text provides the rule:
            // NP -> Article Adjs Noun, which is NOT in Chomsky Normal Form
            //
            // We instead define AP (Article Phrase) AP -> Article Adjs, to get around this
            rules.Add(new Rule("AP", "ARTICLE,ADJS", (float)1.0));
            // add preposition phrase
            rules.Add(new Rule("PP", "PREP,NP", (float)1.00));
            // add relative clause
            rules.Add(new Rule("RELCLAUSE", "RELPRO,VP", (float)1.00));

            // Now we can add all rules that derive terminal symbols, which are in 
            // this case words.
            Lexicon wumpusLex = LexiconExamples.buildWumpusLex();
            ICollection<Rule> terminalRules = CollectionFactory.CreateQueue<Rule>(wumpusLex.getAllTerminalRules());
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
