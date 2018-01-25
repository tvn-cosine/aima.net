namespace aima.net.nlp.parsing
{
    public class LexWord
    {
        string word;
        float prob;

        public LexWord(string word, float prob)
        {
            this.word = word;
            this.prob = prob;
        }

        public string getWord() { return word; }
        public float getProb() { return prob; }
    }
}
