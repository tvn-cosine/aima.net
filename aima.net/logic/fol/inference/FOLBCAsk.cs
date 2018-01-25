using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.logic.fol.inference.proof;
using aima.net.logic.fol.kb;
using aima.net.logic.fol.kb.data;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.logic.fol.inference
{
    /**
     * Artificial Intelligence A Modern Approach (2nd Edition): Figure 9.6, page
     * 288.<br>
     * <br>
     * 
     * <pre>
     * function FOL-BC-ASK(KB, goals, theta) returns a set of substitutions
     *   input: KB, a knowledge base
     *          goals, a list of conjuncts forming a query (theta already applied)
     *          theta, the current substitution, initially the empty substitution {}
     *   local variables: answers, a set of substitutions, initially empty
     *   
     *   if goals is empty then return {theta}
     *   qDelta &lt;- SUBST(theta, FIRST(goals))
     *   for each sentence r in KB where STANDARDIZE-APART(r) = (p1 &circ; ... &circ; pn =&gt; q)
     *          and thetaDelta &lt;- UNIFY(q, qDelta) succeeds
     *       new_goals &lt;- [p1,...,pn|REST(goals)]
     *       answers &lt;- FOL-BC-ASK(KB, new_goals, COMPOSE(thetaDelta, theta)) U answers
     *   return answers
     * </pre>
     * 
     * Figure 9.6 A simple backward-chaining algorithm.
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class FOLBCAsk : InferenceProcedure
    {
        public FOLBCAsk()
        { }
         
        /**
         * Returns a set of substitutions
         * 
         * @param KB
         *            a knowledge base
         * @param query
         *            goals, a list of conjuncts forming a query
         * 
         * @return a set of substitutions
         */
        public InferenceResult ask(FOLKnowledgeBase KB, Sentence query)
        {
            // Assertions on the type queries this Inference procedure
            // supports
            if (!(query is AtomicSentence))
            {
                throw new IllegalArgumentException("Only Atomic Queries are supported.");
            }

            ICollection<Literal> goals = CollectionFactory.CreateQueue<Literal>();
            goals.Add(new Literal((AtomicSentence)query));

            BCAskAnswerHandler ansHandler = new BCAskAnswerHandler();

            ICollection<ICollection<ProofStepBwChGoal>> allProofSteps = folbcask(KB, ansHandler, goals, CollectionFactory.CreateInsertionOrderedMap<Variable, Term>());

            ansHandler.setAllProofSteps(allProofSteps);

            return ansHandler;
        }
         
        /**
         * <code>
         * function FOL-BC-ASK(KB, goals, theta) returns a set of substitutions
         *   input: KB, a knowledge base
         *          goals, a list of conjuncts forming a query (theta already applied)
         *          theta, the current substitution, initially the empty substitution {}
         * </code>
         */
        private ICollection<ICollection<ProofStepBwChGoal>> folbcask(FOLKnowledgeBase KB,
                BCAskAnswerHandler ansHandler, ICollection<Literal> goals,
                IMap<Variable, Term> theta)
        {
            ICollection<ICollection<ProofStepBwChGoal>> thisLevelProofSteps = CollectionFactory.CreateQueue<ICollection<ProofStepBwChGoal>>();
            // local variables: answers, a set of substitutions, initially empty

            // if goals is empty then return {theta}
            if (goals.IsEmpty())
            {
                thisLevelProofSteps.Add(CollectionFactory.CreateQueue<ProofStepBwChGoal>());
                return thisLevelProofSteps;
            }

            // qDelta <- SUBST(theta, FIRST(goals))
            Literal qDelta = KB.subst(theta, goals.Get(0));

            // for each sentence r in KB where
            // STANDARDIZE-APART(r) = (p1 ^ ... ^ pn => q)
            foreach (Clause rIter in KB.getAllDefiniteClauses())
            {
                Clause r = rIter;
                r = KB.standardizeApart(r);
                // and thetaDelta <- UNIFY(q, qDelta) succeeds
                IMap<Variable, Term> thetaDelta = KB.unify(r.getPositiveLiterals()
                        .Get(0).getAtomicSentence(), qDelta.getAtomicSentence());
                if (null != thetaDelta)
                {
                    // new_goals <- [p1,...,pn|REST(goals)]
                    ICollection<Literal> newGoals = CollectionFactory.CreateQueue<Literal>(r.getNegativeLiterals());
                    newGoals.AddAll(goals.subList(1, goals.Size()));
                    // answers <- FOL-BC-ASK(KB, new_goals, COMPOSE(thetaDelta,
                    // theta)) U answers
                    IMap<Variable, Term> composed = compose(KB, thetaDelta, theta);
                    ICollection<ICollection<ProofStepBwChGoal>> lowerLevelProofSteps = folbcask(
                            KB, ansHandler, newGoals, composed);

                    ansHandler.addProofStep(lowerLevelProofSteps, r, qDelta, composed);

                    thisLevelProofSteps.AddAll(lowerLevelProofSteps);
                }
            }

            // return answers
            return thisLevelProofSteps;
        }

        // Artificial Intelligence A Modern Approach (2nd Edition): page 288.
        // COMPOSE(delta, tau) is the substitution whose effect is identical to
        // the effect of applying each substitution in turn. That is,
        // SUBST(COMPOSE(theta1, theta2), p) = SUBST(theta2, SUBST(theta1, p))
        private IMap<Variable, Term> compose(FOLKnowledgeBase KB, IMap<Variable, Term> theta1, IMap<Variable, Term> theta2)
        {
            IMap<Variable, Term> composed = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

            // So that it behaves like:
            // SUBST(theta2, SUBST(theta1, p))
            // There are two steps involved here.
            // See: http://logic.stanford.edu/classes/cs157/2008/notes/chap09.pdf
            // for a detailed discussion:

            // 1. Apply theta2 to the range of theta1.
            foreach (Variable v in theta1.GetKeys())
            {
                composed.Put(v, KB.subst(theta2, theta1.Get(v)));
            }

            // 2. Adjoin to delta all pairs from tau with different
            // domain variables.
            foreach (Variable v in theta2.GetKeys())
            {
                if (!theta1.ContainsKey(v))
                {
                    composed.Put(v, theta2.Get(v));
                }
            }

            return cascadeSubstitutions(KB, composed);
        }

        // See:
        // http://logic.stanford.edu/classes/cs157/2008/miscellaneous/faq.html#jump165
        // for need for this.
        private IMap<Variable, Term> cascadeSubstitutions(FOLKnowledgeBase KB,
                IMap<Variable, Term> theta)
        {
            foreach (Variable v in theta.GetKeys())
            {
                Term t = theta.Get(v);
                theta.Put(v, KB.subst(theta, t));
            }

            return theta;
        }

        class BCAskAnswerHandler : InferenceResult
        {
            private ICollection<Proof> proofs = CollectionFactory.CreateQueue<Proof>();

            public BCAskAnswerHandler()
            { }

            //
            // START-InferenceResult
            public bool isPossiblyFalse()
            {
                return proofs.Size() == 0;
            }

            public bool isTrue()
            {
                return proofs.Size() > 0;
            }

            public bool isUnknownDueToTimeout()
            {
                return false;
            }

            public bool isPartialResultDueToTimeout()
            {
                return false;
            }

            public ICollection<Proof> getProofs()
            {
                return proofs;
            }

            // END-InferenceResult
            //

            public void setAllProofSteps(ICollection<ICollection<ProofStepBwChGoal>> allProofSteps)
            {
                foreach (ICollection<ProofStepBwChGoal> steps in allProofSteps)
                {
                    ProofStepBwChGoal lastStep = steps.Get(steps.Size() - 1);
                    IMap<Variable, Term> theta = lastStep.getBindings();
                    proofs.Add(new ProofFinal(lastStep, theta));
                }
            }

            public void addProofStep(
                    ICollection<ICollection<ProofStepBwChGoal>> currentLevelProofSteps,
                    Clause toProve, Literal currentGoal,
                    IMap<Variable, Term> bindings)
            {

                if (currentLevelProofSteps.Size() > 0)
                {
                    ProofStepBwChGoal predecessor = new ProofStepBwChGoal(toProve, currentGoal, bindings);
                    foreach (ICollection<ProofStepBwChGoal> steps in currentLevelProofSteps)
                    {
                        if (steps.Size() > 0)
                        {
                            steps.Get(0).setPredecessor(predecessor);
                        }
                        steps.Insert(0, predecessor);
                    }
                }
            }
        }
    }
}
