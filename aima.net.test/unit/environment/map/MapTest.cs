using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.map;

namespace aima.net.test.unit.environment.map
{
    [TestClass]
    public class MapTest
    {
        ExtendableMap aMap;

        [TestInitialize]
        public void setUp()
        {
            aMap = new ExtendableMap();
            aMap.addBidirectionalLink("A", "B", 5.0);
            aMap.addBidirectionalLink("A", "C", 6.0);
            aMap.addBidirectionalLink("B", "C", 4.0);
            aMap.addBidirectionalLink("C", "D", 7.0);
            aMap.addUnidirectionalLink("B", "E", 14.0);
        }

        [TestMethod]
        public void testLocationsLinkedTo()
        {
            ICollection<string> locations = CollectionFactory.CreateQueue<string>();
            ICollection<string> linkedTo;

            linkedTo = aMap.getPossibleNextLocations("A");
            locations.Clear();
            locations.Add("B");
            locations.Add("C");
            Assert.IsTrue(locations.ContainsAll(linkedTo) && linkedTo.Size() == 2);

            linkedTo = aMap.getPossibleNextLocations("B");
            locations.Clear();
            locations.Add("A");
            locations.Add("C");
            locations.Add("E");
            Assert.IsTrue(locations.ContainsAll(linkedTo) && linkedTo.Size() == 3);

            linkedTo = aMap.getPossibleNextLocations("C");
            locations.Clear();
            locations.Add("A");
            locations.Add("B");
            locations.Add("D");
            Assert.IsTrue(locations.ContainsAll(linkedTo) && linkedTo.Size() == 3);

            linkedTo = aMap.getPossibleNextLocations("D");
            locations.Clear();
            locations.Add("C");
            Assert.IsTrue(locations.ContainsAll(linkedTo) && linkedTo.Size() == 1);

            linkedTo = aMap.getPossibleNextLocations("E");
            Assert.IsTrue(linkedTo.Size() == 0);
        }

        [TestMethod]
        public void testDistances()
        {
            Assert.AreEqual(5D, aMap.getDistance("A", "B"));
            Assert.AreEqual(6D, aMap.getDistance("A", "C"));
            Assert.AreEqual(4D, aMap.getDistance("B", "C"));
            Assert.AreEqual(7D, aMap.getDistance("C", "D"));
            Assert.AreEqual(14D, aMap.getDistance("B", "E"));
            //
            Assert.AreEqual(5D, aMap.getDistance("B", "A"));
            Assert.AreEqual(6D, aMap.getDistance("C", "A"));
            Assert.AreEqual(4D, aMap.getDistance("C", "B"));
            Assert.AreEqual(7D, aMap.getDistance("D", "C"));

            // No distances should be returned if links not established or locations
            // do not exist
            Assert.IsNull(aMap.getDistance("X", "Z"));
            Assert.IsNull(aMap.getDistance("A", "Z"));
            Assert.IsNull(aMap.getDistance("A", "E"));
            // B->E is unidirectional so should not have opposite direction
            Assert.IsNull(aMap.getDistance("E", "B"));
        }

        [TestMethod]
        public void testRandomGeneration()
        {
            ICollection<string> locations = CollectionFactory.CreateQueue<string>();
            locations.Add("A");
            locations.Add("B");
            locations.Add("C");
            locations.Add("D");
            locations.Add("E");

            for (int i = 0; i < locations.Size();++i)
            {
                Assert.IsTrue(locations.Contains(aMap.randomlyGenerateDestination()));
            }
        }
    } 
}
