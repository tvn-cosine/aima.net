using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.probability.api;

namespace aima.net.probability.proposition
{
    public class EquivalentProposition : AbstractDerivedProposition
    {
        private string toString = null;

        public EquivalentProposition(string name, params IRandomVariable[] equivs)
                : base(name)
        {
            if (null == equivs || 1 >= equivs.Length)
            {
                throw new IllegalArgumentException("Equivalent variables must be specified.");
            }
            foreach (IRandomVariable rv in equivs)
            {
                addScope(rv);
            }
        }

        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
        {
            bool holds = true;
            bool first = true;

            IRandomVariable rvL = null;
            foreach (IRandomVariable rvC in getScope())
            {
                if (first)
                {
                    rvL = rvC;
                    first = false;
                    continue;
                }
                if (!possibleWorld.Get(rvL).Equals(possibleWorld.Get(rvC)))
                {
                    holds = false;
                    break;
                }
                rvL = rvC;
            }

            return holds;
        }

        public override string ToString()
        {
            if (null == toString)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append(getDerivedName());
                foreach (IRandomVariable rv in getScope())
                {
                    sb.Append(" = ");
                    sb.Append(rv);
                }
                toString = sb.ToString();
            }
            return toString;
        }
    }
}
