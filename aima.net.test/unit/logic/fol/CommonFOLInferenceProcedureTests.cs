using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.logic.fol.inference;
using aima.net.logic.fol.inference.proof;
using aima.net.logic.fol.kb;
using aima.net.logic.fol.parsing.ast;

namespace aima.net.test.unit.logic.fol
{
    public abstract class CommonFOLInferenceProcedureTests
    {
        protected void testDefiniteClauseKBKingsQueryCriminalXFalse(InferenceProcedure infp)
        {
            FOLKnowledgeBase kkb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("Criminal", terms);
            InferenceResult answer = kkb.ask(query);
            Assert.IsTrue(null != answer);
            Assert.IsTrue(answer.isPossiblyFalse());
            Assert.IsFalse(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(0 == answer.getProofs().Size());
        }

        protected void testDefiniteClauseKBKingsQueryRichardEvilFalse(InferenceProcedure infp)
        {
            FOLKnowledgeBase kkb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("Richard"));
            Predicate query = new Predicate("Evil", terms);
            InferenceResult answer = kkb.ask(query);
            Assert.IsTrue(null != answer);
            Assert.IsTrue(answer.isPossiblyFalse());
            Assert.IsFalse(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(0 == answer.getProofs().Size());
        }

        protected void testDefiniteClauseKBKingsQueryJohnEvilSucceeds(InferenceProcedure infp)
        {
            FOLKnowledgeBase kkb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("John"));
            Predicate query = new Predicate("Evil", terms);
            InferenceResult answer = kkb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(1 == answer.getProofs().Size());
            Assert.IsTrue(0 == answer.getProofs().Get(0).getAnswerBindings().Size());
        }

        protected void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(InferenceProcedure infp)
        {
            FOLKnowledgeBase kkb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("Evil", terms);
            InferenceResult answer = kkb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(1 == answer.getProofs().Size());
            Assert.IsTrue(1 == answer.getProofs().Get(0).getAnswerBindings().Size());
            Assert.AreEqual(new Constant("John"), answer.getProofs().Get(0).getAnswerBindings().Get(new Variable("x")));
        }

        protected void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(InferenceProcedure infp)
        {
            FOLKnowledgeBase kkb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("King", terms);
            InferenceResult answer = kkb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(2 == answer.getProofs().Size());
            Assert.IsTrue(1 == answer.getProofs().Get(0).getAnswerBindings().Size());
            Assert.IsTrue(1 == answer.getProofs().Get(1).getAnswerBindings().Size());

            bool gotJohn, gotRichard;
            gotJohn = gotRichard = false;
            Constant cJohn = new Constant("John");
            Constant cRichard = new Constant("Richard");
            foreach (Proof p in answer.getProofs())
            {
                IMap<Variable, Term> ans = p.getAnswerBindings();
                Assert.AreEqual(1, ans.Size());
                if (cJohn.Equals(ans.Get(new Variable("x"))))
                {
                    gotJohn = true;
                }
                if (cRichard.Equals(ans.Get(new Variable("x"))))
                {
                    gotRichard = true;
                }
            }
            Assert.IsTrue(gotJohn);
            Assert.IsTrue(gotRichard);
        }

        protected void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(InferenceProcedure infp)
        {
            FOLKnowledgeBase wkb = FOLKnowledgeBaseFactory.createWeaponsKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("Criminal", terms);

            InferenceResult answer = wkb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(1 == answer.getProofs().Size());
            Assert.IsTrue(1 == answer.getProofs().Get(0).getAnswerBindings().Size());
            Assert.AreEqual(new Constant("West"), answer.getProofs().Get(0).getAnswerBindings().Get(new Variable("x")));
        }

