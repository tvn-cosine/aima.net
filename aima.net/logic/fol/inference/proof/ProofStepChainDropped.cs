using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepChainDropped : AbstractProofStep
    { 
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Chain dropped = null;
        private Chain droppedOff = null;

        public ProofStepChainDropped(Chain dropped, Chain droppedOff)
        {
            this.dropped = dropped;
            this.droppedOff = droppedOff;
            this.predecessors.Add(droppedOff.getProofStep());
        }

        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return dropped.ToString();
        }
         
        public override string getJustification()
        {
            return "Dropped: " + droppedOff.getProofStep().getStepNumber();
        }
    }
}
