using aima.net.api;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.collections
{
    public abstract class CollectionBase<T> : IEnumerable<T>, IHashable, IStringable, IEquatable
    {
        public abstract IEnumerator<T> GetEnumerator();
           
        public override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
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
         
        public class Comparer : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return System.Collections.Generic.Comparer<T>.Default.Compare(x, y);
            }
        }

        public class QueueItemEqualityComparer : IEqualityComparer<ICollection<T>>
        {
            public bool Equals(ICollection<T> x, ICollection<T> y)
            {
                if (x.Size() != y.Size())
                {
                    return false;
                }

                for (int i = 0; i < x.Size(); ++i)
                {
                    if (!x.Get(i).Equals(y.Get(i)))
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(ICollection<T> obj)
            {
                return obj.GetHashCode();
            }
        }
         
        public class EqualityComparerAdapter : System.Collections.Generic.IEqualityComparer<T>
        {
            private IEqualityComparer<T> comparer;

            public EqualityComparerAdapter(IEqualityComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public bool Equals(T x, T y)
            {
                return comparer.Equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }

        public class ComparerAdaptor : System.Collections.Generic.Comparer<T>
        {
            private readonly IComparer<T> comparer;

            public ComparerAdaptor(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public override int Compare(T x, T y)
            {
                return this.comparer.Compare(x, y);
            }
        }
    }
}
