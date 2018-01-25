using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.probability.api;
using aima.net.probability.proposition.api;

namespace aima.net.probability.proposition
{
    public class NotProposition : AbstractProposition, IUnarySentenceProposition
    {
        private IProposition proposition;
        //
        private string toString = null;

        public NotProposition(IProposition prop)
        {
            if (null == prop)
            {
                throw new IllegalArgumentException("Proposition to be negated must be specified.");
            }
            // Track nested scope
            addScope(prop.getScope());
            addUnboundScope(prop.getUnboundScope());

            proposition = prop;
        }


        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
        {
            return !proposition.holds(possibleWorld);
        }
         
        public override string ToString()
        {
            if (null == toString)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append("(NOT ");
                sb.Append(proposition.ToString());
                sb.Append(")");

                toString = sb.ToString();
            }
            return toString;
        }
    }
}
