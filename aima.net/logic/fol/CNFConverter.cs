using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 345.<br>
     * <br>
     * Every sentence of first-order logic can be converted into an inferentially
     * equivalent CNF sentence.<br>
     * <br>
     * <b>Note:</b> Transformation rules extracted from 346 and 347, which are
     * essentially the INSEADO method outlined in: <a
     * href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture09.pdf"
     * >INSEADO Rules</a>
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class CNFConverter
    {
        private FOLParser parser = null;
        private SubstVisitor substVisitor;

        public CNFConverter(FOLParser parser)
        {
            this.parser = parser;
            this.substVisitor = new SubstVisitor();
        }

        /**
         * Returns the specified sentence as a list of clauses, where each clause is
         * a disjunction of literals.
         * 
         * @param aSentence
         *            a sentence in first order logic (predicate calculus)
         * 
         * @return the specified sentence as a list of clauses, where each clause is
         *         a disjunction of literals.
         */
        public CNF convertToCNF(Sentence aSentence)
        {
            // I)mplications Out:
            Sentence implicationsOut = (Sentence)aSentence.accept(new ImplicationsOut(), null);

            // N)egations In:
            Sentence negationsIn = (Sentence)implicationsOut.accept(new NegationsIn(), null);

            // S)tandardize variables:
            // For sentences like:
            // (FORALL x P(x)) V (EXISTS x Q(x)),
            // which use the same variable name twice, change the name of one of the
            // variables.
            Sentence saQuantifiers = (Sentence)negationsIn.accept(
                    new StandardizeQuantiferVariables(substVisitor),
                    CollectionFactory.CreateSet<Variable>());

            // Remove explicit quantifiers, by skolemizing existentials
            // and dropping universals:
            // E)xistentials Out
            // A)lls Out:
            Sentence andsAndOrs = (Sentence)saQuantifiers.accept(new RemoveQuantifiers(parser), CollectionFactory.CreateSet<Variable>());

            // D)istribution
            // V over ^:
            Sentence orDistributedOverAnd = (Sentence)andsAndOrs.accept(new DistributeOrOverAnd(), null);

            // O)perators Out
            return new CNFConstructor().construct(orDistributedOverAnd);
        }
    }

    class ImplicationsOut : FOLVisitor
    {
        public ImplicationsOut()
        { }

        public object visitPredicate(Predicate p, object arg)
        {
            return p;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            return equality;
        }

        public object visitVariable(Variable variable, object arg)
        {
            return variable;
        }

        public object visitConstant(Constant constant, object arg)
        {
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            return function;
        }

        public object visitNotSentence(NotSentence notSentence, object arg)
        {
            Sentence negated = notSentence.getNegated();

            return new NotSentence((Sentence)negated.accept(this, arg));
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            Sentence alpha = (Sentence)sentence.getFirst().accept(this, arg);
            Sentence beta = (Sentence)sentence.getSecond().accept(this, arg);

            // Eliminate <=>, bi-conditional elimination,
            // replace (alpha <=> beta) with (~alpha V beta) ^ (alpha V ~beta).
            if (Connectors.isBICOND(sentence.getConnector()))
            {
                Sentence first = new ConnectedSentence(Connectors.OR, new NotSentence(alpha), beta);
                Sentence second = new ConnectedSentence(Connectors.OR, alpha, new NotSentence(beta));

                return new ConnectedSentence(Connectors.AND, first, second);
            }

            // Eliminate =>, implication elimination,
            // replacing (alpha => beta) with (~alpha V beta)
            if (Connectors.isIMPLIES(sentence.getConnector()))
            {
                return new ConnectedSentence(Connectors.OR, new NotSentence(alpha), beta);
            }

            return new ConnectedSentence(sentence.getConnector(), alpha, beta);
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {

            return new QuantifiedSentence(sentence.getQuantifier(),
                    sentence.getVariables(), (Sentence)sentence.getQuantified().accept(this, arg));
        }
    }

    class NegationsIn : FOLVisitor
    {
        public NegationsIn()
        { }

        public object visitPredicate(Predicate p, object arg)
        {
            return p;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            return equality;
        }

        public object visitVariable(Variable variable, object arg)
        {
            return variable;
        }

        public object visitConstant(Constant constant, object arg)
        {
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            return function;
        }

        public object visitNotSentence(NotSentence notSentence, object arg)
        {
            // CNF requires NOT (~) to appear only in literals, so we 'move ~
            // inwards' by repeated application of the following equivalences:
            Sentence negated = notSentence.getNegated();

            // ~(~alpha) equivalent to alpha (double negation elimination)
            if (negated is NotSentence)
            {
                return ((NotSentence)negated).getNegated().accept(this, arg);
            }

            if (negated is ConnectedSentence)
            {
                ConnectedSentence negConnected = (ConnectedSentence)negated;
                Sentence alpha = negConnected.getFirst();
                Sentence beta = negConnected.getSecond();
                // ~(alpha ^ beta) equivalent to (~alpha V ~beta) (De Morgan)
                if (Connectors.isAND(negConnected.getConnector()))
                {
                    // I need to ensure the ~s are moved in deeper
                    Sentence notAlpha = (Sentence)(new NotSentence(alpha)).accept(this, arg);
                    Sentence notBeta = (Sentence)(new NotSentence(beta)).accept(this, arg);
                    return new ConnectedSentence(Connectors.OR, notAlpha, notBeta);
                }

                // ~(alpha V beta) equivalent to (~alpha ^ ~beta) (De Morgan)
                if (Connectors.isOR(negConnected.getConnector()))
                {
                    // I need to ensure the ~s are moved in deeper
                    Sentence notAlpha = (Sentence)(new NotSentence(alpha)).accept(this, arg);
                    Sentence notBeta = (Sentence)(new NotSentence(beta)).accept(this, arg);
                    return new ConnectedSentence(Connectors.AND, notAlpha, notBeta);
                }
            }

            // in addition, rules for negated quantifiers:
            if (negated is QuantifiedSentence)
            {
                QuantifiedSentence negQuantified = (QuantifiedSentence)negated;
                // I need to ensure the ~ is moved in deeper
                Sentence notP = (Sentence)(new NotSentence(negQuantified.getQuantified())).accept(this, arg);

                // ~FORALL x p becomes EXISTS x ~p
                if (Quantifiers.isFORALL(negQuantified.getQuantifier()))
                {
                    return new QuantifiedSentence(Quantifiers.EXISTS, negQuantified.getVariables(), notP);
                }

                // ~EXISTS x p becomes FORALL x ~p
                if (Quantifiers.isEXISTS(negQuantified.getQuantifier()))
                {
                    return new QuantifiedSentence(Quantifiers.FORALL, negQuantified.getVariables(), notP);
                }
            }

            return new NotSentence((Sentence)negated.accept(this, arg));
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            return new ConnectedSentence(sentence.getConnector(),
                    (Sentence)sentence.getFirst().accept(this, arg),
                    (Sentence)sentence.getSecond().accept(this, arg));
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {

            return new QuantifiedSentence(sentence.getQuantifier(),
                    sentence.getVariables(), (Sentence)sentence.getQuantified().accept(this, arg));
        }
    }

    class StandardizeQuantiferVariables : FOLVisitor
    {
        class StandardizeQuantiferVariablesStandardizeApartIndexical : StandardizeApartIndexical
        {
            private int index = 0;

            public string getPrefix()
            {
                return "q";
            }

            public int getNextIndex()
            {
                return index++;
            }
        }
        // Just use a localized indexical here.
        private StandardizeApartIndexical quantifiedIndexical = new StandardizeQuantiferVariablesStandardizeApartIndexical();

        private SubstVisitor substVisitor = null;

        public StandardizeQuantiferVariables(SubstVisitor substVisitor)
        {
            this.substVisitor = substVisitor;
        }

        public object visitPredicate(Predicate p, object arg)
        {
            return p;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            return equality;
        }

        public object visitVariable(Variable variable, object arg)
        {
            return variable;
        }

        public object visitConstant(Constant constant, object arg)
        {
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            return function;
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            return new NotSentence((Sentence)sentence.getNegated().accept(this, arg));
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            return new ConnectedSentence(sentence.getConnector(),
                    (Sentence)sentence.getFirst().accept(this, arg),
                    (Sentence)sentence.getSecond().accept(this, arg));
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            ISet<Variable> seenSoFar = (Set<Variable>)arg;

            // Keep track of what I have to subst locally and
            // what my renamed variables will be.
            IMap<Variable, Term> localSubst = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
            ICollection<Variable> replVariables = CollectionFactory.CreateQueue<Variable>();
            foreach (Variable v in sentence.getVariables())
            {
                // If local variable has be renamed already
                // then I need to come up with own name
                if (seenSoFar.Contains(v))
                {
                    Variable sV = new Variable(quantifiedIndexical.getPrefix() + quantifiedIndexical.getNextIndex());
                    localSubst.Put(v, sV);
                    // Replacement variables should contain new name for variable
                    replVariables.Add(sV);
                }
                else
                {
                    // Not already replaced, this name is good
                    replVariables.Add(v);
                }
            }

            // Apply the local subst
            Sentence subst = substVisitor.subst(localSubst, sentence.getQuantified());

            // Ensure all my existing and replaced variable
            // names are tracked
            seenSoFar.AddAll(replVariables);

            Sentence sQuantified = (Sentence)subst.accept(this, arg);

            return new QuantifiedSentence(sentence.getQuantifier(), replVariables, sQuantified);
        }
    }

    class RemoveQuantifiers : FOLVisitor
    {
        private FOLParser parser = null;
        private SubstVisitor substVisitor = null;

        public RemoveQuantifiers(FOLParser parser)
        {
            this.parser = parser;

            substVisitor = new SubstVisitor();
        }

        public object visitPredicate(Predicate p, object arg)
        {
            return p;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            return equality;
        }

        public object visitVariable(Variable variable, object arg)
        {
            return variable;
        }

        public object visitConstant(Constant constant, object arg)
        {
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            return function;
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            return new NotSentence((Sentence)sentence.getNegated().accept(this, arg));
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            return new ConnectedSentence(sentence.getConnector(),
                    (Sentence)sentence.getFirst().accept(this, arg),
                    (Sentence)sentence.getSecond().accept(this, arg));
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            Sentence quantified = sentence.getQuantified();
            ISet<Variable> universalScope = (Set<Variable>)arg;

            // Skolemize: Skolemization is the process of removing existential
            // quantifiers by elimination. This is done by introducing Skolem
            // functions. The general rule is that the arguments of the Skolem
            // function are all the universally quantified variables in whose
            // scope the existential quantifier appears.
            if (Quantifiers.isEXISTS(sentence.getQuantifier()))
            {
                IMap<Variable, Term> skolemSubst = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
                foreach (Variable eVar in sentence.getVariables())
                {
                    if (universalScope.Size() > 0)
                    {
                        // Replace with a Skolem Function
                        string skolemFunctionName = parser.getFOLDomain().addSkolemFunction();

                        ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
                        foreach (Variable v in universalScope)
                        {
                            terms.Add(v);
                        }
                        skolemSubst.Put(eVar, new Function(skolemFunctionName, terms));
                    }
                    else
                    {
                        // Replace with a Skolem Constant
                        string skolemConstantName = parser.getFOLDomain().addSkolemConstant();
                        skolemSubst.Put(eVar, new Constant(skolemConstantName));
                    }
                }

                Sentence skolemized = substVisitor.subst(skolemSubst, quantified);
                return skolemized.accept(this, arg);
            }

            // Drop universal quantifiers.
            if (Quantifiers.isFORALL(sentence.getQuantifier()))
            {
                // Add to the universal scope so that
                // existential skolemization may be done correctly
                universalScope.AddAll(sentence.getVariables());

                Sentence droppedUniversal = (Sentence)quantified.accept(this, arg);

                // Enusre my scope is removed before moving back up
                // the call stack when returning
                universalScope.RemoveAll(sentence.getVariables());

                return droppedUniversal;
            }

            // Should not reach here as have already
            // handled the two quantifiers.
            throw new IllegalStateException("Unhandled Quantifier:" + sentence.getQuantifier());
        }
    }

    class DistributeOrOverAnd : FOLVisitor
    {
        public DistributeOrOverAnd()
        { }

        public object visitPredicate(Predicate p, object arg)
        {
            return p;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            return equality;
        }

        public object visitVariable(Variable variable, object arg)
        {
            return variable;
        }

        public object visitConstant(Constant constant, object arg)
        {
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            return function;
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            return new NotSentence((Sentence)sentence.getNegated().accept(this, arg));
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            // Distribute V over ^:

            // This will cause flattening out of nested ^s and Vs
            Sentence alpha = (Sentence)sentence.getFirst().accept(this, arg);
            Sentence beta = (Sentence)sentence.getSecond().accept(this, arg);

            // (alpha V (beta ^ gamma)) equivalent to
            // ((alpha V beta) ^ (alpha V gamma))
            if (Connectors.isOR(sentence.getConnector())
                    && beta is ConnectedSentence)
            {
                ConnectedSentence betaAndGamma = (ConnectedSentence)beta;
                if (Connectors.isAND(betaAndGamma.getConnector()))
                {
                    beta = betaAndGamma.getFirst();
                    Sentence gamma = betaAndGamma.getSecond();
                    return new ConnectedSentence(Connectors.AND,
                            (Sentence)(new ConnectedSentence(Connectors.OR, alpha, beta)).accept(this, arg),
                            (Sentence)(new ConnectedSentence(Connectors.OR, alpha, gamma)).accept(this, arg));
                }
            }

            // ((alpha ^ gamma) V beta) equivalent to
            // ((alpha V beta) ^ (gamma V beta))
            if (Connectors.isOR(sentence.getConnector())
                    && alpha is ConnectedSentence)
            {
                ConnectedSentence alphaAndGamma = (ConnectedSentence)alpha;
                if (Connectors.isAND(alphaAndGamma.getConnector()))
                {
                    alpha = alphaAndGamma.getFirst();
                    Sentence gamma = alphaAndGamma.getSecond();
                    return new ConnectedSentence(Connectors.AND,
                            (Sentence)(new ConnectedSentence(Connectors.OR, alpha,
                                    beta)).accept(this, arg),
                            (Sentence)(new ConnectedSentence(Connectors.OR, gamma,
                                    beta)).accept(this, arg));
                }
            }

            return new ConnectedSentence(sentence.getConnector(), alpha, beta);
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            // This should not be called as should have already
            // removed all of the quantifiers.
            throw new IllegalStateException("All quantified sentences should have already been removed.");
        }
    }

    class CNFConstructor : FOLVisitor
    {
        public CNFConstructor()
        { }

        public CNF construct(Sentence orDistributedOverAnd)
        {
            ArgData ad = new ArgData();

            orDistributedOverAnd.accept(this, ad);

            return new CNF(ad.clauses);
        }

        public object visitPredicate(Predicate p, object arg)
        {
            ArgData ad = (ArgData)arg;
            if (ad.negated)
            {
                ad.clauses.Get(ad.clauses.Size() - 1).addNegativeLiteral(p);
            }
            else
            {
                ad.clauses.Get(ad.clauses.Size() - 1).addPositiveLiteral(p);
            }
            return p;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            ArgData ad = (ArgData)arg;
            if (ad.negated)
            {
                ad.clauses.Get(ad.clauses.Size() - 1).addNegativeLiteral(equality);
            }
            else
            {
                ad.clauses.Get(ad.clauses.Size() - 1).addPositiveLiteral(equality);
            }
            return equality;
        }

        public object visitVariable(Variable variable, object arg)
        {
            // This should not be called
            throw new IllegalStateException("visitVariable() should not be called.");
        }

        public object visitConstant(Constant constant, object arg)
        {
            // This should not be called
            throw new IllegalStateException("visitConstant() should not be called.");
        }

        public object visitFunction(Function function, object arg)
        {
            // This should not be called
            throw new IllegalStateException("visitFunction() should not be called.");
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            ArgData ad = (ArgData)arg;
            // Indicate that the enclosed predicate is negated
            ad.negated = true;
            sentence.getNegated().accept(this, arg);
            ad.negated = false;

            return sentence;
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            ArgData ad = (ArgData)arg;
            Sentence first = sentence.getFirst();
            Sentence second = sentence.getSecond();

            first.accept(this, arg);
            if (Connectors.isAND(sentence.getConnector()))
            {
                ad.clauses.Add(new Clause());
            }
            second.accept(this, arg);

            return sentence;
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            // This should not be called as should have already
            // removed all of the quantifiers.
            throw new IllegalStateException("All quantified sentences should have already been removed.");
        }

        class ArgData
        {
            public ICollection<Clause> clauses = CollectionFactory.CreateQueue<Clause>();
            public bool negated = false;

            public ArgData()
            {
                clauses.Add(new Clause());
            }
        }
    }
}
