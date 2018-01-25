using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.nlp.ranking;

namespace aima.net.test.unit.nlp.rank
{
    [TestClass]
    public class WikiLinkFinderTest
    {

        Page testPage; IMap<string, Page> pageTable;
        WikiLinkFinder wLF;

        [TestInitialize]
        public void setUp()
        {
            testPage = new Page("tester");
            pageTable = CollectionFactory.CreateInsertionOrderedMap<string, Page>();
            wLF = new WikiLinkFinder();
        }

        [TestMethod]
        public void testGetOutlinks()
        {
            ICollection<string> outLinks;
            ICollection<string> validLinks = CollectionFactory.CreateQueue<string>(new[] { "/wiki/thisisthefinallink" });
            string content = "Some example text with certain <aa href=\"link1\"></aa> links"
                    + "inside. Here is another href=\"link2\" without the surrounding tags. "
                    + "This isn't a link because there are no quotes -> href=notALink. The following"
                    + "is a link < href=\"www.link3.com\" ></> and should be found. Let's do a couple"
                    + "more. Penultimate link is <a href=\"penultimateLink.com.au\">hyperlink</a>. Final"
                    + "link is href href=\"/wiki/thisIsTheFinalLink\" href=notLink2, href\"notLink4\". Done";
            testPage.setContent(content);
            outLinks = wLF.getOutlinks(testPage);
            Assert.IsTrue(outLinks.ContainsAll(validLinks)); // note that locations are stored in lowercase
            Assert.IsTrue(!outLinks.Contains("notALink"));
            Assert.IsTrue(!outLinks.Contains("notLink4"));

        }

        [TestMethod]
        public void testGetInlinks()
        {

            Page targetP = new Page("targetPage");
            // create some test Pages
            Page test1 = new Page("test1"); Page test2 = new Page("test2");
            Page test3 = new Page("test3"); Page test4 = new Page("test4");
            test1.getOutlinks().AddAll(CollectionFactory.CreateQueue<string>(new[] { "a", "b", "targetPage", "d" }));
            test2.getOutlinks().AddAll(CollectionFactory.CreateQueue<string>(new[] { "targetpage", "b", "c", "d", "e" }));
            test3.getOutlinks().AddAll(CollectionFactory.CreateQueue<string>(new[] { "target", "page", "c", "d" }));
            test4.getOutlinks().AddAll(CollectionFactory.CreateQueue<string>(new[] { "TARGETPAGE", "b" }));
            pageTable.Put("test1", test1); pageTable.Put("test2", test2);
            pageTable.Put("test3", test3); pageTable.Put("test4", test4);
            ICollection<string> outLinks = wLF.getInlinks(targetP, pageTable);
            Assert.IsTrue(outLinks.Contains("test1"));
            Assert.IsTrue(outLinks.ContainsAll(CollectionFactory.CreateQueue<string>(new[] { "test1", "test2", "test4" })));
            Assert.IsTrue(!outLinks.Contains("test3"));
        }

    }

}
