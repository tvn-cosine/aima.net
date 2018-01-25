using aima.net.api; 

namespace aima.net.collections.api
{
    public interface ICollection<T> : IEnumerable<T>, IStringable
    {
        T Get(int index);
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index); 
        void AddAll(ICollection<T> items);
        bool IsReadonly();
        bool Add(T item);
        bool IsEmpty();
        int Size();
        T Pop();
        T Peek();
        void Clear();
        bool Contains(T item);
        bool ContainsAll(ICollection<T> other);
        bool Remove(T item);
        void RemoveAll(ICollection<T> items);
        void Sort(IComparer<T> comparer);
        T[] ToArray();
        void Reverse();
        ICollection<T> subList(int startPos, int endPos);
        void Set(int position, T item);
        bool SequenceEqual(ICollection<T> queue);
    }
}
