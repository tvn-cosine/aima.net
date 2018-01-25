using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.learning.framework.api;
using aima.net.util;

namespace aima.net.learning.framework
{
    public class DataSetFactory
    {
        public DataSet fromFile(string filename, DataSetSpecification spec, string separator)
        {
            // assumed file in data directory and ends in .csv
            DataSet ds = new DataSet(spec);

            if (!System.IO.File.Exists(filename + ".csv"))
            {
                throw new FileNotFoundException(filename + ".csv" + " does not exist.");
            }
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename + ".csv"))
            {
                string line = string.Empty;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(line))
                    {
                        ds.add(exampleFromString(line, spec, separator));
                    }
                }

            }

            return ds;
        }

        public static Example exampleFromString(string data, DataSetSpecification dataSetSpec, string separator)
        {
            IRegularExpression splitter = TextFactory.CreateRegularExpression(separator);
            IMap<string, IAttribute> attributes = CollectionFactory.CreateInsertionOrderedMap<string, IAttribute>();
            ICollection<string> attributeValues = CollectionFactory.CreateQueue<string>(splitter.Split(data));
            if (dataSetSpec.isValid(attributeValues))
            {
                ICollection<string> names = dataSetSpec.getAttributeNames();
                int min = names.Size() > attributes.Size() ? names.Size() : attributes.Size();

                for (int i = 0; i < min; ++i)
                {
                    string name = names.Get(i);
                    IAttributeSpecification attributeSpec = dataSetSpec.getAttributeSpecFor(name);
                    IAttribute attribute = attributeSpec.CreateAttribute(attributeValues.Get(i));
                    attributes.Put(name, attribute);
                }
                string targetAttributeName = dataSetSpec.getTarget();
                return new Example(attributes, attributes.Get(targetAttributeName));
            }
            else
            {
                throw new RuntimeException("Unable to construct Example from " + data);
            }
        }

        public static DataSet getRestaurantDataSet()
        {
            DataSetSpecification spec = createRestaurantDataSetSpec();
            return new DataSetFactory().fromFile("restaurant", spec, "\\s+");
        }

        public static DataSetSpecification createRestaurantDataSetSpec()
        {
            DataSetSpecification dss = new DataSetSpecification();
            dss.defineStringAttribute("alternate", Util.YesNo());
            dss.defineStringAttribute("bar", Util.YesNo());
            dss.defineStringAttribute("fri/sat", Util.YesNo());
            dss.defineStringAttribute("hungry", Util.YesNo());
            dss.defineStringAttribute("patrons", new string[] { "None", "Some", "Full" });
            dss.defineStringAttribute("price", new string[] { "$", "$$", "$$$" });
            dss.defineStringAttribute("raining", Util.YesNo());
            dss.defineStringAttribute("reservation", Util.YesNo());
            dss.defineStringAttribute("type", new string[] { "French", "Italian", "Thai", "Burger" });
            dss.defineStringAttribute("wait_estimate", new string[] { "0-10", "10-30", "30-60", ">60" });
            dss.defineStringAttribute("will_wait", Util.YesNo());
            // last attribute is the target attribute unless the target is explicitly reset with dss.setTarget(name)

            return dss;
        }

        public static DataSet getIrisDataSet()
        {
            DataSetSpecification spec = createIrisDataSetSpec();
            return new DataSetFactory().fromFile("iris", spec, ",");
        }

        public static DataSet getAnimalDataSet()
        {
            DataSetSpecification spec = createAnimalDataSetSpec();
            return new DataSetFactory().fromFile("animal-test", spec, ",");
        }

        public static DataSetSpecification createCSVDataSetSpec()
        {
            DataSetSpecification dss = new DataSetSpecification();

            for (int i = 0; i < 13; ++i)
            {
                dss.defineNumericAttribute(i.ToString());
            }

            return dss;
        }
         
        public static DataSetSpecification createAnimalDataSetSpec()
        {
            DataSetSpecification dss = new DataSetSpecification();
            for (int i = 1; i <= 20; ++i)
            {
                dss.defineNumericAttribute(string.Format("feature_{0}", i));
            }
            dss.defineStringAttribute("animal_name", new string[] { "GIRAFFE", "HIPPO", "LION" });
            return dss;
        }

        public static DataSetSpecification createIrisDataSetSpec()
        {
            DataSetSpecification dss = new DataSetSpecification();
            dss.defineNumericAttribute("sepal_length");
            dss.defineNumericAttribute("sepal_width");
            dss.defineNumericAttribute("petal_length");
            dss.defineNumericAttribute("petal_width");
            dss.defineStringAttribute("plant_category", new string[] { "setosa", "versicolor", "virginica" });
            return dss;
        }
    }
}
