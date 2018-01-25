using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.learning.framework;

namespace aima.net.learning.inductive
{
    public class DecisionListTestFactory
    {
        public virtual ICollection<DecisionListTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            if (i != 1)
            {
                throw new RuntimeException("For now DLTests with only 1 attribute can be craeted , not" + i);
            }
            ICollection<string> nonTargetAttributes = ds.getNonTargetAttributes();
            ICollection<DecisionListTest> tests = CollectionFactory.CreateQueue<DecisionListTest>();
            foreach (string ntAttribute in nonTargetAttributes)
            {
                ICollection<string> ntaValues = ds.getPossibleAttributeValues(ntAttribute);
                foreach (string ntaValue in ntaValues)
                {
                    DecisionListTest test = new DecisionListTest();
                    test.add(ntAttribute, ntaValue);
                    tests.Add(test);
                }
            }
            return tests;
        }
    }
}
