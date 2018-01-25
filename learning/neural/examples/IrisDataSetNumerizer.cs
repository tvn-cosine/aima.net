using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.neural.api;

namespace aima.net.learning.neural.examples
{
    public class IrisDataSetNumerizer : INumerizer
    {
        public Pair<ICollection<double>, ICollection<double>> Numerize(Example e)
        {
            ICollection<double> input = CollectionFactory.CreateQueue<double>();
            ICollection<double> desiredOutput = CollectionFactory.CreateQueue<double>();

            double sepal_length = e.getAttributeValueAsDouble("sepal_length");
            double sepal_width = e.getAttributeValueAsDouble("sepal_width");
            double petal_length = e.getAttributeValueAsDouble("petal_length");
            double petal_width = e.getAttributeValueAsDouble("petal_width");

            input.Add(sepal_length);
            input.Add(sepal_width);
            input.Add(petal_length);
            input.Add(petal_width);

            string plant_category_string = e.getAttributeValueAsString("plant_category");

            desiredOutput = convertCategoryToListOfDoubles(plant_category_string);

            Pair<ICollection<double>, ICollection<double>> io = new Pair<ICollection<double>, ICollection<double>>(input, desiredOutput);

            return io;
        }

        public string Denumerize(ICollection<double> outputValue)
        {
            ICollection<double> rounded = CollectionFactory.CreateQueue<double>();
            foreach (double d in outputValue)
            {
                rounded.Add(round(d));
            }
            if (rounded.Equals(CollectionFactory.CreateQueue<double>(new[] { 0.0, 0.0, 1.0 })))
            {
                return "setosa";
            }
            else if (rounded.Equals(CollectionFactory.CreateQueue<double>(new[] { 0.0, 1.0, 0.0 })))
            {
                return "versicolor";
            }
            else if (rounded.Equals(CollectionFactory.CreateQueue<double>(new[] { 1.0, 0.0, 0.0 })))
            {
                return "virginica";
            }
            else
            {
                return "unknown";
            }
        }
         
        private double round(double d)
        {
            if (d < 0)
            {
                return 0.0;
            }
            if (d > 1)
            {
                return 1.0;
            }
            else
            {
                return System.Math.Round(d);
            }
        }

        private ICollection<double> convertCategoryToListOfDoubles(string plant_category_string)
        {
            if (plant_category_string.Equals("setosa"))
            {
                return CollectionFactory.CreateQueue<double>(new[] { 0.0, 0.0, 1.0 });
            }
            else if (plant_category_string.Equals("versicolor"))
            {
                return CollectionFactory.CreateQueue<double>(new[] { 0.0, 1.0, 0.0 });
            }
            else if (plant_category_string.Equals("virginica"))
            {
                return CollectionFactory.CreateQueue<double>(new[] { 1.0, 0.0, 0.0 });
            }
            else
            {
                throw new RuntimeException("invalid plant category");
            }
        }
    } 
}
