using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.probability.bayes.exact;

namespace aima.net.test.unit.probability.bayes.exact
{
    [TestClass]
    public class EnumerationAskTest : BayesianInferenceTest
    {
        [TestInitialize]

        public override void setUp()
        {
            bayesInference = new EnumerationAsk();
        }
    }
}
