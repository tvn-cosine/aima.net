using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepChainContrapositive : AbstractProofStep
    { 
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Chain contrapositive = null;
        private Chain contrapositiveOf = null;

        public ProofStepChainContrapositive(Chain contrapositive, Chain contrapositiveOf)
        {
            this.contrapositive = contrapositive;
            this.contrapositiveOf = contrapositiveOf;
            this.predecessors.Add(contrapositiveOf.getProofStep());
        }

        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return contrapositive.ToString();
        }

        public override string getJustification()
        {
            return "Contrapositive: " + contrapositiveOf.getProofStep().getStepNumber();
        }
    }
}
