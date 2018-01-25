using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.nlp.ranking
{
    public class WikiLinkFinder : LinkFinder
    {
        // TODO
        // Make more intelligent link search
        public ICollection<string> getOutlinks(Page page)
        {
            string content = page.getContent();
            ICollection<string> outLinks = CollectionFactory.CreateQueue<string>();
            // search content for all href="x" outlinks
            ICollection<string> allMatches = CollectionFactory.CreateQueue<string>();
            IRegularExpression m = TextFactory.CreateRegularExpression("href=\"(/wiki/.*?)\"");
            foreach (string ma in m.Matches(content))
            {
                allMatches.Add(ma);
            }
            for (int i = 0; i < allMatches.Size(); ++i)
            {
                string match = allMatches.Get(i);
                string[] tokens = TextFactory.CreateRegularExpression("\"").Split(match);
                string location = tokens[1].ToLower(); // also, tokens[0] = the
                                                       // text before the first
                                                       // quote,
                                                       // and tokens[2] is the
                                                       // text after the second
                                                       // quote
                outLinks.Add(location);
            }

            return outLinks;
        }


        public ICollection<string> getInlinks(Page target, IMap<string, Page> pageTable)
        {

            string location = target.getLocation().ToLower(); // make comparison
                                                              // case
                                                              // insensitive
            ICollection<string> inlinks = CollectionFactory.CreateQueue<string>(); // initialise a list for
                                                                                   // the inlinks

            // go through all pages and if they link back to target then add that
            // page's location to the target's inlinks
            foreach (var pair in pageTable)
            {
                Page p = pair.GetValue();
                for (int i = 0; i < p.getOutlinks().Size(); ++i)
                {
                    string pForward = p.getOutlinks().Get(i).ToLower().Replace('\\', '/');
                    string pBackward = p.getOutlinks().Get(i).ToLower().Replace('/', '\\');
                    if (pForward.Equals(location) || pBackward.Equals(location))
                    {
                        inlinks.Add(p.getLocation().ToLower());
                        break;
                    }
                }
            }
            return inlinks;
        }
    }
}
