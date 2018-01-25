using aima.net.svm;

namespace aima.net.demo.svm
{
    public class SVMExample
    {
        public static void Main(params string[] args)
        {
            Parameter param = new Parameter();
            // default values
            param.svm_type = Parameter.C_SVC;
            param.kernel_type = Parameter.RBF;
            param.degree = 3;
            param.gamma = 0;    // 1/num_features
            param.coef0 = 0;
            param.nu = 0.5;
            param.cache_size = 100;
            param.C = 1;
            param.eps = 1e-3;
            param.p = 0.1;
            param.shrinking = 1;
            param.probability = 1;
            param.nr_weight = 0;
            param.weight_label = new int[0];
            param.weight = new double[0];
            Problem problem = SVMFactory.LoadSVMFile("heart_scale.data", param);
            Model model = SupportVectorMachine.Train(problem, param);

            int errors = 0;
            for (int i = 0; i < problem.y.Length; ++i)
            {
                double[] prob = new double[3];
                var newPred = SupportVectorMachine.PredictProbability(model, problem.x[i], prob);
                var prediction = SupportVectorMachine.Predict(model, problem.x[i]);
                if (prediction != problem.y[i])
                {
                    ++errors;

                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                }
                else
                {
                    System.Console.ForegroundColor = System.ConsoleColor.White;
                }
                System.Console.WriteLine("Expected: {0} || Returned: {1}", problem.y[i], prediction);
                System.Console.WriteLine("1: {0}% || 2: {1}\n", prob[0] * 100D, prob[1] * 100D);
            }

            double perc = ((double)errors / problem.y.Length) * 100D;
            System.Console.WriteLine("Errors {0} at {1}%", errors, perc);
            System.Console.ReadLine();
        }
    }
}
