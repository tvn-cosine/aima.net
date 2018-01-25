using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.util
{ 
    public class Util
    {
        public const string NO = "No";
        public const string YES = "Yes";
        private static IRandom random = CommonFactory.CreateRandom();

        private const double EPSILON = 0.000000000001;

        /**
         * Get the first element from a list.
         * 
         * @param l
         *            the list the first element is to be extracted from.
         * @return the first element of the passed in list.
         */
        public static T first<T>(ICollection<T> l)
        {
            if (null == l
             || l.Size() < 1)
            {
                return default(T);
            }
            else
            {
                return l.Get(0);
            }
        }

        /**
         * Get a sublist of all of the elements in the list except for first.
         * 
         * @param l
         *            the list the rest of the elements are to be extracted from.
         * @return a list of all of the elements in the passed in list except for
         *         the first element.
         */
        public static ICollection<T> rest<T>(ICollection<T> l)
        {
            if (null == l
             || l.Size() < 2)
            {
                return CollectionFactory.CreateQueue<T>();
            }
            else
            {
                ICollection<T> obj = CollectionFactory.CreateQueue<T>();
                for (int i = 1; i < l.Size(); ++i)
                {
                    obj.Add(l.Get(i));
                }
                return obj;
            }
        }

        /**
         * Create a IMap<K, V> with the passed in keys having their values
         * initialized to the passed in value.
         * 
         * @param keys
         *            the keys for the newly constructed map.
         * @param value
         *            the value to be associated with each of the maps keys.
         * @return a map with the passed in keys initialized to value.
         */
        public static IMap<K, V> create<K, V>(ICollection<K> keys, V value)
        {
            IMap<K, V> map = CollectionFactory.CreateInsertionOrderedMap<K, V>();

            foreach (K k in keys)
            {
                map.Put(k, value);
            }

            return map;
        }

        /**
         * Create a set for the provided values.
         * @param values
         *        the sets initial values.
         * @return a Set of the provided values.
         */
        public static ISet<V> createSet<V>(params V[] values)
        {
            ISet<V> set = CollectionFactory.CreateSet<V>();
            foreach (V value in values)
            {
                set.Add(value);
            }
            return set;
        }

        /**
         * Randomly select an element from a list.
         * 
         * @param <T>
         *            the type of element to be returned from the list l.
         * @param l
         *            a list of type T from which an element is to be selected
         *            randomly.
         * @return a randomly selected element from l.
         */
        public static T selectRandomlyFromList<T>(ICollection<T> l)
        {
            return l.Get(random.Next(l.Size()));
        }

        public static T selectRandomlyFromSet<T>(ISet<T> set)
        {
            int i = random.Next(set.Size());
            foreach (T item in set)
            {
                --i;
                if (i < 1)
                {
                    return item;
                }
            }

            return default(T);
        }

        public static bool randomBoolean()
        {
            return random.NextBoolean();
        }

        public static double[] normalize(double[] probDist)
        {
            int len = probDist.Length;
            double total = 0.0;
            foreach (double d in probDist)
            {
                total = total + d;
            }

            double[] normalized = new double[len];
            if (total != 0)
            {
                for (int i = 0; i < len;++i)
                {
                    normalized[i] = probDist[i] / total;
                }
            }

            return normalized;
        }

        public static ICollection<double> normalize(ICollection<double> values)
        {
            double[] valuesAsArray = new double[values.Size()];
            for (int i = 0; i < valuesAsArray.Length;++i)
                valuesAsArray[i] = values.Get(i);
            double[] normalized = normalize(valuesAsArray);
            ICollection<double> results = CollectionFactory.CreateQueue<double>();
            foreach (double aNormalized in normalized)
                results.Add(aNormalized);
            return results;
        }

        public static int min(int i, int j)
        {
            return (i > j ? j : i);
        }

        public static int max(int i, int j)
        {
            return (i < j ? j : i);
        }

        public static int max(int i, int j, int k)
        {
            return max(max(i, j), k);
        }

        public static int min(int i, int j, int k)
        {
            return min(min(i, j), k);
        }

        public static T mode<T>(ICollection<T> l)
        {
            IMap<T, int> hash = CollectionFactory.CreateInsertionOrderedMap<T, int>();
            foreach (T obj in l)
            {
                if (hash.ContainsKey(obj))
                {
                    hash.Put(obj, hash.Get(obj) + 1);
                }
                else
                {
                    hash.Put(obj, 1);
                }
            }

            T maxkey = hash.GetKeys().Get(0);
            foreach (T key in hash.GetKeys())
            {
                if (hash.Get(key) > hash.Get(maxkey))
                {
                    maxkey = key;
                }
            }
            return maxkey;
        }

        public static string[] YesNo()
        {
            return new string[] { YES, NO };
        }

        public static double log2(double d)
        {
            return System.Math.Log(d) / System.Math.Log(2);
        }

        public static double information(double[] probabilities)
        {
            double total = 0.0;
            foreach (double d in probabilities)
            {
                total += (-1.0 * log2(d) * d);
            }
            return total;
        }

        public static ICollection<T> removeFrom<T>(ICollection<T> list, T member)
        {
            ICollection<T> newList = CollectionFactory.CreateQueue<T>(list);
            newList.Remove(member);
            return newList;
        }

        public static double sumOfSquares<T>(ICollection<T> list) where T : INumber
        {
            double accum = 0;
            foreach (T item in list)
            {
                accum = accum + (item.DoubleValue() * item.DoubleValue());
            }
            return accum;
        }

        public static double sumOfSquares(params double[] list)
        {
            double accum = 0;
            foreach (double item in list)
            {
                accum = accum + (item * item);
            }
            return accum;
        }

        public static string ntimes(string s, int n)
        {
            IStringBuilder builder = TextFactory.CreateStringBuilder();
            for (int i = 0; i < n;++i)
            {
                builder.Append(s);
            }
            return builder.ToString();
        }

        public static void checkForNanOrInfinity(double d)
        {
            if (double.IsNaN(d))
            {
                throw new RuntimeException("Not a Number");
            }
            if (double.IsInfinity(d))
            {
                throw new RuntimeException("Infinite Number");
            }
        }

        public static int randomNumberBetween(int i, int j)
        {
            /* i,j bothinclusive */
            return random.Next(j - i + 1) + i;
        }

        public static double calculateMean(ICollection<double> lst)
        {
            double sum = 0.0;
            foreach (double d in lst)
            {
                sum = sum + d;
            }
            return sum / lst.Size();
        }

        public static double calculateStDev(ICollection<double> values, double mean)
        {
            int listSize = values.Size();

            double sumOfDiffSquared = 0.0;
            foreach (double value in values)
            {
                double diffFromMean = value - mean;
                sumOfDiffSquared += ((diffFromMean * diffFromMean) / (listSize - 1));
                // division moved here to avoid sum becoming too big if this
                // doesn't work use incremental formulation

            }
            double variance = sumOfDiffSquared;
            // (listSize - 1);
            // assumes at least 2 members in list.
            return System.Math.Sqrt(variance);
        }

        public static ICollection<double> normalizeFromMeanAndStdev(ICollection<double> values, double mean, double stdev)
        {
            ICollection<double> obj = CollectionFactory.CreateQueue<double>();
            foreach (double d in values)
            {
                if (d == mean)
                {
                    mean = d - EPSILON;
                }
                if (0 == stdev)
                {
                    stdev = EPSILON;
                }
                obj.Add((d - mean) / stdev);
            }

            return obj;
            // return values.stream().map(d-> (d - mean) / stdev).collect(Collectors.toList());
        }

        /**
         * Generates a random double between two limits. Both limits are inclusive.
         * @param lowerLimit the lower limit.
         * @param upperLimit the upper limit.
         * @return a random double bigger or equals {@code lowerLimit} and smaller or equals {@code upperLimit}.
         */
        public static double generateRandomDoubleBetween(double lowerLimit, double upperLimit)
        {
            return lowerLimit + ((upperLimit - lowerLimit) * random.NextDouble());
        }

        /**
         * Generates a random float between two limits. Both limits are inclusive.
         * @param lowerLimit the lower limit.
         * @param upperLimit the upper limit.
         * @return a random float bigger or equals {@code lowerLimit} and smaller or equals {@code upperLimit}.
         */
        public static float generateRandomFloatBetween(float lowerLimit, float upperLimit)
        {
            return lowerLimit + ((upperLimit - lowerLimit) * (float)random.NextDouble());
        }

        /**
         * Compares two doubles for equality.
         * @param a the first double.
         * @param b the second double.
         * @return true if both doubles contain the same value or the absolute deviation between them is below {@code EPSILON}.
         */
        public static bool compareDoubles(double a, double b)
        {
            if (double.IsNaN(a) && double.IsNaN(b)) return true;
            if (!double.IsInfinity(a) && !double.IsInfinity(b)) return System.Math.Abs(a - b) <= EPSILON;
            return a == b;
        }

        /**
         * Compares two floats for equality.
         * @param a the first floats.
         * @param b the second floats.
         * @return true if both floats contain the same value or the absolute deviation between them is below {@code EPSILON}.
         */
        public static bool compareFloats(float a, float b)
        {
            if (float.IsNaN(a) && float.IsNaN(b)) return true;
            if (!float.IsInfinity(a) && !float.IsInfinity(b)) return System.Math.Abs(a - b) <= EPSILON;
            return a == b;
        }
    }
}
