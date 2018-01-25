using aima.net.collections.api;

namespace aima.net.nlp.ranking
{
    public interface LinkFinder
    {

        /**
         * Take a Page object and return its outlinks as a list of strings. The Page
         * object must therefore possess the information to determine what it links
         * to.
         * 
         * @param page
         * @return
         */
        ICollection<string> getOutlinks(Page page);

        /**
         * Take a Page object and return its inlinks (who links to it) as a list of
         * strings.
         * 
         * @param page
         * @param pageTable
         * @return
         */
        ICollection<string> getInlinks(Page page, IMap<string, Page> pageTable);

    }
}
