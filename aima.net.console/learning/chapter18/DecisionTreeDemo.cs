using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.learners;
using aima.net.util;

namespace aima.net.demo.learning.chapter18
{
    class DecisionTreeDemo 
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\nDecisionTree Demo - Inducing a DecisionList from the Restaurant DataSet\n ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            decisionTreeDemo();
        }

        public static void decisionTreeDemo()
        {
            try
            {
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                DecisionTreeLearner learner = new DecisionTreeLearner();
                learner.Train(ds);
                System.Console.WriteLine("The Induced Decision Tree is ");
                System.Console.WriteLine(learner.getDecisionTree());
                int[] result = learner.Test(ds);

                System.Console.WriteLine("\nThis Decision Tree classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
                System.Console.WriteLine("\n");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Decision Tree Demo Failed  ");
                throw e;
            }
        }
    }
}
