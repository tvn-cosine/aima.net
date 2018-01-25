using aima.net.logic.fol.kb.data;

namespace aima.net.logic.fol.inference.otter
{
    public interface ClauseSimplifier
    {
        Clause simplify(Clause c);
    }
}
