using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.logic.common
{
    /**
     * A runtime exception to be used to describe Parser exceptions. In particular
     * it provides information to help in identifying which tokens proved
     * problematic in the parse.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ParserException : RuntimeException
    {
        private ICollection<Token> problematicTokens = CollectionFactory.CreateQueue<Token>();

        public ParserException(string message, params Token[] problematicTokens)
            : base(message)
        {

            if (problematicTokens != null)
            {
                foreach (Token pt in problematicTokens)
                {
                    this.problematicTokens.Add(pt);
                }
            }
        }

        public ParserException(string message, Exception cause, params Token[] problematicTokens)
            : base(message, cause)
        {

            if (problematicTokens != null)
            {
                foreach (Token pt in problematicTokens)
                {
                    this.problematicTokens.Add(pt);
                }
            }
        }

        /**
         * 
         * @return a list of 0 or more tokens from the input stream that are
         *         believed to have contributed to the parse exception.
         */
        public ICollection<Token> getProblematicTokens()
        {
            return problematicTokens;
        }
    }
}
