using System;
using aima.net.api;

namespace aima.net
{
    public class Random : System.Random, IRandom
    {
        public bool NextBoolean()
        {
            return this.Next(2) == 1;
        }

        public double NextGaussian(double mu, double sigma)
        {
            double u1 = this.NextDouble();
            double u2 = this.NextDouble();

            double rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                     Math.Sin(2.0 * Math.PI * u2);

            double rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        public double NextTraingular(double minimum, double maximum, double mode)
        {
            double u = this.NextDouble();

            return u < (mode - minimum) / (maximum - minimum)
                       ? minimum + Math.Sqrt(u * (maximum - minimum) * (mode - minimum))
                       : maximum - Math.Sqrt((1 - u) * (maximum - minimum) * (maximum - mode));
        }
    }
}
