namespace aima.net.learning.framework.api
{
    public interface ILearner
    {
        void Train(DataSet ds);

        /// <summary>
        /// Returns the outcome predicted for the specified example
        /// </summary>
        /// <param name="e">an example</param>
        /// <returns>the outcome predicted for the specified example</returns>
        string Predict(Example e);

        /// <summary>
        /// Returns the accuracy of the hypothesis on the specified set of examples
        /// </summary>
        /// <param name="ds">the test data set.</param>
        /// <returns>the accuracy of the hypothesis on the specified set of examples</returns>
        int[] Test(DataSet ds);
    }
}
