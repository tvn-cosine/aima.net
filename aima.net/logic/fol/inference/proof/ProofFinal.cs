using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference.proof
{
    public class ProofFinal : Proof
    {
        private IMap<Variable, Term> answerBindings = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
        private ProofStep finalStep = null;
        private ICollection<ProofStep> proofSteps = null;

        public ProofFinal(ProofStep finalStep, IMap<Variable, Term> answerBindings)
        {
            this.finalStep = finalStep;
            this.answerBindings.PutAll(answerBindings);
        }

        public ICollection<ProofStep> getSteps()
        {
            // Only calculate if the proof steps are actually requested.
            if (null == proofSteps)
            {
                calcualteProofSteps();
            }
            return proofSteps;
        }

        public IMap<Variable, Term> getAnswerBindings()
        {
            return answerBindings;
        }

        public void replaceAnswerBindings(IMap<Variable, Term> updatedBindings)
        {
            answerBindings.Clear();
            answerBindings.PutAll(updatedBindings);
        }

        public override string ToString()
        {
            return answerBindings.ToString();
        }

        private void calcualteProofSteps()
        {
            proofSteps = CollectionFactory.CreateQueue<ProofStep>();
            addToProofSteps(finalStep);

            // Move all premises to the front of the
            // list of steps
            int to = 0;
            for (int i = 0; i < proofSteps.Size();++i)
            {
                if (proofSteps.Get(i) is ProofStepPremise)
                {
                    ProofStep m = proofSteps.Get(i);
                    proofSteps.Remove(m);
                    proofSteps.Insert(to, m);
                    to++;
                }
            }

            // Move the Goals after the premises
            for (int i = 0; i < proofSteps.Size();++i)
            {
                if (proofSteps.Get(i) is ProofStepGoal)
                {
                    ProofStep m = proofSteps.Get(i);
                    proofSteps.Remove(m);
                    proofSteps.Insert(to, m);
                    to++;
                }
            }

            // Assign the step #s now that all the proof
            // steps have been unwound
            for (int i = 0; i < proofSteps.Size();++i)
            {
                proofSteps.Get(i).setStepNumber(i + 1);
            }
        }

        private void addToProofSteps(ProofStep step)
        {
            if (!proofSteps.Contains(step))
            {
                proofSteps.Insert(0, step);
            }
            else
            {
                proofSteps.Remove(step);
                proofSteps.Insert(0, step);
            }
            ICollection<ProofStep> predecessors = step.getPredecessorSteps();
            for (int i = predecessors.Size() - 1; i >= 0; i--)
            {
                addToProofSteps(predecessors.Get(i));
            }
        }
    }
}
