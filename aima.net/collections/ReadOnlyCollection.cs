using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.collections
{
    public class ReadOnlyCollection<T> : ICollection<T>
    {
        private readonly ICollection<T> backingQueue;

        public ReadOnlyCollection(ICollection<T> backingQueue)
        {
            this.backingQueue = backingQueue;
        }

        public bool Contains(T item)
        {
            return backingQueue.Contains(item);
        }

        public T Get(int index)
        {
            return backingQueue.Get(index);
        }

        public bool SequenceEqual(ICollection<T> other)
        {
            return backingQueue.SequenceEqual(other);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return backingQueue.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return backingQueue.IndexOf(item);
        }

        public bool IsEmpty()
        {
            return backingQueue.IsEmpty();
        }

        public bool IsReadonly()
        {
            return true;
        }

        public T Peek()
        {
            return backingQueue.Peek();
        }

        public int Size()
        {
            return backingQueue.Size();
        }

        public T[] ToArray()
        {
            return backingQueue.ToArray();
        }

        public bool ContainsAll(ICollection<T> other)
        {
            return backingQueue.ContainsAll(other);
        }

        public ICollection<T> subList(int startPos, int endPos)
        {
            return backingQueue.subList(startPos, endPos);
        }

        void ICollection<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        T ICollection<T>.Pop()
        {
            throw new NotSupportedException("Not supported");
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        bool ICollection<T>.Add(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.AddAll(ICollection<T> items)
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException("Not supported");
        }

        void ICollection<T>.Insert(int index, T item)
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

        void ICollection<T>.Set(int position, T item)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
