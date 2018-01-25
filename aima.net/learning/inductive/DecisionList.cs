using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.learning.framework;

namespace aima.net.learning.inductive
{
    public class DecisionList : IStringable
    {
        private string positive, negative;
        private ICollection<DecisionListTest> tests;
        private IMap<DecisionListTest, string> testOutcomes;

        public DecisionList(string positive, string negative)
        {
            this.positive = positive;
            this.negative = negative;
            this.tests = CollectionFactory.CreateQueue<DecisionListTest>();
            testOutcomes = CollectionFactory.CreateInsertionOrderedMap<DecisionListTest, string>();
        }

        public string predict(Example example)
        {
            if (tests.Size() == 0)
            {
                return negative;
            }
            foreach (DecisionListTest test in tests)
            {
                if (test.matches(example))
                {
                    return testOutcomes.Get(test);
                }
            }
            return negative;
        }

        public void add(DecisionListTest test, string outcome)
        {
            tests.Add(test);
            testOutcomes.Put(test, outcome);
        }

        public DecisionList mergeWith(DecisionList dlist2)
        {
            DecisionList merged = new DecisionList(positive, negative);
            foreach (DecisionListTest test in tests)
            {
                merged.add(test, testOutcomes.Get(test));
            }
            foreach (DecisionListTest test in dlist2.tests)
            {
                merged.add(test, dlist2.testOutcomes.Get(test));
            }
            return merged;
        }

        public override string ToString()
        {
            IStringBuilder buf = TextFactory.CreateStringBuilder();
            foreach (DecisionListTest test in tests)
            {
                buf.Append(test.ToString() + " => " + testOutcomes.Get(test) + " ELSE \n");
            }
            buf.Append("END");
            return buf.ToString();
        }
    } 
}
