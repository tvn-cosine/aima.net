using aima.net.logic.propositional.parsing;
using aima.net.logic.propositional.parsing.ast;

namespace aima.net.logic.propositional.visitors
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253.<br>
     * <br>
     * Eliminate <=>, replacing &alpha; <=> &beta;<br>
     * with (&alpha; => &beta;) & (&beta; => &alpha;)
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class BiconditionalElimination : AbstractPLVisitor<object>
    {

        /**
         * Eliminate the biconditionals from a sentence.
         * 
         * @param sentence
         *            a propositional logic sentence.
         * @return an equivalent Sentence to the input with all biconditionals
         *         eliminated.
         */
        public static Sentence eliminate(Sentence sentence)
        {
            BiconditionalElimination eliminator = new BiconditionalElimination();

            Sentence result = sentence.accept(eliminator, null);

            return result;
        }


        public override Sentence visitBinarySentence(ComplexSentence s, object arg)
        {
            Sentence result = null;
            if (s.isBiconditionalSentence())
            {
                // Eliminate <=>, replace &alpha; <=> &beta;
                // with (&alpha; => &beta;) & (&beta; => &alpha;)
                Sentence alpha = s.getSimplerSentence(0).accept(this, arg);
                Sentence beta = s.getSimplerSentence(1).accept(this, arg);
                Sentence alphaImpliesBeta = new ComplexSentence(
                        Connective.IMPLICATION, alpha, beta);
                Sentence betaImpliesAlpha = new ComplexSentence(
                        Connective.IMPLICATION, beta, alpha);

                result = new ComplexSentence(Connective.AND, alphaImpliesBeta,
                        betaImpliesAlpha);
            }
            else
            {
                result = base.visitBinarySentence(s, arg);
            }
            return result;
        }
    }
}
