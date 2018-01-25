using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.propositional.parsing;
using aima.net.logic.propositional.parsing.ast;
using aima.net.logic.propositional.visitors;

namespace aima.net.test.unit.logic.propositional.visitors
{
    [TestClass] public class ConvertToCNFTest
    {
        private PLParser parser = new PLParser();

        [TestInitialize]
        public void setUp()
        {
        }

        [TestMethod]
        public void testSymbolTransform()
        {
            Sentence symbol = parser.parse("A");
            Sentence transformed = ConvertToCNF.convert(symbol);
            Assert.AreEqual("A", transformed.ToString());
        }

        [TestMethod]
        public void testBasicSentenceTransformation()
        {
            Sentence and = parser.parse("A & B");
            Sentence transformedAnd = ConvertToCNF.convert(and);
            Assert.AreEqual("A & B", transformedAnd.ToString());

            Sentence or = parser.parse("A | B");
            Sentence transformedOr = ConvertToCNF.convert(or);
            Assert.AreEqual("A | B", transformedOr.ToString());

            Sentence not = parser.parse("~C");
            Sentence transformedNot = ConvertToCNF.convert(not);
            Assert.AreEqual("~C", transformedNot.ToString());
        }

        [TestMethod]
        public void testImplicationTransformation()
        {
            Sentence impl = parser.parse("A => B");
            Sentence transformedImpl = ConvertToCNF.convert(impl);
            Assert.AreEqual("~A | B", transformedImpl.ToString());
        }

        [TestMethod]
        public void testBiConditionalTransformation()
        {
            Sentence bic = parser.parse("A <=> B");
            Sentence transformedBic = ConvertToCNF.convert(bic);
            Assert.AreEqual("(~A | B) & (~B | A)", transformedBic.ToString());
        }

        [TestMethod]
        public void testTwoSuccessiveNotsTransformation()
        {
            Sentence twoNots = parser.parse("~~A");
            Sentence transformed = ConvertToCNF.convert(twoNots);
            Assert.AreEqual("A", transformed.ToString());
        }

        [TestMethod]
        public void testThreeSuccessiveNotsTransformation()
        {
            Sentence threeNots = parser.parse("~~~A");
            Sentence transformed = ConvertToCNF.convert(threeNots);
            Assert.AreEqual("~A", transformed.ToString());
        }

        [TestMethod]
        public void testFourSuccessiveNotsTransformation()
        {
            Sentence fourNots = parser.parse("~~~~A");
            Sentence transformed = ConvertToCNF.convert(fourNots);
            Assert.AreEqual("A", transformed.ToString());
        }

        [TestMethod]
        public void testDeMorgan1()
        {
            Sentence dm = parser.parse("~(A & B)");
            Sentence transformed = ConvertToCNF.convert(dm);
            Assert.AreEqual("~A | ~B", transformed.ToString());
        }

        [TestMethod]
        public void testDeMorgan2()
        {
            Sentence dm = parser.parse("~(A | B)");
            Sentence transformed = ConvertToCNF.convert(dm);
            Assert.AreEqual("~A & ~B", transformed.ToString());
        }

        [TestMethod]
        public void testOrDistribution1()
        {
            Sentence or = parser.parse("A & B | C)");
            Sentence transformed = ConvertToCNF.convert(or);
            Assert.AreEqual("(A | C) & (B | C)", transformed.ToString());
        }

        [TestMethod]
        public void testOrDistribution2()
        {
            Sentence or = parser.parse("A | B & C");
            Sentence transformed = ConvertToCNF.convert(or);
            Assert.AreEqual("(A | B) & (A | C)", transformed.ToString());
        }

        [TestMethod]
        public void testAimaExample()
        {
            Sentence aimaEg = parser.parse("B11 <=> P12 | P21");
            Sentence transformed = ConvertToCNF.convert(aimaEg);
            Assert.AreEqual("(~B11 | P12 | P21) & (~P12 | B11) & (~P21 | B11)", transformed.ToString());
        }

        [TestMethod]
        public void testNested()
        {
            Sentence nested = parser.parse("A | (B | (C | (D & E)))");
            Sentence transformed = ConvertToCNF.convert(nested);
            Assert.AreEqual("(A | B | C | D) & (A | B | C | E)", transformed.ToString());

            nested = parser.parse("A | (B | (C & (D & E)))");
            transformed = ConvertToCNF.convert(nested);
            Assert.AreEqual("(A | B | C) & (A | B | D) & (A | B | E)", transformed.ToString());

            nested = parser.parse("A | (B | (C & (D & (E | F))))");
            transformed = ConvertToCNF.convert(nested);
            Assert.AreEqual("(A | B | C) & (A | B | D) & (A | B | E | F)", transformed.ToString());

            nested = parser.parse("(A | (B | (C & D))) | E | (F | (G | (H & I)))");
            transformed = ConvertToCNF.convert(nested);
            Assert.AreEqual("(A | B | C | E | F | G | H) & (A | B | D | E | F | G | H) & (A | B | C | E | F | G | I) & (A | B | D | E | F | G | I)", transformed.ToString());

            nested = parser.parse("(((~P | ~Q) => ~(P | Q)) => R)");
            transformed = ConvertToCNF.convert(nested);
            Assert.AreEqual("(~P | ~Q | R) & (P | Q | R)", transformed.ToString());

            nested = parser.parse("~(((~P | ~Q) => ~(P | Q)) => R)");
            transformed = ConvertToCNF.convert(nested);
            Assert.AreEqual("(P | ~P) & (Q | ~P) & (P | ~Q) & (Q | ~Q) & ~R", transformed.ToString());
        }

        [TestMethod]
        public void testIssue78()
        {
            // (  ( NOT J1007 )  OR  ( NOT ( OR J1008 J1009 J1010 J1011 J1012 J1013 J1014 J1015  )  )  )
            Sentence issue78Eg = parser.parse("(  ( ~ J1007 )  |  ( ~ ( J1008 | J1009 | J1010 | J1011 | J1012 | J1013 | J1014 | J1015  )  ) )");
            Sentence transformed = ConvertToCNF.convert(issue78Eg);
            Assert.AreEqual("(~J1007 | ~J1008) & (~J1007 | ~J1009) & (~J1007 | ~J1010) & (~J1007 | ~J1011) & (~J1007 | ~J1012) & (~J1007 | ~J1013) & (~J1007 | ~J1014) & (~J1007 | ~J1015)", transformed.ToString());
        }

        [TestMethod]
        public void testDistributingOrCorrectly()
        {
            Sentence s = parser.parse("A & B & C & D & (E | (F & G)) & H & I & J & K");
            Sentence transformed = ConvertToCNF.convert(s);
            Assert.AreEqual("A & B & C & D & (E | F) & (E | G) & H & I & J & K", transformed.ToString());
        }
    }

}
