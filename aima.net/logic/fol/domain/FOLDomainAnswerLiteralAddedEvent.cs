namespace aima.net.logic.fol.domain
{
    public class FOLDomainAnswerLiteralAddedEvent : FOLDomainEvent
    {
        private string answerLiteralName;

        public FOLDomainAnswerLiteralAddedEvent(object source, string answerLiteralName)
            : base(source)
        {
            this.answerLiteralName = answerLiteralName;
        }

        public string getAnswerLiteralNameName()
        {
            return answerLiteralName;
        }

        public override void notifyListener(FOLDomainListener listener)
        {
            listener.answerLiteralNameAdded(this);
        }
    }
}
