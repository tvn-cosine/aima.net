using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.ranking;

namespace aima.net.test.unit.nlp.rank
{
    [TestClass] public class HITSTest
    {

        HITS hits;

        [TestInitialize]
        public void setUp()
        {
            IMap<string, Page> pageTable = PagesDataset.loadTestPages();
            hits = new HITS(pageTable);
        }

        [TestMethod]
        public void testMatches()
        {
            string query = "purple horse";
            string queryTwo = "puurple horse";
            string queryThree = "green";
            string text = "This text contains the words 'purple horse' and the word 'green'";
            Assert.IsTrue(hits.matches(query, text));
            Assert.IsFalse(hits.matches(queryTwo, text));
            Assert.IsTrue(hits.matches(queryThree, text));
        }

        [TestMethod]
        public void testNormalize()
        {
          ICollection<Page> pages = CollectionFactory.CreateQueue<Page>();
            Page p1 = new Page(""); Page p2 = new Page("");
            Page p3 = new Page(""); Page p4 = new Page("");
            p1.hub = 3; p1.authority = 2;
            p2.hub = 2; p2.authority = 3;
            p3.hub = 1; p1.authority = 4;
            p4.hub = 0; p4.authority = 10;
            pages.Add(p1); pages.Add(p2); pages.Add(p3); pages.Add(p4);
            // hub total will be 9 + 4 + 1 + 0 = 14
            // authority total will 4 + 9 + 16 + 100 = 129
            double p1HubNorm = 0.214285; double p2HubNorm = 0.142857;
            hits.normalize(pages);
            Assert.AreEqual(  p1HubNorm, pages.Get(0).hub, 0.02);
            Assert.AreEqual(  pages.Get(1).hub, p2HubNorm, 0.02);
        } 
     } 
}
