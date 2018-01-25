using aima.net.collections.api;

namespace aima.net.logic.fol.parsing.ast
{
    public interface AtomicSentence : Sentence
    {
        new ICollection<Term> getArgs();
        new AtomicSentence copy();
    }
}
