using aima.net.exceptions;
using aima.net.probability.domain.api;

namespace aima.net.probability.domain
{
    public abstract class AbstractContinuousDomain : IContinuousDomain
    {
        public virtual bool IsFinite()
        {
            return false;
        }

        public virtual bool IsInfinite()
        {
            return true;
        }

        public virtual int Size()
        {
            throw new IllegalStateException("You cannot determine the size of an infinite domain");
        }

        public abstract bool IsOrdered();
    }
}
