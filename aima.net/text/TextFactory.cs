using System.Globalization;
using aima.net.text.api;

namespace aima.net.text
{
    public static class TextFactory
    {
        public static IStringBuilder CreateStringBuilder()
        {
            return new StringBuilder();
        }

        public static IStringBuilder CreateStringBuilder(string value)
        {
            return new StringBuilder(value);
        }

        public static IRegularExpression CreateRegularExpression(string pattern)
        {
            return new RegularExpression(pattern);
        }

        public static bool IsValidDouble(this string input)
        {
            double o;
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out o))
            {
                return true;
            }
            return false;
        }

        public static int ParseInt(this string input)
        {
            return int.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public static float ParseFloat(this string input)
        {
            return float.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public static double ParseDouble(this string input)
        {
            return double.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
         
        public static long ParseLong(this string input)
        {
            return long.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
