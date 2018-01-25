using aima.net;
using aima.net.api;

namespace aima.net.logic.fol.domain
{
    public abstract class FOLDomainEvent : EventObject
    {
        public FOLDomainEvent(object source)
                : base(source)
        { }

        public abstract void notifyListener(FOLDomainListener listener);
    }
}
