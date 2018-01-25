namespace aima.net.svm
{
    internal abstract class Kernel : QMatrix
    {
        private Node[][] x;
        private readonly double[] x_square;

        // svm_parameter
        private readonly int kernel_type;
        private readonly int degree;
        private readonly double gamma;
        private readonly double coef0;

        public override void swap_index(int i, int j)
        {
            do { Node[] _ = x[i]; x[i] = x[j]; x[j] = _; } while (false);
            if (x_square != null) do { double _ = x_square[i]; x_square[i] = x_square[j]; x_square[j] = _; } while (false);
        }

        private static double powi(double _base, int times)
        {
            double tmp = _base, ret = 1.0;

            for (int t = times; t > 0; t /= 2)
            {
                if (t % 2 == 1) ret *= tmp;
                tmp = tmp * tmp;
            }
            return ret;
        }

        public double kernel_function(int i, int j)
        {
            switch (kernel_type)
            {
                case Parameter.LINEAR:
                    return dot(x[i], x[j]);
                case Parameter.POLY:
                    return powi(gamma * dot(x[i], x[j]) + coef0, degree);
                case Parameter.RBF:
                    return System.Math.Exp(-gamma * (x_square[i] + x_square[j] - 2 * dot(x[i], x[j])));
                case Parameter.SIGMOID:
                    return System.Math.Tanh(gamma * dot(x[i], x[j]) + coef0);
                case Parameter.PRECOMPUTED:
                    return x[i][(int)(x[j][0].value)].value;
                default:
                    return 0;   // java
            }
        }
         
        public Kernel(int l, Node[][] x_, Parameter param)
        {
            this.kernel_type = param.kernel_type;
            this.degree = param.degree;
            this.gamma = param.gamma;
            this.coef0 = param.coef0;

            x = (Node[][])x_.Clone();

            if (kernel_type == Parameter.RBF)
            {
                x_square = new double[l];
                for (int i = 0; i < l; i++)
                    x_square[i] = dot(x[i], x[i]);
            }
            else x_square = null;
        }

        public static double dot(Node[] x, Node[] y)
        {
            double sum = 0;
            int xlen = x.Length;
            int ylen = y.Length;
            int i = 0;
            int j = 0;
            while (i < xlen && j < ylen)
            {
                if (x[i].index == y[j].index)
                    sum += x[i++].value * y[j++].value;
                else
                {
                    if (x[i].index > y[j].index)
                        ++j;
                    else
                        ++i;
                }
            }
            return sum;
        }

        public static double k_function(Node[] x, Node[] y,
                          Parameter param)
        {
            switch (param.kernel_type)
            {
                case Parameter.LINEAR:
                    return dot(x, y);
                case Parameter.POLY:
                    return powi(param.gamma * dot(x, y) + param.coef0, param.degree);
                case Parameter.RBF:
                    {
                        double sum = 0;
                        int xlen = x.Length;
                        int ylen = y.Length;
                        int i = 0;
                        int j = 0;
                        while (i < xlen && j < ylen)
                        {
                            if (x[i].index == y[j].index)
                            {
                                double d = x[i++].value - y[j++].value;
                                sum += d * d;
                            }
                            else if (x[i].index > y[j].index)
                            {
                                sum += y[j].value * y[j].value;
                                ++j;
                            }
                            else
                            {
                                sum += x[i].value * x[i].value;
                                ++i;
                            }
                        }

                        while (i < xlen)
                        {
                            sum += x[i].value * x[i].value;
                            ++i;
                        }

                        while (j < ylen)
                        {
                            sum += y[j].value * y[j].value;
                            ++j;
                        }

                        return System.Math.Exp(-param.gamma * sum);
                    }
                case Parameter.SIGMOID:
                    return System.Math.Tanh(param.gamma * dot(x, y) + param.coef0);
                case Parameter.PRECOMPUTED:
                    return x[(int)(y[0].value)].value;
                default:
                    return 0;   // java
            }
        }
    }
}
