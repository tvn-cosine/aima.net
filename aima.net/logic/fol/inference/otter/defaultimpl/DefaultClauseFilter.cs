using aima.net.collections.api;
using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol.inference.otter.defaultimpl
{
    public class DefaultClauseFilter : ClauseFilter
    {
        public DefaultClauseFilter()
        { }

        public ISet<Clause> filter(ISet<Clause> clauses)
        {
            return clauses;
        }
    }
}
