using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.framework.api;
using aima.net.util;

namespace aima.net.learning.learners
{ 
    public class AdaBoostLearner : ILearner
    {
        private ICollection<ILearner> learners;
        private DataSet dataSet;
        private double[] exampleWeights;
        private IMap<ILearner, double> learnerWeights;

        public AdaBoostLearner(ICollection<ILearner> learners, DataSet ds)
        {
            this.learners = learners;
            this.dataSet = ds;

            initializeExampleWeights(ds.examples.Size());
            initializeHypothesisWeights(learners.Size());
        }

        public void Train(DataSet ds)
        {
            initializeExampleWeights(ds.examples.Size());

            foreach (ILearner learner in learners)
            {
                learner.Train(ds);

                double error = calculateError(ds, learner);
                if (error < 0.0001)
                {
                    break;
                }

                adjustExampleWeights(ds, learner, error);

                double newHypothesisWeight = learnerWeights.Get(learner) * System.Math.Log((1.0 - error) / error);
                learnerWeights.Put(learner, newHypothesisWeight);
            }
        }

        public string Predict(Example e)
        {
            return weightedMajority(e);
        }

        public int[] Test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(Predict(e)))
                {
                    results[0] = results[0] + 1;
                }
                else
                {
                    results[1] = results[1] + 1;
                }
            }
            return results;
        }
         
        private string weightedMajority(Example e)
        {
            ICollection<string> targetValues = dataSet.getPossibleAttributeValues(dataSet.getTargetAttributeName());

            Table<string, ILearner, double> table = createTargetValueLearnerTable(targetValues, e);
            return getTargetValueWithTheMaximumVotes(targetValues, table);
        }

        private Table<string, ILearner, double> createTargetValueLearnerTable(ICollection<string> targetValues, Example e)
        {
            // create a table with target-attribute values as rows and learners as
            // columns and cells containing the weighted votes of each Learner for a
            // target value
            // Learner1 Learner2 Laerner3 .......
            // Yes 0.83 0.5 0
            // No 0 0 0.6

            Table<string, ILearner, double> table = new Table<string, ILearner, double>(targetValues, learners);
            // initialize table
            foreach (ILearner l in learners)
            {
                foreach (string s in targetValues)
                {
                    table.Set(s, l, 0.0);
                }
            }
            foreach (ILearner learner in learners)
            {
                string predictedValue = learner.Predict(e);
                foreach (string v in targetValues)
                {
                    if (predictedValue.Equals(v))
                    {
                        table.Set(v, learner, table.Get(v, learner)
                                + learnerWeights.Get(learner) * 1);
                    }
                }
            }
            return table;
        }

        private string getTargetValueWithTheMaximumVotes(ICollection<string> targetValues, Table<string, ILearner, double> table)
        {
            string targetValueWithMaxScore = targetValues.Get(0);
            double score = scoreOfValue(targetValueWithMaxScore, table, learners);
            foreach (string value in targetValues)
            {
                double _scoreOfValue = scoreOfValue(value, table, learners);
                if (_scoreOfValue > score)
                {
                    targetValueWithMaxScore = value;
                    score = _scoreOfValue;
                }
            }
            return targetValueWithMaxScore;
        }

        private void initializeExampleWeights(int size)
        {
            if (size == 0)
            {
                throw new RuntimeException("cannot initialize Ensemble learning with Empty Dataset");
            }
            double value = 1.0 / (1.0 * size);
            exampleWeights = new double[size];
            for (int i = 0; i < size;++i)
            {
                exampleWeights[i] = value;
            }
        }

        private void initializeHypothesisWeights(int size)
        {
            if (size == 0)
            {
                throw new RuntimeException("cannot initialize Ensemble learning with Zero Learners");
            }

            learnerWeights = CollectionFactory.CreateInsertionOrderedMap<ILearner, double>();
            foreach (ILearner le in learners)
            {
                learnerWeights.Put(le, 1.0);
            }
        }

        private double calculateError(DataSet ds, ILearner l)
        {
            double error = 0.0;
            for (int i = 0; i < ds.examples.Size();++i)
            {
                Example e = ds.getExample(i);
                if (!(l.Predict(e).Equals(e.targetValue())))
                {
                    error = error + exampleWeights[i];
                }
            }
            return error;
        }

        private void adjustExampleWeights(DataSet ds, ILearner l, double error)
        {
            double epsilon = error / (1.0 - error);
            for (int j = 0; j < ds.examples.Size(); j++)
            {
                Example e = ds.getExample(j);
                if ((l.Predict(e).Equals(e.targetValue())))
                {
                    exampleWeights[j] = exampleWeights[j] * epsilon;
                }
            }
            exampleWeights = Util.normalize(exampleWeights);
        }

        private double scoreOfValue(string targetValue, Table<string, ILearner, double> table, ICollection<ILearner> learners)
        {
            double score = 0.0;
            foreach (ILearner l in learners)
            {
                score += table.Get(targetValue, l);
            }
            return score;
        }
    } 
}
