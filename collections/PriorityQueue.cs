using System.Linq;
using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.collections
{
    public class PriorityQueue<T> : CollectionBase<T>, ICollection<T>
    {
        private readonly System.Collections.Generic.List<T> backingList;
        private readonly IComparer<T> comparer;

        public PriorityQueue(IComparer<T> comparer)
        {
            this.comparer = comparer;
            backingList = new System.Collections.Generic.List<T>();
        }

        public bool Add(T item)
        {
            backingList.Add(item);
            int ci = backingList.Count - 1; // child index; start at end
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // parent index
                if (comparer.Compare(backingList[ci], backingList[pi]) >= 0)
                    break; // child item is larger than (or equal) parent so we're done
                T tmp = backingList[ci];
                backingList[ci] = backingList[pi];
                backingList[pi] = tmp;
                ci = pi;
            }
            return true;
        }

        public void AddAll(ICollection<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public bool SequenceEqual(ICollection<T> other)
        {
            Sort(comparer);
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

        public IComparer<T> GetComparer()
        {
            return comparer;
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
            return backingList[index];
        }

        public override IEnumerator<T> GetEnumerator()
        {
            Sort(comparer);
            return new Enumerator(backingList);
        }

        public int IndexOf(T item)
        {
            return backingList.IndexOf(item);
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
            // assumes pq is not empty; up to calling code
            int li = backingList.Count - 1; // last index (before removal)
            T frontItem = backingList[0];   // fetch the front
            backingList[0] = backingList[li];
            backingList.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && comparer.Compare(backingList[rc], backingList[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (comparer.Compare(backingList[pi], backingList[ci]) <= 0)
                    break; // parent is smaller than (or equal to) smallest child so done
                T tmp = backingList[pi];
                backingList[pi] = backingList[ci];
                backingList[ci] = tmp; // swap parent and child
                pi = ci;
            }
            return frontItem;
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
            return backingList.Count();
        }

        public void Sort(IComparer<T> comparer)
        {
            backingList.Sort(new ComparerAdaptor(comparer));
        }

        void ICollection<T>.Insert(int index, T item)
        {
            throw new NotSupportedException("Not supported");
        }

        public bool ContainsAll(ICollection<T> other)
        {
            foreach (T item in other)
                if (!backingList.Contains(item))
                    return false;
            return true;
        }

        public void RemoveAll(ICollection<T> items)
        {
            foreach (T item in items)
                backingList.Remove(item);
        }

        public T[] ToArray()
        {
            Sort(comparer);
            return backingList.ToArray();
        }

        public void Reverse()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<T> subList(int startPos, int endPos)
        {
            throw new System.NotImplementedException();
        }

        public void Set(int position, T item)
        {
            throw new System.NotImplementedException();
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

            public Enumerator(System.Collections.Generic.List<T> backingSortedList)
            {

                this.values = backingSortedList.ToArray();
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
