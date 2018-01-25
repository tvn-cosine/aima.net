using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.collections
{
    public class LifoQueue<T> : CollectionBase<T>, ICollection<T>
    {
        private readonly System.Collections.Generic.Stack<T> backingStack;

        public LifoQueue()
        {
            backingStack = new System.Collections.Generic.Stack<T>();
        }

        public LifoQueue(ICollection<T> items)
            : this()
        {
            AddAll(items);
        }

        public bool Add(T item)
        {
            backingStack.Push(item);
            return true;
        }

        public void AddAll(ICollection<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool SequenceEqual(ICollection<T> other)
        {
            if (null == other
             || other.Size() != Size())
            {
                return false;
            }

            int counter = 0;
            foreach (T item in backingStack)
            {
                if (!item.Equals(other.Get(counter)))
                {
                    return false;
                }
                ++counter;
            }
            return true;
        }

        public void Clear()
        {
            backingStack.Clear();
        }

        public bool Contains(T item)
        {
            return backingStack.Contains(item);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingStack);
        }

        public bool IsEmpty()
        {
            return backingStack.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public T Peek()
        {
            return backingStack.Peek();
        }

        public T Pop()
        {
            return backingStack.Pop();
        }

        public int Size()
        {
            return backingStack.Count;
        }

        public T[] ToArray()
        {
            return backingStack.ToArray();
        }

        public bool ContainsAll(ICollection<T> other)
        {
            foreach (T item in other)
            {
                if (!backingStack.Contains(item))
                {
                    return false;
                }
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
            throw new NotSupportedException("Not supported");
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

            public Enumerator(System.Collections.Generic.Stack<T> backingStack)
            {
                this.values = backingStack.ToArray();
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
