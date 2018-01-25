using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.common;
using aima.net.logic.propositional.parsing;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.test.unit.logic.propositional.parsing
{
    [TestClass]
    public class PLParserTest
    {
        private PLParser parser = null;
        private Sentence sentence = null;
        private string expected = null;

        [TestInitialize]
        public void setUp()
        {
            parser = new PLParser();
        }

        [TestMethod]
        public void testAtomicSentenceTrueParse()
        {
            sentence = parser.parse("true");
            expected = prettyPrintF("True");
            Assert.IsTrue(sentence.isPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("(true)");
            expected = prettyPrintF("True");
            Assert.IsTrue(sentence.isPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("((true))");
            expected = prettyPrintF("True");
            Assert.IsTrue(sentence.isPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testAtomicSentenceFalseParse()
        {
            sentence = parser.parse("faLse");
            expected = prettyPrintF("False");
            Assert.IsTrue(sentence.isPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testAtomicSentenceSymbolParse()
        {
            sentence = parser.parse("AIMA");
            expected = prettyPrintF("AIMA");
            Assert.IsTrue(sentence.isPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testNotSentenceParse()
        {
            sentence = parser.parse("~ AIMA");
            expected = prettyPrintF("~AIMA");
            Assert.IsTrue(sentence.isNotSentence());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testDoubleNegation()
        {
            sentence = parser.parse("~~AIMA");
            expected = prettyPrintF("~~AIMA");
            Assert.IsTrue(sentence.isNotSentence());
            Assert.IsTrue(sentence.getSimplerSentence(0).isNotSentence());
            Assert.IsTrue(sentence.getSimplerSentence(0).getSimplerSentence(0).isPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testBinarySentenceParse()
        {
            sentence = parser.parse("PETER  &  NORVIG");
            expected = prettyPrintF("PETER & NORVIG");
            Assert.IsTrue(sentence.isAndSentence());
            Assert.IsTrue(sentence.getSimplerSentence(0).isPropositionSymbol());
            Assert.IsTrue(sentence.getSimplerSentence(1).isPropositionSymbol());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testComplexSentenceParse()
        {
            sentence = parser.parse("(NORVIG | AIMA | LISP) & TRUE");
            expected = prettyPrintF("(NORVIG | AIMA | LISP) & True");
            Assert.IsTrue(sentence.isAndSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("((NORVIG | AIMA | LISP) & (((LISP => COOL))))");
            expected = prettyPrintF("(NORVIG | AIMA | LISP) & (LISP => COOL)");
            Assert.IsTrue(sentence.isAndSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("((~ (P & Q ))  & ((~ (R & S))))");
            expected = prettyPrintF("~(P & Q) & ~(R & S)");
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("((P & Q) | (S & T))");
            expected = prettyPrintF("P & Q | S & T");
            Assert.IsTrue(sentence.isOrSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("(~ ((P & Q) => (S & T)))");
            expected = prettyPrintF("~(P & Q => S & T)");
            Assert.IsTrue(sentence.isNotSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("(~ (P <=> (S & T)))");
            expected = prettyPrintF("~(P <=> S & T)");
            Assert.IsTrue(sentence.isNotSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("(P <=> (S & T))");
            expected = prettyPrintF("P <=> S & T");
            Assert.IsTrue(sentence.isBiconditionalSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("(P => Q)");
            expected = prettyPrintF("P => Q");
            Assert.IsTrue(sentence.isImplicationSentence());
            Assert.AreEqual(expected, sentence.ToString());

            sentence = parser.parse("((P & Q) => R)");
            expected = prettyPrintF("P & Q => R");
            Assert.IsTrue(sentence.isImplicationSentence());
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testSquareBracketsParse()
        {
            // Instead of
            sentence = parser.parse("[NORVIG | AIMA | LISP] & TRUE");
            expected = prettyPrintF("(NORVIG | AIMA | LISP) & True");
            Assert.AreEqual(expected, sentence.ToString());

            // Alternating
            sentence = parser.parse("[A | B | C] & D & [C | D & (F | G | H & [I | J])]");
            expected = prettyPrintF("(A | B | C) & D & (C | D & (F | G | H & (I | J)))");
            Assert.AreEqual(expected, sentence.ToString());
        }

        [TestMethod]
        public void testParserException()
        {
            try
            {
                sentence = parser.parse("");
                Assert.Fail("A Parser Exception should have been thrown.");
            }
            catch (ParserException pex)
            {
                Assert.AreEqual(0, pex.getProblematicTokens().Size());
            }

            try
            {
                sentence = parser.parse("A A1.2");
                Assert.Fail("A Parser Exception should have been thrown.");
            }
            catch (ParserException pex)
            {
                Assert.AreEqual(0, pex.getProblematicTokens().Size());
                Assert.IsTrue(pex.InnerException is LexerException);
                Assert.AreEqual(4, ((LexerException)pex.InnerException).getCurrentPositionInInputExceptionThrown());
            }

            try
            {
                sentence = parser.parse("A & & B");
                Assert.Fail("A Parser Exception should have been thrown.");
            }
            catch (ParserException pex)
            {
                Assert.AreEqual(1, pex.getProblematicTokens().Size());
                Assert.IsTrue(pex.getProblematicTokens().Get(0).getType() == LogicTokenTypes.CONNECTIVE);
                Assert.AreEqual(4, pex.getProblematicTokens().Get(0).getStartCharPositionInInput());
            }

            try
            {
                sentence = parser.parse("A & (B & C &)");
                Assert.Fail("A Parser Exception should have been thrown.");
            }
            catch (ParserException pex)
            {
                Assert.AreEqual(1, pex.getProblematicTokens().Size());
                Assert.IsTrue(pex.getProblematicTokens().Get(0).getType() == LogicTokenTypes.CONNECTIVE);
                Assert.AreEqual(11, pex.getProblematicTokens().Get(0).getStartCharPositionInInput());
            }
        }

        [TestMethod]
        public void testIssue72()
        {
            // filter1 AND filter2 AND filter3 AND filter4
            sentence = parser.parse("filter1 & filter2 & filter3 & filter4");
            expected = prettyPrintF("filter1 & filter2 & filter3 & filter4");
            Assert.AreEqual(expected, sentence.ToString());

            // (filter1 AND filter2) AND (filter3 AND filter4)
            sentence = parser.parse("(filter1 & filter2) & (filter3 & filter4)");
            expected = prettyPrintF("filter1 & filter2 & filter3 & filter4");
            Assert.AreEqual(expected, sentence.ToString());

            // ((filter1 AND filter2) AND (filter3 AND filter4))
            sentence = parser.parse("((filter1 & filter2) & (filter3 & filter4))");
            expected = prettyPrintF("filter1 & filter2 & filter3 & filter4");
            Assert.AreEqual(expected, sentence.ToString());
        }

        private string prettyPrintF(string prettyPrintedFormula)
        {
            Sentence s = parser.parse(prettyPrintedFormula);

            Assert.AreEqual(prettyPrintedFormula, "" + s);

            return prettyPrintedFormula;
        }
    }

}
