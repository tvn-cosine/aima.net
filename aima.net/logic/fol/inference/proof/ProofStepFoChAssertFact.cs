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
    public class ProofStepFoChAssertFact : AbstractProofStep
    {
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        //
        private Clause implication = null;
        private Literal fact = null;
        private IMap<Variable, Term> bindings = null;

        public ProofStepFoChAssertFact(Clause implication, Literal fact, IMap<Variable, Term> bindings, ProofStep predecessor)
        {
            this.implication = implication;
            this.fact = fact;
            this.bindings = bindings;
            if (null != predecessor)
            {
                predecessors.Add(predecessor);
            }
        }
         
        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            ICollection<Literal> nLits = implication.getNegativeLiterals();
            for (int i = 0; i < implication.getNumberNegativeLiterals();++i)
            {
                sb.Append(nLits.Get(i).getAtomicSentence());
                if (i != (implication.getNumberNegativeLiterals() - 1))
                {
                    sb.Append(" AND ");
                }
            }
            sb.Append(" => ");
            sb.Append(implication.getPositiveLiterals().Get(0));
            return sb.ToString();
        }


        public override string getJustification()
        {
            return "Assert fact " + fact.ToString() + ", " + bindings;
        } 
    }
}
