using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepClauseClausifySentence : AbstractProofStep
    {
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Clause clausified = null;

        public ProofStepClauseClausifySentence(Clause clausified, Sentence origSentence)
        {
            this.clausified = clausified;
            this.predecessors.Add(new ProofStepPremise(origSentence));
        }

        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return clausified.ToString();
        }

        public override string getJustification()
        {
            return "Clausified " + predecessors.Get(0).getStepNumber();
        }
    }
}
