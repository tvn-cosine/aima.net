using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.collections
{
    public class ReadOnlySet<T> : ISet<T>
    {
        private readonly ISet<T> backingSet;

        public ReadOnlySet(ISet<T> backingSet)
        {
            this.backingSet = backingSet;
        }

        public ReadOnlySet(ICollection<T> backingQueue)
        {
            this.backingSet = new Set<T>(backingQueue);
        }

        public bool IsReadonly()
        {
            return true;
        }

        public int Size()
        {
            return backingSet.Size();
        }

        public bool Contains(T item)
        {
            return backingSet.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return backingSet.GetEnumerator();
        }

        public T[] ToArray()
        {
            return backingSet.ToArray();
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

        public bool SequenceEqual(ICollection<T> other)
        {
            return backingSet.SequenceEqual(other);
        }
         
        public T Get(int index)
        {
            return backingSet.Get(index);
        }

        public int IndexOf(T item)
        {
            return backingSet.IndexOf(item);
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

        void ICollection<T>.RemoveAt(int index)
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

        void ICollection<T>.AddAll(ICollection<T> items)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        bool ICollection<T>.Add(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        public bool IsEmpty()
        {
            return backingSet.IsEmpty();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException("Not supported");
        }

        bool ICollection<T>.Remove(T item)
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
    }
}
