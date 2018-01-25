using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.parsing.grammars;

namespace aima.net.test.unit.nlp.parse
{
    [TestClass]
    public class GrammarTest
    {

        ProbUnrestrictedGrammar g;
        //ProbUnrestrictedGrammar g2;

        [TestInitialize]
        public void setup()
        {
            g = new ProbUnrestrictedGrammar(); // reset grammar before each test
        }

        [TestMethod]
        public void testValidRule()
        {
            Rule invalidR = new Rule(null, CollectionFactory.CreateQueue<string>(new[] { "W", "Z" }), (float)0.5);
            Rule validR = new Rule(CollectionFactory.CreateQueue<string>(new[] { "W" }),
                                      CollectionFactory.CreateQueue<string>(new[] { "a", "s" }), (float)0.5);
            Rule validR2 = new Rule(CollectionFactory.CreateQueue<string>(new[] { "W" }), null, (float)0.5);
            Assert.IsFalse(g.validRule(invalidR));
            Assert.IsTrue(g.validRule(validR));
            Assert.IsTrue(g.validRule(validR2));

        }

        /**
         * Grammar should not allow a rule of the form 
         * null -> X, where X is a combo of variables and terminals
         */
        [TestMethod]
        public void testRejectNullLhs()
        {
            Rule r = new Rule(CollectionFactory.CreateQueue<string>(), CollectionFactory.CreateQueue<string>(), (float)0.50); // test completely null rule
                                                                                                          // test only null lhs
            Rule r2 = new Rule(null, CollectionFactory.CreateQueue<string>(new[] { "W", "Z" }), (float)0.50);
            Assert.IsFalse(g.addRule(r));
            Assert.IsFalse(g.addRule(r2));
        }

        /**
         * Grammar (unrestricted) should accept all the rules in the test,
         * as they have non-null left hand sides
         */
        [TestMethod]
        public void testAcceptValidRules()
        {
            Rule unrestrictedRule = new Rule(CollectionFactory.CreateQueue<string>(new[] { "A", "a", "A", "B" }),
                                              CollectionFactory.CreateQueue<string>(new[] { "b", "b", "A", "C" }), (float)0.50);
            Rule contextSensRule = new Rule(CollectionFactory.CreateQueue<string>(new[] { "A", "a", "A" }),
                                                CollectionFactory.CreateQueue<string>(new[] { "b", "b", "A", "C" }), (float)0.50);
            Rule contextFreeRule = new Rule(CollectionFactory.CreateQueue<string>(new[] { "A" }),
                                                CollectionFactory.CreateQueue<string>(new[] { "b", "b", "A", "C" }), (float)0.50);
            Rule regularRule = new Rule(CollectionFactory.CreateQueue<string>(new[] { "A" }),
                                               CollectionFactory.CreateQueue<string>(new[] { "b", "C" }), (float)0.50);
            Rule nullRHSRule = new Rule(CollectionFactory.CreateQueue<string>(new[] { "A", "B" }), null, (float)0.50);
            // try adding these rules in turn
            Assert.IsTrue(g.addRule(unrestrictedRule));
            Assert.IsTrue(g.addRule(contextSensRule));
            Assert.IsTrue(g.addRule(contextFreeRule));
            Assert.IsTrue(g.addRule(regularRule));
            Assert.IsTrue(g.addRule(nullRHSRule));
        }

        /**
         * Test that Grammar class correctly updates its 
         * list of variables and terminals when a new rule is added
         */
        [TestMethod]
        public void testUpdateVarsAndTerminals()
        {
            // add a rule that has variables and terminals not 
            // already in the grammar
            g.addRule(new Rule(CollectionFactory.CreateQueue<string>(new[] { "Z" }),
                                   CollectionFactory.CreateQueue<string>(new[] { "z", "Z" }), (float)0.50));
            Assert.IsTrue(g.terminals.Contains("z") && !g.terminals.Contains("Z"));
            Assert.IsTrue(g.vars.Contains("Z") && !g.vars.Contains("z"));
        }

        [TestMethod]
        public void testIsVariable()
        {
            Assert.IsTrue(ProbUnrestrictedGrammar.isVariable("S"));
            Assert.IsTrue(ProbUnrestrictedGrammar.isVariable("SSSSS"));
            Assert.IsFalse(ProbUnrestrictedGrammar.isVariable("s"));
            Assert.IsFalse(ProbUnrestrictedGrammar.isVariable("tttt"));
        }

        [TestMethod]
        public void testIsTerminal()
        {
            Assert.IsTrue(ProbUnrestrictedGrammar.isTerminal("x"));
            Assert.IsTrue(ProbUnrestrictedGrammar.isTerminal("xxxxx"));
            Assert.IsFalse(ProbUnrestrictedGrammar.isTerminal("X"));
            Assert.IsFalse(ProbUnrestrictedGrammar.isTerminal("XXXXXX"));
        }
    }

}
