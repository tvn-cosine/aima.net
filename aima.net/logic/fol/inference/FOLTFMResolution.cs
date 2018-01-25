using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.logic.fol.inference.proof;
using aima.net.logic.fol.inference.trace;
using aima.net.logic.fol.kb;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 347.<br>
     * <br>
     * The algorithmic approach is identical to the propositional case, described in
     * Figure 7.12.<br>
     * <br>
     * However, this implementation will use the T)wo F)inger M)ethod for looking
     * for resolvents between clauses, which is very inefficient.<br>
     * <br>
     * see:<br>
     * <a
     * href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture04.pdf">
     * http://logic.stanford.edu/classes/cs157/2008/lectures/lecture04.pdf</a>,
     * slide 21 for the propositional case. In addition, an Answer literal will be
     * used so that queries with Variables may be answered (see pg. 350 of AIMA3e).
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLTFMResolution : InferenceProcedure
    {
        private long maxQueryTime = 10 * 1000;

        private FOLTFMResolutionTracer tracer = null;

        public FOLTFMResolution()
        { }

        public FOLTFMResolution(long maxQueryTime)
        {
            setMaxQueryTime(maxQueryTime);
        }

        public FOLTFMResolution(FOLTFMResolutionTracer tracer)
        {
            setTracer(tracer);
        }

        public long getMaxQueryTime()
        {
            return maxQueryTime;
        }

        public void setMaxQueryTime(long maxQueryTime)
        {
            this.maxQueryTime = maxQueryTime;
        }

        public FOLTFMResolutionTracer getTracer()
        {
            return tracer;
        }

        public void setTracer(FOLTFMResolutionTracer tracer)
        {
            this.tracer = tracer;
        }

        //
        // START-InferenceProcedure
        public InferenceResult ask(FOLKnowledgeBase KB, Sentence alpha)
        {
            // clauses <- the set of clauses in CNF representation of KB ^ ~alpha
            ISet<Clause> clauses = CollectionFactory.CreateSet<Clause>();
            foreach (Clause cIter in KB.getAllClauses())
            {
                Clause c = cIter;
                c = KB.standardizeApart(c);
                c.setStandardizedApartCheckNotRequired();
                clauses.AddAll(c.getFactors());
            }
            Sentence notAlpha = new NotSentence(alpha);
            // Want to use an answer literal to pull
            // query variables where necessary
            Literal answerLiteral = KB.createAnswerLiteral(notAlpha);
            ISet<Variable> answerLiteralVariables = KB.collectAllVariables(answerLiteral.getAtomicSentence());
            Clause answerClause = new Clause();

            if (answerLiteralVariables.Size() > 0)
            {
                Sentence notAlphaWithAnswer = new ConnectedSentence(Connectors.OR,
                        notAlpha, answerLiteral.getAtomicSentence());
                foreach (Clause cIter in KB.convertToClauses(notAlphaWithAnswer))
                {
                    Clause c = cIter;
                    c = KB.standardizeApart(c);
                    c.setProofStep(new ProofStepGoal(c));
                    c.setStandardizedApartCheckNotRequired();
                    clauses.AddAll(c.getFactors());
                }

                answerClause.addLiteral(answerLiteral);
            }
            else
            {
                foreach (Clause cIter in KB.convertToClauses(notAlpha))
                {
                    Clause c = cIter;
                    c = KB.standardizeApart(c);
                    c.setProofStep(new ProofStepGoal(c));
                    c.setStandardizedApartCheckNotRequired();
                    clauses.AddAll(c.getFactors());
                }
            }

            TFMAnswerHandler ansHandler = new TFMAnswerHandler(answerLiteral,
                    answerLiteralVariables, answerClause, maxQueryTime);

            // new <- {}
            ISet<Clause> newClauses = CollectionFactory.CreateSet<Clause>();
            ISet<Clause> toAdd = CollectionFactory.CreateSet<Clause>();
            // loop do
            int noOfPrevClauses = clauses.Size();
            do
            {
                if (null != tracer)
                {
                    tracer.stepStartWhile(clauses, clauses.Size(), newClauses.Size());
                }

                newClauses.Clear();

                // for each Ci, Cj in clauses do
                Clause[] clausesA = clauses.ToArray();

                // Basically, using the simple T)wo F)inger M)ethod here.
                for (int i = 0; i < clausesA.Length; i++)
                {
                    Clause cI = clausesA[i];
                    if (null != tracer)
                    {
                        tracer.stepOuterFor(cI);
                    }
                    for (int j = i; j < clausesA.Length; j++)
                    {
                        Clause cJ = clausesA[j];

                        if (null != tracer)
                        {
                            tracer.stepInnerFor(cI, cJ);
                        }

                        // resolvent <- FOL-RESOLVE(Ci, Cj)
                        ISet<Clause> resolvents = cI.binaryResolvents(cJ);

                        if (resolvents.Size() > 0)
                        {
                            toAdd.Clear();
                            // new <- new <UNION> resolvent
                            foreach (Clause rc in resolvents)
                            {
                                toAdd.AddAll(rc.getFactors());
                            }

                            if (null != tracer)
                            {
                                tracer.stepResolved(cI, cJ, toAdd);
                            }

                            ansHandler.checkForPossibleAnswers(toAdd);

                            if (ansHandler.isComplete())
                            {
                                break;
                            }

                            newClauses.AddAll(toAdd);
                        }

                        if (ansHandler.isComplete())
                        {
                            break;
                        }
                    }
                    if (ansHandler.isComplete())
                    {
                        break;
                    }
                }

                noOfPrevClauses = clauses.Size();

                // clauses <- clauses <UNION> new
                clauses.AddAll(newClauses);

                if (ansHandler.isComplete())
                {
                    break;
                }

                // if new is a <SUBSET> of clauses then finished
                // searching for an answer
                // (i.e. when they were added the # clauses
                // did not increase).
            } while (noOfPrevClauses < clauses.Size());

            if (null != tracer)
            {
                tracer.stepFinished(clauses, ansHandler);
            }

            return ansHandler;
        }

        // END-InferenceProcedure
        //

        //
        // PRIVATE METHODS
        //
        class TFMAnswerHandler : InferenceResult
        {
            private Literal answerLiteral = null;
            private ISet<Variable> answerLiteralVariables = null;
            private Clause answerClause = null;
            private IDateTime finishTime;
            private bool complete = false;
            private ICollection<Proof> proofs = CollectionFactory.CreateQueue<Proof>();
            private bool timedOut = false;

            public TFMAnswerHandler(Literal answerLiteral,
                    ISet<Variable> answerLiteralVariables, Clause answerClause,
                    long maxQueryTime)
            {
                this.answerLiteral = answerLiteral;
                this.answerLiteralVariables = answerLiteralVariables;
                this.answerClause = answerClause;
                //
                this.finishTime = CommonFactory.Now().AddMilliseconds(maxQueryTime);
            }

            //
            // START-InferenceResult
            public bool isPossiblyFalse()
            {
                return !timedOut && proofs.Size() == 0;
            }

            public bool isTrue()
            {
                return proofs.Size() > 0;
            }

            public bool isUnknownDueToTimeout()
            {
                return timedOut && proofs.Size() == 0;
            }

            public bool isPartialResultDueToTimeout()
            {
                return timedOut && proofs.Size() > 0;
            }

            public ICollection<Proof> getProofs()
            {
                return proofs;
            }

            // END-InferenceResult
            //

            public bool isComplete()
            {
                return complete;
            }

            public void checkForPossibleAnswers(ISet<Clause> resolvents)
            {
                // If no bindings being looked for, then
                // is just a true false query.
                foreach (Clause aClause in resolvents)
                {
                    if (answerClause.isEmpty())
                    {
                        if (aClause.isEmpty())
                        {
                            proofs.Add(new ProofFinal(aClause.getProofStep(), CollectionFactory.CreateMap<Variable, Term>()));
                            complete = true;
                        }
                    }
                    else
                    {
                        if (aClause.isEmpty())
                        {
                            // This should not happen
                            // as added an answer literal, which
                            // implies the database (i.e. premises) are
                            // unsatisfiable to begin with.
                            throw new IllegalStateException("Generated an empty clause while looking for an answer, implies original KB is unsatisfiable");
                        }

                        if (aClause.isUnitClause()
                                && aClause.isDefiniteClause()
                                && aClause
                                        .getPositiveLiterals()
                                        .Get(0)
                                        .getAtomicSentence()
                                        .getSymbolicName()
                                        .Equals(answerLiteral.getAtomicSentence()
                                                .getSymbolicName()))
                        {
                            IMap<Variable, Term> answerBindings = CollectionFactory.CreateMap<Variable, Term>();
                            ICollection<Term> answerTerms = aClause.getPositiveLiterals().Get(0).getAtomicSentence().getArgs();
                            int idx = 0;
                            foreach (Variable v in answerLiteralVariables)
                            {
                                answerBindings.Put(v, answerTerms.Get(idx));
                                idx++;
                            }
                            bool addNewAnswer = true;
                            foreach (Proof p in proofs)
                            {
                                if (p.getAnswerBindings().SequenceEqual(answerBindings))
                                {
                                    addNewAnswer = false;
                                    break;
                                }
                            }
                            if (addNewAnswer)
                            {
                                proofs.Add(new ProofFinal(aClause.getProofStep(), answerBindings));
                            }
                        }
                    }

                    if (CommonFactory.Now().BiggerThan(finishTime))
                    {
                        complete = true;
                        // Indicate that I have run out of query time
                        timedOut = true;
                    }
                }
            }

            public override string ToString()
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append("isComplete=" + complete);
                sb.Append("\n");
                sb.Append("result=" + proofs);
                return sb.ToString();
            }
        }
    }
}
