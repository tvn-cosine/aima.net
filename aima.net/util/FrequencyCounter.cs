using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.util
{
    /**
     * A utility class for keeping counts of objects. Will return 0 for any object
     * for which it has not recorded a count against.
     * 
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class FrequencyCounter<T> : IStringable
    {
        private IMap<T, int> counter;
        private int total;

        /**
         * Default Constructor.
         */
        public FrequencyCounter()
        {
            counter = CollectionFactory.CreateInsertionOrderedMap<T, int>();
            total = 0;
        }

        /**
         * Returns the count to which the specified key is mapped in this frequency
         * counter, or 0 if the map contains no mapping for this key.
         * 
         * @param key
         *            the key whose associated count is to be returned.
         * 
         * @return the count to which this map maps the specified key, or 0 if the
         *         map contains no mapping for this key.
         */
        public int getCount(T key)
        {
            if (!counter.ContainsKey(key))
            {
                return 0;
            }
            return counter.Get(key);
        }

        /**
         * Increments the count to which the specified key is mapped in this
         * frequency counter, or puts 1 if the map contains no mapping for this key.
         * 
         * @param key
         *            the key whose associated count is to be returned.
         */
        public void incrementFor(T key)
        {
            if (!counter.ContainsKey(key))
            {
                counter.Put(key, 1);
            }
            else
            {
                counter.Put(key, counter.Get(key) + 1);
            }
            // Keep track of the total
            total++;
        }

        /**
         * Returns the count to which the specified key is mapped in this frequency
         * counter, divided by the total of all counts.
         * 
         * @param key
         *            the key whose associated count is to be divided.
         * 
         * @return the count to which this map maps the specified key, divided by
         *         the total count.
         */
        public double probabilityOf(T key)
        {
            int value = getCount(key);
            if (0 == total || 0 == value)
            {
                return 0.0;
            }
            else
            {
                return (double)value / (double)total;
            }
        }

        /**
         * 
         * @return a set of objects for which frequency counts have been recorded.
         */
        public ISet<T> getStates()
        {
            return CollectionFactory.CreateSet<T>(counter.GetKeys());
        }

        /**
         * Remove all the currently recorded frequency counts.
         */
        public void clear()
        {
            counter.Clear();
            total = 0;
        }

        public override string ToString()
        {
            return counter.ToString();
        }
    } 
}
