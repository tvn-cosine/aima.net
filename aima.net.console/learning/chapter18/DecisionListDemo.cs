using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.inductive;
using aima.net.learning.learners;
using aima.net.util;


namespace aima.net.demo.learning.chapter18
{
    public class DecisionListDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("DecisionList Demo - Inducing a DecisionList from the Restaurant DataSet\n ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            decisionListDemo();
        }

        static void decisionListDemo()
        {
            try
            {
                DataSet ds = DataSetFactory.getRestaurantDataSet();
                DecisionListLearner learner = new DecisionListLearner("Yes", "No",
                        new DecisionListTestFactory());
                learner.Train(ds);
                System.Console.WriteLine("The Induced DecisionList is");
                System.Console.WriteLine(learner.getDecisionList());
                int[] result = learner.Test(ds);

                System.Console.WriteLine("\nThis Decision List classifies the data set with "
                            + result[0]
                            + " successes"
                            + " and "
                            + result[1]
                            + " failures");
                System.Console.WriteLine("\n");

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Decision ListDemo Failed");
                throw e;
            }
        }
    }
}
