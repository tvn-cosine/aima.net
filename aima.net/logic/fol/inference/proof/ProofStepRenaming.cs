using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepRenaming : AbstractProofStep
    {
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private object proof = "";

        public ProofStepRenaming(object proof, ProofStep predecessor)
        {
            this.proof = proof;
            this.predecessors.Add(predecessor);
        }
         
        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return proof.ToString();
        }
         
        public override string getJustification()
        {
            return "Renaming of " + predecessors.Get(0).getStepNumber();
        }
    }
}
