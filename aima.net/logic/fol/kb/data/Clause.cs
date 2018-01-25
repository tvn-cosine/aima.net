using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.logic.fol.inference.proof;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;
using aima.net.util.math;

namespace aima.net.logic.fol.kb.data
{
    /**
     * A Clause: A disjunction of literals. 
     */
    public class Clause
    {
        //
        private static StandardizeApartIndexical _saIndexical = StandardizeApartIndexicalFactory.newStandardizeApartIndexical('c');
        private static Unifier _unifier = new Unifier();
        private static SubstVisitor _substVisitor = new SubstVisitor();
        private static VariableCollector _variableCollector = new VariableCollector();
        private static StandardizeApart _standardizeApart = new StandardizeApart();
        private static LiteralsSorter _literalSorter = new LiteralsSorter();
        //
        private readonly ISet<Literal> literals = CollectionFactory.CreateSet<Literal>();
        private readonly ICollection<Literal> positiveLiterals = CollectionFactory.CreateQueue<Literal>();
        private readonly ICollection<Literal> negativeLiterals = CollectionFactory.CreateQueue<Literal>();
        private bool immutable = false;
        private bool saCheckRequired = true;
        private string equalityIdentity = "";
        private ISet<Clause> factors = null;
        private ISet<Clause> nonTrivialFactors = null;
        private string stringRep = null;
        private ProofStep proofStep = null;

        public Clause()
        {
            // i.e. the empty clause
        }

        public Clause(ICollection<Literal> lits)
        {
            this.literals.AddAll(lits);
            foreach (Literal l in literals)
            {
                if (l.isPositiveLiteral())
                {
                    this.positiveLiterals.Add(l);
                }
                else
                {
                    this.negativeLiterals.Add(l);
                }
            }
            recalculateIdentity();
        }

        public Clause(ICollection<Literal> lits1, ICollection<Literal> lits2)
        {
            literals.AddAll(lits1);
            literals.AddAll(lits2);
            foreach (Literal l in literals)
            {
                if (l.isPositiveLiteral())
                {
                    this.positiveLiterals.Add(l);
                }
                else
                {
                    this.negativeLiterals.Add(l);
                }
            }
            recalculateIdentity();
        }

        public ProofStep getProofStep()
        {
            if (null == proofStep)
            {
                // Assume was a premise
                proofStep = new ProofStepPremise(this);
            }
            return proofStep;
        }

        public void setProofStep(ProofStep proofStep)
        {
            this.proofStep = proofStep;
        }

        public bool isImmutable()
        {
            return immutable;
        }

        public void setImmutable()
        {
            immutable = true;
        }

        public bool isStandardizedApartCheckRequired()
        {
            return saCheckRequired;
        }

        public void setStandardizedApartCheckNotRequired()
        {
            saCheckRequired = false;
        }

        public bool isEmpty()
        {
            return literals.Size() == 0;
        }

        public bool isUnitClause()
        {
            return literals.Size() == 1;
        }

        public bool isDefiniteClause()
        {
            // A Definite Clause is a disjunction of literals of which exactly 1 is
            // positive.
            return !isEmpty() && positiveLiterals.Size() == 1;
        }

        public bool isImplicationDefiniteClause()
        {
            // An Implication Definite Clause is a disjunction of literals of
            // which exactly 1 is positive and there is 1 or more negative
            // literals.
            return isDefiniteClause() && negativeLiterals.Size() >= 1;
        }

        public bool isHornClause()
        {
            // A Horn clause is a disjunction of literals of which at most one is
            // positive.
            return !isEmpty() && positiveLiterals.Size() <= 1;
        }

