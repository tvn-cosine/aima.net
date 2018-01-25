using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections.api;
using aima.net.logic.fol.inference;
using aima.net.logic.fol.inference.trace;
using aima.net.logic.fol.kb.data;
using aima.net.test.unit.logic.fol;

namespace aima.net.test.performance.logic.fol.inference
{
    [TestClass] 
    public class FOLTFMResolutionPerformance : CommonFOLInferenceProcedureTests
    {
        [TestMethod] 
        public void testFullFOLKBLovesAnimalQueryKillsJackTunaFalse()
        {
            // This query will not return using TFM as keep expanding
            // clauses through resolution for this KB.
            FOLTFMResolution ip = new FOLTFMResolution(10 * 1000);
            ip.setTracer(new RegressionFOLTFMResolutionTracer());
            testFullFOLKBLovesAnimalQueryKillsJackTunaFalse(ip, true);
        }

        private class RegressionFOLTFMResolutionTracer : FOLTFMResolutionTracer
        {
            private int outerCnt = 1;
            private int noPairsConsidered = 0;
            private int noPairsResolved = 0;
            private int maxClauseSizeSeen = 0;

            public void stepStartWhile(ISet<Clause> clauses, int totalNoClauses, int totalNoNewCandidateClauses)
            {
                outerCnt = 1;

                System.Console.WriteLine("");
                System.Console.WriteLine("Total # clauses=" + totalNoClauses
                        + ", total # new candidate clauses="
                        + totalNoNewCandidateClauses);
            }

            public void stepOuterFor(Clause i)
            {
                System.Console.Write(" " + outerCnt);
                if (outerCnt % 50 == 0)
                {
                    System.Console.WriteLine("");
                }
                outerCnt++;
            }

            public void stepInnerFor(Clause i, Clause j)
            {
                noPairsConsidered++;
            }

            public void stepResolved(Clause iFactor, Clause jFactor, ISet<Clause> resolvents)
            {
                noPairsResolved++;

                Clause egLargestClause = null;
                foreach (Clause c in resolvents)
                {
                    if (c.getNumberLiterals() > maxClauseSizeSeen)
                    {
                        egLargestClause = c;
                        maxClauseSizeSeen = c.getNumberLiterals();
                    }
                }
                if (null != egLargestClause)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine("E.g. largest clause so far=" + maxClauseSizeSeen + ", " + egLargestClause);
                    System.Console.WriteLine("i=" + iFactor);
                    System.Console.WriteLine("j=" + jFactor);
                }
            }

            public void stepFinished(ISet<Clause> clauses, InferenceResult result)
            {
                System.Console.WriteLine("Total # Pairs of Clauses Considered:" + noPairsConsidered);
                System.Console.WriteLine("Total # Pairs of Clauses Resolved  :" + noPairsResolved);
                noPairsConsidered = 0;
                noPairsResolved = 0;
                maxClauseSizeSeen = 0;
            }
        }
    }
}
