using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.collections
{
    public class List<T> : CollectionBase<T>, ICollection<T>
    {
        private System.Collections.Generic.List<T> backingList;

        public List()
        {
            backingList = new System.Collections.Generic.List<T>();
        }

        public List(ICollection<T> items)
            : this()
        {
            AddAll(items);
        }

        public List(params T[] items)
            : this()
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool Add(T item)
        {
            backingList.Add(item);
            return true;
        }

        public bool SequenceEqual(ICollection<T> other)
        {
            if (null == other
             || other.Size() != Size())
            {
                return false;
            }

            int counter = 0;
            foreach (T item in backingList)
            {
                if (!item.Equals(other.Get(counter)))
                {
                    return false;
                }
                ++counter;
            }
            return true;
        }

        public void AddAll(ICollection<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            backingList.Clear();
        }

        public bool Contains(T item)
        {
            return backingList.Contains(item);
        }

        public T Get(int index)
        {
            if (0 > index
             || backingList.Count <= index)
            {
                throw new ArgumentOutOfRangeException();
            }

            return backingList[index];
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingList);
        }

        public int IndexOf(T item)
        {
            return backingList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            backingList.Insert(index, item);
        }

        public bool IsEmpty()
        {
            return backingList.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public T Peek()
        {
            return backingList[0];
        }

        public T Pop()
        {
            T item = backingList[0];
            RemoveAt(0);
            return item;
        }

        public bool Remove(T item)
        {
            return backingList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            backingList.RemoveAt(index);
        }

        public int Size()
        {
            return backingList.Count;
        }

        public bool ContainsAll(ICollection<T> other)
        {
            foreach (T item in other)
            {
                if (!backingList.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveAll(ICollection<T> items)
        {
            foreach (T item in items)
            {
                backingList.Remove(item);
            }
        }

        public T[] ToArray()
        {
            return backingList.ToArray();
        }

        public void Reverse()
        {
            backingList.Reverse();
        }

        public ICollection<T> subList(int startPos, int endPos)
        { 
            if (startPos < 0
               || startPos > endPos
               || endPos > backingList.Count)
            {
                throw new NotSupportedException("Not supported");
            }

            if (startPos == endPos)
            {
                return new List<T>();
            }

            ICollection<T> obj = new List<T>();
            for (int i = startPos; i < endPos; ++i)
            {
                obj.Add(backingList[i]);
            }

            return obj;
        }

        public void Set(int position, T item)
        {
            backingList[position] = item;
        }

        public void Sort(IComparer<T> comparer)
        {
            backingList.Sort(new ComparerAdaptor(comparer));
        }

        class Enumerator : IEnumerator<T>
        {
            private readonly T[] values;

            private int position = -1;

            public T Current
            {
                get
                {
                    return GetCurrent();
                }
            }

            public Enumerator(System.Collections.Generic.List<T> backingList)
            {
                this.values = backingList.ToArray();
            }

            public T GetCurrent()
            {
                return values[position];
            }

            public void Dispose()
            { }

            public bool MoveNext()
            {
                ++position;
                return (position < values.Length);
            }

            public void Reset()
            {
                position = -1;
            }
        }
    }
}
