using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.collections
{
    public class FifoQueue<T> : CollectionBase<T>, ICollection<T>
    {
        private readonly System.Collections.Generic.Queue<T> backingQueue;

        public FifoQueue()
        {
            backingQueue = new System.Collections.Generic.Queue<T>();
        }

        public FifoQueue(ICollection<T> items)
            : this()
        {
            AddAll(items);
        }

        public bool Add(T item)
        {
            backingQueue.Enqueue(item);
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
            backingQueue.Clear();
        }

        public bool Contains(T item)
        {
            return backingQueue.Contains(item);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingQueue);
        }

        public bool IsEmpty()
        {
            return backingQueue.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public T Peek()
        {
            return backingQueue.Peek();
        }

        public T Pop()
        {
            return backingQueue.Dequeue();
        }

        public int Size()
        {
            return backingQueue.Count;
        }

        public T[] ToArray()
        {
            return backingQueue.ToArray();
        }

        public bool ContainsAll(ICollection<T> other)
        {
            foreach (T item in other)
            {
                if (!backingQueue.Contains(item))
                {
                    return false;
                }
            }

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
            foreach (T item in backingQueue)
            {
                if (!item.Equals(other.Get(counter)))
                {
                    return false;
                }
                ++counter;
            }
            return true;
        }

        void ICollection<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        T ICollection<T>.Get(int index)
        {
            if (0 > index
             || backingQueue.Count < index)
            {
                return default(T);
            }

            int counter = 0;
            foreach (T item in backingQueue)
            {
                if (index == counter)
                {
                    return item;
                }
                ++counter;
            }
            return default(T);
        }

        int ICollection<T>.IndexOf(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.Insert(int index, T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.RemoveAll(ICollection<T> items)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.Reverse()
        {
            throw new NotSupportedException("Not supported");
        }

        ICollection<T> ICollection<T>.subList(int startPos, int endPos)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.Set(int position, T item)
        {
            throw new NotSupportedException("Not supported");
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

            public Enumerator(System.Collections.Generic.Queue<T> backingQueue)
            {
                this.values = backingQueue.ToArray();
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
