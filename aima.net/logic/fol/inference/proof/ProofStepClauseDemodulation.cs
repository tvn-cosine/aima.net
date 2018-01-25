using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepClauseDemodulation : AbstractProofStep
    {
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Clause demodulated = null;
        private Clause origClause = null;
        private TermEquality assertion = null;

        public ProofStepClauseDemodulation(Clause demodulated, Clause origClause, TermEquality assertion)
        {
            this.demodulated = demodulated;
            this.origClause = origClause;
            this.assertion = assertion;
            this.predecessors.Add(origClause.getProofStep());
        }

        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return demodulated.ToString();
        }

        public override string getJustification()
        {
            return "Demodulation: " + origClause.getProofStep().getStepNumber() + ", [" + assertion + "]";
        }
    }
}
