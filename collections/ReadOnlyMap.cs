using aima.net.api;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.exceptions;

namespace aima.net.collections
{
    public class ReadOnlyMap<KEY, VALUE> : IMap<KEY, VALUE>
    {
        private readonly IMap<KEY, VALUE> backingMap;

        public ReadOnlyMap(IMap<KEY, VALUE> backingMap)
        {
            this.backingMap = backingMap;
        }

        public int Size()
        {
            return backingMap.Size();
        }
         
        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            return backingMap.Contains(item);
        }

        public bool ContainsKey(KEY key)
        {
            return backingMap.ContainsKey(key);
        }

        public bool Equals(IMap<KEY, VALUE> other)
        {
            return backingMap.Equals(other);
        }

        public VALUE Get(KEY key)
        {
            return backingMap.Get(key);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return backingMap.GetEnumerator();
        }

        public ISet<KEY> GetKeys()
        {
            return backingMap.GetKeys();
        }

        public ICollection<VALUE> GetValues()
        {
            return backingMap.GetValues();
        }

        public bool IsEmpty()
        {
            return backingMap.IsEmpty();
        }

        public bool IsReadonly()
        {
            return true;
        }

        public bool ContainsAll(ICollection<KeyValuePair<KEY, VALUE>> other)
        {
            return backingMap.ContainsAll(other);
        }

        bool ICollection<KeyValuePair<KEY, VALUE>>.SequenceEqual(ICollection<KeyValuePair<KEY, VALUE>> other)
        {
            throw new NotSupportedException("Not supported");
        }
         
        void ICollection<KeyValuePair<KEY, VALUE>>.Sort(IComparer<KeyValuePair<KEY, VALUE>> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        void IMap<KEY, VALUE>.Put(KEY key, VALUE value)
        {
            throw new NotSupportedException("Not supported");
        }

        bool ICollection<KeyValuePair<KEY, VALUE>>.Remove(KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IMap<KEY, VALUE>.Remove(KEY key)
        {
            throw new NotSupportedException("Not supported");
        }

        bool ICollection<KeyValuePair<KEY, VALUE>>.Add(KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<KeyValuePair<KEY, VALUE>>.AddAll(ICollection<KeyValuePair<KEY, VALUE>> items)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<KeyValuePair<KEY, VALUE>>.Clear()
        {
            throw new NotSupportedException("Not supported");
        }

        KeyValuePair<KEY, VALUE> ICollection<KeyValuePair<KEY, VALUE>>.Get(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        int ICollection<KeyValuePair<KEY, VALUE>>.IndexOf(KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<KeyValuePair<KEY, VALUE>>.Insert(int index, KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        KeyValuePair<KEY, VALUE> ICollection<KeyValuePair<KEY, VALUE>>.Peek()
        {
            throw new NotSupportedException("Not supported");
        }

        KeyValuePair<KEY, VALUE> ICollection<KeyValuePair<KEY, VALUE>>.Pop()
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<KeyValuePair<KEY, VALUE>>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        void IMap<KEY, VALUE>.PutAll(IMap<KEY, VALUE> map)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<KeyValuePair<KEY, VALUE>>.RemoveAll(ICollection<KeyValuePair<KEY, VALUE>> items)
        {
            throw new NotSupportedException("Not supported");
        }

        public KeyValuePair<KEY, VALUE>[] ToArray()
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<KeyValuePair<KEY, VALUE>>.Reverse()
        {
            throw new NotSupportedException("Not supported");
        }

        ICollection<KeyValuePair<KEY, VALUE>> ICollection<KeyValuePair<KEY, VALUE>>.subList(int startPos, int endPos)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<KeyValuePair<KEY, VALUE>>.Set(int position, KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
