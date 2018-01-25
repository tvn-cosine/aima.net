using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.nlp.ranking
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 871.<br>
     * <br>
     * 
     * <pre>
     * function HITS(query) returns pages (with hub and authority numbers)
     *   pages &larr; EXPAND-PAGES(RELEVANT-PAGES(query))
     *   for each p in pages do 
     *   	p.AUTHORITY &larr; 1
     *   	p.HUB &larr; 1
     *   repeat until convergence do
     *   	for each p in pages do
     *   		p.AUTHORITY &larr; &Sigma;<sub>i</sub> INLINK<sub>i</sub>(p).HUB
     *   		p.HUB &larr; &Sigma;<sub>i</sub> OUTLINK<sub>i</sub>(p).AUTHORITY
     *   	NORMALIZE(pages)
     *   return pages
     * </pre>
     * 
     * Figure 22.1 The HITS algorithm for computing hubs and authorities with
     * respect to a query. RELEVANT-PAGES fetches the pages that match the query,
     * and EXPAND-PAGES add in every page that links to or is linked from one of the
     * relevant pages. NORMALIZE divides each page's score by the sum of the squares
     * of all pages' scores (separately for both the authority and hubs scores.<br>
     * <br>
     * 
     * @author Jonathon Belotti (thundergolfer)
     *
     */
    public class HITS
    {
        readonly int RANK_HISTORY_DEPTH;
        readonly double DELTA_TOLERANCE; // somewhat arbitrary
        IMap<string, Page> pTable;
        // DETECT CONVERGENCE VARS
        double[] prevAuthVals;
        double[] prevHubVals;
        double prevAveHubDelta = 0;
        double prevAveAuthDelta = 0;
        ////////////////////////////

        // TODO: Improve the convergence detection functionality
        public HITS(IMap<string, Page> pTable, int rank_hist_depth, double delta_tolerance)
        {
            this.pTable = pTable;
            this.RANK_HISTORY_DEPTH = rank_hist_depth;
            this.DELTA_TOLERANCE = delta_tolerance;

        }

        public HITS(IMap<string, Page> pTable)
            : this(pTable, 3, 0.05)
        { }

        // function HITS(query) returns pages with hub and authority number
        public ICollection<Page> hits(string query)
        {
            // pages <- EXPAND-PAGES(RELEVANT-PAGES(query))
            ICollection<Page> pages = expandPages(relevantPages(query));
            // for each p in pages
            foreach (Page p in pages)
            {
                // p.AUTHORITY <- 1
                p.authority = 1;
                // p.HUB <- 1
                p.hub = 1;
            }
            // repeat until convergence do
            while (!convergence(pages))
            {
                // for each p in pages do
                foreach (Page p in pages)
                {
                    // p.AUTHORITY <- &Sigma<sub>i</sub> INLINK<sub>i</sub>(p).HUB
                    p.authority = SumInlinkHubScore(p);
                    // p.HUB <- &Sigma;<sub>i</sub> OUTLINK<sub>i</sub>(p).AUTHORITY
                    p.hub = SumOutlinkAuthorityScore(p);
                }
                // NORMALIZE(pages)
                normalize(pages);
            }
            return pages;

        }

        /**
         * Fetches and returns all pages that match the query
         * 
         * @param query
         * @return
         * @throws UnsupportedEncodingException
         */
        public ICollection<Page> relevantPages(string query)
        {
            ICollection<Page> relevantPages = CollectionFactory.CreateQueue<Page>();
            foreach (Page p in pTable.GetValues())
            {
                if (matches(query, p.getContent()))
                {
                    relevantPages.Add(p);
                }
            }
            return relevantPages;
        }

        /**
         * Simple check if query string is a substring of a block of text.
         * 
         * @param query
         * @param text
         * @return
         */
        public bool matches(string query, string text)
        {
            return text.Contains(query);
        }

        /**
         * Adds pages that are linked to or is linked from one of the pages passed
         * as argument.
         * 
         * @param pages
         * @return
         */
        public ICollection<Page> expandPages(ICollection<Page> pages)
        {

            ICollection<Page> expandedPages = CollectionFactory.CreateQueue<Page>();
            ISet<string> inAndOutLinks = CollectionFactory.CreateSet<string>();
            // Go through all pages an build a list of string links
            foreach (Page currP in pages)
            {
                if (!expandedPages.Contains(currP))
                    expandedPages.Add(currP);
                ICollection<string> currInlinks = currP.getInlinks();
                foreach (string currInlink in currInlinks)
                    inAndOutLinks.Add(currInlink);
                ICollection<string> currOutlinks = currP.getOutlinks();
                foreach (string currOutlink in currOutlinks)
                    inAndOutLinks.Add(currOutlink);
            }
            // go through string links and add their respective pages to our return
            // list
            foreach (string addr in inAndOutLinks)
            {
                Page p = pTable.Get(addr);
                if (p != null && !expandedPages.Contains(p))
                { // a valid link may
                  // not have an
                  // associated page
                  // in our table
                    expandedPages.Add(p);
                }
            }
            return expandedPages;
        } // end expandPages();

        /**
         * Divides each page's score by the sum of the squares of all pages' scores
         * (separately for both the authority and hubs scores
         * 
         * @param pages
         * @return
         */
        public ICollection<Page> normalize(ICollection<Page> pages)
        {
            double hubTotal = 0;
            double authTotal = 0;
            foreach (Page p in pages)
            {
                // Sum Hub scores over all pages
                hubTotal += System.Math.Pow(p.hub, 2);
                // Sum Authority scores over all pages
                authTotal += System.Math.Pow(p.authority, 2);
            }
            // divide all hub and authority scores for all pages
            foreach (Page p in pages)
            {
                if (hubTotal > 0)
                {
                    p.hub /= hubTotal;
                }
                else
                {
                    p.hub = 0;
                }
                if (authTotal > 0)
                {
                    p.authority /= authTotal;
                }
                else
                {
                    p.authority = 0;
                }
            }
            return pages; // with normalised scores now
        } // end normalize()

        /**
         * Calculate the Authority score of a page by summing the Hub scores of that
         * page's inlinks.
         * 
         * @param page
         * @param pagesTable
         * @return
         */
        public double SumInlinkHubScore(Page page)
        {
            ICollection<string> inLinks = page.getInlinks();
            double hubScore = 0;
            foreach (string inLink1 in inLinks)
            {
                Page inLink = pTable.Get(inLink1);
                if (inLink != null)
                    hubScore += inLink.hub;
                // else: page is linked to by a Page not in our table
            }
            return hubScore;
        } // end SumInlinkHubScore()

        /**
         * Calculate the Hub score of a page by summing the Authority scores of that
         * page's outlinks.
         * 
         * @param page
         * @param pagesTable
         * @return
         */
        public double SumOutlinkAuthorityScore(Page page)
        {
            ICollection<string> outLinks = page.getOutlinks();
            double authScore = 0;
            foreach (string outLink1 in outLinks)
            {
                Page outLink = pTable.Get(outLink1);
                if (outLink != null)
                    authScore += outLink.authority;
            }
            return authScore;
        }

        /**
         * pg. 872 : "If we then normalize the scores and repeat k times the process
         * will converge"
         * 
         * @return
         */
        private bool convergence(ICollection<Page> pages)
        {
            double aveHubDelta = 100;
            double aveAuthDelta = 100;
            if (pages == null)
            {
                return true;
            }

            // get current values from pages
            double[] currHubVals = new double[pages.Size()];
            double[] currAuthVals = new double[pages.Size()];
            for (int i = 0; i < pages.Size();++i)
            {
                Page currPage = pages.Get(i);
                currHubVals[i] = currPage.hub;
                currHubVals[i] = currPage.authority;
            }
            if (prevHubVals == null || prevAuthVals == null)
            {
                prevHubVals = currHubVals;
                prevAuthVals = currAuthVals;
                return false;
            }
            // compare to past values
            aveHubDelta = getAveDelta(currHubVals, prevHubVals);
            aveAuthDelta = getAveDelta(currAuthVals, prevAuthVals);
            if (aveHubDelta + aveAuthDelta < DELTA_TOLERANCE || (System.Math.Abs(prevAveHubDelta - aveHubDelta) < 0.01
                    && System.Math.Abs(prevAveAuthDelta - aveAuthDelta) < 0.01))
            {
                return true;
            }
            else
            {
                prevHubVals = currHubVals;
                prevAuthVals = currAuthVals;
                prevAveHubDelta = aveHubDelta;
                prevAveAuthDelta = aveAuthDelta;
                return false;
            }
        }

        /**
         * Determine how much values in a list are changing. Useful for detecting
         * convergence of data values.
         * 
         * @param r
         * @return
         */
        public double getAveDelta(double[] curr, double[] prev)
        {
            double aveDelta = 0;
            if (curr.Length != prev.Length)
            {
                throw new IllegalArgumentException("curr must be == to prev");
            }
            for (int j = 0; j < curr.Length; j++)
            {
                aveDelta += System.Math.Abs(curr[j] - prev[j]);
            }
            aveDelta /= curr.Length;
            return aveDelta;
        }

        /**
         * Return from a set of Pages the Page with the greatest Hub value
         * 
         * @param pageTable
         * @return
         */
        public Page getMaxHub(ICollection<Page> result)
        {
            Page maxHub = null;
            foreach (Page currPage in result)
            {
                if (maxHub == null || currPage.hub > maxHub.hub)
                    maxHub = currPage;
            }
            return maxHub;
        }

        /**
         * Return from a set of Pages the Page with the greatest Authority value
         * 
         * @param pageTable
         * @return
         */
        public Page getMaxAuthority(ICollection<Page> result)
        {
            Page maxAuthority = null;
            foreach (Page currPage in result)
            {
                if (maxAuthority == null || currPage.authority > maxAuthority.authority)
                    maxAuthority = currPage;
            }
            return maxAuthority;
        }

        /**
         * Organize the list of pages according to their descending Hub scores.
         * 
         * @param result
         */
        class SortHubSorter : IComparer<Page>
        {
            public int Compare(Page p1, Page p2)
            {
                // Sorts by 'TimeStarted' property
                return p1.hub < p2.hub ? -1 : p1.hub > p2.hub ? 1 : secondaryOrderSort(p1, p2);
            }

            // If 'TimeStarted' property is equal sorts by 'TimeEnded' property
            public int secondaryOrderSort(Page p1, Page p2)
            {
                return p1.getLocation().ToLower().CompareTo(p2.getLocation().ToLower()) < 1 ? -1
                        : p1.getLocation().ToLower().CompareTo(p2.getLocation().ToLower()) > 1 ? 1 : 0;
            }
        }

        public void sortHub(ICollection<Page> result)
        {
            result.Sort(new SortHubSorter());
        }

        class SortAuthoritySorter : IComparer<Page>
        {
            public int Compare(Page p1, Page p2)
            {
                // Sorts by 'TimeStarted' property
                return p1.hub < p2.hub ? -1 : p1.hub > p2.hub ? 1 : secondaryOrderSort(p1, p2);
            }

            // If 'TimeStarted' property is equal sorts by 'TimeEnded' property
            public int secondaryOrderSort(Page p1, Page p2)
            {
                return p1.getLocation().ToLower().CompareTo(p2.getLocation().ToLower()) < 1 ? -1
                        : p1.getLocation().ToLower().CompareTo(p2.getLocation().ToLower()) > 1 ? 1 : 0;
            }
        }

        /**
         * Organize the list of pages according to their descending Authority Scores
         * 
         * @param result
         */
        public void sortAuthority(ICollection<Page> result)
        {
            result.Sort(new SortAuthoritySorter());
        }

        /**
         * Simple console display of HITS Algorithm results.
         * 
         * @param result
         */
        public void report(ICollection<Page> result)
        {

            // Print Pages out ranked by highest authority
            sortAuthority(result);
            System.Console.WriteLine("AUTHORITY RANKINGS : ");
            foreach (Page currP in result)
                System.Console.Write(currP.getLocation() + ": " + "%.5f" + '\n', currP.authority);
            System.Console.WriteLine();
            // Print Pages out ranked by highest hub
            sortHub(result);
            System.Console.WriteLine("HUB RANKINGS : ");
            foreach (Page currP in result)
                System.Console.Write(currP.getLocation() + ": " + "%.5f" + '\n', currP.hub);
            System.Console.WriteLine();
            // Print Max Authority
            System.Console.WriteLine("Page with highest Authority score: " + getMaxAuthority(result).getLocation());
            // Print Max Authority
            System.Console.WriteLine("Page with highest Hub score: " + getMaxAuthority(result).getLocation());
        }

    }
}
