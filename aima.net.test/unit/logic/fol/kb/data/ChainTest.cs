using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.test.unit.logic.fol.kb.data
{
    [TestClass]
    public class ChainTest
    {
        [TestMethod]
        public void testIsEmpty()
        {
            Chain c = new Chain();

            Assert.IsTrue(c.isEmpty());

            c.addLiteral(new Literal(new Predicate("P", CollectionFactory.CreateQueue<Term>())));

            Assert.IsFalse(c.isEmpty());

            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();

            lits.Add(new Literal(new Predicate("P", CollectionFactory.CreateQueue<Term>())));

            c = new Chain(lits);

            Assert.IsFalse(c.isEmpty());
        }

        [TestMethod]
        public void testContrapositives()
        {
            ICollection<Chain> conts;
            Literal p = new Literal(new Predicate("P", CollectionFactory.CreateQueue<Term>()));
            Literal notq = new Literal(new Predicate("Q", CollectionFactory.CreateQueue<Term>()),
                    true);
            Literal notr = new Literal(new Predicate("R", CollectionFactory.CreateQueue<Term>()),
                    true);

            Chain c = new Chain();

            conts = c.getContrapositives();
            Assert.AreEqual(0, conts.Size());

            c.addLiteral(p);
            conts = c.getContrapositives();
            Assert.AreEqual(0, conts.Size());

            c.addLiteral(notq);
            conts = c.getContrapositives();
            Assert.AreEqual(1, conts.Size());
            Assert.AreEqual("<~Q(),P()>", conts.Get(0).ToString());

            c.addLiteral(notr);
            conts = c.getContrapositives();
            Assert.AreEqual(2, conts.Size());
            Assert.AreEqual("<~Q(),P(),~R()>", conts.Get(0).ToString());
            Assert.AreEqual("<~R(),P(),~Q()>", conts.Get(1).ToString());
        }
    }

}