        public bool isTautology()
        {
            foreach (Literal pl in positiveLiterals)
            {
                // Literals in a clause must be exact complements
                // for tautology elimination to apply. Do not
                // remove non-identical literals just because
                // they are complements under unification, see pg16:
                // http://logic.stanford.edu/classes/cs157/2008/notes/chap09.pdf
                foreach (Literal nl in negativeLiterals)
                {
                    if (pl.getAtomicSentence().Equals(nl.getAtomicSentence()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void addLiteral(Literal literal)
        {
            if (isImmutable())
            {
                throw new IllegalStateException("Clause is immutable, cannot be updated.");
            }
            int origSize = literals.Size();
            literals.Add(literal);
            if (literals.Size() > origSize)
            {
                if (literal.isPositiveLiteral())
                {
                    positiveLiterals.Add(literal);
                }
                else
                {
                    negativeLiterals.Add(literal);
                }
            }
            recalculateIdentity();
        }

        public void addPositiveLiteral(AtomicSentence atom)
        {
            addLiteral(new Literal(atom));
        }

        public void addNegativeLiteral(AtomicSentence atom)
        {
            addLiteral(new Literal(atom, true));
        }

        public int getNumberLiterals()
        {
            return literals.Size();
        }

        public int getNumberPositiveLiterals()
        {
            return positiveLiterals.Size();
        }

        public int getNumberNegativeLiterals()
        {
            return negativeLiterals.Size();
        }

        public ISet<Literal> getLiterals()
        {
            return CollectionFactory.CreateReadOnlySet<Literal>(literals);
        }

        public ICollection<Literal> getPositiveLiterals()
        {
            return CollectionFactory.CreateReadOnlyQueue<Literal>(positiveLiterals);
        }

        public ICollection<Literal> getNegativeLiterals()
        {
            return CollectionFactory.CreateReadOnlyQueue<Literal>(negativeLiterals);
        }

        public ISet<Clause> getFactors()
        {
            if (null == factors)
            {
                calculateFactors(null);
            }
            return CollectionFactory.CreateReadOnlySet<Clause>(factors);
        }

        public ISet<Clause> getNonTrivialFactors()
        {
            if (null == nonTrivialFactors)
            {
                calculateFactors(null);
            }
            return CollectionFactory.CreateReadOnlySet<Clause>(nonTrivialFactors);
        }

        public bool subsumes(Clause othC)
        {
            bool subsumes = false;

            // Equality is not subsumption
            if (!(this == othC))
            {
                // Ensure this has less literals total and that
                // it is a subset of the other clauses positive and negative counts
                if (this.getNumberLiterals() < othC.getNumberLiterals()
                        && this.getNumberPositiveLiterals() <= othC
                                .getNumberPositiveLiterals()
                        && this.getNumberNegativeLiterals() <= othC
                                .getNumberNegativeLiterals())
                {

                    IMap<string, ICollection<Literal>> thisToTry = collectLikeLiterals(this.literals);
                    IMap<string, ICollection<Literal>> othCToTry = collectLikeLiterals(othC.literals);
                    // Ensure all like literals from this clause are a subset
                    // of the other clause.
                    if (othCToTry.GetKeys().ContainsAll(thisToTry.GetKeys()))
                    {
                        bool isAPossSubset = true;
                        // Ensure that each set of same named literals
                        // from this clause is a subset of the other
                        // clauses same named literals.
                        foreach (string pk in thisToTry.GetKeys())
                        {
                            if (thisToTry.Get(pk).Size() > othCToTry.Get(pk).Size())
                            {
                                isAPossSubset = false;
                                break;
                            }
                        }
                        if (isAPossSubset)
                        {
                            // At this point I know this this Clause's
                            // literal/arity names are a subset of the
                            // other clauses literal/arity names
                            subsumes = checkSubsumes(othC, thisToTry, othCToTry);
                        }
                    }
                }
            }

            return subsumes;
        }

        // Note: Applies binary resolution rule
        // Note: returns a set with an empty clause if both clauses
        // are empty, otherwise returns a set of binary resolvents.
        public ISet<Clause> binaryResolvents(Clause othC)
        {
            ISet<Clause> resolvents = CollectionFactory.CreateSet<Clause>();
            // Resolving two empty clauses
            // gives you an empty clause
            if (isEmpty() && othC.isEmpty())
            {
                resolvents.Add(new Clause());
                return resolvents;
            }

            // Ensure Standardized Apart
            // Before attempting binary resolution
            othC = saIfRequired(othC);

            ICollection<Literal> allPosLits = CollectionFactory.CreateQueue<Literal>();
            ICollection<Literal> allNegLits = CollectionFactory.CreateQueue<Literal>();
            allPosLits.AddAll(this.positiveLiterals);
            allPosLits.AddAll(othC.positiveLiterals);
            allNegLits.AddAll(this.negativeLiterals);
            allNegLits.AddAll(othC.negativeLiterals);

            ICollection<Literal> trPosLits = CollectionFactory.CreateQueue<Literal>();
            ICollection<Literal> trNegLits = CollectionFactory.CreateQueue<Literal>();
            ICollection<Literal> copyRPosLits = CollectionFactory.CreateQueue<Literal>();
            ICollection<Literal> copyRNegLits = CollectionFactory.CreateQueue<Literal>();

            for (int i = 0; i < 2; ++i)
            {
                trPosLits.Clear();
                trNegLits.Clear();

                if (i == 0)
                {
                    // See if this clauses positives
                    // unify with the other clauses
                    // negatives
                    trPosLits.AddAll(this.positiveLiterals);
                    trNegLits.AddAll(othC.negativeLiterals);
                }
                else
                {
                    // Try the other way round now
                    trPosLits.AddAll(othC.positiveLiterals);
                    trNegLits.AddAll(this.negativeLiterals);
                }

                // Now check to see if they resolve
                IMap<Variable, Term> copyRBindings = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
                foreach (Literal pl in trPosLits)
                {
                    foreach (Literal nl in trNegLits)
                    {
                        copyRBindings.Clear();
                        if (null != _unifier.unify(pl.getAtomicSentence(),
                                nl.getAtomicSentence(), copyRBindings))
                        {
                            copyRPosLits.Clear();
                            copyRNegLits.Clear();
                            bool found = false;
                            foreach (Literal l in allPosLits)
                            {
                                if (!found && pl.Equals(l))
                                {
                                    found = true;
                                    continue;
                                }
                                copyRPosLits.Add(_substVisitor.subst(copyRBindings, l));
                            }
                            found = false;
                            foreach (Literal l in allNegLits)
                            {
                                if (!found && nl.Equals(l))
                                {
                                    found = true;
                                    continue;
                                }
                                copyRNegLits.Add(_substVisitor.subst(copyRBindings, l));
                            }
                            // Ensure the resolvents are standardized apart
                            IMap<Variable, Term> renameSubstitituon = _standardizeApart
                                    .standardizeApart(copyRPosLits, copyRNegLits, _saIndexical);
                            Clause c = new Clause(copyRPosLits, copyRNegLits);
                            c.setProofStep(new ProofStepClauseBinaryResolvent(c,
                                    pl, nl, this, othC, copyRBindings,
                                    renameSubstitituon));
                            if (isImmutable())
                            {
                                c.setImmutable();
                            }
                            if (!isStandardizedApartCheckRequired())
                            {
                                c.setStandardizedApartCheckNotRequired();
                            }
                            resolvents.Add(c);
                        }
                    }
                }
            }

            return resolvents;
        }
         
        public override string ToString()
        {
            if (null == stringRep)
            {
                ICollection<Literal> sortedLiterals = CollectionFactory.CreateQueue<Literal>(literals);
                sortedLiterals.Sort(_literalSorter);

                stringRep = sortedLiterals.ToString();
            }
            return stringRep;
        }
         
        public override int GetHashCode()
        {
            return equalityIdentity.GetHashCode();
        }

        public override bool Equals(object othObj)
        {
            if (null == othObj)
            {
                return false;
            }
            if (this == othObj)
            {
                return true;
            }
            if (!(othObj is Clause))
            {
                return false;
            }
            Clause othClause = (Clause)othObj;

            return equalityIdentity.Equals(othClause.equalityIdentity);
        }

        public string getEqualityIdentity()
        {
            return equalityIdentity;
        }

        //
        // PRIVATE METHODS
        //
        private void recalculateIdentity()
        {
            // Sort the literals first based on negation, atomic sentence,
            // constant, function and variable.
            ICollection<Literal> sortedLiterals = CollectionFactory.CreateQueue<Literal>(literals);
            sortedLiterals.Sort(_literalSorter);

            // All variables are considered the same as regards
            // sorting. Therefore, to determine if two clauses
            // are equivalent you need to determine
            // the # of unique variables they contain and
            // there positions across the clauses
            ClauseEqualityIdentityConstructor ceic = new ClauseEqualityIdentityConstructor(sortedLiterals, _literalSorter);

            equalityIdentity = ceic.getIdentity();

            // Reset, these as will need to re-calcualte
            // if requested for again, best to only
            // access lazily.
            factors = null;
            nonTrivialFactors = null;
            // Reset the objects string representation
            // until it is requested for.
            stringRep = null;
        }

        private void calculateFactors(ISet<Clause> parentFactors)
        {
            nonTrivialFactors = CollectionFactory.CreateSet<Clause>();

            IMap<Variable, Term> theta = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            ICollection<Literal> lits = CollectionFactory.CreateQueue<Literal>();
            for (int i = 0; i < 2; ++i)
            {
                lits.Clear();
                if (i == 0)
                {
                    // Look at the positive literals
                    lits.AddAll(positiveLiterals);
                }
                else
                {
                    // Look at the negative literals
                    lits.AddAll(negativeLiterals);
                }
                for (int x = 0; x < lits.Size(); x++)
                {
                    for (int y = x + 1; y < lits.Size(); y++)
                    {
                        Literal litX = lits.Get(x);
                        Literal litY = lits.Get(y);

                        theta.Clear();
                        IMap<Variable, Term> substitution = _unifier.unify(
                                litX.getAtomicSentence(), litY.getAtomicSentence(),
                                theta);
                        if (null != substitution)
                        {
                            ICollection<Literal> posLits = CollectionFactory.CreateQueue<Literal>();
                            ICollection<Literal> negLits = CollectionFactory.CreateQueue<Literal>();
                            if (i == 0)
                            {
                                posLits.Add(_substVisitor.subst(substitution, litX));
                            }
                            else
                            {
                                negLits.Add(_substVisitor.subst(substitution, litX));
                            }
                            foreach (Literal pl in positiveLiterals)
                            {
                                if (pl == litX || pl == litY)
                                {
                                    continue;
                                }
                                posLits.Add(_substVisitor.subst(substitution, pl));
                            }
                            foreach (Literal nl in negativeLiterals)
                            {
                                if (nl == litX || nl == litY)
                                {
                                    continue;
                                }
                                negLits.Add(_substVisitor.subst(substitution, nl));
                            }
                            // Ensure the non trivial factor is standardized apart
                            IMap<Variable, Term> renameSubst = _standardizeApart
                                    .standardizeApart(posLits, negLits,
                                            _saIndexical);
                            Clause c = new Clause(posLits, negLits);
                            c.setProofStep(new ProofStepClauseFactor(c, this, litX,
                                    litY, substitution, renameSubst));
                            if (isImmutable())
                            {
                                c.setImmutable();
                            }
                            if (!isStandardizedApartCheckRequired())
                            {
                                c.setStandardizedApartCheckNotRequired();
                            }
                            if (null == parentFactors)
                            {
                                c.calculateFactors(nonTrivialFactors);
                                nonTrivialFactors.AddAll(c.getFactors());
                            }
                            else
                            {
                                if (!parentFactors.Contains(c))
                                {
                                    c.calculateFactors(nonTrivialFactors);
                                    nonTrivialFactors.AddAll(c.getFactors());
                                }
                            }
                        }
                    }
                }
            }

            factors = CollectionFactory.CreateSet<Clause>();
            // Need to add self, even though a non-trivial
            // factor. See: slide 30
            // http://logic.stanford.edu/classes/cs157/2008/lectures/lecture10.pdf
            // for example of incompleteness when
            // trivial factor not included.
            factors.Add(this);
            factors.AddAll(nonTrivialFactors);
        }

        private Clause saIfRequired(Clause othClause)
        {

            // If performing resolution with self
            // then need to standardize apart in
            // order to work correctly.
            if (isStandardizedApartCheckRequired() || this == othClause)
            {
                ISet<Variable> mVariables = _variableCollector
                        .collectAllVariables(this);
                ISet<Variable> oVariables = _variableCollector
                        .collectAllVariables(othClause);

                ISet<Variable> cVariables = CollectionFactory.CreateSet<Variable>();
                cVariables.AddAll(mVariables);
                cVariables.AddAll(oVariables);

                if (cVariables.Size() < (mVariables.Size() + oVariables.Size()))
                {
                    othClause = _standardizeApart.standardizeApart(othClause, _saIndexical);
                }
            }

            return othClause;
        }

        private IMap<string, ICollection<Literal>> collectLikeLiterals(ISet<Literal> literals)
        {
            IMap<string, ICollection<Literal>> likeLiterals = CollectionFactory.CreateInsertionOrderedMap<string, ICollection<Literal>>();
            foreach (Literal l in literals)
            {
                // Want to ensure P(a, b) is considered different than P(a, b, c)
                // i.e. consider an atom's arity P/#.
                string literalName = (l.isNegativeLiteral() ? "~" : "")
                        + l.getAtomicSentence().getSymbolicName() + "/"
                        + l.getAtomicSentence().getArgs().Size();
                ICollection<Literal> like = likeLiterals.Get(literalName);
                if (null == like)
                {
                    like = CollectionFactory.CreateQueue<Literal>();
                    likeLiterals.Put(literalName, like);
                }
                like.Add(l);
            }
            return likeLiterals;
        }

        private bool checkSubsumes(Clause othC,
                IMap<string, ICollection<Literal>> thisToTry,
                IMap<string, ICollection<Literal>> othCToTry)
        {
            bool subsumes = false;

            ICollection<Term> thisTerms = CollectionFactory.CreateQueue<Term>();
            ICollection<Term> othCTerms = CollectionFactory.CreateQueue<Term>();

            // Want to track possible number of permuations
            ICollection<int> radices = CollectionFactory.CreateQueue<int>();
            foreach (string literalName in thisToTry.GetKeys())
            {
                int sizeT = thisToTry.Get(literalName).Size();
                int sizeO = othCToTry.Get(literalName).Size();

                if (sizeO > 1)
                {
                    // The following is being used to
                    // track the number of permutations
                    // that can be mapped from the
                    // other clauses like literals to this
                    // clauses like literals.
                    // i.e. n!/(n-r)!
                    // where n=sizeO and r =sizeT
                    for (int i = 0; i < sizeT; ++i)
                    {
                        int r = sizeO - i;
                        if (r > 1)
                        {
                            radices.Add(r);
                        }
                    }
                }
                // Track the terms for this clause
                foreach (Literal tl in thisToTry.Get(literalName))
                {
                    thisTerms.AddAll(tl.getAtomicSentence().getArgs());
                }
            }

            MixedRadixNumber permutation = null;
            long numPermutations = 1L;
            if (radices.Size() > 0)
            {
                permutation = new MixedRadixNumber(0, radices);
                numPermutations = permutation.GetMaxAllowedValue() + 1;
            }
            // Want to ensure none of the othCVariables are
            // part of the key set of a unification as
            // this indicates it is not a legal subsumption.
            ISet<Variable> othCVariables = _variableCollector
                    .collectAllVariables(othC);
            IMap<Variable, Term> theta = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            ICollection<Literal> literalPermuations = CollectionFactory.CreateQueue<Literal>();
            for (long l = 0L; l < numPermutations; l++)
            {
                // Track the other clause's terms for this
                // permutation.
                othCTerms.Clear();
                int radixIdx = 0;
                foreach (string literalName in thisToTry.GetKeys())
                {
                    int sizeT = thisToTry.Get(literalName).Size();
                    literalPermuations.Clear();
                    literalPermuations.AddAll(othCToTry.Get(literalName));
                    int sizeO = literalPermuations.Size();

                    if (sizeO > 1)
                    {
                        for (int i = 0; i < sizeT; ++i)
                        {
                            int r = sizeO - i;
                            if (r > 1)
                            {
                                // If not a 1 to 1 mapping then you need
                                // to use the correct permuation
                                int numPos = permutation.GetCurrentNumeralValue(radixIdx);
                                Literal lit = literalPermuations.Get(numPos);
                                literalPermuations.Remove(lit);
                                othCTerms.AddAll(lit.getAtomicSentence().getArgs());
                                radixIdx++;
                            }
                            else
                            {
                                // is the last mapping, therefore
                                // won't be on the radix
                                othCTerms.AddAll(literalPermuations.Get(0).getAtomicSentence().getArgs());
                            }
                        }
                    }
                    else
                    {
                        // a 1 to 1 mapping
                        othCTerms.AddAll(literalPermuations.Get(0)
                                .getAtomicSentence().getArgs());
                    }
                }

                // Note: on unifier
                // unifier.unify(P(w, x), P(y, z)))={w=y, x=z}
                // unifier.unify(P(y, z), P(w, x)))={y=w, z=x}
                // Therefore want this clause to be the first
                // so can do the othCVariables check for an invalid
                // subsumes.
                theta.Clear();
                if (null != _unifier.unify(thisTerms, othCTerms, theta))
                {
                    bool containsAny = false;
                    foreach (Variable v in theta.GetKeys())
                    {
                        if (othCVariables.Contains(v))
                        {
                            containsAny = true;
                            break;
                        }
                    }
                    if (!containsAny)
                    {
                        subsumes = true;
                        break;
                    }
                }

                // If there is more than 1 mapping
                // keep track of where I am in the
                // possible number of mapping permutations.
                if (null != permutation)
                {
                    permutation.Increment();
                }
            }

            return subsumes;
        }
    }

    class LiteralsSorter : IComparer<Literal>
    {
        public int Compare(Literal o1, Literal o2)
        {
            int rVal = 0;
            // If literals are not negated the same
            // then positive literals are considered
            // (by convention here) to be of higher
            // order than negative literals
            if (o1.isPositiveLiteral() != o2.isPositiveLiteral())
            {
                if (o1.isPositiveLiteral())
                {
                    return 1;
                }
                return -1;
            }

            // Check their symbolic names for order first
            rVal = o1.getAtomicSentence().getSymbolicName().CompareTo(o2.getAtomicSentence().getSymbolicName());

            // If have same symbolic names
            // then need to compare individual arguments
            // for order.
            if (0 == rVal)
            {
                rVal = compareArgs(o1.getAtomicSentence().getArgs(), o2.getAtomicSentence().getArgs());
            }

            return rVal;
        }

        private int compareArgs(ICollection<Term> args1, ICollection<Term> args2)
        {
            int rVal = 0;

            // Compare argument sizes first
            rVal = args1.Size() - args2.Size();

            if (0 == rVal && args1.Size() > 0)
            {
                // Move forward and compare the
                // first arguments
                Term t1 = args1.Get(0);
                Term t2 = args2.Get(0);

                if (t1.GetType() == t2.GetType())
                {
                    // Note: Variables are considered to have
                    // the same order
                    if (t1 is Constant)
                    {
                        rVal = t1.getSymbolicName().CompareTo(t2.getSymbolicName());
                    }
                    else if (t1 is Function)
                    {
                        rVal = t1.getSymbolicName().CompareTo(t2.getSymbolicName());
                        if (0 == rVal)
                        {
                            // Same function names, therefore
                            // compare the function arguments
                            rVal = compareArgs(t1.getArgs(), t2.getArgs());
                        }
                    }

                    // If the first args are the same
                    // then compare the ordering of the
                    // remaining arguments
                    if (0 == rVal)
                    {
                        rVal = compareArgs(args1.subList(1, args1.Size()),
                                args2.subList(1, args2.Size()));
                    }
                }
                else
                {
                    // Order for different Terms is:
                    // Constant > Function > Variable
                    if (t1 is Constant)
                    {
                        rVal = 1;
                    }
                    else if (t2 is Constant)
                    {
                        rVal = -1;
                    }
                    else if (t1 is Function)
                    {
                        rVal = 1;
                    }
                    else
                    {
                        rVal = -1;
                    }
                }
            }

            return rVal;
        }
    }

    class ClauseEqualityIdentityConstructor : FOLVisitor
    {
        private IStringBuilder identity = TextFactory.CreateStringBuilder();
        private int noVarPositions = 0;
        private int[] clauseVarCounts = null;
        private int currentLiteral = 0;
        private IMap<string, ICollection<int>> varPositions = CollectionFactory.CreateInsertionOrderedMap<string, ICollection<int>>();

        public ClauseEqualityIdentityConstructor(ICollection<Literal> literals, LiteralsSorter sorter)
        {

            clauseVarCounts = new int[literals.Size()];

            foreach (Literal l in literals)
            {
                if (l.isNegativeLiteral())
                {
                    identity.Append("~");
                }
                identity.Append(l.getAtomicSentence().getSymbolicName());
                identity.Append("(");
                bool firstTerm = true;
                foreach (Term t in l.getAtomicSentence().getArgs())
                {
                    if (firstTerm)
                    {
                        firstTerm = false;
                    }
                    else
                    {
                        identity.Append(",");
                    }
                    t.accept(this, null);
                }
                identity.Append(")");
                currentLiteral++;
            }

            int min, max;
            min = max = 0;
            for (int i = 0; i < literals.Size(); ++i)
            {
                int incITo = i;
                int next = i + 1;
                max += clauseVarCounts[i];
                while (next < literals.Size())
                {
                    if (0 != sorter.Compare(literals.Get(i), literals.Get(next)))
                    {
                        break;
                    }
                    max += clauseVarCounts[next];
                    incITo = next; // Need to skip to the end of the range
                    next++;
                }
                // This indicates two or more literals are identical
                // except for variable naming (note: identical
                // same name would be removed as are working
                // with sets so don't need to worry about this).
                if ((next - i) > 1)
                {
                    // Need to check each variable
                    // and if it has a position within the
                    // current min/max range then need
                    // to include its alternative
                    // sort order positions as well
                    foreach (string key in varPositions.GetKeys())
                    {
                        ICollection<int> positions = varPositions.Get(key);
                        ICollection<int> additPositions = CollectionFactory.CreateQueue<int>();
                        // Add then subtract for all possible
                        // positions in range
                        foreach (int pos in positions)
                        {
                            if (pos >= min && pos < max)
                            {
                                int pPos = pos;
                                int nPos = pos;
                                for (int candSlot = i; candSlot < (next - 1); candSlot++)
                                {
                                    pPos += clauseVarCounts[i];
                                    if (pPos >= min && pPos < max)
                                    {
                                        if (!positions.Contains(pPos)
                                                && !additPositions.Contains(pPos))
                                        {
                                            additPositions.Add(pPos);
                                        }
                                    }
                                    nPos -= clauseVarCounts[i];
                                    if (nPos >= min && nPos < max)
                                    {
                                        if (!positions.Contains(nPos)
                                                && !additPositions.Contains(nPos))
                                        {
                                            additPositions.Add(nPos);
                                        }
                                    }
                                }
                            }
                        }
                        positions.AddAll(additPositions);
                    }
                }
                min = max;
                i = incITo;
            }

            // Determine the maxWidth
            int maxWidth = 1;
            while (noVarPositions >= 10)
            {
                noVarPositions = noVarPositions / 10;
                maxWidth++;
            }

            // Sort the individual position lists
            // And then add their string representations
            // together
            ICollection<string> varOffsets = CollectionFactory.CreateQueue<string>();
            foreach (string key in varPositions.GetKeys())
            {
                ICollection<int> positions = varPositions.Get(key);
                positions.Sort(new List<int>.Comparer());
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                foreach (int pos in positions)
                {
                    string posStr = pos.ToString();
                    int posStrLen = posStr.Length;
                    int padLen = maxWidth - posStrLen;
                    for (int i = 0; i < padLen; ++i)
                    {
                        sb.Append('0');
                    }
                    sb.Append(posStr);
                }
                varOffsets.Add(sb.ToString());
            }
            varOffsets.Sort(new List<string>.Comparer());
            for (int i = 0; i < varOffsets.Size(); ++i)
            {
                identity.Append(varOffsets.Get(i));
                if (i < (varOffsets.Size() - 1))
                {
                    identity.Append(",");
                }
            }
        }

        public string getIdentity()
        {
            return identity.ToString();
        }

        //
        // START-FOLVisitor
        public object visitVariable(Variable var, object arg)
        {
            // All variables will be marked with an *
            identity.Append("*");

            ICollection<int> positions = varPositions.Get(var.getValue());
            if (null == positions)
            {
                positions = CollectionFactory.CreateQueue<int>();
                varPositions.Put(var.getValue(), positions);
            }
            positions.Add(noVarPositions);

            noVarPositions++;
            clauseVarCounts[currentLiteral]++;
            return var;
        }

        public object visitConstant(Constant constant, object arg)
        {
            identity.Append(constant.getValue());
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            bool firstTerm = true;
            identity.Append(function.getFunctionName());
            identity.Append("(");
            foreach (Term t in function.getTerms())
            {
                if (firstTerm)
                {
                    firstTerm = false;
                }
                else
                {
                    identity.Append(",");
                }
                t.accept(this, arg);
            }
            identity.Append(")");

            return function;
        }

        public object visitPredicate(Predicate predicate, object arg)
        {
            throw new IllegalStateException("Should not be called");
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            throw new IllegalStateException("Should not be called");
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            throw new IllegalStateException("Should not be called");
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            throw new IllegalStateException("Should not be called");
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            throw new IllegalStateException("Should not be called");
        }
    }
}
