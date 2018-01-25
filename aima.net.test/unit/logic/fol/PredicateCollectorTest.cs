using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections.api;
using aima.net.logic.fol;
using aima.net.logic.fol.domain;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.test.unit.logic.fol
{
    [TestClass]
    public class PredicateCollectorTest
    {
        PredicateCollector collector;

        FOLParser parser;

        [TestInitialize]
        public void setUp()
        {
            collector = new PredicateCollector();
            parser = new FOLParser(DomainFactory.weaponsDomain());
        }

        [TestMethod]
        public void testSimpleSentence()
        {
            Sentence s = parser.parse("(Missile(x) => Weapon(x))");
            ICollection<Predicate> predicates = collector.getPredicates(s);
            Assert.IsNotNull(predicates);
        }
    }

}
