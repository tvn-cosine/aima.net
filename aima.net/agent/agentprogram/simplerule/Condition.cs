using aima.net.api;
using aima.net.util;

namespace aima.net.agent.agentprogram.simplerule
{
    /// <summary>
    /// Base abstract class for describing conditions.
    /// </summary>
    public abstract class Condition : IEquatable, IHashable, IStringable
    {
        public abstract bool evaluate(ObjectWithDynamicAttributes p); 

        public override bool Equals(object o)
        {
            return o != null 
                && GetType() == o.GetType() 
                && ToString().Equals(o.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
