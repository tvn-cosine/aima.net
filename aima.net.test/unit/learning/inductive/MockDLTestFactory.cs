using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections.api;
using aima.net.learning.framework;
using aima.net.learning.inductive;

namespace aima.net.test.unit.learning.inductive
{
    [TestClass]
    public class MockDLTestFactory : DecisionListTestFactory
    {
        private ICollection<aima.net.learning.inductive.DecisionListTest> tests;

        public MockDLTestFactory(ICollection<aima.net.learning.inductive.DecisionListTest> tests)
        {
            this.tests = tests;
        }

        public override ICollection<aima.net.learning.inductive.DecisionListTest> createDLTestsWithAttributeCount(DataSet ds, int i)
        {
            return tests;
        }
    } 
}
