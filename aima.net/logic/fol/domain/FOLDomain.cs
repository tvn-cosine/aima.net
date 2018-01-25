using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.logic.fol.domain
{
    public class FOLDomain
    {
        private ISet<string> constants, functions, predicates;
        private int skolemConstantIndexical = 0;
        private int skolemFunctionIndexical = 0;
        private int answerLiteralIndexical = 0;
        private ICollection<FOLDomainListener> listeners = CollectionFactory.CreateQueue<FOLDomainListener>();

        public FOLDomain()
        {
            this.constants = CollectionFactory.CreateSet<string>();
            this.functions = CollectionFactory.CreateSet<string>();
            this.predicates = CollectionFactory.CreateSet<string>();
        }

        public FOLDomain(FOLDomain toCopy)
            : this(toCopy.getConstants(), toCopy.getFunctions(), toCopy.getPredicates())
        { }

        public FOLDomain(ISet<string> constants, ISet<string> functions, ISet<string> predicates)
        {
            this.constants = CollectionFactory.CreateSet<string>(constants);
            this.functions = CollectionFactory.CreateSet<string>(functions);
            this.predicates = CollectionFactory.CreateSet<string>(predicates);
        }

        public ISet<string> getConstants()
        {
            return constants;
        }

        public ISet<string> getFunctions()
        {
            return functions;
        }

        public ISet<string> getPredicates()
        {
            return predicates;
        }

        public void addConstant(string constant)
        {
            constants.Add(constant);
        }

        public string addSkolemConstant()
        {

            string sc = null;
            do
            {
                sc = "SC" + (skolemConstantIndexical++);
            } while (constants.Contains(sc)
                    || functions.Contains(sc)
                    || predicates.Contains(sc));

            addConstant(sc);
            notifyFOLDomainListeners(new FOLDomainSkolemConstantAddedEvent(this, sc));

            return sc;
        }

        public void addFunction(string function)
        {
            functions.Add(function);
        }

        public string addSkolemFunction()
        {
            string sf = null;
            do
            {
                sf = "SF" + (skolemFunctionIndexical++);
            } while (constants.Contains(sf) || functions.Contains(sf)
                    || predicates.Contains(sf));

            addFunction(sf);
            notifyFOLDomainListeners(new FOLDomainSkolemFunctionAddedEvent(this, sf));

            return sf;
        }

        public void addPredicate(string predicate)
        {
            predicates.Add(predicate);
        }

        public string addAnswerLiteral()
        {
            string al = null;
            do
            {
                al = "Answer" + (answerLiteralIndexical++);
            } while (constants.Contains(al) || functions.Contains(al)
                    || predicates.Contains(al));

            addPredicate(al);
            notifyFOLDomainListeners(new FOLDomainAnswerLiteralAddedEvent(this, al));

            return al;
        }

        public void addFOLDomainListener(FOLDomainListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void removeFOLDomainListener(FOLDomainListener listener)
        {
            listeners.Remove(listener);
        }
         
        private void notifyFOLDomainListeners(FOLDomainEvent even)
        {
            foreach (FOLDomainListener l in listeners)
            { 
                even.notifyListener(l);
            }
        }
    }
}
