namespace aima.net.logic.fol.inference.trace
{
    public interface FOLModelEliminationTracer
    {
        void reset();

        void increment(int depth, int noFarParents);
    }
}
