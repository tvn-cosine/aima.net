using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.csp;
using aima.net.search.csp.api;
using aima.net.search.csp.examples;

namespace aima.net.test.unit.search.csp
{
    [TestClass]
    public class TreeCspSolverTest
    {
        private static readonly Variable WA = new Variable("wa");
        private static readonly Variable NT = new Variable("nt");
        private static readonly Variable Q = new Variable("q");
        private static readonly Variable NSW = new Variable("nsw");
        private static readonly Variable V = new Variable("v");

        private static readonly IConstraint<Variable, string> C1 = new NotEqualConstraint<Variable, string>(WA, NT);
        private static readonly IConstraint<Variable, string> C2 = new NotEqualConstraint<Variable, string>(NT, Q);
        private static readonly IConstraint<Variable, string> C3 = new NotEqualConstraint<Variable, string>(Q, NSW);
        private static readonly IConstraint<Variable, string> C4 = new NotEqualConstraint<Variable, string>(NSW, V);

        private Domain<string> colors;

        private ICollection<Variable> variables;

        [TestInitialize]
        public void setUp()
        {
            variables = CollectionFactory.CreateQueue<Variable>();
            variables.Add(WA);
            variables.Add(NT);
            variables.Add(Q);
            variables.Add(NSW);
            variables.Add(V);
            colors = new Domain<string>("red", "green", "blue");
        }

        [TestMethod]
        public void testConstraintNetwork()
        {
            CSP<Variable, string> csp = new CSP<Variable, string>(variables);
            csp.addConstraint(C1);
            csp.addConstraint(C2);
            csp.addConstraint(C3);
            csp.addConstraint(C4);
            Assert.IsNotNull(csp.getConstraints());
            Assert.AreEqual(4, csp.getConstraints().Size());
            Assert.IsNotNull(csp.getConstraints(WA));
            Assert.AreEqual(1, csp.getConstraints(WA).Size());
            Assert.IsNotNull(csp.getConstraints(NT));
            Assert.AreEqual(2, csp.getConstraints(NT).Size());
            Assert.IsNotNull(csp.getConstraints(Q));
            Assert.AreEqual(2, csp.getConstraints(Q).Size());
            Assert.IsNotNull(csp.getConstraints(NSW));
            Assert.AreEqual(2, csp.getConstraints(NSW).Size());
        }

        [TestMethod]
        public void testDomainChanges()
        {
            Domain<string> colors2 = new Domain<string>(colors.asList());
            Assert.AreEqual(colors, colors2);

            CSP<Variable, string> csp = new CSP<Variable, string>(variables);
            csp.addConstraint(C1);
            Assert.IsNotNull(csp.getDomain(WA));
            Assert.AreEqual(0, csp.getDomain(WA).size());
            Assert.IsNotNull(csp.getConstraints(WA));

            csp.setDomain(WA, colors);
            Assert.AreEqual(colors, csp.getDomain(WA));
            Assert.AreEqual(3, csp.getDomain(WA).size());
            Assert.AreEqual("red", csp.getDomain(WA).get(0));

            CSP<Variable, string> cspCopy = csp.copyDomains();
            Assert.IsNotNull(cspCopy.getDomain(WA));
            Assert.AreEqual(3, cspCopy.getDomain(WA).size());
            Assert.AreEqual("red", cspCopy.getDomain(WA).get(0));
            Assert.IsNotNull(cspCopy.getDomain(NT));
            Assert.AreEqual(0, cspCopy.getDomain(NT).size());
            Assert.IsNotNull(cspCopy.getConstraints(NT));
            Assert.AreEqual(C1, cspCopy.getConstraints(NT).Get(0));

            cspCopy.removeValueFromDomain(WA, "red");
            Assert.AreEqual(2, cspCopy.getDomain(WA).size());
            Assert.AreEqual("green", cspCopy.getDomain(WA).get(0));
            Assert.AreEqual(3, csp.getDomain(WA).size());
            Assert.AreEqual("red", csp.getDomain(WA).get(0));
        }

        [TestMethod]
        public void testCSPSolver()
        {

            CSP<Variable, string> csp = new CSP<Variable, string>(variables);
            csp.addConstraint(C1);
            csp.addConstraint(C2);
            csp.addConstraint(C3);
            csp.addConstraint(C4);

            csp.setDomain(WA, colors);
            csp.setDomain(NT, colors);
            csp.setDomain(Q, colors);
            csp.setDomain(NSW, colors);
            csp.setDomain(V, colors);

            TreeCspSolver<Variable, string> treeCSPSolver = new TreeCspSolver<Variable, string>();
            Assignment<Variable, string> assignment = treeCSPSolver.solve(csp);
            Assert.IsTrue(assignment != null);
            Assert.IsTrue(assignment.isComplete(csp.getVariables()));
            Assert.IsTrue(assignment.isSolution(csp));
        }
    }

}
