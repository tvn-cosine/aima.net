using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.util;

namespace aima.net.agent.agentprogram.simplerule
{
    /// <summary>
    /// Implementation of an OR condition.
    /// </summary>
    public class ORCondition : Condition
    {
        private Condition left;
        private Condition right;

        public ORCondition(Condition leftCon, Condition rightCon)
        {
            if (null == leftCon ||
                null == rightCon)
            {
                throw new ArgumentNullException("leftCon, rightCon cannot be null");
            }

            left = leftCon;
            right = rightCon;
        }

        public override bool evaluate(ObjectWithDynamicAttributes p)
        {
            return (left.evaluate(p) || right.evaluate(p));
        }

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("[")
              .Append(left)
              .Append(" || ")
              .Append(right)
              .Append("]");

            return sb.ToString();
        }
    }
}
