using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.datastructures;

namespace aima.net.test.unit.util.datastructure
{
    [TestClass] public class XYLocationTest
    {

        [TestMethod]
        public void testXYLocationAtributeSettingOnConstruction()
        {
            XYLocation loc = new XYLocation(3, 4);
            Assert.AreEqual(3, loc.GetXCoOrdinate());
            Assert.AreEqual(4, loc.GetYCoOrdinate());
        }

        [TestMethod]
        public void testEquality()
        {
            XYLocation loc1 = new XYLocation(3, 4);
            XYLocation loc2 = new XYLocation(3, 4);
            Assert.AreEqual(loc1, loc2);
        }
    }
}
