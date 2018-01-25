using aima.net.text;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.search.framework
{
    /**
     * Stores key-value pairs for efficiency analysis. 
     */
    public class Metrics : IStringable
    {
        private IMap<string, string> hash;

        public Metrics()
        {
            this.hash = CollectionFactory.CreateInsertionOrderedMap<string, string>();
        }

        public void set(string name, int i)
        {
            hash.Put(name, i.ToString());
        }

        public void set(string name, double d)
        {
            hash.Put(name, d.ToString());
        }

        public void incrementInt(string name)
        {
            set(name, getInt(name) + 1);
        }

        public void set(string name, long l)
        {
            hash.Put(name, l.ToString());
        }

        public int getInt(string name)
        {
            return hash.ContainsKey(name) ? TextFactory.ParseInt(hash.Get(name)) : 0;
        }

        public double getDouble(string name)
        {
            return hash.ContainsKey(name) ? TextFactory.ParseDouble(hash.Get(name).Replace(',','.')) : double.NaN;
        }

        public long getLong(string name)
        { 
            return hash.ContainsKey(name) ? TextFactory.ParseLong(hash.Get(name)) : 0L;
        }

        public string get(string name)
        {
            return hash.Get(name);
        }

        public ISet<string> keySet()
        {
            return CollectionFactory.CreateSet<string>(hash.GetKeys());
        }

        /** Sorts the key-value pairs by key names and formats them as equations. */
        public override string ToString()
        {
            IMap<string, string> map = CollectionFactory.CreateTreeMap<string, string>(hash);
            return map.ToString();
        }
    }
}
