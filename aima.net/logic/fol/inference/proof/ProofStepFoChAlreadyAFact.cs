using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepFoChAlreadyAFact : AbstractProofStep
    { 
        private static readonly ICollection<ProofStep> _noPredecessors = CollectionFactory.CreateQueue<ProofStep>(); 
        private Literal fact = null;

        public ProofStepFoChAlreadyAFact(Literal fact)
        {
            this.fact = fact;
        }
         
        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(_noPredecessors);
        }
         
        public override string getProof()
        {
            return fact.ToString();
        }
         
        public override string getJustification()
        {
            return "Already a known fact in the KB.";
        } 
    }
}
