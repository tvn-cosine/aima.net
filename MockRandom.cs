using aima.net.api;
using aima.net.exceptions;

namespace aima.net
{
    public class MockRandom : IRandom
    {
        private double[] values;
        private int index;
         
        public MockRandom(double[] values)
        {
            this.values = new double[values.Length];
            System.Array.Copy(values, 0, this.values, 0, values.Length);
            this.index = 0;
        }

        public bool NextBoolean()
        {
            throw new NotImplementedException();
        }

        public int Next()
        {
            throw new NotImplementedException();
        }

        public int Next(int minimumValue, int maximumValue)
        {
            throw new NotImplementedException();
        }

        public int Next(int maximumValue)
        {
            throw new NotImplementedException();
        }

        public double NextDouble()
        {
            if (index == values.Length)
            {
                index = 0;
            }

            return values[index++];
        }

        public double NextGaussian(double mu, double sigma)
        {
            throw new System.NotImplementedException();
        }

        public double NextTraingular(double minimum, double maximum, double mode)
        {
            throw new System.NotImplementedException();
        }
    }
}
