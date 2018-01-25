using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.nlp.ranking
{
    public class Page
    { 
        public double authority;
        public double hub;
        private string location;
        private string content;
        private ICollection<string> linkTo;
        private ICollection<string> linkedFrom;

        public Page(string location)
        {
            authority = 0;
            hub = 0;
            this.location = location;
            this.linkTo = CollectionFactory.CreateQueue<string>();
            this.linkedFrom = CollectionFactory.CreateQueue<string>();
        }

        public string getLocation()
        {
            return location;
        }

        public string getContent()
        {
            return content;
        }

        public bool setContent(string content)
        {
            this.content = content;
            return true;
        }

        public ICollection<string> getInlinks()
        {
            return linkedFrom;
        }

        public ICollection<string> getOutlinks()
        {
            return linkTo;
        }
    }
}
