using aima.net.collections.api;
using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol.inference.otter
{
    public interface ClauseFilter
    {
        ISet<Clause> filter(ISet<Clause> clauses);
    }
}
