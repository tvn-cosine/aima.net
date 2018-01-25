using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.framework.api;
using aima.net.learning.inductive;
using aima.net.learning.learners;
using aima.net.util;

namespace aima.net.demo.learning.chapter18
{
    public class EnsembleLearningDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n Ensemble Decision Demo - Weak Learners co operating to give Superior decisions ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            ensembleLearningDemo();

            System.Console.ReadKey();
        }

        static void ensembleLearningDemo()
        {
            try
            {
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                ICollection<DecisionTree> stumps = DecisionTree.getStumpsFor(ds, "Yes", "No");
                ICollection<ILearner> learners = CollectionFactory.CreateQueue<ILearner>();

                System.Console.WriteLine("\nStump Learners vote to decide in this algorithm");
                foreach (object stump in stumps)
                {
                    DecisionTree sl = (DecisionTree)stump;
                    StumpLearner stumpLearner = new StumpLearner(sl, "No");
                    learners.Add(stumpLearner);
                }
                AdaBoostLearner learner = new AdaBoostLearner(learners, ds);
                learner.Train(ds);
                var answer = learner.Predict(ds.getExample(0));
                int[] result = learner.Test(ds);
                System.Console.WriteLine("\nThis Ensemble Learner  classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
                System.Console.WriteLine("\n");

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
