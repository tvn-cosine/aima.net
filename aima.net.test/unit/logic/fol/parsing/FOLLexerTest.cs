using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.common;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.parsing;

namespace aima.net.test.unit.logic.fol.parsing
{
    [TestClass] public class FOLLexerTest
    {
        FOLLexer lexer;

        [TestInitialize]
        public void setUp()
        {
            FOLDomain domain = new FOLDomain();
            domain.addConstant("P");
            domain.addConstant("John");
            domain.addConstant("Saladin");
            domain.addFunction("LeftLeg");
            domain.addFunction("BrotherOf");
            domain.addFunction("EnemyOf");
            domain.addPredicate("HasColor");
            domain.addPredicate("King");
            lexer = new FOLLexer(domain);
        }

        [TestMethod]
        public void testLexBasicExpression()
        {
            lexer.setInput("( P )");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONSTANT, "P", 2),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 4),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 5),
                    lexer.nextToken());
        }

        [TestMethod]
        public void testConnectors()
        {
            lexer.setInput(" p  AND q");
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "p", 1),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "AND", 4),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "q", 8),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 9),
                    lexer.nextToken());
        }

        [TestMethod]
        public void testFunctions()
        {
            lexer.setInput(" LeftLeg(q)");
            Assert.AreEqual(new Token(LogicTokenTypes.FUNCTION, "LeftLeg", 1),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 8),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "q", 9),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 10),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 11),
                    lexer.nextToken());
        }

        [TestMethod]
        public void testPredicate()
        {
            lexer.setInput(" HasColor(r)");
            Assert.AreEqual(new Token(LogicTokenTypes.PREDICATE, "HasColor", 1),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 9),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "r", 10),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 11),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 12),
                    lexer.nextToken());
        }

        [TestMethod]
        public void testMultiArgPredicate()
        {
            lexer.setInput(" King(x,y)");
            Assert.AreEqual(new Token(LogicTokenTypes.PREDICATE, "King", 1),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 5),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "x", 6),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.COMMA, ",", 7),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "y", 8),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 9),
                    lexer.nextToken());
        }

        [TestMethod]
        public void testQuantifier()
        {
            lexer.setInput("FORALL x,y");
            Assert.AreEqual(new Token(LogicTokenTypes.QUANTIFIER, "FORALL", 0),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "x", 7),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.COMMA, ",", 8),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.VARIABLE, "y", 9),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 10),
                    lexer.nextToken());
        }

        [TestMethod]
        public void testTermEquality()
        {
            lexer.setInput("BrotherOf(John) = EnemyOf(Saladin)");
            Assert.AreEqual(new Token(LogicTokenTypes.FUNCTION, "BrotherOf", 0),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 9),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONSTANT, "John", 10),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 14),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.EQUALS, "=", 16),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.FUNCTION, "EnemyOf", 18),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 25),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONSTANT, "Saladin", 26),
                    lexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 33),
                    lexer.nextToken());
        }
    }

}
