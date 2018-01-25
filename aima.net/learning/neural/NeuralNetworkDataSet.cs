using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.text;
using aima.net.text.api;
using aima.net.learning.framework;
using aima.net.learning.neural.api;
using aima.net.util;

namespace aima.net.learning.neural
{
    /// <summary>
    /// This class represents a source of examples to the rest of the nn
    /// framework. Assumes only one function approximator works on an instance at
    /// a given point in time
    /// </summary>
    public abstract class NeuralNetworkDataSet
    {
        /// <summary>
        /// the parsed and preprocessed form of the dataset.
        /// </summary>
        private ICollection<NeuralNetworkExample> dataset;
        /// <summary>
        /// a copy from which examples are drawn.
        /// </summary>
        private ICollection<NeuralNetworkExample> presentlyProcessed = CollectionFactory.CreateQueue<NeuralNetworkExample>();

        /// <summary>
        /// list of mean Values for all components of raw data set
        /// </summary>
        private ICollection<double> means;

        /// <summary>
        /// list of stdev Values for all components of raw data set
        /// </summary>
        private ICollection<double> stdevs;

        /// <summary>
        /// the normalized data set
        /// </summary>
        protected ICollection<ICollection<double>> nds;

        /// <summary>
        /// the column numbers of the "target"
        /// </summary>
        protected ICollection<int> targetColumnNumbers;

        /// <summary>
        /// population delegated to subclass because only subclass knows which column(s) is target
        /// </summary>
        public abstract void SetTargetColumns();

