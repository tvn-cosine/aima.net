using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.data.grammars;
using aima.net.nlp.parsing.grammars;

namespace aima.net.test.unit.nlp.parse
{
    [TestClass]
    public class ProbContextFreeGrammarTest
    {
        ProbUnrestrictedGrammar g;
        ProbUnrestrictedGrammar cfG;

        [TestInitialize]
        public void setup()
        {
            g = new ProbUnrestrictedGrammar();
            cfG = ProbContextFreeExamples.buildWumpusGrammar();
        }

        [TestMethod]
        public void testValidRule()
        {
            // This rule is a valid Context-Free rule
            Rule validR = new Rule(CollectionFactory.CreateQueue<string>(new[] { "W" }),
                                   CollectionFactory.CreateQueue<string>(new[] { "a", "s" }), (float)0.5);
            // This rule is of correct form but not a context-free rule
            Rule invalidR = new Rule(CollectionFactory.CreateQueue<string>(new[] { "W", "A" }), null, (float)0.5);
            Assert.IsFalse(cfG.validRule(invalidR));
            Assert.IsTrue(cfG.validRule(validR));
        }

    }

}
