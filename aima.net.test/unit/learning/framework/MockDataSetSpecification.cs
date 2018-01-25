using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.learning.framework;

namespace aima.net.test.unit.learning.framework
{
    [TestClass]
    public class MockDataSetSpecification : DataSetSpecification
    { 
        public MockDataSetSpecification(string targetAttributeName)
        {
            setTarget(targetAttributeName);
        }

        public override ICollection<string> getAttributeNames()
        {
            return CollectionFactory.CreateQueue<string>();
        }
    } 
}
