using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepChainCancellation : AbstractProofStep
    { 
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Chain cancellation = null;
        private Chain cancellationOf = null;
        private IMap<Variable, Term> subst = null;

        public ProofStepChainCancellation(Chain cancellation, Chain cancellationOf, IMap<Variable, Term> subst)
        {
            this.cancellation = cancellation;
            this.cancellationOf = cancellationOf;
            this.subst = subst;
            this.predecessors.Add(cancellationOf.getProofStep());
        }
         
        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return cancellation.ToString();
        }
         
        public override string getJustification()
        {
            return "Cancellation: " + cancellationOf.getProofStep().getStepNumber() + " " + subst;
        } 
    } 
}
