using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.util;

namespace aima.net.agent.agentprogram.simplerule
{
    /// <summary>
    /// Implementation of a NOT condition.
    /// </summary>
    public class NOTCondition : Condition
    {
        private Condition con;

        public NOTCondition(Condition con)
        {
            if (null == con)
            {
                throw new ArgumentNullException("con cannot be null");
            }

            this.con = con;
        }

        public override bool evaluate(ObjectWithDynamicAttributes p)
        {
            return (!con.evaluate(p));
        }

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("![")
              .Append(con)
              .Append("]");

            return sb.ToString();
        }
    }
}
