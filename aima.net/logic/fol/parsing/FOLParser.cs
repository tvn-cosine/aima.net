using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.common;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.parsing
{
    public class FOLParser
    {
        private FOLLexer lexer;

        protected Token[] lookAheadBuffer;

        protected int _lookAhead = 1;

        public FOLParser(FOLLexer lexer)
        {
            this.lexer = lexer;
            lookAheadBuffer = new Token[_lookAhead];
        }

        public FOLParser(FOLDomain domain)
            : this(new FOLLexer(domain))
        { }

        public FOLDomain getFOLDomain()
        {
            return lexer.getFOLDomain();
        }

        public Sentence parse(string s)
        {
            setUpToParse(s);
            return parseSentence();
        }

        public void setUpToParse(string s)
        {
            lookAheadBuffer = new Token[1];
            lexer.setInput(s);
            fillLookAheadBuffer();

        }

        private Term parseTerm()
        {
            Token t = lookAhead(1);
            int tokenType = t.getType();
            if (tokenType == LogicTokenTypes.CONSTANT)
            {
                return parseConstant();
            }
            else if (tokenType == LogicTokenTypes.VARIABLE)
            {
                return parseVariable();
            }
            else if (tokenType == LogicTokenTypes.FUNCTION)
            {
                return parseFunction();
            }

            else
            {
                return null;
            }
        }

        public Term parseVariable()
        {
            Token t = lookAhead(1);
            string value = t.getText();
            consume();
            return new Variable(value);
        }

        public Term parseConstant()
        {
            Token t = lookAhead(1);
            string value = t.getText();
            consume();
            return new Constant(value);
        }

        public Term parseFunction()
        {
            Token t = lookAhead(1);
            string functionName = t.getText();
            ICollection<Term> terms = processTerms();
            return new Function(functionName, terms);
        }

        public Sentence parsePredicate()
        {
            Token t = lookAhead(1);
            string predicateName = t.getText();
            ICollection<Term> terms = processTerms();
            return new Predicate(predicateName, terms);
        }

        private ICollection<Term> processTerms()
        {
            consume();
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            match("(");
            Term term = parseTerm();
            terms.Add(term);

            while (lookAhead(1).getType() == LogicTokenTypes.COMMA)
            {
                match(",");
                term = parseTerm();
                terms.Add(term);
            }
            match(")");
            return terms;
        }

        public Sentence parseTermEquality()
        {
            Term term1 = parseTerm();
            match("=");
            // System.Console.WriteLine("=");
            Term term2 = parseTerm();
            return new TermEquality(term1, term2);
        }

        public Sentence parseNotSentence()
        {
            match("NOT");
            return new NotSentence(parseSentence());
        }

        //
        // PROTECTED METHODS
        //
        protected Token lookAhead(int i)
        {
            return lookAheadBuffer[i - 1];
        }

        protected void consume()
        {
            // System.Console.WriteLine("consuming" +lookAheadBuffer[0].getText());
            loadNextTokenFromInput();
            // System.Console.WriteLine("next token " +lookAheadBuffer[0].getText());
        }

        protected void loadNextTokenFromInput()
        {

            bool eoiEncountered = false;
            for (int i = 0; i < _lookAhead - 1;++i)
            {

                lookAheadBuffer[i] = lookAheadBuffer[i + 1];
                if (isEndOfInput(lookAheadBuffer[i]))
                {
                    eoiEncountered = true;
                    break;
                }
            }
            if (!eoiEncountered)
            {
                try
                {
                    lookAheadBuffer[_lookAhead - 1] = lexer.nextToken();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        protected bool isEndOfInput(Token t)
        {
            return (t.getType() == LogicTokenTypes.EOI);
        }

        protected void fillLookAheadBuffer()
        {
            for (int i = 0; i < _lookAhead;++i)
            {
                lookAheadBuffer[i] = lexer.nextToken();
            }
        }

        protected void match(string terminalSymbol)
        {
            if (lookAhead(1).getText().Equals(terminalSymbol))
            {
                consume();
            }
            else
            {
                throw new RuntimeException(
                        "Syntax error detected at match. Expected "
                                + terminalSymbol + " but got "
                                + lookAhead(1).getText());
            }

        }

        //
        // PRIVATE METHODS
        //

        private Sentence parseSentence()
        {
            Token t = lookAhead(1);
            if (lParen(t))
            {
                return parseParanthizedSentence();
            }
            else if ((lookAhead(1).getType() == LogicTokenTypes.QUANTIFIER))
            {

                return parseQuantifiedSentence();
            }
            else if (notToken(t))
            {
                return parseNotSentence();
            }
            else if (predicate(t))
            {
                return parsePredicate();
            }
            else if (term(t))
            {
                return parseTermEquality();
            }

            throw new RuntimeException("parse failed with Token " + t.getText());
        }

        private Sentence parseQuantifiedSentence()
        {
            string quantifier = lookAhead(1).getText();
            consume();
            ICollection<Variable> variables = CollectionFactory.CreateQueue<Variable>();
            Variable var = (Variable)parseVariable();
            variables.Add(var);
            while (lookAhead(1).getType() == LogicTokenTypes.COMMA)
            {
                consume();
                var = (Variable)parseVariable();
                variables.Add(var);
            }
            Sentence sentence = parseSentence();
            return new QuantifiedSentence(quantifier, variables, sentence);
        }

        private Sentence parseParanthizedSentence()
        {
            match("(");
            Sentence sen = parseSentence();
            while (binaryConnector(lookAhead(1)))
            {
                string connector = lookAhead(1).getText();
                consume();
                Sentence other = parseSentence();
                sen = new ConnectedSentence(connector, sen, other);
            }
            match(")");
            return sen; /* new ParanthizedSentence */

        }

        private bool binaryConnector(Token t)
        {
            if ((t.getType() == LogicTokenTypes.CONNECTIVE)
                    && (!(t.getText().Equals("NOT"))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool lParen(Token t)
        {
            if (t.getType() == LogicTokenTypes.LPAREN)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool term(Token t)
        {
            if ((t.getType() == LogicTokenTypes.FUNCTION)
                    || (t.getType() == LogicTokenTypes.CONSTANT)
                    || (t.getType() == LogicTokenTypes.VARIABLE))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool predicate(Token t)
        {
            if ((t.getType() == LogicTokenTypes.PREDICATE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool notToken(Token t)
        {
            if ((t.getType() == LogicTokenTypes.CONNECTIVE)
                    && (t.getText().Equals("NOT")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
