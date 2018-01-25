using aima.net.api;

namespace aima.net.svm
{
    public class Parameter : ICloneable<Parameter>
    {
        /* svm_type */
        public const int C_SVC = 0;
        public const int NU_SVC = 1;
        public const int ONE_CLASS = 2;
        public const int EPSILON_SVR = 3;
        public const int NU_SVR = 4;

        /* kernel_type */
        public const int LINEAR = 0;
        public const int POLY = 1;
        public const int RBF = 2;
        public const int SIGMOID = 3;
        public const int PRECOMPUTED = 4;

        public int svm_type;
        public int kernel_type;
        public int degree;  // for poly
        public double gamma;    // for poly/rbf/sigmoid
        public double coef0;    // for poly/sigmoid

        // these are for training only
        public double cache_size; // in MB
        public double eps;  // stopping criteria
        public double C;    // for C_SVC, EPSILON_SVR and NU_SVR
        public int nr_weight;       // for C_SVC
        public int[] weight_label;  // for C_SVC
        public double[] weight;     // for C_SVC
        public double nu;   // for NU_SVC, ONE_CLASS, and NU_SVR
        public double p;    // for EPSILON_SVR
        public int shrinking;   // use the shrinking heuristics
        public int probability; // do probability estimates

        public Parameter Clone()
        {
            Parameter obj = new Parameter();

            obj.svm_type = svm_type;
            obj.kernel_type = kernel_type;
            obj.degree = degree;  // for poly
            obj.gamma = gamma;    // for poly/rbf/sigmoid
            obj.coef0 = coef0;    // for poly/sigmoid

            // these are for training only
            obj.cache_size = cache_size;
            obj.eps = eps;
            obj.C = C;
            obj.nr_weight = nr_weight;
            obj.weight_label = (int[])weight_label.Clone();
            obj.weight = (double[])weight.Clone();
            obj.nu = nu;
            obj.p = p;
            obj.shrinking = shrinking;
            obj.probability = probability;

            return obj;
        }
    }
}
