using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;
using aima.net.learning.framework;
using aima.net.logic.common;
using aima.net.logic.fol.domain;

namespace aima.net.learning.knowledge
{ 
    public class FOLDataSetDomain : FOLDomain
    {
        private static IRegularExpression allowableCharactersRegEx = TextFactory.CreateRegularExpression("[^a-zA-Z_$0-9]");
        //
        private DataSetSpecification dataSetSpecification;
        private string trueGoalValue = null;
        // Default example prefix, see pg679 of AIMA
        private string examplePrefix = "X";
        private ICollection<string> descriptionPredicateNames = CollectionFactory.CreateQueue<string>();
        private ICollection<string> descriptionDataSetNames = CollectionFactory.CreateQueue<string>();
        private IMap<string, string> dsToFOLNameMap = CollectionFactory.CreateInsertionOrderedMap<string, string>();

        //
        // PUBLIC METHODS
        //
        public FOLDataSetDomain(DataSetSpecification dataSetSpecification, string trueGoalValue)
        {
            this.dataSetSpecification = dataSetSpecification;
            this.trueGoalValue = trueGoalValue;
            constructFOLDomain();
        }

        public string getDataSetTargetName()
        {
            return dataSetSpecification.getTarget();
        }

        public string getGoalPredicateName()
        {
            return getFOLName(dataSetSpecification.getTarget());
        }

        public string getTrueGoalValue()
        {
            return trueGoalValue;
        }

        public ICollection<string> getDescriptionPredicateNames()
        {
            return descriptionPredicateNames;
        }

        public ICollection<string> getDescriptionDataSetNames()
        {
            return descriptionDataSetNames;
        }

        public bool isMultivalued(string descriptiveDataSetName)
        {
            ICollection<string> possibleValues = dataSetSpecification.getPossibleAttributeValues(descriptiveDataSetName);
            // If more than two possible values
            // then is multivalued
            if (possibleValues.Size() > 2)
            {
                return true;
            }
            // If one of the possible values for the attribute
            // matches the true goal value then consider
            // it not being multivalued.
            foreach (string pv in possibleValues)
            {
                if (trueGoalValue.Equals(pv))
                {
                    return false;
                }
            }

            return true;
        }

        public string getExampleConstant(int egNo)
        {
            string egConstant = examplePrefix + egNo;
            addConstant(egConstant);
            return egConstant;
        }

        public string getFOLName(string dsName)
        {
            string folName = dsToFOLNameMap.Get(dsName);
            if (null == folName)
            {
                folName = dsName;
                if (!Character.isSourceCodeIdentifierStart(dsName[0]))
                {
                    folName = "_" + dsName;
                }
                folName = allowableCharactersRegEx.Replace(folName, "_");
                dsToFOLNameMap.Put(dsName, folName);
            }

            return folName;
        }
         
        private void constructFOLDomain()
        {
            // Ensure the target predicate is included
            addPredicate(getFOLName(dataSetSpecification.getTarget()));
            // Create the descriptive predicates
            foreach (string saName in dataSetSpecification.getNamesOfStringAttributes())
            {
                if (dataSetSpecification.getTarget().Equals(saName))
                {
                    // Don't add the target to the descriptive predicates
                    continue;
                }
                string folSAName = getFOLName(saName);
                // Add a predicate for the attribute
                addPredicate(folSAName);

                descriptionPredicateNames.Add(folSAName);
                descriptionDataSetNames.Add(saName);

                ICollection<string> attributeValues = dataSetSpecification.getPossibleAttributeValues(saName);
                // If a multivalued attribute need to setup
                // Constants for the different possible values
                if (isMultivalued(saName))
                {
                    foreach (string av in attributeValues)
                    {
                        addConstant(getFOLName(av));
                    }
                }
            }
        }
    }
}
