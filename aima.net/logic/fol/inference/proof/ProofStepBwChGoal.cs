using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepBwChGoal : AbstractProofStep
    { 
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        //
        private Clause toProve = null;
        private Literal currentGoal = null;
        private IMap<Variable, Term> bindings = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

        public ProofStepBwChGoal(Clause toProve, Literal currentGoal, IMap<Variable, Term> bindings)
        {
            this.toProve = toProve;
            this.currentGoal = currentGoal;
            this.bindings.PutAll(bindings);
        }

        public IMap<Variable, Term> getBindings()
        {
            return bindings;
        }

        public void setPredecessor(ProofStep predecessor)
        {
            predecessors.Clear();
            predecessors.Add(predecessor);
        }

        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            ICollection<Literal> nLits = toProve.getNegativeLiterals();
            for (int i = 0; i < toProve.getNumberNegativeLiterals();++i)
            {
                sb.Append(nLits.Get(i).getAtomicSentence());
                if (i != (toProve.getNumberNegativeLiterals() - 1))
                {
                    sb.Append(" AND ");
                }
            }
            if (toProve.getNumberNegativeLiterals() > 0)
            {
                sb.Append(" => ");
            }
            sb.Append(toProve.getPositiveLiterals().Get(0));
            return sb.ToString();
        }
         
        public override string getJustification()
        {
            return "Current Goal " + currentGoal.getAtomicSentence().ToString() + ", " + bindings;
        }
    }
}