        protected void testHornClauseKBRingOfThievesQuerySkisXReturnsNancyRedBertDrew(InferenceProcedure infp)
        {
            FOLKnowledgeBase rotkb = FOLKnowledgeBaseFactory.createRingOfThievesKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("Skis", terms);

            InferenceResult answer = rotkb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            // DB can expand infinitely so is only partial.
            Assert.IsTrue(answer.isPartialResultDueToTimeout());
            Assert.AreEqual(4, answer.getProofs().Size());
            Assert.AreEqual(1, answer.getProofs().Get(0).getAnswerBindings().Size());
            Assert.AreEqual(1, answer.getProofs().Get(1).getAnswerBindings().Size());
            Assert.AreEqual(1, answer.getProofs().Get(2).getAnswerBindings().Size());
            Assert.AreEqual(1, answer.getProofs().Get(3).getAnswerBindings().Size());

            ICollection<Constant> expected = CollectionFactory.CreateQueue<Constant>();
            expected.Add(new Constant("Nancy"));
            expected.Add(new Constant("Red"));
            expected.Add(new Constant("Bert"));
            expected.Add(new Constant("Drew"));
            foreach (Proof p in answer.getProofs())
            {
                expected.Remove(p.getAnswerBindings().Get(new Variable("x")) as Constant);
            }
            Assert.AreEqual(0, expected.Size());
        }

        protected void testFullFOLKBLovesAnimalQueryKillsCuriosityTunaSucceeds(InferenceProcedure infp, bool expectedToTimeOut)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createLovesAnimalKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("Curiosity"));
            terms.Add(new Constant("Tuna"));
            Predicate query = new Predicate("Kills", terms);

            InferenceResult answer = akb.ask(query);
            Assert.IsTrue(null != answer);
            if (expectedToTimeOut)
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsTrue(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsTrue(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(1 == answer.getProofs().Size());
                Assert.IsTrue(0 == answer.getProofs().Get(0).getAnswerBindings().Size());
            }
        }

