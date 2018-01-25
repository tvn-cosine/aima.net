using aima.net.learning.framework;
using aima.net.learning.inductive;

namespace aima.net.learning.learners
{
    public class StumpLearner : DecisionTreeLearner
    {
        public StumpLearner(DecisionTree sl, string unable_to_classify)
                : base(sl, unable_to_classify)
        { }

        public override void Train(DataSet ds)
        {
            // System.Console.WriteLine("Stump learner training");
            // do nothing the stump is not inferred from the dataset
        }
    }
}
