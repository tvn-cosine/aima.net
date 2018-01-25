using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.neural.api;

namespace aima.net.learning.neural.examples
{
    public class AnimalDataSetNumerizer : INumerizer
    {
        public Pair<ICollection<double>, ICollection<double>> Numerize(Example e)
        {
            ICollection<double> input = CollectionFactory.CreateQueue<double>();
            ICollection<double> desiredOutput = CollectionFactory.CreateQueue<double>();


            for (int i = 1; i <= 20; ++i)
            {
                input.Add(e.getAttributeValueAsDouble(string.Format("feature_{0}", i)));
            } 

            string animal_name_string = e.getAttributeValueAsString("animal_name");

            desiredOutput = convertCategoryToListOfDoubles(animal_name_string);

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
                return "GIRAFFE";
            }
            else if (rounded.Equals(CollectionFactory.CreateQueue<double>(new[] { 0.0, 1.0, 0.0 })))
            {
                return "HIPPO";
            }
            else if (rounded.Equals(CollectionFactory.CreateQueue<double>(new[] { 1.0, 0.0, 0.0 })))
            {
                return "LION";
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
            if (plant_category_string.Equals("GIRAFFE"))
            {
                return CollectionFactory.CreateQueue<double>(new[] { 0.0, 0.0, 1.0 });
            }
            else if (plant_category_string.Equals("HIPPO"))
            {
                return CollectionFactory.CreateQueue<double>(new[] { 0.0, 1.0, 0.0 });
            }
            else if (plant_category_string.Equals("LION"))
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
