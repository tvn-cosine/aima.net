using aima.net.logic.fol.kb;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference
{
    public interface InferenceProcedure
    {
        /**
         * 
         * @param kb
         *            the knowledge base against which the query is to be made.
         * @param query
         *            the query to be answered.
         * @return an InferenceResult.
         */
        InferenceResult ask(FOLKnowledgeBase kb, Sentence query);
    }
}
