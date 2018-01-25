using aima.net;
using aima.net.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.logic.common;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.logic.propositional.parsing
{
    /**
     * A concrete implementation of a lexical analyzer for the propositional language.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class PLLexer : Lexer
    {
        /**
         * Default Constructor.
         */
        public PLLexer()
        { }

        /**
         * Constructs a propositional expression lexer with the specified character
         * stream.
         * 
         * @param inputString
         *            a sequence of characters to be converted into a sequence of
         *            tokens.
         */
        public PLLexer(string inputString)
        {
            setInput(inputString);
        }

        /**
         * Returns the next propositional token from the character stream.
         * 
         * @return the next propositional token from the character stream.
         */

        public override Token nextToken()
        {
            int startPosition = getCurrentPositionInInput();
            if (lookAhead(1) == '(')
            {
                consume();
                return new Token(LogicTokenTypes.LPAREN, "(", startPosition);
            }
            else if (lookAhead(1) == '[')
            {
                consume();
                return new Token(LogicTokenTypes.LSQRBRACKET, "[", startPosition);
            }
            else if (lookAhead(1) == ')')
            {
                consume();
                return new Token(LogicTokenTypes.RPAREN, ")", startPosition);
            }
            else if (lookAhead(1) == ']')
            {
                consume();
                return new Token(LogicTokenTypes.RSQRBRACKET, "]", startPosition);
            }
            else if (null != lookAhead(1) && char.IsWhiteSpace(lookAhead(1).Value))
            {
                consume();
                return nextToken();
            }
            else if (null != lookAhead(1) && connectiveDetected(lookAhead(1).Value))
            {
                return connective();
            }
            else if (null != lookAhead(1) && symbolDetected(lookAhead(1).Value))
            {
                return symbol();
            }
            else if (null == lookAhead(1))
            {
                return new Token(LogicTokenTypes.EOI, "EOI", startPosition);
            }
            else
            {
                throw new LexerException("Lexing error on character " + lookAhead(1) + " at position " + getCurrentPositionInInput(), getCurrentPositionInInput());
            }
        }

        private bool connectiveDetected(char leadingChar)
        {
            return Connective.isConnectiveIdentifierStart(leadingChar);
        }

        private bool symbolDetected(char leadingChar)
        {
            return PropositionSymbol.isPropositionSymbolIdentifierStart(leadingChar);
        }

        private Token connective()
        {
            int startPosition = getCurrentPositionInInput();
            IStringBuilder sbuf = TextFactory.CreateStringBuilder();
            // Ensure pull out just one connective at a time, the isConnective(...)
            // test ensures we handle chained expressions like the following:
            // ~~P
            while (null != lookAhead(1) && Connective.isConnectiveIdentifierPart(lookAhead(1).Value) && !isConnective(sbuf.ToString()))
            {
                sbuf.Append(lookAhead(1));
                consume();
            }

            string symbol = sbuf.ToString();
            if (isConnective(symbol))
            {
                return new Token(LogicTokenTypes.CONNECTIVE, sbuf.ToString(), startPosition);
            }

            throw new LexerException("Lexing error on connective " + symbol + " at position " + getCurrentPositionInInput(), getCurrentPositionInInput());
        }

        private Token symbol()
        {
            int startPosition = getCurrentPositionInInput();
            IStringBuilder sbuf = TextFactory.CreateStringBuilder();
            while (null != lookAhead(1) && PropositionSymbol.isPropositionSymbolIdentifierPart(lookAhead(1).Value))
            {
                sbuf.Append(lookAhead(1));
                consume();
            }
            string symbol = sbuf.ToString();
            if (PropositionSymbol.isAlwaysTrueSymbol(symbol))
            {
                return new Token(LogicTokenTypes.TRUE, PropositionSymbol.TRUE_SYMBOL, startPosition);
            }
            else if (PropositionSymbol.isAlwaysFalseSymbol(symbol))
            {
                return new Token(LogicTokenTypes.FALSE, PropositionSymbol.FALSE_SYMBOL, startPosition);
            }
            else if (PropositionSymbol.isPropositionSymbol(symbol))
            {
                return new Token(LogicTokenTypes.SYMBOL, sbuf.ToString(), startPosition);
            }

            throw new LexerException("Lexing error on symbol " + symbol + " at position " + getCurrentPositionInInput(), getCurrentPositionInInput());
        }

        private bool isConnective(string aSymbol)
        {
            return Connective.isConnective(aSymbol);
        }
    }
}
