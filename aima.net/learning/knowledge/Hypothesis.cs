using aima.net.logic.fol.parsing.ast;

namespace aima.net.learning.knowledge
{ 
    public class Hypothesis
    {
        private Sentence hypothesis = null;

        public Hypothesis(Sentence hypothesis)
        {
            this.hypothesis = hypothesis;
        }
         
        public Sentence getHypothesis()
        {
            return hypothesis;
        }
    }
}
