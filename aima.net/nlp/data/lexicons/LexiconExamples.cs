using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.parsing;

namespace aima.net.nlp.data.lexicons
{
    /**
     * A store of simple lexicon's for the purpose of testing and demonstrating the CYK Algorithm.
     * @author Jonathon
     *
     */
    public class LexiconExamples
    { 
        /**
         * Builds an expanded version of the 'wumpus lexicon' found on page 891 of AIMA V3
         * @return
         */
        public static Lexicon buildWumpusLex()
        {
            Lexicon l = new Lexicon();
            ICollection<LexWord> list = CollectionFactory.CreateQueue<LexWord>();
            // noun list
            list.Add(new LexWord("stench", (float)0.05));
            list.Add(new LexWord("breeze", (float)0.10));
            list.Add(new LexWord("wumpus", (float)0.15));
            list.Add(new LexWord("pits", (float)0.05));
            list.Add(new LexWord("friend", (float)0.10)); // not in textbook
            list.Add(new LexWord("enemy", (float)0.10)); // not in textbook
            list.Add(new LexWord("dog", (float)0.10)); // not in textbook
            list.Add(new LexWord("superhero", (float)0.20)); // not in textbook
            list.Add(new LexWord("virus", (float)0.15)); // not in textbook
            l.Put("NOUN", list);
            // verb list
            ICollection<LexWord> verbList = CollectionFactory.CreateQueue<LexWord>();
            verbList.Add(new LexWord("is", (float)0.10));
            verbList.Add(new LexWord("feel", (float)0.10));
            verbList.Add(new LexWord("smells", (float)0.10));
            verbList.Add(new LexWord("stinks", (float)0.05));
            verbList.Add(new LexWord("wants", (float)0.20)); // not in textbook
            verbList.Add(new LexWord("flies", (float)0.10)); // not in textbook
            verbList.Add(new LexWord("keeps", (float)0.05)); // not in textbook
            verbList.Add(new LexWord("leaves", (float)0.10)); // not in textbook
            verbList.Add(new LexWord("throws", (float)0.20)); // not in textbook
            l.Put("VERB", verbList);
            // adjective list
            ICollection<LexWord> adjList = CollectionFactory.CreateQueue<LexWord>();
            adjList.Add(new LexWord("right", (float)0.10));
            adjList.Add(new LexWord("dead", (float)0.05));
            adjList.Add(new LexWord("smelly", (float)0.02));
            adjList.Add(new LexWord("breezy", (float)0.02));
            adjList.Add(new LexWord("foul", (float)0.10));
            adjList.Add(new LexWord("black", (float)0.05));
            adjList.Add(new LexWord("white", (float)0.05));
            adjList.Add(new LexWord("callous", (float)0.10));
            adjList.Add(new LexWord("proud", (float)0.10));
            adjList.Add(new LexWord("right", (float)0.10));
            adjList.Add(new LexWord("gold", (float)0.06));
            adjList.Add(new LexWord("normal", (float)0.25));
            l.Put("ADJS", adjList);
            // Adverb list
            l.addLexWords("ADVERB", "here", "0.05", "ahead", "0.05", "nearby", "0.02",
                          "quickly", "0.05", "badly", "0.05", "slowly", "0.08",
                          "sadly", "0.10", "silently", "0.10", "easily", "0.10",
                          "seldom", "0.10", "sometimes", "0.10", "loudly", "0.10",
                          "cordially", "0.05", "frequently", "0.05");
            // Pronoun list
            l.addLexWords("PRONOUN", "me", "0.10", "you", "0.03", "i", "0.10", "it", "0.10", // remember "I" has to be lowercase "i"
                                    "us", "0.07", "they", "0.20", "he", "0.20", "she", "0.20");
            // RelPro
            l.addLexWords("RELPRO", "that", "0.40", "which", "0.15", "who", "0.20", "whom", "0.02",
                                    "whose", "0.08", "whabt", "0.15");
            // Name list
            l.addLexWords(buildNameLexicon());

            // Article list
            l.addLexWords("ARTICLE", "the", "0.40", "a", "0.30", "an", "0.10", "every", "0.05", "some", "0.15");

            // Prepositions list
            l.addLexWords("PREP", "to", "0.20", "in", "0.10", "on", "0.05", "near", "0.10", "after", "0.10",
                                  "among", "0.05", "around", "0.20", "against", "0.10", "across", "0.10");

            // Conjugations list
            l.addLexWords("CONJ", "and", "0.50", "or", "0.10", "but", "0.20", "yet", "0.02", "since", "0.08",
                                  "unless", "0.10");

            // Digits list
            l.addLexWords("DIGIT", "0", "0.20", "1", "0.20", "2", "0.20", "3", "0.20", "4", "0.20");

            return l;
        }

        /**
         * A lexicon of names that complements the 'wumpus lexicon' above. There are 50 names 
         * of equal derivation likelihood (%2)
         * @return
         */
        public static Lexicon buildNameLexicon()
        {
            Lexicon l = new Lexicon();
            string[] names = {"John","Mary","Boston","Xiao","Hollie","Kendrick","Beverly"
                          ,"Garnet","Zora","Shavonda","Peg","Katherin","Beatriz","Deirdre","Gaylord"
                          ,"Desirae","Tresa","Gwyneth","Rashida","Garfield","Pinkie","Claretta","Teressa"
                          ,"Andy","Eugena","Carie","Dinorah","Tess","Johnie","Keely","Antonetta","Darcey"
                          ,"Bud","Veta","Janey","Rosalina","Frederica","Lou","Essie","Marinda","Elene"
                          ,"Juliana","Marilyn","Maxima","Branden","Ethan","Donovan","Erinn","Ramon","Jacquiline"};

            for (int i = 0; i < names.Length;++i)
            {
                l.addLexWords("NAME", names[i], "0.02");
            }

            return l;

        }

        /**
         * A more restraining lexicon for simple testing and demonstration.
         * @return
         */
        public static Lexicon buildTrivialLexicon()
        {
            Lexicon l = new Lexicon();
            l.addLexWords("ARTICLE", "the", "0.50", "a", "0.50");
            l.addLexWords("NOUN", "man", "0.20", "woman", "0.20", "table", "0.20", "shoelace", "0.20", "saw", "0.20");
            l.addLexWords("PRONOUN", "i", "0.40", "you", "0.40", "it", "0.20"); // remember "I" has to be lowercase "i"
            l.addLexWords("VERB", "saw", "0.30", "liked", "0.30", "feel", "0.40");
            l.addLexWords("ADVERB", "happily", "0.30", "sadly", "0.20", "morosely", "0.50");
            return l;
        } 
    }
}
