using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.probability.domain
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 486.
    /// <para />
    /// As in CSPs, domains can be sets of arbitrary tokens; we might choose the
    /// domain of Age to be {juvenile,teen,adult} and the domain of
    /// Weather might be {sunny,rain,cloudy,snow}.
    /// </summary>
    public class ArbitraryTokenDomain : AbstractFiniteDomain<object>
    {
        private ISet<object> possibleValues = null;
        private bool ordered = false;

        public ArbitraryTokenDomain(params object[] pValues)
                : this(false, pValues)
        { }

        public ArbitraryTokenDomain(bool ordered, params object[] pValues)
        {
            this.ordered = ordered;
            // Keep consistent order
            possibleValues = CollectionFactory.CreateSet<object>();
            foreach (object v in pValues)
            {
                possibleValues.Add(v);
            }
            // Ensure cannot be modified
            possibleValues = CollectionFactory.CreateReadOnlySet<object>(possibleValues);

            indexPossibleValues(possibleValues);
        }
        
        public override int Size()
        {
            return possibleValues.Size();
        }


        public override bool IsOrdered()
        {
            return ordered;
        }
         
        public override ISet<object> GetPossibleValues()
        {
            return possibleValues;
        }
        
        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if (!(o is ArbitraryTokenDomain))
            {
                return false;
            }

            ArbitraryTokenDomain other = (ArbitraryTokenDomain)o;

            return this.possibleValues.Equals(other.possibleValues);
        } 

        public override int GetHashCode()
        {
            return possibleValues.GetHashCode();
        }
    } 
}
