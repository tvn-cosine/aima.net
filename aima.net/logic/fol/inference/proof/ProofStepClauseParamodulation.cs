using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepClauseParamodulation : AbstractProofStep
    {
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Clause paramodulated = null;
        private Clause topClause = null;
        private Clause equalityClause = null;
        private TermEquality assertion = null;

        public ProofStepClauseParamodulation(Clause paramodulated,
                Clause topClause, Clause equalityClause, TermEquality assertion)
        {
            this.paramodulated = paramodulated;
            this.topClause = topClause;
            this.equalityClause = equalityClause;
            this.assertion = assertion;
            this.predecessors.Add(topClause.getProofStep());
            this.predecessors.Add(equalityClause.getProofStep());
        }

        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return paramodulated.ToString();
        }
         
        public override string getJustification()
        {
            return "Paramodulation: " + topClause.getProofStep().getStepNumber()
                    + ", " + equalityClause.getProofStep().getStepNumber() + ", ["
                    + assertion + "]"; 
        } 
    }
}
