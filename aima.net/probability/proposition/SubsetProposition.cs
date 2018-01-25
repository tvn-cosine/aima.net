using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.probability.api; 
using aima.net.probability.domain.api;

namespace aima.net.probability.proposition
{
    public class SubsetProposition : AbstractDerivedProposition
    {
        private IFiniteDomain subsetDomain = null;
        private IRandomVariable varSubsetOf = null;
        //
        private string toString = null;

        public SubsetProposition(string name, IFiniteDomain subsetDomain, IRandomVariable ofVar)
            : base(name)
        {


            if (null == subsetDomain)
            {
                throw new IllegalArgumentException("Sum Domain must be specified.");
            }
            this.subsetDomain = subsetDomain;
            this.varSubsetOf = ofVar;
            addScope(this.varSubsetOf);
        }
         
        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
        {
            return subsetDomain.GetPossibleValues().Contains(possibleWorld.Get(varSubsetOf));
        }

        public override string ToString()
        {
            if (null == toString)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append(getDerivedName());
                sb.Append(" = ");
                sb.Append(subsetDomain.ToString());
                toString = sb.ToString();
            }
            return toString;
        }
    } 
}
