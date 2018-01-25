namespace aima.net.logic.fol.domain
{
    public interface FOLDomainListener
    {
        void skolemConstantAdded(FOLDomainSkolemConstantAddedEvent even); 
        void skolemFunctionAdded(FOLDomainSkolemFunctionAddedEvent even); 
        void answerLiteralNameAdded(FOLDomainAnswerLiteralAddedEvent even);
    }
}
