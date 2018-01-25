using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.learning.framework;
using aima.net.logic.fol;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.learning.knowledge
{ 
    public class FOLExample : IStringable
    {
        private FOLDataSetDomain folDSDomain = null;
        private Example example = null;
        private int egNo = 0;
        //
        private Constant ithExampleConstant = null;
        private Sentence classification = null;
        private Sentence description = null;
         
        public FOLExample(FOLDataSetDomain folDSDomain, Example example, int egNo)
        {
            this.folDSDomain = folDSDomain;
            this.example = example;
            this.egNo = egNo;
            constructFOLEg();
        }

        public int getExampleNumber()
        {
            return egNo;
        }

        public Sentence getClassification()
        {
            return classification;
        }

        public Sentence getDescription()
        {
            return description;
        }

        public override string ToString()
        {
            return classification.ToString()
                 + " "
                 + Connectors.AND + " "
                 + description.ToString();
        }
         
        private void constructFOLEg()
        {
            ithExampleConstant = new Constant(folDSDomain.getExampleConstant(egNo));

            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(ithExampleConstant);
            // Create the classification sentence
            classification = new Predicate(folDSDomain.getGoalPredicateName(), terms);
            if (!example.getAttributeValueAsString(
                    folDSDomain.getDataSetTargetName()).Equals(folDSDomain.getTrueGoalValue()))
            {
                // if not true then needs to be a Not sentence
                classification = new NotSentence(classification);
            }

            // Create the description sentence
            ICollection<Sentence> descParts = CollectionFactory.CreateQueue<Sentence>();
            foreach (string dname in folDSDomain.getDescriptionDataSetNames())
            {
                string foldDName = folDSDomain.getFOLName(dname);
                terms = CollectionFactory.CreateQueue<Term>();
                terms.Add(ithExampleConstant);
                // If multivalued becomes a two place predicate
                // e.g: Patrons(X1, Some)
                // otherwise: Hungry(X1) or ~ Hungry(X1)
                // see pg 769 of AIMA
                Sentence part = null;
                if (folDSDomain.isMultivalued(dname))
                {
                    terms.Add(new Constant(folDSDomain.getFOLName(example.getAttributeValueAsString(dname))));
                    part = new Predicate(foldDName, terms);
                }
                else
                {
                    part = new Predicate(foldDName, terms);
                    // Need to determine if false
                    if (!folDSDomain.getTrueGoalValue().Equals(
                            example.getAttributeValueAsString(dname)))
                    {
                        part = new NotSentence(part);
                    }
                }
                descParts.Add(part);
            }
            if (descParts.Size() == 1)
            {
                description = descParts.Get(0);
            }
            else if (descParts.Size() > 1)
            {
                description = new ConnectedSentence(Connectors.AND, descParts.Get(0), descParts.Get(1));
                for (int i = 2; i < descParts.Size();++i)
                {
                    description = new ConnectedSentence(Connectors.AND, description, descParts.Get(i));
                }
            }
        }
    } 
}
