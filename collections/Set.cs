using System.Linq; 
using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.collections
{
    public class Set<T> : ISet<T>
    {
        protected readonly System.Collections.Generic.ISet<T> backingSet;

        public Set()
        {
            backingSet = new System.Collections.Generic.HashSet<T>();
        }

        public Set(params T[] items)
            : this()
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public Set(ICollection<T> items)
            : this()
        {
            AddAll(items);
        }

        void ICollection<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        public bool Add(T item)
        {
            return backingSet.Add(item);
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
            backingSet.Clear();
        }

        public bool Contains(T item)
        {
            return backingSet.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingSet);
        }

        public bool IsEmpty()
        {
            return backingSet.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public bool Remove(T item)
        {
            return backingSet.Remove(item);
        }

        public int Size()
        {
            return backingSet.Count;
        }

        public bool ContainsAll(ICollection<T> other)
        {
            foreach (T item in other)
            {
                if (!backingSet.Contains(item))
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
                backingSet.Remove(item);
            }
        }

        public T[] ToArray()
        {
            return backingSet.ToArray();
        }

        void ICollection<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        T ICollection<T>.Get(int index)
        {
            if (index > backingSet.Count
             || index < 0)
            {
                throw new NotSupportedException("Not supported");
            }

            int counter = 0;

            foreach (T item in backingSet)
            {
                if (counter == index)
                {
                    return item;
                }
                ++counter;
            }

            return default(T);
        }

        public bool SequenceEqual(ICollection<T> other)
        {
            if (null == other
             || other.Size() != Size())
            {
                return false;
            }
             
            foreach (T item in backingSet)
            {
                if (!other.Contains(item))
                {
                    return false;
                } 
            }
            return true;
        }

        int ICollection<T>.IndexOf(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.Insert(int index, T item)
        {
            throw new NotSupportedException("Not supported");
        }

        T ICollection<T>.Peek()
        {
            throw new NotSupportedException("Not supported");
        }

        T ICollection<T>.Pop()
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

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            sb.Append('[');
            bool first = true;
            foreach (var item in this)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(item.ToString());
            }
            sb.Append(']');
            return sb.ToString();
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

            public Enumerator(System.Collections.Generic.ISet<T> backingSet)
            {
                this.values = backingSet.ToArray();
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