        /// <summary> 
        /// create a normalized data "table" from the data in the file. At this
        /// stage, the data isnot split into input pattern and tragets
        /// </summary>
        /// <param name="filename"></param>
        public void CreateNormalizedDataFromFile(string filename)
        { 
            ICollection<ICollection<double>> rds = CollectionFactory.CreateQueue<ICollection<double>>();

            // create raw data set
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename + ".csv"))
            { 
                string line = string.Empty;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    rds.Add(exampleFromString(line, ","));
                }
            }

            // normalize raw dataset
            nds = normalize(rds);
        }

        /// <summary>
        /// create a normalized data "table" from the DataSet using numerizer. At
        /// this stage, the data isnot split into input pattern and targets TODO
        /// remove redundancy of recreating the target columns. the numerizer has
        /// already isolated the targets
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="numerizer"></param>
        public void CreateNormalizedDataFromDataSet(DataSet ds, INumerizer numerizer)
        { 
            ICollection<ICollection<double>> rds = rawExamplesFromDataSet(ds, numerizer);
            // normalize raw dataset
            nds = normalize(rds);
        }

        /// <summary>
        /// Gets (and removes) a random example from the 'presentlyProcessed'
        /// </summary>
        /// <returns></returns>
        public NeuralNetworkExample GetExampleAtRandom()
        {
            int i = Util.randomNumberBetween(0, (presentlyProcessed.Size() - 1));
            return GetExample(i);
        }

        /// <summary>
        /// Gets (and removes) a random example from the 'presentlyProcessed'
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NeuralNetworkExample GetExample(int index)
        {
            NeuralNetworkExample obj = presentlyProcessed.Get(index);
            presentlyProcessed.Remove(obj);
            return obj;
        }

        /// <summary>
        /// check if any more examples remain to be processed
        /// </summary>
        /// <returns></returns>
        public bool HasMoreExamples()
        {
            return presentlyProcessed.Size() > 0;
        }

        /// <summary>
        /// check how many examples remain to be processed
        /// </summary>
        /// <returns></returns>
        public int HowManyExamplesLeft()
        {
            return presentlyProcessed.Size();
        }

        /// <summary> 
        /// refreshes the presentlyProcessed dataset so it can be used for a new
        /// epoch of training.
        /// </summary>
        public void RefreshDataset()
        {
            presentlyProcessed = CollectionFactory.CreateQueue<NeuralNetworkExample>();
            foreach (NeuralNetworkExample e in dataset)
            {
                presentlyProcessed.Add(e.CopyExample());
            }
        }

        /// <summary>
        /// method called by clients to set up data set and make it ready for processing
        /// </summary>
        /// <param name="filename"></param>
        public void CreateExamplesFromFile(string filename)
        {
            CreateNormalizedDataFromFile(filename);
            SetTargetColumns();
            createExamples();
        }

        /// <summary>
        /// method called by clients to set up data set and make it ready for processing
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="numerizer"></param>
        public void CreateExamplesFromDataSet(DataSet ds, INumerizer numerizer)
        {
            CreateNormalizedDataFromDataSet(ds, numerizer);
            SetTargetColumns();
            createExamples(); 
        }

        public ICollection<ICollection<double>> GetNormalizedData()
        {
            return nds;
        }

        public ICollection<double> GetMeans()
        {
            return means;
        }

        public ICollection<double> GetStdevs()
        {
            return stdevs;
        }

        /// <summary>
        /// create Example instances from a normalized data "table".
        /// </summary>
        private void createExamples()
        {
            dataset = CollectionFactory.CreateQueue<NeuralNetworkExample>();
            foreach (ICollection<double> dataLine in nds)
            {
                ICollection<double> input = CollectionFactory.CreateQueue<double>();
                ICollection<double> target = CollectionFactory.CreateQueue<double>();
                for (int i = 0; i < dataLine.Size(); ++i)
                {
                    if (targetColumnNumbers.Contains(i))
                    {
                        target.Add(dataLine.Get(i));
                    }
                    else
                    {
                        input.Add(dataLine.Get(i));
                    }
                }
                dataset.Add(new NeuralNetworkExample(input, target));
            }
            RefreshDataset();// to populate the preentlyProcessed dataset
        }

        private ICollection<ICollection<double>> normalize(ICollection<ICollection<double>> rds)
        {
            int rawDataLength = rds.Get(0).Size();
            ICollection<ICollection<double>> nds = CollectionFactory.CreateQueue<ICollection<double>>();

            means = CollectionFactory.CreateQueue<double>();
            stdevs = CollectionFactory.CreateQueue<double>();

            ICollection<ICollection<double>> normalizedColumns = CollectionFactory.CreateQueue<ICollection<double>>();
            // clculate means for each coponent of example data
            for (int i = 0; i < rawDataLength; ++i)
            {
                ICollection<double> columnValues = CollectionFactory.CreateQueue<double>();
                foreach (ICollection<double> rawDatum in rds)
                {
                    columnValues.Add(rawDatum.Get(i));
                }
                double mean = Util.calculateMean(columnValues);
                means.Add(mean);

                double stdev = Util.calculateStDev(columnValues, mean);
                stdevs.Add(stdev);

                normalizedColumns.Add(Util.normalizeFromMeanAndStdev(columnValues, mean, stdev));

            }
            // re arrange data from columns
            // TODO Assert normalized columns have same size etc

            int columnLength = normalizedColumns.Get(0).Size();
            int numberOfColumns = normalizedColumns.Size();
            for (int i = 0; i < columnLength; ++i)
            {
                ICollection<double> lst = CollectionFactory.CreateQueue<double>();
                for (int j = 0; j < numberOfColumns; j++)
                {
                    lst.Add(normalizedColumns.Get(j).Get(i));
                }
                nds.Add(lst);
            }
            return nds;
        }

        private ICollection<double> exampleFromString(string line, string separator)
        {
            // assumes all values for inout and target are doubles
            ICollection<double> rexample = CollectionFactory.CreateQueue<double>();
            IRegularExpression regex = TextFactory.CreateRegularExpression(separator);
            ICollection<string> attributeValues = CollectionFactory.CreateQueue<string>(regex.Split(line));
            foreach (string valString in attributeValues)
            {
                rexample.Add(double.Parse(valString, 
                    System.Globalization.NumberStyles.Any, 
                    System.Globalization.CultureInfo.InvariantCulture));
            }
            return rexample;
        }

        private ICollection<ICollection<double>> rawExamplesFromDataSet(DataSet ds, INumerizer numerizer)
        {
            // assumes all values for inout and target are doubles
            ICollection<ICollection<double>> rds = CollectionFactory.CreateQueue<ICollection<double>>();
            for (int i = 0; i < ds.size(); ++i)
            {
                ICollection<double> rexample = CollectionFactory.CreateQueue<double>();
                Example e = ds.getExample(i);
                Pair<ICollection<double>, ICollection<double>> p = numerizer.Numerize(e);
                ICollection<double> attributes = p.GetFirst();
                foreach (double d in attributes)
                {
                    rexample.Add(d);
                }
                ICollection<double> targets = p.getSecond();
                foreach (double d in targets)
                {
                    rexample.Add(d);
                }
                rds.Add(rexample);
            }
            return rds;
        }
    }
}
