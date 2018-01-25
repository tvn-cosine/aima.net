using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;

namespace aima.net.test.unit.util.datastructure
{
    [TestClass]
    public class TableTest
    {
        private Table<string, string, int?> table;

        [TestInitialize]
        public void setUp()
        {
            ICollection<string> rowHeaders = CollectionFactory.CreateQueue<string>();
            ICollection<string> columnHeaders = CollectionFactory.CreateQueue<string>();

            rowHeaders.Add("row1");
            rowHeaders.Add("ravi");
            rowHeaders.Add("peter");

            columnHeaders.Add("col1");
            columnHeaders.Add("iq");
            columnHeaders.Add("age");
            table = new Table<string, string, int?>(rowHeaders, columnHeaders);

        }

        [TestMethod]
        public void testTableInitialization()
        {
            Assert.IsNull(table.Get("ravi", "iq"));
            table.Set("ravi", "iq", 50);
            int? i = table.Get("ravi", "iq");
            Assert.AreEqual(50, i);
        }

        [TestMethod]
        public void testNullAccess()
        {
            // No value yet assigned
            Assert.IsNull(table.Get("row1", "col2"));
            table.Set("row1", "col1", 1);
            Assert.AreEqual(1, (int)table.Get("row1", "col1"));
            // Check null returned if column does not exist
            Assert.IsNull(table.Get("row1", "col2"));
            // Check null returned if row does not exist
            Assert.IsNull(table.Get("row2", "col1"));
        } 
    } 
}
