using System.IO; 
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.nlp.ranking
{
    public class PagesDataset
    {
        static string wikiPagesFolderPath = "src\\main\\resources\\aima\\core\\ranking\\data\\pages";
        static string testFilesFolderPath = "src\\main\\resources\\aima\\core\\ranking\\data\\pages\\test_pages";

        private static WikiLinkFinder wlf;

        public static IMap<string, Page> loadDefaultPages()
        {
            return loadPages(wikiPagesFolderPath);
        }

        public static IMap<string, Page> loadTestPages()
        {
            return loadPages(testFilesFolderPath);
        }

        /**
         * Access a folder of .txt files containing wikipedia html source, and give
         * back a hashtable of pages, which each page having it's correct inlink
         * list and outlink list.
         * 
         * @param folderPath
         * @return a hashtable of Page objects, accessed by article name (which is a
         *         location for wikipedia: \wiki\*article name*)
         */
        public static IMap<string, Page> loadPages(string folderPath)
        {

            IMap<string, Page> pageTable = CollectionFactory.CreateInsertionOrderedMap<string, Page>();
            Page currPage;
            string[] listOfFiles;
            wlf = new WikiLinkFinder();

            if (Directory.Exists(folderPath))
            {
                listOfFiles = Directory.GetFiles(folderPath);
            }
            else
            {
                return null;
            } // maybe should throw exception instead?

            // Access each .txt file to create a new Page object for that file's
            // article
            for (int i = 0; i < listOfFiles.Length;++i)
            {
                currPage = wikiPageFromFile(folderPath, new FileInfo(listOfFiles[i]));
                pageTable.Put(currPage.getLocation(), currPage);
            }
            // now that all pages are loaded and their outlinks have been determined,
            // we can determine a page's inlinks and then return the loaded table
            return pageTable = determineAllInlinks(pageTable);
        } // end loadPages()

        public static Page wikiPageFromFile(string folder, FileInfo file)
        {
            Page p;
            string pageLocation = getPageName(file); // will be like: \wiki\*article
                                                  // name*.toLowercase()
            string content = loadFileText(folder, file); // get html source as string
            p = new Page(pageLocation); // create the page object
            p.setContent(content); // give the page its html source as a string
            p.getOutlinks().AddAll(wlf.getOutlinks(p)); // search html source for
                                                        // links
            return p;
        }

        public static IMap<string, Page> determineAllInlinks(IMap<string, Page> pageTable)
        {
            Page currPage;
            foreach (var pair in pageTable)
            {
                currPage = pair.GetValue();
                // add the inlinks to an currently empty IQueue<string> object
                currPage.getInlinks().AddAll(wlf.getInlinks(currPage, pageTable));
            }
            return pageTable;
        }

        public static string getPageName(FileInfo f)
        {
            string wikiPrefix = "/wiki/";
            string filename = f.Name;
            if (filename.IndexOf(".") > 0)
                filename = filename.Substring(0, filename.LastIndexOf("."));
            return wikiPrefix + filename.ToLower();
        } // end getPageName()

        public static string loadFileText(string folderPath, FileInfo file)
        {
            IStringBuilder pageContent = TextFactory.CreateStringBuilder();

            using (StreamReader sr = new StreamReader(file.FullName))
            {
                while (!sr.EndOfStream)
                {
                    pageContent.Append(sr.ReadLine());
                }
            }

            return pageContent.ToString();
        } // end loadFileText()

        // TODO:
        // Be able to automatically retrieve an arbitrary number of
        // wikipaedia pages and create a hashtable of Pages from them.

        // TODO:
        // Be able to automatically retreive an arbitraru number of webpages
        // that are in a network conducive to application of the HITS algorithm
    }
}
