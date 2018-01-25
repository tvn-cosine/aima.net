using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.logic.fol.inference;

namespace aima.net.test.unit.logic.fol.inference
{
    [TestClass]
    public class FOLBCAskTest : CommonFOLInferenceProcedureTests
    {

        [TestMethod] 
        public void testDefiniteClauseKBKingsQueryCriminalXFalse()
        {
            testDefiniteClauseKBKingsQueryCriminalXFalse(new FOLBCAsk());
        }

        [TestMethod] 
        public void testDefiniteClauseKBKingsQueryRichardEvilFalse()
        {
            testDefiniteClauseKBKingsQueryRichardEvilFalse(new FOLBCAsk());
        }

        [TestMethod] 
        public void testDefiniteClauseKBKingsQueryJohnEvilSucceeds()
        {
            testDefiniteClauseKBKingsQueryJohnEvilSucceeds(new FOLBCAsk());
        }

        [TestMethod] 
        public void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds()
        {
            testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(new FOLBCAsk());
        }

        [TestMethod] 
        public void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds()
        {
            testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(new FOLBCAsk());
        }

        [TestMethod] 
        public void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds()
        {
            testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(new FOLBCAsk());
        }
    }
}
