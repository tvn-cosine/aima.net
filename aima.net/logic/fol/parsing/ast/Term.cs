using aima.net.collections.api;

namespace aima.net.logic.fol.parsing.ast
{
    public interface Term : FOLNode
    {
        new ICollection<Term> getArgs();

        new Term copy();
    }
}
