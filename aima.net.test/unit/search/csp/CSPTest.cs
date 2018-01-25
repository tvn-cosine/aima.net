using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.csp;
using aima.net.search.csp.api;
using aima.net.search.csp.examples;

namespace aima.net.test.unit.search.csp
{
    [TestClass] public class CSPTest
    {
        private static readonly Variable X = new Variable("x");
        private static readonly Variable Y = new Variable("y");
        private static readonly Variable Z = new Variable("z");

        private static readonly IConstraint<Variable, string> C1 = new NotEqualConstraint<Variable, string>(X, Y);
        private static readonly IConstraint<Variable, string> C2 = new NotEqualConstraint<Variable, string>(X, Y);

        private Domain<string> colors;
        private Domain<string> animals;

        private ICollection<Variable> variables;

        [TestInitialize]
        public void setUp()
        {
            variables = CollectionFactory.CreateQueue<Variable>();
            variables.Add(X);
            variables.Add(Y);
            variables.Add(Z);
            colors = new Domain<string>("red", "green", "blue");
            animals = new Domain<string>("cat", "dog");
        }

        [TestMethod]
        public void testConstraintNetwork()
        {
            CSP<Variable, string> csp = new CSP<Variable, string>(variables);
            csp.addConstraint(C1);
            csp.addConstraint(C2);
            Assert.IsNotNull(csp.getConstraints());
            Assert.AreEqual(2, csp.getConstraints().Size());
            Assert.IsNotNull(csp.getConstraints(X));
            Assert.AreEqual(2, csp.getConstraints(X).Size());
            Assert.IsNotNull(csp.getConstraints(Y));
            Assert.AreEqual(2, csp.getConstraints(Y).Size());
            Assert.IsNotNull(csp.getConstraints(Z));
            Assert.AreEqual(0, csp.getConstraints(Z).Size());
        }

        [TestMethod]
        public void testDomainChanges()
        {
            Domain<string> colors2 = new Domain<string>(colors.asList());
            Assert.AreEqual(colors, colors2);

            CSP<Variable, string> csp = new CSP<Variable, string>(variables);
            csp.addConstraint(C1);
            Assert.IsNotNull(csp.getDomain(X));
            Assert.AreEqual(0, csp.getDomain(X).size());
            Assert.IsNotNull(csp.getConstraints(X));

            csp.setDomain(X, colors);
            Assert.AreEqual(colors, csp.getDomain(X));
            Assert.AreEqual(3, csp.getDomain(X).size());
            Assert.AreEqual("red", csp.getDomain(X).get(0));

            CSP<Variable, string> cspCopy = csp.copyDomains();
            Assert.IsNotNull(cspCopy.getDomain(X));
            Assert.AreEqual(3, cspCopy.getDomain(X).size());
            Assert.AreEqual("red", cspCopy.getDomain(X).get(0));
            Assert.IsNotNull(cspCopy.getDomain(Y));
            Assert.AreEqual(0, cspCopy.getDomain(Y).size());
            Assert.IsNotNull(cspCopy.getConstraints(X));
            Assert.AreEqual(C1, cspCopy.getConstraints(X).Get(0));

            cspCopy.removeValueFromDomain(X, "red");
            Assert.AreEqual(2, cspCopy.getDomain(X).size());
            Assert.AreEqual("green", cspCopy.getDomain(X).get(0));
            Assert.AreEqual(3, csp.getDomain(X).size());
            Assert.AreEqual("red", csp.getDomain(X).get(0));

            cspCopy.setDomain(X, animals);
            Assert.AreEqual(2, cspCopy.getDomain(X).size());
            Assert.AreEqual("cat", cspCopy.getDomain(X).get(0));
            Assert.AreEqual(3, csp.getDomain(X).size());
            Assert.AreEqual("red", csp.getDomain(X).get(0));
        }
    }

}
