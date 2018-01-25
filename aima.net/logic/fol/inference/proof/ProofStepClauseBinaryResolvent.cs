using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofStepClauseBinaryResolvent : AbstractProofStep
    {
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Clause resolvent = null;
        private Literal posLiteral = null;
        private Literal negLiteral = null;
        private Clause parent1, parent2 = null;
        private IMap<Variable, Term> subst = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
        private IMap<Variable, Term> renameSubst = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

        public ProofStepClauseBinaryResolvent(Clause resolvent, Literal pl,
                Literal nl, Clause parent1, Clause parent2,
                IMap<Variable, Term> subst, IMap<Variable, Term> renameSubst)
        {
            this.resolvent = resolvent;
            this.posLiteral = pl;
            this.negLiteral = nl;
            this.parent1 = parent1;
            this.parent2 = parent2;
            this.subst.PutAll(subst);
            this.renameSubst.PutAll(renameSubst);
            this.predecessors.Add(parent1.getProofStep());
            this.predecessors.Add(parent2.getProofStep());
        }
         
        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return resolvent.ToString();
        }

        public override string getJustification()
        {
            int lowStep = parent1.getProofStep().getStepNumber();
            int highStep = parent2.getProofStep().getStepNumber();

            if (lowStep > highStep)
            {
                lowStep = highStep;
                highStep = parent1.getProofStep().getStepNumber();
            }

            return "Resolution: " + lowStep + ", " + highStep + "  [" + posLiteral
                    + ", " + negLiteral + "], subst=" + subst + ", renaming="
                    + renameSubst;
        } 
    }
}
