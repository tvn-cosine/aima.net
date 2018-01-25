using aima.net.probability.proposition.api;

namespace aima.net.probability.proposition
{
    public abstract class AbstractDerivedProposition : AbstractProposition, IDerivedProposition
    { 
        private string name = null;

        public AbstractDerivedProposition(string name)
        {
            this.name = name;
        }
         
        public virtual string getDerivedName()
        {
            return name;
        } 
    }
}
