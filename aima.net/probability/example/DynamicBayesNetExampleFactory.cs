using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability.api;
using aima.net.probability.bayes;
using aima.net.probability.bayes.api;

namespace aima.net.probability.example
{
    public class DynamicBayesNetExampleFactory
    {
        /**
         * Return a Dynamic Bayesian Network of the Umbrella World Network.
         * 
         * @return a Dynamic Bayesian Network of the Umbrella World Network.
         */
        public static IDynamicBayesianNetwork getUmbrellaWorldNetwork()
        {
            IFiniteNode prior_rain_tm1 = new FullCPTNode(ExampleRV.RAIN_tm1_RV, new double[] { 0.5, 0.5 });

            BayesNet priorNetwork = new BayesNet(prior_rain_tm1);

            // Prior belief state
            IFiniteNode rain_tm1 = new FullCPTNode(ExampleRV.RAIN_tm1_RV, new double[] { 0.5, 0.5 });
            // Transition Model
            IFiniteNode rain_t = new FullCPTNode(ExampleRV.RAIN_t_RV, new double[] {
				// R_t-1 = true, R_t = true
				0.7,
				// R_t-1 = true, R_t = false
				0.3,
				// R_t-1 = false, R_t = true
				0.3,
				// R_t-1 = false, R_t = false
				0.7 }, rain_tm1);
            // Sensor Model 

            IFiniteNode umbrealla_t = new FullCPTNode(ExampleRV.UMBREALLA_t_RV,
                    new double[] {
						// R_t = true, U_t = true
						0.9,
						// R_t = true, U_t = false
						0.1,
						// R_t = false, U_t = true
						0.2,
						// R_t = false, U_t = false
						0.8 }, rain_t);

            IMap<IRandomVariable, IRandomVariable> X_0_to_X_1 = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, IRandomVariable>();
            X_0_to_X_1.Put(ExampleRV.RAIN_tm1_RV, ExampleRV.RAIN_t_RV);
            ISet<IRandomVariable> E_1 = CollectionFactory.CreateSet<IRandomVariable>();
            E_1.Add(ExampleRV.UMBREALLA_t_RV);

            return new DynamicBayesNet(priorNetwork, X_0_to_X_1, E_1, rain_tm1);
        }
    } 
}
