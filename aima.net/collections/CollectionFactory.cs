using aima.net.api;
using aima.net.collections.api;

namespace aima.net.collections
{
    public static class CollectionFactory
    {
        public static IMap<KEY, VALUE> CreateInsertionOrderedMap<KEY, VALUE>()
        {
            return new InsertionOrderedMap<KEY, VALUE>();
        }
         
        public static IMap<KEY, VALUE> CreateInsertionOrderedMap<KEY, VALUE>(IMap<KEY, VALUE> map)
        {
            return new InsertionOrderedMap<KEY, VALUE>(map);
        }

        public static IMap<KEY, VALUE> CreateMap<KEY, VALUE>()
        {
            return new Map<KEY, VALUE>();
        }
         
        public static IMap<KEY, VALUE> CreateMap<KEY, VALUE>(IMap<KEY, VALUE> map)
        {
            return new Map<KEY, VALUE>(map);
        }

        public static IMap<KEY,VALUE> CreateTreeMap<KEY,VALUE>(IMap<KEY,VALUE> map)
        {
            return new Map<KEY, VALUE>(map);
        }

        public static ICollection<T> CreatePriorityQueue<T>(IComparer<T> comparer)
        {
            return new PriorityQueue<T>(comparer);
        }

        public static ICollection<T> CreateQueue<T>()
        {
            return new List<T>();
        }

        public static ISet<T> CreateSet<T>()
        {
            return new Set<T>();
        }

        public static ISet<T> CreateSet<T>(ICollection<T> collection)
        {
            return new Set<T>(collection);
        }

        public static ISet<T> CreateSet<T>(params T[] collection)
        {
           return new Set<T>(collection); 
        }

        public static ICollection<T> CreateFifoQueueNoDuplicates<T>()
        {
            return new FifoQueueNoDuplicates<T>();
        }

        public static ICollection<T> CreateLifoQueue<T>()
        {
            return new LifoQueue<T>();
        }

        public static ICollection<T> CreateFifoQueue<T>()
        {
            return new FifoQueue<T>();
        }

        public static ICollection<T> CreateQueue<T>(ICollection<T> collection)
        {
            return new List<T>(collection);
        }

        public static ICollection<T> CreateQueue<T>(ISet<T> collection)
        {
            return new List<T>(collection);
        }

        public static ICollection<T> CreateQueue<T>(params T[] collection)
        {
            return new List<T>(collection);
        }

        public static ICollection<T> CreateFifoQueue<T>(ICollection<T> collection)
        {
            return new FifoQueue<T>(collection);
        }

        public static ICollection<T> CreateLifoQueue<T>(ICollection<T> collection)
        {
            return new LifoQueue<T>(collection);
        }

        public static IMap<KEY, VALUE> CreateReadOnlyMap<KEY, VALUE>(IMap<KEY, VALUE> collection)
        {
            return new ReadOnlyMap<KEY, VALUE>(collection);
        }

        public static ISet<T> CreateReadOnlySet<T>(ISet<T> collection)
        {
            return new ReadOnlySet<T>(collection);
        }

        public static ISet<T> CreateReadOnlySet<T>(ICollection<T> collection)
        {
            return new ReadOnlySet<T>(collection);
        }

        public static ICollection<T> CreateReadOnlyQueue<T>(ICollection<T> collection)
        {
            return new ReadOnlyCollection<T>(collection);
        }
    }
}
