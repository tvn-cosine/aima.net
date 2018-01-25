using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.search.csp
{
    /**
     * A domain Di consists of a set of allowable values {v1, ... , vk} for the
     * corresponding variable Xi and defines a default order on those values. This
     * implementation guarantees, that domains are never changed after they have
     * been created. Domain reduction is implemented by replacement instead of
     * modification. So previous states can easily and safely be restored.
     * 
     * @author Ruediger Lunde
     */
    public class Domain<VAL> : IEnumerable<VAL>
    {
        private readonly object[] values;

        public Domain(ICollection<VAL> values)
        {
            this.values = new object[values.Size()];
            for (int i = 0; i < values.Size(); ++i)
            {
                this.values[i] = values.Get(i);
            }
        }

        public Domain(params VAL[] values)
        {
            this.values = new object[values.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                this.values[i] = values[i];
            }
        }

        public int size()
        {
            return values.Length;
        }

        public VAL get(int index)
        {
            return (VAL)values[index];
        }

        public bool isEmpty()
        {
            return values.Length == 0;
        }

        public bool contains(VAL value)
        {
            foreach (object v in values)
                if (value.Equals(v))
                    return true;
            return false;
        }

        public IEnumerator<VAL> GetEnumerator()
        {
            return new Enumerator(values);
        }

        public ICollection<VAL> asList()
        {
            ICollection<VAL> obj = CollectionFactory.CreateQueue<VAL>();
            foreach (var o in values)
            {
                obj.Add((VAL)o);
            }
            return obj;
        }
         
        public override bool Equals(object obj)
        {
            if (obj != null && GetType() == obj.GetType())
            {
                Domain<VAL> d = (Domain<VAL>)obj;
                if (d.values.Length != values.Length)
                    return false;
                for (int i = 0; i < values.Length;++i)
                    if (!values[i].Equals(d.values[i]))
                        return false;
                return true;
            }
            return false;
        }
         
        public override int GetHashCode()
        {
            int hash = 9; // arbitrary seed value
            int multiplier = 13; // arbitrary multiplier value
            foreach (object value in values)
                hash = hash * multiplier + value.GetHashCode();
            return hash;
        }


        public override string ToString()
        {
            IStringBuilder result = TextFactory.CreateStringBuilder("{");
            bool comma = false;
            foreach (object value in values)
            {
                if (comma)
                    result.Append(", ");
                result.Append(value.ToString());
                comma = true;
            }
            result.Append("}");
            return result.ToString();
        }

        class Enumerator : IEnumerator<VAL>
        {
            private readonly object[] values;

            private int position = -1;

            public VAL Current
            {
                get
                {
                    return GetCurrent();
                }
            }

            public Enumerator(object[] values)
            {
                this.values = values;
            }

            public VAL GetCurrent()
            {
                return (VAL)values[position];
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
