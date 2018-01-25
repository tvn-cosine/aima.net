using aima.net.collections.api;
using aima.net.logic.fol.kb;

namespace aima.net.learning.knowledge
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 19.2, page 771. 
     * 
     * 
     * function CURRENT-BEST-LEARNING(examples, h) returns a hypothesis or fail
     * 
     *   if examples is empty then
     *      return h
     *   e <- FIRST(examples)
     *   if e is consistent with h then
     *      return CURRENT-BEST-LEARNING(REST(examples), h)
     *   else if e is a false positive for h then
     *     for each h' in specializations of h consistent with examples seen so far do
     *       h'' <- CURRENT-BEST-LEARNING(REST(examples), h')
     *       if h'' != fail then return h''
     *   else if e is a false negative for h then
     *     for each h' in generalization of h consistent with examples seen so far do
     *       h'' <- CURRENT-BEST-LEARNING(REST(examples), h')
     *       if h'' != fail then return h''
     *   return fail
     * 
     * Figure 19.2 The current-best-hypothesis learning algorithm. It searches for a
     * consistent hypothesis that fits all the examples and backtracks when no
     * consistent specialization/generalization can be found. To start the
     * algorithm, any hypothesis can be passed in; it will be specialized or
     * generalized as needed.  
     */
    public class CurrentBestLearning
    { 
        private FOLDataSetDomain folDSDomain = null;
        private FOLKnowledgeBase kbForLearning = null;
         
        public CurrentBestLearning(FOLDataSetDomain folDSDomain,
                FOLKnowledgeBase kbForLearning)
        {
            this.folDSDomain = folDSDomain;
            this.kbForLearning = kbForLearning;
        }

        // TODO - Implement!!!
        public Hypothesis currentBestLearning(ICollection<FOLExample> examples)
        { 
            // TODO-use the default from pg 769 for now.
            string c1 = "patrons(v,Some)";
            string c2 = "patrons(v,Full) AND (hungry(v) AND type(v,French))";
            string c3 = "patrons(v,Full) AND (hungry(v) AND (type(v,Thai) AND fri_sat(v)))";
            string c4 = "patrons(v,Full) AND (hungry(v) AND type(v,Burger))";
            string sh = "FORALL v (will_wait(v) <=> (" + c1 + " OR (" + c2
                    + " OR (" + c3 + " OR (" + c4 + ")))))";

            Hypothesis h = new Hypothesis(kbForLearning.tell(sh));

            return h;
        } 
    } 
}
