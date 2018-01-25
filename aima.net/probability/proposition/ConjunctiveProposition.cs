using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.probability.api;
using aima.net.probability.proposition.api;

namespace aima.net.probability.proposition
{
    public class ConjunctiveProposition : AbstractProposition, IBinarySentenceProposition
    {
        private IProposition left = null;
        private IProposition right = null;
        //
        private string toString = null;

        public ConjunctiveProposition(IProposition left, IProposition right)
        {
            if (null == left)
            {
                throw new IllegalArgumentException("Left side of conjunction must be specified.");
            }
            if (null == right)
            {
                throw new IllegalArgumentException("Right side of conjunction must be specified.");
            }
            // Track nested scope
            addScope(left.getScope());
            addScope(right.getScope());
            addUnboundScope(left.getUnboundScope());
            addUnboundScope(right.getUnboundScope());

            this.left = left;
            this.right = right;
        }


        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
        {
            return left.holds(possibleWorld) && right.holds(possibleWorld);
        }

        public override string ToString()
        {
            if (null == toString)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append("(");
                sb.Append(left.ToString());
                sb.Append(" AND ");
                sb.Append(right.ToString());
                sb.Append(")");

                toString = sb.ToString();
            }

            return toString;
        }
    }
}
