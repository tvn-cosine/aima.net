using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.util;

namespace aima.net.agent.agentprogram.simplerule
{
    /// <summary>
    /// Implementation of an EQUALity condition.
    /// </summary>
    public class EQUALCondition : Condition
    {
        private object key;
        private object value;

        public EQUALCondition(object key, object value)
        {
            if (null == key ||
                null == value)
            {
                throw new ArgumentNullException("key, value cannot be null");
            }

            this.key = key;
            this.value = value;
        }

        public override bool evaluate(ObjectWithDynamicAttributes p)
        {
            return value.Equals(p.GetAttribute(key));
        }

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append(key)
              .Append("==")
              .Append(value);

            return sb.ToString();
        }
    }
}
