using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.inference.proof;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol
{
    public class StandardizeApart
    {
        private VariableCollector variableCollector = null;
        private SubstVisitor substVisitor = null;

        public StandardizeApart()
        {
            variableCollector = new VariableCollector();
            substVisitor = new SubstVisitor();
        }

        public StandardizeApart(VariableCollector variableCollector, SubstVisitor substVisitor)
        {
            this.variableCollector = variableCollector;
            this.substVisitor = substVisitor;
        }

        // Note: see page 327.
        public StandardizeApartResult standardizeApart(Sentence sentence, StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = variableCollector.collectAllVariables(sentence);
            IMap<Variable, Term> renameSubstitution = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            IMap<Variable, Term> reverseSubstitution = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix() + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution.Put(var, v);
                reverseSubstitution.Put(v, var);
            }

            Sentence standardized = substVisitor.subst(renameSubstitution, sentence);

            return new StandardizeApartResult(sentence, standardized, renameSubstitution, reverseSubstitution);
        }

        public Clause standardizeApart(Clause clause, StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = variableCollector.collectAllVariables(clause);
            IMap<Variable, Term> renameSubstitution = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix() + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution.Put(var, v);
            }

            if (renameSubstitution.Size() > 0)
            {
                ICollection<Literal> literals = CollectionFactory.CreateQueue<Literal>();

                foreach (Literal l in clause.getLiterals())
                {
                    literals.Add(substVisitor.subst(renameSubstitution, l));
                }
                Clause renamed = new Clause(literals);
                renamed.setProofStep(new ProofStepRenaming(renamed, clause.getProofStep()));
                return renamed;
            }

            return clause;
        }

        public Chain standardizeApart(Chain chain, StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = variableCollector.collectAllVariables(chain);
            IMap<Variable, Term> renameSubstitution = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution.Put(var, v);
            }

            if (renameSubstitution.Size() > 0)
            {
                ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();

                foreach (Literal l in chain.getLiterals())
                {
                    AtomicSentence atom = (AtomicSentence)substVisitor.subst(renameSubstitution, l.getAtomicSentence());
                    lits.Add(l.newInstance(atom));
                }

                Chain renamed = new Chain(lits);

                renamed.setProofStep(new ProofStepRenaming(renamed, chain.getProofStep()));

                return renamed;
            }

            return chain;
        }

        public IMap<Variable, Term> standardizeApart(ICollection<Literal> l1Literals,
                ICollection<Literal> l2Literals,
                StandardizeApartIndexical standardizeApartIndexical)
        {
            ISet<Variable> toRename = CollectionFactory.CreateSet<Variable>();

            foreach (Literal pl in l1Literals)
            {
                toRename.AddAll(variableCollector.collectAllVariables(pl
                        .getAtomicSentence()));
            }
            foreach (Literal nl in l2Literals)
            {
                toRename.AddAll(variableCollector.collectAllVariables(nl.getAtomicSentence()));
            }

            IMap<Variable, Term> renameSubstitution = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

            foreach (Variable var in toRename)
            {
                Variable v = null;
                do
                {
                    v = new Variable(standardizeApartIndexical.getPrefix()
                            + standardizeApartIndexical.getNextIndex());
                    // Ensure the new variable name is not already
                    // accidentally used in the sentence
                } while (toRename.Contains(v));

                renameSubstitution.Put(var, v);
            }

            ICollection<Literal> posLits = CollectionFactory.CreateQueue<Literal>();
            ICollection<Literal> negLits = CollectionFactory.CreateQueue<Literal>();

            foreach (Literal pl in l1Literals)
            {
                posLits.Add(substVisitor.subst(renameSubstitution, pl));
            }
            foreach (Literal nl in l2Literals)
            {
                negLits.Add(substVisitor.subst(renameSubstitution, nl));
            }

            l1Literals.Clear();
            l1Literals.AddAll(posLits);
            l2Literals.Clear();
            l2Literals.AddAll(negLits);

            return renameSubstitution;
        }
    }
}
