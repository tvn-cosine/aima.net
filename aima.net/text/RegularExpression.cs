using aima.net.text.api;

namespace aima.net.text
{
    public class RegularExpression : IRegularExpression
    {
        private readonly System.Text.RegularExpressions.Regex backingRegularExpression;

        public RegularExpression(string pattern)
        {
            backingRegularExpression = new System.Text.RegularExpressions.Regex(pattern);
        }

        public bool IsMatch(string input)
        {
            return backingRegularExpression.IsMatch(input);
        }

        public bool IsMatch(string input, int startat)
        {
            return backingRegularExpression.IsMatch(input, startat);
        }

        public string[] Matches(string input)
        {
            System.Text.RegularExpressions.MatchCollection matches = backingRegularExpression.Matches(input);
            string[] obj = new string[matches.Count];
            int counter = 0;

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                obj[counter] = match.Value;
                ++counter;
            }
            return obj;
        }

        public string Replace(string input, string replacement)
        {
            return backingRegularExpression.Replace(input, replacement);
        }

        public string Replace(string input, string replacement, int count)
        {
            return backingRegularExpression.Replace(input, replacement, count);
        }

        public string Replace(string input, string replacement, int count, int startat)
        {
            return backingRegularExpression.Replace(input, replacement, count, startat);
        }

        public string[] Split(string input)
        {
            return backingRegularExpression.Split(input);
        }

        public string[] Split(string input, int count)
        {
            return backingRegularExpression.Split(input, count);
        }

        public string[] Split(string input, int count, int startat)
        {
            return backingRegularExpression.Split(input, count, startat);
        }

        public override string ToString()
        {
            return backingRegularExpression.ToString();
        }
    }
}
