using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.common;
using aima.net.logic.propositional.parsing;

namespace aima.net.test.unit.logic.propositional.parsing
{
    [TestClass]
    public class PLLexerTest
    {
        private PLLexer pllexer;

        [TestInitialize]
        public void setUp()
        {
            pllexer = new PLLexer();
        }

        [TestMethod]
        public void testLexBasicExpression()
        {
            pllexer.setInput("(P)");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0),
                    pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "P", 1),
                    pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 2),
                    pllexer.nextToken());

            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 3),
                    pllexer.nextToken());
        }

        [TestMethod]
        public void testLexNotExpression()
        {
            pllexer.setInput("(~ P)");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0),
                    pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "~", 1),
                    pllexer.nextToken());

            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "P", 3),
                    pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.RPAREN, ")", 4),
                    pllexer.nextToken());

            Assert.AreEqual(new Token(LogicTokenTypes.EOI, "EOI", 5),
                    pllexer.nextToken());
        }

        [TestMethod]
        public void testLexImpliesExpression()
        {
            pllexer.setInput("(P => Q)");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0),
                    pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "P", 1),
                    pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "=>", 3),
                    pllexer.nextToken());
        }

        [TestMethod]
        public void testLexBiCOnditionalExpression()
        {
            pllexer.setInput("(B11 <=> (P12 | P21))");
            Assert.AreEqual(new Token(LogicTokenTypes.LPAREN, "(", 0), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.SYMBOL, "B11", 1), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "<=>", 5),
                    pllexer.nextToken());
        }

        [TestMethod]
        public void testChainedConnectiveExpression()
        {
            pllexer.setInput("~~&&||=>=><=><=>");
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "~", 0), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "~", 1), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "&", 2), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "&", 3), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "|", 4), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "|", 5), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "=>", 6), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "=>", 8), pllexer.nextToken());
            Assert.AreEqual(new Token(LogicTokenTypes.CONNECTIVE, "<=>", 10), pllexer.nextToken());
        }

        [TestMethod]
        public void testLexerException()
        {
            try
            {
                pllexer.setInput("A & B.1 & C");
                pllexer.nextToken();
                pllexer.nextToken();
                pllexer.nextToken();
                // the , after 'B' is not a legal character
                pllexer.nextToken();
                Assert.Fail("A LexerException should have been thrown here");
            }
            catch (LexerException le)
            {
                // Ensure the correct position in the input is identified.
                Assert.AreEqual(5, le.getCurrentPositionInInputExceptionThrown());
            }
        }
    }

}
