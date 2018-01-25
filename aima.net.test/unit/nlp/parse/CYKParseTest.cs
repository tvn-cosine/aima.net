using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.data.grammars;
using aima.net.nlp.parsing;
using aima.net.nlp.parsing.grammars;

namespace aima.net.test.unit.nlp.parse
{
    [TestClass]
    public class CYKParseTest
    {

        CYK parser;
        ICollection<string> words1;
     //   IQueue<string> words2;
        ProbCNFGrammar trivGrammar = ProbCNFGrammarExamples.buildTrivialGrammar();
        // Get Example Grammar 2

        [TestInitialize]
        public void setUp()
        {
            parser = new CYK();
            words1 = CollectionFactory.CreateQueue<string>(new[] { "the", "man", "liked", "a", "woman" });

        } // end setUp()

        [TestMethod]
        public void testParseReturn()
        {
            float[,,] probTable = null;
            probTable = parser.parse(words1, trivGrammar);
            Assert.IsNotNull(probTable);
        }

        [TestMethod]
        public void testParse()
        {
            float[,,] probTable;
            probTable = parser.parse(words1, trivGrammar);
            Assert.IsTrue(probTable[5,0,4] > 0); // probTable[5,0,4] = [S][Start=0][Length=5] 
        }
    } 
}
