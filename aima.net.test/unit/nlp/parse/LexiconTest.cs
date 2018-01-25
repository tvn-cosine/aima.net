using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections.api;
using aima.net.nlp.data.lexicons;
using aima.net.nlp.parsing;
using aima.net.nlp.parsing.grammars;

namespace aima.net.test.unit.nlp.parse
{
    [TestClass]
    public class LexiconTest
    {

        Lexicon l;
        Lexicon wumpusLex;

        [TestInitialize]
        public void setUp()
        {
            l = new Lexicon();
            wumpusLex = LexiconExamples.buildWumpusLex();
        }

        [TestMethod]
        public void testAddEntry()
        {
            l.addEntry("EXAMPLE", "word", (float)0.10);
            Assert.IsTrue(l.ContainsKey("EXAMPLE"));
            Assert.AreEqual(l.Get("EXAMPLE").Size(), 1);
        }

        [TestMethod]
        public void testAddEntryExistingCategory()
        {
            l.addEntry("EXAMPLE", "word", (float)0.10);
            l.addEntry("EXAMPLE", "second", (float)0.90);
            Assert.IsTrue(l.ContainsKey("EXAMPLE"));
            Assert.IsTrue(l.GetKeys().Size() == 1);
            Assert.IsTrue(l.Get("EXAMPLE").Get(1).getWord().Equals("second"));

        }

        [TestMethod]
        public void testAddLexWords()
        {
            string key = "EXAMPLE";
            l.addLexWords(key, "stench", "0.05", "breeze", "0.10", "wumpus", "0.15");
            Assert.IsTrue(l.Get(key).Size() == 3);
            Assert.IsTrue(l.Get(key).Get(0).getWord().Equals("stench"));

        }

        [TestMethod]
        public void testAddLexWordsWithInvalidArgs()
        {
            string key = "EXAMPLE";
            Assert.IsFalse(l.addLexWords(key, "stench", "0.05", "breeze"));
            Assert.IsFalse(l.ContainsKey(key));
        }

        [TestMethod]
        public void testGetTerminalRules()
        {
            string key1 = "A"; string key2 = "B"; string key3 = "C";
            l.addLexWords(key1, "apple", "0.25", "alpha", "0.5", "arrow", "0.25");
            l.addLexWords(key2, "ball", "0.25", "bench", "0.25", "blue", "0.25", "bell", "0.25");
            l.addLexWords(key3, "carrot", "0.25", "canary", "0.5", "caper", "0.25");
            ICollection<Rule> rules1 = l.getTerminalRules(key1);
            ICollection<Rule> rules2 = l.getTerminalRules(key2);
            ICollection<Rule> rules3 = l.getTerminalRules(key3);
            Assert.AreEqual(rules1.Size(), 3);
            Assert.AreEqual(rules1.Get(0).rhs.Get(0), "apple");
            Assert.AreEqual(rules2.Size(), 4);
            Assert.AreEqual(rules2.Get(3).rhs.Get(0), "bell");
            Assert.AreEqual(rules3.Size(), 3);
            Assert.AreEqual(rules3.Get(1).lhs.Get(0), "C");
        }

        [TestMethod]
        public void testGetAllTerminalRules()
        {
            string key1 = "A"; string key2 = "B"; string key3 = "C";
            l.addLexWords(key1, "apple", "0.25", "alpha", "0.5", "arrow", "0.25");
            l.addLexWords(key2, "ball", "0.25", "bench", "0.25", "blue", "0.25", "bell", "0.25");
            l.addLexWords(key3, "carrot", "0.25", "canary", "0.5", "caper", "0.25");
            ICollection<Rule> allRules = l.getAllTerminalRules();
            Assert.AreEqual(allRules.Size(), 10);
            Assert.IsTrue(allRules.Get(0).rhs.Get(0).Equals("apple") ||
                        allRules.Get(0).rhs.Get(0).Equals("ball") ||
                        allRules.Get(0).rhs.Get(0).Equals("carrot"));
        } 
    }

}
