using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.probability.api;

namespace aima.net.probability.proposition
{
    public class AssignmentProposition : AbstractTermProposition
    {
        private object value = null;
        //
        private string toString = null;

        public AssignmentProposition(IRandomVariable forVariable, object value)
            : base(forVariable)
        {
            setValue(value);
        }

        public object getValue()
        {
            return value;
        }

        public void setValue(object value)
        {
            if (null == value)
            {
                throw new IllegalArgumentException("The value for the Random Variable must be specified.");
            }
            this.value = value;
        }
         
        public override bool holds(IMap<IRandomVariable, object> possibleWorld)
        {
            return value.Equals(possibleWorld.Get(getTermVariable()));
        }
         
        public override string ToString()
        {
            if (null == toString)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append(getTermVariable().getName());
                sb.Append(" = ");
                sb.Append(value);

                toString = sb.ToString();
            }
            return toString;
        }
    } 
}
