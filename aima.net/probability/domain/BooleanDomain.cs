using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.probability.domain
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 486.
    /// <para />
    /// A Boolean random variable has the domain {true,false}.
    /// </summary>
    public class BooleanDomain : AbstractFiniteDomain<bool>
    {
        private static ISet<bool> _possibleValues;
        static BooleanDomain()
        {
            // Keep consistent order
            _possibleValues = CollectionFactory.CreateSet<bool>();
            _possibleValues.Add(true);
            _possibleValues.Add(false);
            // Ensure cannot be modified
            _possibleValues = CollectionFactory.CreateReadOnlySet<bool>(_possibleValues);
        }

        public BooleanDomain()
        {
            indexPossibleValues(_possibleValues);
        }

        public override int Size()
        {
            return 2;
        }
         
        public override bool IsOrdered()
        {
            return false;
        }

        //ISet<bool> getPossibleValues()
        //{ 
        //    return _possibleValues;
        //}

        public override ISet<object> GetPossibleValues()
        {
            ISet<object> obj = CollectionFactory.CreateSet<object>();
            foreach (bool value in _possibleValues)
            {
                obj.Add(value);
            }
            return obj;
        }

        public override bool Equals(object o)
        {
            return o is BooleanDomain;
        }

        public override int GetHashCode()
        {
            return _possibleValues.GetHashCode();
        }
    }
}