        protected void testFullFOLKBLovesAnimalQueryNotKillsJackTunaSucceeds(InferenceProcedure infp, bool expectedToTimeOut)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createLovesAnimalKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("Jack"));
            terms.Add(new Constant("Tuna"));
            NotSentence query = new NotSentence(new Predicate("Kills", terms));

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            if (expectedToTimeOut)
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsTrue(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsTrue(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(1 == answer.getProofs().Size());
                Assert.IsTrue(0 == answer.getProofs().Get(0).getAnswerBindings().Size());
            }
        }

        protected void testFullFOLKBLovesAnimalQueryKillsJackTunaFalse(InferenceProcedure infp, bool expectedToTimeOut)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createLovesAnimalKnowledgeBase(infp);
            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("Jack"));
            terms.Add(new Constant("Tuna"));
            Predicate query = new Predicate("Kills", terms);

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            if (expectedToTimeOut)
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsTrue(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsTrue(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
        }

        protected void testEqualityAxiomsKBabcAEqualsCSucceeds(                InferenceProcedure infp)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCEqualityKnowledgeBase(infp, true);

            TermEquality query = new TermEquality(new Constant("A"), new Constant("C"));

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(1 == answer.getProofs().Size());
            Assert.IsTrue(0 == answer.getProofs().Get(0).getAnswerBindings().Size());
        }

        protected void testEqualityAndSubstitutionAxiomsKBabcdFFASucceeds(InferenceProcedure infp)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCDEqualityAndSubstitutionKnowledgeBase(infp, true);

            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("A"));
            Function fa = new Function("F", terms);
            terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(fa);
            TermEquality query = new TermEquality(new Function("F", terms), new Constant("A"));

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(1 == answer.getProofs().Size());
            Assert.IsTrue(0 == answer.getProofs().Get(0).getAnswerBindings().Size());
        }

        protected void testEqualityAndSubstitutionAxiomsKBabcdPDSucceeds(InferenceProcedure infp)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCDEqualityAndSubstitutionKnowledgeBase(infp, true);

            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("D"));
            Predicate query = new Predicate("P", terms);

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            Assert.IsFalse(answer.isPossiblyFalse());
            Assert.IsTrue(answer.isTrue());
            Assert.IsFalse(answer.isUnknownDueToTimeout());
            Assert.IsFalse(answer.isPartialResultDueToTimeout());
            Assert.IsTrue(1 == answer.getProofs().Size());
            Assert.IsTrue(0 == answer.getProofs().Get(0).getAnswerBindings().Size());
        }

        protected void testEqualityAndSubstitutionAxiomsKBabcdPFFASucceeds(InferenceProcedure infp, bool expectedToTimeOut)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCDEqualityAndSubstitutionKnowledgeBase(infp, true);

            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("A"));
            Function fa = new Function("F", terms);
            terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(fa);
            Function ffa = new Function("F", terms);
            terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(ffa);
            Predicate query = new Predicate("P", terms);

            InferenceResult answer = akb.ask(query);

            if (expectedToTimeOut)
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsTrue(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsTrue(null != answer);
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsTrue(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(1 == answer.getProofs().Size());
                Assert.IsTrue(0 == answer.getProofs().Get(0)
                        .getAnswerBindings().Size());
            }
        }

        protected void testEqualityNoAxiomsKBabcAEqualsCSucceeds(InferenceProcedure infp, bool expectedToFail)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCEqualityKnowledgeBase(infp, false);

            TermEquality query = new TermEquality(new Constant("A"), new Constant("C"));

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            if (expectedToFail)
            {
                Assert.IsTrue(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsTrue(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(1 == answer.getProofs().Size());
                Assert.IsTrue(0 == answer.getProofs().Get(0)
                        .getAnswerBindings().Size());
            }
        }

        protected void testEqualityAndSubstitutionNoAxiomsKBabcdFFASucceeds(InferenceProcedure infp, bool expectedToFail)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCDEqualityAndSubstitutionKnowledgeBase(infp, false);

            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("A"));
            Function fa = new Function("F", terms);
            terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(fa);
            TermEquality query = new TermEquality(new Function("F", terms), new Constant("A"));

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            if (expectedToFail)
            {
                Assert.IsTrue(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsTrue(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(1 == answer.getProofs().Size());
                Assert.IsTrue(0 == answer.getProofs().Get(0)
                        .getAnswerBindings().Size());
            }
        }

        protected void testEqualityAndSubstitutionNoAxiomsKBabcdPDSucceeds(InferenceProcedure infp, bool expectedToFail)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCDEqualityAndSubstitutionKnowledgeBase(infp, false);

            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("D"));
            Predicate query = new Predicate("P", terms);

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            if (expectedToFail)
            {
                Assert.IsTrue(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsTrue(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(1 == answer.getProofs().Size());
                Assert.IsTrue(0 == answer.getProofs().Get(0)
                        .getAnswerBindings().Size());
            }
        }

        protected void testEqualityAndSubstitutionNoAxiomsKBabcdPFFASucceeds(InferenceProcedure infp, bool expectedToFail)
        {
            FOLKnowledgeBase akb = FOLKnowledgeBaseFactory.createABCDEqualityAndSubstitutionKnowledgeBase(infp, false);

            ICollection<Term> terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(new Constant("A"));
            Function fa = new Function("F", terms);
            terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(fa);
            Function ffa = new Function("F", terms);
            terms = CollectionFactory.CreateQueue<Term>();
            terms.Add(ffa);
            Predicate query = new Predicate("P", terms);

            InferenceResult answer = akb.ask(query);

            Assert.IsTrue(null != answer);
            if (expectedToFail)
            {
                Assert.IsTrue(answer.isPossiblyFalse());
                Assert.IsFalse(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(0 == answer.getProofs().Size());
            }
            else
            {
                Assert.IsFalse(answer.isPossiblyFalse());
                Assert.IsTrue(answer.isTrue());
                Assert.IsFalse(answer.isUnknownDueToTimeout());
                Assert.IsFalse(answer.isPartialResultDueToTimeout());
                Assert.IsTrue(1 == answer.getProofs().Size());
                Assert.IsTrue(0 == answer.getProofs().Get(0)
                        .getAnswerBindings().Size());
            }
        }
    }
}
