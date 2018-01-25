using aima.net.collections.api;

namespace aima.net.logic.fol.inference.proof
{
    public interface ProofStep
    {
        int getStepNumber();

        void setStepNumber(int step);

        ICollection<ProofStep> getPredecessorSteps();

        string getProof();

        string getJustification();
    }
}
