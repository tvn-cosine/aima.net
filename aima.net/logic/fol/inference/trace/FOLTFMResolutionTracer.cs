using aima.net.collections.api;
using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol.inference.trace
{
    public interface FOLTFMResolutionTracer
    {
        void stepStartWhile(ISet<Clause> clauses, int totalNoClauses, int totalNoNewCandidateClauses);
        void stepOuterFor(Clause i);
        void stepInnerFor(Clause i, Clause j);
        void stepResolved(Clause iFactor, Clause jFactor, ISet<Clause> resolvents);
        void stepFinished(ISet<Clause> clauses, InferenceResult result);
    }
}
